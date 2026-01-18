namespace Stellar.Shared.Excel;

public class ExcelColumnConfig
{
    public string Header { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public string? CustomFormat { get; set; }
    public double? ColumnWidth { get; set; }
}
