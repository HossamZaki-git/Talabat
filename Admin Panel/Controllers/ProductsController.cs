using Admin_Panel.Models.ProductsVMs;
using Admin_Panel.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Climate;
using Talabat.Core;
using Talabat.Core.Domain_Models;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Repository;
using Product = Talabat.Core.Domain_Models.Product;
namespace Admin_Panel.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var productsRepo = unitOfWork.Repository<Product>();
            var products = await productsRepo.GetAllAsync_withSpecification(new ProductSpecification(null, useIncludes: true));
            await unitOfWork.DisposeAsync();
            List<ProductVM> productsvms = new List<ProductVM>();
            foreach (var product in products)
                productsvms.Add(new ProductVM
                {
                    ID = product.ID,
                    Name = product.Name,
                    PictureURL = product.PictureURL,
                    BrandName = product.brand.Name,
                    CategoryName = product.category.Name,
                    Price = product.Price
                });
            return View(productsvms);
        }

        public async Task<IActionResult> EditProduct(int ID)
        {
            var privilegeCheckResult = await PrivilegesChecker.CheckAsync(HttpContext, Privilege.Products_EditDelete);
            if (!privilegeCheckResult.isAllowed)
            {
                TempData["Message"] = privilegeCheckResult.Message;
                TempData["MessageColor"] = "danger";
                return RedirectToAction("Index");
            }
            var product = await unitOfWork.Repository<Product>().GetFirstAsync_withSpecification(new ProductSpecification(P => P.ID == ID));
            if (product is null)
                return RedirectToAction("Index");
            return View(new ProductEditVM
            {
                ID = product.ID,
                Name = product.Name,
                PictureURL = product.PictureURL,
                BrandName = product.brand.Name,
                CategoryName = product.category.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryID = product.category.ID,
                BrandID = product.brand.ID
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductEditVM editVM)
        {
            if (!ModelState.IsValid)
                return View(editVM);

            var productsRepo = unitOfWork.Repository<Product>();
            var product = await productsRepo.GetFirstAsync_withSpecification(new ProductSpecification(P => P.ID == editVM.ID, false));
            product.Name = editVM.Name;
            product.Price = editVM.Price;
            product.BrandID = editVM.BrandID.Value;
            product.CategoryID = editVM.CategoryID.Value;
            product.Description = editVM.Description;

            productsRepo.Update(product);

            // Handling the image
            if (editVM.ImageFile is not null)
            {
                string newImagePath = await Utilities.FilesHandler.SaveProductImageAsync(productID: editVM.ID, ProductName: editVM.Name, img: editVM.ImageFile); Utilities.FilesHandler.SaveProductImageAsync(productID: editVM.ID, ProductName: editVM.Name, img: editVM.ImageFile);
                product.PictureURL = newImagePath;
            }

            bool successFlag = (await unitOfWork.CompleteAsync()) != 0;

            TempData["Message"] = successFlag ? "✅ Product edited successfully!" : "❌ A problem occurred while editing the product";
            TempData["MessageColor"] = successFlag ? "success" : "danger";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateAsync()
        {
            var privilegeCheckResult = await PrivilegesChecker.CheckAsync(HttpContext, Privilege.ProductsCreation);
            if (!privilegeCheckResult.isAllowed)
            {
                TempData["Message"] = privilegeCheckResult.Message;
                TempData["MessageColor"] = "danger";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreationVM productVM)
        {
            if(!ModelState.IsValid)
                return View(productVM);
            try
            {
                productVM.PictureURL = "";
                Product product = productVM;
                await unitOfWork.Repository<Product>().AddAsync(product);
                // Saving the changes to load the product.ID value with the identity value assigned to it in the db
                await unitOfWork.CompleteAsync();
                product.PictureURL = await FilesHandler.SaveProductImageAsync(product.ID, product.Name, productVM.ImageFile);
                await unitOfWork.CompleteAsync();
                TempData["Message"] = "✅ Product edited successfully!";
                TempData["MessageColor"] = "success";

            }
            catch (Exception ex)
            {
                TempData["Message"] = "❌ A problem occurred while editing the product";
                TempData["MessageColor"] = "danger";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProduct(int ID)
        {
            var privilegeCheckResult = await PrivilegesChecker.CheckAsync(HttpContext, Privilege.Products_EditDelete);
            if (!privilegeCheckResult.isAllowed)
            {
                TempData["Message"] = privilegeCheckResult.Message;
                TempData["MessageColor"] = "danger";
                return RedirectToAction("Index");
            }
            var productsRepo = unitOfWork.Repository<Product>();
            var product = await productsRepo.GetByIDAsync(ID);
            if (product is null)
            {
                TempData["Message"] = "❌ That product wasn't found";
                TempData["MessageColor"] = "danger";
            }
            else
            {
                productsRepo.Delete(product);
                await unitOfWork.CompleteAsync();
                TempData["Message"] = "✅ Deleted successfully";
                TempData["MessageColor"] = "success";
            }
            return RedirectToAction("Index");
        }
    }
}
