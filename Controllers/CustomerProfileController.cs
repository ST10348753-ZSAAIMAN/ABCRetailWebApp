using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class CustomerProfileController : Controller
{
    private readonly TableStorageService _tableStorageService;

    public CustomerProfileController(TableStorageService tableStorageService)
    {
        _tableStorageService = tableStorageService;
    }

    public async Task<IActionResult> Index()
    {
        var profiles = await _tableStorageService.GetAllCustomerProfilesAsync();
        return View(profiles);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerProfile profile)
    {
        if (ModelState.IsValid)
        {
            await _tableStorageService.InsertCustomerProfileAsync(profile);
            return RedirectToAction(nameof(Index));
        }
        return View(profile);
    }

    public async Task<IActionResult> Delete(string partitionKey, string rowKey)
    {
        await _tableStorageService.DeleteCustomerProfileAsync(partitionKey, rowKey);
        return RedirectToAction(nameof(Index));
    }
}
