using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using MyStore.Entities;
using MyStore.Models;
using MyStore.Repositories;
using System.Linq.Expressions;

namespace MyStore.Services
{
    public class ProductService
    {
        GenericRepository<Category> _categoryRepository;
        GenericRepository<Product> _productRepository;
        IWebHostEnvironment _webHostEnvironment;

        public ProductService(GenericRepository<Category> categoryRepository, GenericRepository<Product> productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IEnumerable<ProductVM>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync(
                includes: new Expression<Func<Product, object>>[] { p => p.Category! }
               );

            var productsVM = products.Select(p =>
                new ProductVM
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageName = p.ImageName,
                    Category = new CategoryVM
                    {
                        CategoryId = p.Category!.CategoryId,
                        Name = p.Category.Name
                    }
                }
            ).ToList();

            return productsVM;

        }


        public async Task<ProductVM> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            var categories = await _categoryRepository.GetAllAsync();

            var productVW = new ProductVM();
            if (product != null)
            {
                productVW = new ProductVM()
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Stock = product.Stock,
                    ImageName = product.ImageName,
                    Category = new CategoryVM
                    {
                        CategoryId = product.Category!.CategoryId,
                        Name = product.Category.Name
                    }
                };
            }

            productVW.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList();

            return productVW;

        }

        public async Task AddAsync(ProductVM entity)
        {
            if (entity.ImageFile != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(entity.ImageFile.FileName);
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await entity.ImageFile.CopyToAsync(fileStream);
                }
                var product = new Product
                {
                    Name = entity.Name,
                    Description = entity.Description,
                    Price = entity.Price,
                    Stock = entity.Stock,
                    ImageName = fileName,
                    CategoryId = entity.Category.CategoryId
                };
                await _productRepository.AddAsync(product);
            }
        }

        public async Task EditAsync(ProductVM entity)
        {
            var product = await _productRepository.GetByIdAsync(entity.ProductId);
            if (entity.ImageFile != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(entity.ImageFile.FileName);
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await entity.ImageFile.CopyToAsync(fileStream);
                }

                if (!product.ImageName.IsNullOrEmpty())
                {
                    var previusImage = product.ImageName;
                    string previusImagePath = Path.Combine(wwwRootPath + "/images/", previusImage);

                    if (File.Exists(previusImage)) System.IO.File.Delete(previusImagePath);
                }
                entity.ImageName = fileName;
            }
            else
            {
                entity.ImageName = product.ImageName;
            }

            product.Name = entity.Name;
            product.Description = entity.Description;
            product.Price = entity.Price;
            product.Stock = entity.Stock;
            product.ImageName = entity.ImageName;
            product.CategoryId = entity.Category.CategoryId;


            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product.ImageName != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                var previusImage = product.ImageName;
                string previusImagePath = Path.Combine(wwwRootPath + "/images/", previusImage);

                if (File.Exists(previusImage))
                {
                    System.IO.File.Delete(previusImagePath);
                }
            }
            await _productRepository.DeleteAsync(product!);
        }

        public async Task<IEnumerable<ProductVM>> GetCatalogAsync(int categoryId = 0, string search = "")
        {
            var conditions = new List<Expression<Func<Product, bool>>>()
            {
                x => x.Stock > 0
            };

            if (categoryId != 0) conditions.Add(x => x.CategoryId == categoryId);
            if (!string.IsNullOrEmpty(search)) conditions.Add(x => x.Name.Contains(search) || x.Description.Contains(search));

            var products = await _productRepository.GetAllAsync(
                conditions: conditions.ToArray()
               );

            var productsVM = products.Select(p =>
                new ProductVM
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageName = p.ImageName,
                }
            ).ToList();

            return productsVM;

        }


    }
}
