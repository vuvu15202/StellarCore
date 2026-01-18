using MediaService.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediaService.Domain.Models.Values;

namespace MediaService.Domain.Services;

public interface IFileService
{
    Task<MediaResponse> UploadFileAsync(IFormFile file, string subFolder = "");
    Task<Stream> DownloadFileAsync(string filePath);
    Task DeleteFileAsync(string filePath);
    Task<List<MediaResponse>> ListFilesAsync(string path = "");
    Task CreateDirectoryAsync(string path);
    Task DeleteDirectoryAsync(string path);
}
