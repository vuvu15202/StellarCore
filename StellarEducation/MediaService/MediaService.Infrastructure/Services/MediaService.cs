using MediaService.Application.DTOs;
using MediaService.Application.Interfaces;
using MediaService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediaService.Infrastructure.Services;

public class MediaService : IMediaService
{
    private readonly string _rootPath;
    private readonly IMediaRepository _mediaRepository;

    public MediaService(IMediaRepository mediaRepository, string rootPath = "uploads")
    {
        _mediaRepository = mediaRepository;
        _rootPath = Path.Combine(Directory.GetCurrentDirectory(), rootPath);
        if (!Directory.Exists(_rootPath))
        {
            Directory.CreateDirectory(_rootPath);
        }
    }

    public async Task<MediaResponse> UploadFileAsync(IFormFile file, string subFolder = "")
    {
        var targetDir = Path.Combine(_rootPath, subFolder);
        if (!Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(targetDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var relativePath = Path.Combine(subFolder, fileName).Replace("\\", "/");

        var media = new Media
        {
            Name = file.FileName,
            Path = relativePath,
            Size = file.Length,
            Extension = Path.GetExtension(file.FileName),
            IsDirectory = false
        };

        _mediaRepository.Save(media);

        return new MediaResponse
        {
            Id = media.Id,
            Name = media.Name,
            Path = media.Path,
            Size = media.Size,
            IsDirectory = media.IsDirectory,
            LastModified = media.CreatedAt
        };
    }

    public Task<Stream> DownloadFileAsync(string filePath)
    {
        var fullPath = Path.Combine(_rootPath, filePath);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }

        return Task.FromResult<Stream>(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
    }

    public Task DeleteFileAsync(string filePath)
    {
        var fullPath = Path.Combine(_rootPath, filePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        return Task.CompletedTask;
    }

    public Task<List<MediaResponse>> ListFilesAsync(string path = "")
    {
        var targetDir = Path.Combine(_rootPath, path);
        if (!Directory.Exists(targetDir))
        {
            return Task.FromResult(new List<MediaResponse>());
        }

        var items = new DirectoryInfo(targetDir).GetFileSystemInfos()
            .Select(info => new MediaResponse
            {
                Name = info.Name,
                Path = Path.Combine(path, info.Name).Replace("\\", "/"),
                Size = (info is FileInfo f) ? f.Length : 0,
                IsDirectory = info is DirectoryInfo,
                LastModified = info.LastWriteTime
            })
            .ToList();

        return Task.FromResult(items);
    }

    public Task CreateDirectoryAsync(string path)
    {
        var fullPath = Path.Combine(_rootPath, path);
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }
        return Task.CompletedTask;
    }

    public Task DeleteDirectoryAsync(string path)
    {
        var fullPath = Path.Combine(_rootPath, path);
        if (Directory.Exists(fullPath))
        {
            Directory.Delete(fullPath, true);
        }
        return Task.CompletedTask;
    }
}
