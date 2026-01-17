using MediaService.Domain.Entities;
using MediaService.Application.DTOs;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MediaService.Application.Interfaces;

public interface IMediaService
{
    Task<MediaResponse> UploadFileAsync(IFormFile file, string subFolder = "");
    Task<Stream> DownloadFileAsync(string filePath);
    Task DeleteFileAsync(string filePath);
    Task<List<MediaResponse>> ListFilesAsync(string path = "");
    Task CreateDirectoryAsync(string path);
    Task DeleteDirectoryAsync(string path);
}
