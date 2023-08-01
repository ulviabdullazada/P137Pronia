using Microsoft.EntityFrameworkCore;
using P137Pronia.DataAccess;
using P137Pronia.ExtensionServices.Interfaces;
using P137Pronia.Models;
using P137Pronia.Services.Interfaces;
using P137Pronia.ViewModels.ProductVMs;

namespace P137Pronia.Services.Implements;

public class ProductService : IProductService
{
	readonly ProniaDbContext _context;
	readonly IFileService _fileService;
	readonly ICategoryService _catService;

    public ProductService(ProniaDbContext context,
        IFileService fileService,
        ICategoryService catService)
    {
        _context = context;
        _fileService = fileService;
        _catService = catService;
    }

    public IQueryable<Product> GetTable { get => _context.Set<Product>(); }

    public async Task Create(CreateProductVM productVM)
	{
        if (productVM.CategoryIds.Count > 4)
			throw new Exception();
		if (!await _catService.IsAllExist(productVM.CategoryIds))
			throw new ArgumentException();
		List<ProductCategory> prodCategories = new List<ProductCategory>();
		foreach (var id in productVM.CategoryIds)
		{
			prodCategories.Add(new ProductCategory
			{
				CategoryId = id
			});
        }
		Product entity = new Product()
		{
			Name = productVM.Name,
			Description = productVM.Description,
			Discount = productVM.Discount,
			Price = productVM.Price,
			Rating = productVM.Rating,
			StockCount = productVM.StockCount,
			MainImage = await _fileService.UploadAsync(productVM.MainImageFile, Path.Combine("assets", "imgs", "products")),
			ProductCategories = prodCategories
		};
        if (productVM.ImageFiles != null)
        {
			List<ProductImage> imgs = new();
			foreach (var file in productVM.ImageFiles)
			{
				string fileName = await _fileService.UploadAsync(file, Path.Combine("assets", "imgs", "products"));
				
                imgs.Add(new ProductImage
                {
                    Name = fileName,
                });
			}
			entity.ProductImages = imgs;
        }
        if (productVM.HoverImageFile != null)
			entity.HoverImage = await _fileService.UploadAsync(productVM.HoverImageFile, Path.Combine("assets", "imgs", "products"));
		await _context.AddAsync(entity);
		await _context.SaveChangesAsync();
    }

	public async Task Delete(int? id)
		{
		var entity = await GetById(id,true);
		_context.Remove(entity);
		_fileService.Delete(entity.MainImage);
        if (entity.HoverImage != null)
        {
			_fileService.Delete(entity.HoverImage);
        }
        if (entity.ProductImages != null)
        {
			foreach (var item in entity.ProductImages)
			{
				_fileService.Delete(item.Name);
			}
        }
        await _context.SaveChangesAsync();
	}

    public async Task DeleteImage(int? id)
    {
		if (id == null || id <= 0) throw new ArgumentException();
		var entity = await _context.ProductImages.FindAsync(id);
		if (entity == null) throw new NullReferenceException();
		_fileService.Delete(entity.Name);
		_context.ProductImages.Remove(entity);
		await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetAll(bool takeAll)
	{
        if (takeAll)
        {
			return await _context.Products.ToListAsync();
        }
        return await _context.Products.Where(p=> p.IsDeleted == false).ToListAsync();
	}

	public async Task<Product> GetById(int? id, bool takeAll = false)
	{
		if (id == null || id < 1) throw new ArgumentException();
		Product? entity;
        if (takeAll)
        {
            entity = await _context.Products.FindAsync(id);
        }
		else
		{
			entity = await _context.Products.SingleOrDefaultAsync(p => p.IsDeleted == false && p.Id == id);
		}
		if (entity is null) throw new NullReferenceException();
        return entity;
	}

    public async Task SoftDelete(int? id)
    {
        var entity = await GetById(id, true);
        entity.IsDeleted = !entity.IsDeleted;
        await _context.SaveChangesAsync();
    }

    public async Task Update(int? id, UpdateProductVM vm)
	{
        if (vm.CategoryIds.Count > 4)
            throw new Exception();

        if (!await _catService.IsAllExist(vm.CategoryIds))
            throw new ArgumentException();

		List<ProductCategory> prodCategories = new List<ProductCategory>();

		foreach (var catid in vm.CategoryIds)
        {
            prodCategories.Add(new ProductCategory
            {
                CategoryId = catid
            });
        }

        var entity = await _context.Products.Include(p=>p.ProductCategories).SingleOrDefaultAsync(p=>p.Id == id);

        if (entity.ProductCategories != null)
        {
			entity.ProductCategories.Clear();
        }
        entity.Name = vm.Name;
		entity.Description = vm.Description;
		entity.Discount = vm.Discount;
		entity.Price = vm.Price;
		entity.Rating = vm.Rating;
		entity.StockCount = vm.StockCount;
		entity.ProductCategories = prodCategories;
        if (vm.MainImage != null)
        {
			_fileService.Delete(entity.MainImage);
			entity.MainImage = await _fileService.UploadAsync(vm.MainImage, Path.Combine("assets", "imgs", "products"));
        }
		if(vm.HoverImage != null)
		{
            if (entity.HoverImage != null)
            {
	            _fileService.Delete(entity.HoverImage);
            }
			entity.HoverImage = await _fileService.UploadAsync(vm.HoverImage, Path.Combine("assets", "imgs", "products"));
        }
        if (vm.ProductImageFiles != null)
        {
			if (entity.ProductImages == null) entity.ProductImages = new List<ProductImage>();
            foreach (var img in vm.ProductImageFiles)
            {
				ProductImage prodImg = new ProductImage
				{
					Name = await _fileService.UploadAsync(img, Path.Combine("assets", "imgs", "products"))
				};
				entity.ProductImages.Add(prodImg);
            }
        }
		await _context.SaveChangesAsync();
    }
}
