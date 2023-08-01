using Microsoft.EntityFrameworkCore;
using P137Pronia.DataAccess;
using P137Pronia.Models;
using P137Pronia.Services.Interfaces;

namespace P137Pronia.Services.Implements;

public class CategoryService : ICategoryService
{
    readonly ProniaDbContext _context;

    public CategoryService(ProniaDbContext context)
    {
        _context = context;
    }
    public IQueryable<Category> GetTable => _context.Set<Category>();
    public async Task Create(string name)
    {
        if (name == null) throw new ArgumentNullException();
        if (await _context.Categories.AnyAsync(c => c.Name == name))
            throw new Exception();
        await _context.Categories.AddAsync(new Category() { Name = name });
        await _context.SaveChangesAsync();
    }

    public Task Delete(int? id)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Category>> GetAll()
        => await _context.Categories.ToListAsync();

    public Task<Category> GetById(int? id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsAllExist(List<int> ids)
    {
        foreach (var id in ids)
        {
            if (!await IsExist(id))
                return false;
        }
        return true;
    }

    public async Task<bool> IsExist(int id)
        => await _context.Categories.AnyAsync(c=> c.Id == id);
    public Task Update(int? id, string name)
    {
        throw new NotImplementedException();
    }
}
