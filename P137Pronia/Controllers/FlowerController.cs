using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P137Pronia.Services.Interfaces;

namespace P137Pronia.Controllers;

public class FlowerController : Controller
{
    readonly IProductService _service;

    public FlowerController(IProductService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null || id <= 0) return BadRequest();

        //var entity = await _service.GetById(id);
        var entity = await _service.GetTable.Include(p => p.ProductImages).SingleOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
        if (entity == null) return NotFound();
        return View(entity);
    }
}
