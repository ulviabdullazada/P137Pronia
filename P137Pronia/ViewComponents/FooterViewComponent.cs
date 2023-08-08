using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P137Pronia.DataAccess;

namespace P137Pronia.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    readonly ProniaDbContext _context;

    public FooterViewComponent(ProniaDbContext context)
    {
        _context = context;
    }
    //public IViewComponentResult Invoke();
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View(await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value));
    }
}
