using NPOI.SS.UserModel;
using NPOI.XSSF.Streaming;
using NPOI.XSSF.UserModel;
using System.Collections;
using System.Reflection;

namespace Stellar.Shared.Excel;

public class ExcelService : IExcelService
{
    private const string DEFAULT_DATE_FORMAT = "mm:HH dd/MM/yyyy";
    private const string DEFAULT_NUMBER_FORMAT = "#,##0.00";

    public async Task<MemoryStream> ExportToStreamAsync<T>(IEnumerable<T> data, string sheetName, List<ExcelColumnConfig>? configs = null)
    {
        // SXSSFWorkbook is used for high-performance streaming (port of Java SXSSF)
        // 100 is the row window in memory
        using var workbook = new SXSSFWorkbook(100);
        var sheet = workbook.CreateSheet(sheetName);

        var headerStyle = CreateHeaderStyle(workbook);
        var bodyStyle = CreateBodyStyle(workbook);
        var dateStyle = CreateDataStyle(workbook, DEFAULT_DATE_FORMAT);
        var numberStyle = CreateDataStyle(workbook, DEFAULT_NUMBER_FORMAT);

        // If configs are not provided, generate from properties
        if (configs == null || configs.Count == 0)
        {
            configs = GenerateConfigsFromType<T>();
        }

        // Create Header Row
        var headerRow = sheet.CreateRow(0);
        for (int i = 0; i < configs.Count; i++)
        {
            var cell = headerRow.CreateCell(i);
            cell.SetCellValue(configs[i].Header);
            cell.CellStyle = headerStyle;
            
            if (configs[i].ColumnWidth.HasValue)
            {
                sheet.SetColumnWidth(i, (int)(configs[i].ColumnWidth.Value * 256));
            }
            else
            {
                sheet.SetColumnWidth(i, 20 * 256); // Default 20 chars
            }
        }

        // Fill Data
        int rowIndex = 1;
        foreach (var item in data)
        {
            var row = sheet.CreateRow(rowIndex++);
            for (int i = 0; i < configs.Count; i++)
            {
                var cell = row.CreateCell(i);
                var value = GetValueByPropertyName(item, configs[i].PropertyName);
                
                SetCellValue(cell, value, dateStyle, numberStyle, bodyStyle);
            }
        }

        var memoryStream = new MemoryStream();
        workbook.Write(memoryStream);
        
        // Disposal of workbook may close the underlying stream.
        // We capture the bytes into a new MemoryStream to return an open, independent stream.
        var buffer = memoryStream.ToArray();
        workbook.Dispose(); 
        
        return await Task.FromResult(new MemoryStream(buffer));
    }

    private void SetCellValue(ICell cell, object? value, ICellStyle dateStyle, ICellStyle numberStyle, ICellStyle defaultStyle)
    {
        if (value == null)
        {
            cell.SetCellValue(string.Empty);
            cell.CellStyle = defaultStyle;
            return;
        }

        if (value is DateTime dateValue)
        {
            cell.SetCellValue(dateValue);
            cell.CellStyle = dateStyle;
        }
        else if (value is double dValue)
        {
            cell.SetCellValue(dValue);
            cell.CellStyle = numberStyle;
        }
        else if (value is decimal decValue)
        {
            cell.SetCellValue((double)decValue);
            cell.CellStyle = numberStyle;
        }
        else if (value is int intValue)
        {
            cell.SetCellValue(intValue);
            cell.CellStyle = defaultStyle;
        }
        else if (value is bool boolValue)
        {
            cell.SetCellValue(boolValue);
            cell.CellStyle = defaultStyle;
        }
        else
        {
            cell.SetCellValue(value.ToString());
            cell.CellStyle = defaultStyle;
        }
    }

    private List<ExcelColumnConfig> GenerateConfigsFromType<T>()
    {
        var configs = new List<ExcelColumnConfig>();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in properties)
        {
            configs.Add(new ExcelColumnConfig
            {
                Header = prop.Name,
                PropertyName = prop.Name
            });
        }
        return configs;
    }

    private object? GetValueByPropertyName(object item, string propertyName)
    {
        if (item == null) return null;
        var prop = item.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        return prop?.GetValue(item);
    }

    private ICellStyle CreateHeaderStyle(IWorkbook workbook)
    {
        var style = workbook.CreateCellStyle();
        style.FillForegroundColor = IndexedColors.Grey25Percent.Index;
        style.FillPattern = FillPattern.SolidForeground;
        style.Alignment = HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Center;
        
        var font = workbook.CreateFont();
        font.IsBold = true;
        font.FontHeightInPoints = 11;
        style.SetFont(font);

        AddBorders(style);
        return style;
    }

    private ICellStyle CreateBodyStyle(IWorkbook workbook)
    {
        var style = workbook.CreateCellStyle();
        style.VerticalAlignment = VerticalAlignment.Center;
        AddBorders(style);
        return style;
    }

    private ICellStyle CreateDataStyle(IWorkbook workbook, string format)
    {
        var style = workbook.CreateCellStyle();
        style.VerticalAlignment = VerticalAlignment.Center;
        var dataFormat = workbook.CreateDataFormat();
        style.DataFormat = dataFormat.GetFormat(format);
        AddBorders(style);
        return style;
    }

    private void AddBorders(ICellStyle style)
    {
        style.BorderBottom = BorderStyle.Thin;
        style.BorderTop = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Thin;
    }
}
