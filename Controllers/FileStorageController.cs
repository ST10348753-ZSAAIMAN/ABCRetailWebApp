using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

public class FileStorageController : Controller
{
    private readonly FileStorageService _fileStorageService;

    public FileStorageController(FileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public async Task<IActionResult> Index()
    {
        var files = await _fileStorageService.ListFilesAsync();
        return View(files);
    }

    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file != null)
        {
            using var stream = file.OpenReadStream();
            await _fileStorageService.UploadFileAsync(file.FileName, stream);
            return RedirectToAction(nameof(Index));
        }
        return View();
    }

    public async Task<IActionResult> Download(string fileName)
    {
        var stream = await _fileStorageService.DownloadFileAsync(fileName);
        return File(stream, "application/octet-stream", fileName);
    }

    public async Task<IActionResult> Delete(string fileName)
    {
        await _fileStorageService.DeleteFileAsync(fileName);
        return RedirectToAction(nameof(Index));
    }
}
