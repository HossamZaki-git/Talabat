using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models;
using Talabat.Core.Domain_Models.Order_Module;

namespace Talabat.Repository.Data
{
    public static class DataSeeder
    {
        public static void Seed(StoreContext context)
        {
            if (context is null)
                return;

            string rootPath = "../Talabat.Repository/Data/Data to seed";

            // products, categories & brands seeding
            if(!context.Products.Any())
            {
                var brands = JsonSerializer.Deserialize<List<Brand>>(File.ReadAllText($"{rootPath}/brands.json"))?.Select(B =>
                {
                    B.ID = 0;
                    return B;
                });
                var categories = JsonSerializer.Deserialize<List<Category>>(File.ReadAllText($"{rootPath}/types.json"))?.Select(C =>
                {
                    C.ID = 0;
                    return C;
                });
                var products = JsonSerializer.Deserialize<List<Product>>(File.ReadAllText($"{rootPath}/products.json"));

                if (brands is not null)
                    foreach (var brand in brands)
                        context.Add(brand);

                if (categories is not null)
                    foreach (var category in categories)
                        context.Add(category);

                context.SaveChanges();

                if (products is not null)
                    foreach (var product in products)
                        context.Add(product);
            }
            
            // DeliveryMethods data seeding
            if(!context.DeliveryMethods.Any())
            {
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(File.ReadAllText($"{rootPath}/delivery.json"));

                foreach (var DM in DeliveryMethods)
                    context.DeliveryMethods.Add(DM);
            }

            context.SaveChanges();
        }
    }
}
