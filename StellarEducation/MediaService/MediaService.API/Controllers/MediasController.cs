using MediaService.Application.DTOs;
using MediaService.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IO;

namespace MediaService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediasController : ControllerBase
{
    private readonly IMediaService _mediaService;

    public MediasController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromQuery] string subFolder = "")
    {
        var result = await _mediaService.UploadFileAsync(file, subFolder);
        return Ok(result);
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] string filePath)
    {
        var stream = await _mediaService.DownloadFileAsync(filePath);
        return File(stream, "application/octet-stream", Path.GetFileName(filePath));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string filePath)
    {
        await _mediaService.DeleteFileAsync(filePath);
        return Ok(new { Message = "File deleted successfully" });
    }

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] string path = "")
    {
        var items = await _mediaService.ListFilesAsync(path);
        return Ok(items);
    }

    [HttpPost("directory")]
    public async Task<IActionResult> CreateDirectory([FromQuery] string path)
    {
        await _mediaService.CreateDirectoryAsync(path);
        return Ok(new { Message = "Directory created successfully" });
    }

    [HttpDelete("directory")]
    public async Task<IActionResult> DeleteDirectory([FromQuery] string path)
    {
        await _mediaService.DeleteDirectoryAsync(path);
        return Ok(new { Message = "Directory deleted successfully" });
    }
}
