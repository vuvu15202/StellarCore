using MediaService.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediaService.Domain.Models.Values;

namespace MediaService.Domain.Services;

public interface IFileService
{
    Task<MediaResponse> UploadFileAsync(IFormFile file, Guid? parentId = null);
    Task<FileDownloadResult> DownloadFileAsync(Guid mediaId);
    Task DeleteFileAsync(Guid mediaId);
    Task<List<MediaResponse>> ListFilesAsync(Guid? parentId = null);
    Task<List<MediaResponse>> GetMediaTreeAsync(Guid? parentId = null);
    Task CreateDirectoryAsync(string name, Guid? parentId = null);
    Task DeleteDirectoryAsync(Guid mediaId);
}
