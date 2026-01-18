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

    public async Task<List<T>> ReadFromStreamAsync<T>(Stream stream, List<ExcelColumnConfig>? configs = null) where T : new()
    {
        var result = new List<T>();
        using var workbook = WorkbookFactory.Create(stream);
        var sheet = workbook.GetSheetAt(0);
        if (sheet == null || sheet.LastRowNum < 1) return result;

        // 1. Determine Mappings
        var headerRow = sheet.GetRow(0);
        var columnMappings = new Dictionary<int, PropertyInfo>();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (configs != null && configs.Count > 0)
        {
            // Map based on provided configs
            for (int i = 0; i < configs.Count; i++)
            {
                var config = configs[i];
                // Find column index by header name in the excel
                for (int colIndex = 0; colIndex < headerRow.LastCellNum; colIndex++)
                {
                    var cellValue = headerRow.GetCell(colIndex)?.ToString()?.Trim();
                    if (string.Equals(cellValue, config.Header, StringComparison.OrdinalIgnoreCase))
                    {
                        var prop = properties.FirstOrDefault(p => string.Equals(p.Name, config.PropertyName, StringComparison.OrdinalIgnoreCase));
                        if (prop != null) columnMappings[colIndex] = prop;
                        break;
                    }
                }
            }
        }
        else
        {
            // Auto-map based on header names matching property names
            for (int colIndex = 0; colIndex < headerRow.LastCellNum; colIndex++)
            {
                var cellValue = headerRow.GetCell(colIndex)?.ToString()?.Trim();
                if (string.IsNullOrEmpty(cellValue)) continue;

                var prop = properties.FirstOrDefault(p => string.Equals(p.Name, cellValue, StringComparison.OrdinalIgnoreCase));
                if (prop != null) columnMappings[colIndex] = prop;
            }
        }

        // 2. Read Data Rows
        for (int rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
        {
            var row = sheet.GetRow(rowIndex);
            if (row == null) continue;

            var item = new T();
            bool hasData = false;

            foreach (var mapping in columnMappings)
            {
                var cell = row.GetCell(mapping.Key);
                if (cell == null) continue;

                var value = GetCellValue(cell, mapping.Value.PropertyType);
                if (value != null)
                {
                    mapping.Value.SetValue(item, value);
                    hasData = true;
                }
            }

            if (hasData) result.Add(item);
        }

        return await Task.FromResult(result);
    }

    private object? GetCellValue(ICell cell, Type targetType)
    {
        if (cell == null || cell.CellType == CellType.Blank) return null;

        object? value = null;
        switch (cell.CellType)
        {
            case CellType.Numeric:
                if (DateUtil.IsCellDateFormatted(cell)) value = cell.DateCellValue;
                else value = cell.NumericCellValue;
                break;
            case CellType.String:
                value = cell.StringCellValue;
                break;
            case CellType.Boolean:
                value = cell.BooleanCellValue;
                break;
            case CellType.Formula:
                // For formulas, we usually want the evaluated result as a string
                value = cell.ToString();
                break;
        }

        if (value == null) return null;

        // Robust Conversion
        try
        {
            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (underlyingType == typeof(Guid)) return Guid.Parse(value.ToString() ?? "");
            if (underlyingType.IsEnum) return Enum.Parse(underlyingType, value.ToString() ?? "", true);
            if (underlyingType == typeof(DateTime))
            {
                if (value is DateTime dt) return dt;
                if (DateTime.TryParse(value.ToString(), out var parsedDate)) return parsedDate;
            }

            return Convert.ChangeType(value, underlyingType);
        }
        catch
        {
            return null;
        }
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
