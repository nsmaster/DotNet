using System.Linq;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository repository;
        public int PageSize { get; set; }

        public ProductController(IProductRepository productRepository)
        {
            this.repository = productRepository;

            PageSize = 4;
        }

        public ViewResult List(string category, int page = 1)
        {
            var model = new ProductListViewModal
                        {
                            Products = repository.Products
                                .Where(p => category == null || p.Category == category)
                                .OrderBy(p => p.ProductId)
                                .Skip((page - 1) * PageSize)
                                .Take(PageSize),
                            PagingInfo = new PagingInfo
                                         {
                                             CurrentPage = page,
                                             ItemsPerPage = PageSize,
                                             TotalItems = repository.Products.Count(p => category == null || p.Category == category)
                                         },
                            CurrentCategory = category
                        };

            return View(model);
        }

        public FileContentResult GetImage(int productId)
        {
            Product product = repository.Products.FirstOrDefault(a => a.ProductId == productId);
            if (product != null)
                return File(product.ImageData, product.ImageMimeType);

            return null;
        }
    }
}
