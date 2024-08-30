using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class QueueStorageController : Controller
{
    private readonly QueueStorageService _queueStorageService;

    public QueueStorageController(QueueStorageService queueStorageService)
    {
        _queueStorageService = queueStorageService;
    }

    public async Task<IActionResult> Index()
    {
        var messages = await _queueStorageService.ReceiveMessagesAsync();
        return View(messages);
    }

    public IActionResult SendMessage()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(string message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            await _queueStorageService.SendMessageAsync(message);
            return RedirectToAction(nameof(Index));
        }
        return View();
    }

    public async Task<IActionResult> DeleteMessage(string messageId, string popReceipt)
    {
        await _queueStorageService.DeleteMessageAsync(messageId, popReceipt);
        return RedirectToAction(nameof(Index));
    }
}
