using MediaService.Domain.Models.Entities;
using MediaService.Domain.Services.Persistence;
using MediaService.Domain.Services;
using MediaService.Infrastructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediaService.Domain.Models.Values;

namespace MediaService.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly string _rootPath;
    private readonly MediaPersistence _mediaRepository;

    public FileService(
        MediaPersistence mediaRepository,
        IOptions<MediaOptions> options)
    {
        _mediaRepository = mediaRepository;
        _rootPath = options.Value.RootPath;

        if (string.IsNullOrWhiteSpace(_rootPath))
        {
            throw new ArgumentException("Media RootPath is not configured.");
        }

        if (!Directory.Exists(_rootPath))
        {
            Directory.CreateDirectory(_rootPath);
        }
    }

    public async Task<MediaResponse> UploadFileAsync(IFormFile file, Guid? parentId = null)
    {
        var fileId = Guid.NewGuid();
        var extension = Path.GetExtension(file.FileName);
        var storageName = $"{fileId}{extension}";
        var fullStoragePath = Path.Combine(_rootPath, storageName);

        // 1. Save physical file
        using (var stream = new FileStream(fullStoragePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // 2. Build Hierarchy
        string hierarchy = fileId.ToString();
        if (parentId.HasValue)
        {
            var parent = await _mediaRepository.FindByIdAsync(parentId.Value);
            if (parent != null)
            {
                hierarchy = $"{parent.Hierarchy}/{fileId}";
            }
            else
            {
                 // Fallback if parent not found, treat as root or throw? 
                 // Treating as root for now to avoid data loss, or we could throw.
                 parentId = null; 
            }
        }

        // 3. Save Entity
        var media = new Media
        {
            Id = fileId,
            Name = file.FileName,
            StoragePath = storageName, // Relative to root
            Size = file.Length,
            Extension = extension,
            IsDirectory = false,
            ParentId = parentId,
            Hierarchy = hierarchy,
            Path = hierarchy // Mapping virtual path to Hierarchy for now
        };

        await _mediaRepository.SaveAsync(media);

        return MapToResponse(media);
    }

    public async Task<FileDownloadResult> DownloadFileAsync(Guid mediaId)
    {
        var media = await _mediaRepository.FindByIdAsync(mediaId);
        if (media == null || media.IsDeleted)
        {
            throw new FileNotFoundException("File not found in database.");
        }

        if (media.IsDirectory)
        {
             throw new InvalidOperationException("Cannot download a directory.");
        }

        var fullPath = Path.Combine(_rootPath, media.StoragePath);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("Physical file not found on server.", fullPath);
        }

        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        
        // Simple mime type detection
        string contentType = "application/octet-stream";
        var ext = media.Extension.ToLower();
        if (ext == ".jpg" || ext == ".jpeg") contentType = "image/jpeg";
        else if (ext == ".png") contentType = "image/png";
        else if (ext == ".pdf") contentType = "application/pdf";
        else if (ext == ".txt") contentType = "text/plain";

        return new FileDownloadResult
        {
            Stream = stream,
            FileName = media.Name,
            ContentType = contentType
        };
    }

    public async Task DeleteFileAsync(Guid mediaId)
    {
        var media = await _mediaRepository.FindByIdAsync(mediaId);
        if (media != null)
        {
            media.IsDeleted = true; 
            // Optional: Recursively delete children if directory
             if (media.IsDirectory)
            {
                var children = await _mediaRepository.FindAllByParentIdAsync(mediaId);
                foreach(var child in children)
                {
                    await DeleteFileAsync(child.Id);
                }
            }
            await _mediaRepository.SaveAsync(media);
            
            // Note: Physical file is kept for now (soft delete). 
        }
    }

    public async Task<List<MediaResponse>> ListFilesAsync(Guid? parentId = null)
    {
        var items = await _mediaRepository.FindAllByParentIdAsync(parentId);
        return items.Select(MapToResponse).ToList();
    }

    public async Task<List<MediaResponse>> GetMediaTreeAsync(Guid? parentId = null)
    {
        var items = await _mediaRepository.FindAllByParentIdAsync(parentId);
        var responses = new List<MediaResponse>();

        foreach (var item in items)
        {
            var response = MapToResponse(item);
            if (item.IsDirectory)
            {
                response.Children = await GetMediaTreeAsync(item.Id);
            }
            responses.Add(response);
        }

        return responses;
    }

    public async Task CreateDirectoryAsync(string name, Guid? parentId = null)
    {
        var dirId = Guid.NewGuid();
        string hierarchy = dirId.ToString();

        if (parentId.HasValue)
        {
            var parent = await _mediaRepository.FindByIdAsync(parentId.Value);
            if (parent != null)
            {
                hierarchy = $"{parent.Hierarchy}/{dirId}";
            }
            else 
            {
                parentId = null;
            }
        }

        var media = new Media
        {
            Id = dirId,
            Name = name,
            IsDirectory = true,
            ParentId = parentId,
            Hierarchy = hierarchy,
            Path = hierarchy,
            StoragePath = string.Empty // No physical file
        };

        await _mediaRepository.SaveAsync(media);
    }

    public Task DeleteDirectoryAsync(Guid mediaId)
    {
        return DeleteFileAsync(mediaId);
    }

    private MediaResponse MapToResponse(Media media)
    {
        return new MediaResponse
        {
            Id = media.Id,
            Name = media.Name,
            Path = media.Hierarchy, // Use Hierarchy as the logical path
            Size = media.Size,
            IsDirectory = media.IsDirectory,
            LastModified = media.CreatedAt
        };
    }
}
