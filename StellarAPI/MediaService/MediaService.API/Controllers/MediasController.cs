using MediaService.Domain.Models.Values;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IO;
using MediaService.Domain.Services;

namespace MediaService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediasController : ControllerBase
{
    private readonly IFileService _fileService;

    public MediasController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] UploadFileRequest request, [FromQuery] string subFolder = "")
    {
        var result = await _fileService.UploadFileAsync(request.File, subFolder);
        return Ok(result);
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] string filePath)
    {
        var stream = await _fileService.DownloadFileAsync(filePath);
        return File(stream, "application/octet-stream", Path.GetFileName(filePath));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] string filePath)
    {
        await _fileService.DeleteFileAsync(filePath);
        return Ok(new { Message = "File deleted successfully" });
    }

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] string path = "")
    {
        var items = await _fileService.ListFilesAsync(path);
        return Ok(items);
    }

    [HttpPost("directory")]
    public async Task<IActionResult> CreateDirectory([FromQuery] string path)
    {
        await _fileService.CreateDirectoryAsync(path);
        return Ok(new { Message = "Directory created successfully" });
    }

    [HttpDelete("directory")]
    public async Task<IActionResult> DeleteDirectory([FromQuery] string path)
    {
        await _fileService.DeleteDirectoryAsync(path);
        return Ok(new { Message = "Directory deleted successfully" });
    }

    public class UploadFileRequest
    {
        public IFormFile File { get; set; }
    }

}
