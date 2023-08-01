using P137Pronia.Models;

namespace P137Pronia.ViewModels.HomeVMs;

public class HomeVM
{
    public ICollection<Slider> Sliders { get; set; }
    public ICollection<Product> Products { get; set; }
}
