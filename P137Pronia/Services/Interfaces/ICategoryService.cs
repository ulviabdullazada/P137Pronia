using P137Pronia.Models;
using P137Pronia.ViewModels.SliderVMs;

namespace P137Pronia.Services.Interfaces;

public interface ICategoryService
{
	IQueryable<Category> GetTable { get; }
    Task Create(string name);
    Task Update(int? id, string name);
    Task Delete(int? id);
    Task<ICollection<Category>> GetAll();
    Task<Category> GetById(int? id);
    Task<bool> IsExist(int id);
    Task<bool> IsAllExist(List<int> ids);
}
