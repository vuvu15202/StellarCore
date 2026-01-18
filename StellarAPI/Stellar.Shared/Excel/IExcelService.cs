using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Stellar.Shared.Excel;

public interface IExcelService
{
    Task<MemoryStream> ExportToStreamAsync<T>(IEnumerable<T> data, string sheetName, List<ExcelColumnConfig>? configs = null);
    Task<List<T>> ReadFromStreamAsync<T>(Stream stream, List<ExcelColumnConfig>? configs = null) where T : new();
}
