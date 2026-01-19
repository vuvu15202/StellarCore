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
    public async Task<IActionResult> Upload([FromForm] UploadFileRequest request, [FromQuery] Guid? parentId = null)
    {
        var result = await _fileService.UploadFileAsync(request.File, parentId);
        return Ok(result);
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] Guid mediaId)
    {
        var result = await _fileService.DownloadFileAsync(mediaId);
        return File(result.Stream, result.ContentType, result.FileName);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] Guid mediaId)
    {
        await _fileService.DeleteFileAsync(mediaId);
        return Ok(new { Message = "File deleted successfully" });
    }

    [HttpGet("list")]
    public async Task<IActionResult> List([FromQuery] Guid? parentId = null)
    {
        var items = await _fileService.ListFilesAsync(parentId);
        return Ok(items);
    }

    [HttpGet("tree")]
    public async Task<IActionResult> GetTree([FromQuery] Guid? parentId = null)
    {
        var items = await _fileService.GetMediaTreeAsync(parentId);
        return Ok(items);
    }

    [HttpPost("directory")]
    public async Task<IActionResult> CreateDirectory([FromQuery] string name, [FromQuery] Guid? parentId)
    {
        await _fileService.CreateDirectoryAsync(name, parentId);
        return Ok(new { Message = "Directory created successfully" });
    }

    [HttpDelete("directory")]
    public async Task<IActionResult> DeleteDirectory([FromQuery] Guid mediaId)
    {
        await _fileService.DeleteDirectoryAsync(mediaId);
        return Ok(new { Message = "Directory deleted successfully" });
    }

    public class UploadFileRequest
    {
        public IFormFile File { get; set; }
    }

}
