using P137Pronia.Models;
using P137Pronia.ViewModels.SliderVMs;

namespace P137Pronia.Services.Interfaces;

public interface ISliderService
{
    Task Create(CreateSliderVM sliderVM);
    Task Update(UpdateSliderVM sliderVM);
    Task Delete(int? id);
    Task<ICollection<Slider>> GetAll();
    Task<Slider> GetById(int? id);
}
