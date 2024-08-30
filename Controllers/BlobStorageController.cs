using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

public class BlobStorageController : Controller
{
    private readonly BlobStorageService _blobStorageService;

    public BlobStorageController(BlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    public async Task<IActionResult> Index()
    {
        var blobs = await _blobStorageService.ListBlobsAsync();
        return View(blobs);
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
            await _blobStorageService.UploadBlobAsync(file.FileName, stream);
            return RedirectToAction(nameof(Index));
        }
        return View();
    }

    public async Task<IActionResult> Download(string blobName)
    {
        var stream = await _blobStorageService.DownloadBlobAsync(blobName);
        return File(stream, "application/octet-stream", blobName);
    }

    public async Task<IActionResult> Delete(string blobName)
    {
        await _blobStorageService.DeleteBlobAsync(blobName);
        return RedirectToAction(nameof(Index));
    }
}
