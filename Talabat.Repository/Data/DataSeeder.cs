using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Domain_Models;
using Talabat.Core.Domain_Models.Identity;
using Talabat.Core.Domain_Models.Order_Module;
using Talabat.Repository.Identity;
using File = System.IO.File;
using Product = Talabat.Core.Domain_Models.Product;

namespace Talabat.Repository.Data
{
    public static class DataSeeder
    {
        static string rootPath = "../Talabat.Repository/Data/Data to seed";
        public static void RawSQLDataSeeding(StoreContext context)
        {
            if(!context.Products.Any())
            {
                var brands = JsonSerializer.Deserialize<List<Brand>>(File.ReadAllText($"{rootPath}/brands.json"));
                var categories = JsonSerializer.Deserialize<List<Category>>(File.ReadAllText($"{rootPath}/types.json"));
                StringBuilder Query = new StringBuilder();
                Query.Append(@"INSERT INTO Brands (Name) VALUES");
                for (int i = 0; i < brands?.Count; i++)
                {
                    Query.Append($@"('{brands[i].Name}')");
                    if (i != brands.Count - 1)
                        Query.Append(',');
                }

                Query.Append('\n');
                Query.Append(@"INSERT INTO Categories (Name) VALUES");
                for (int i = 0; i < categories?.Count; i++)
                {
                    Query.Append($@"('{categories[i].Name}')");
                    if (i != categories.Count - 1)
                        Query.Append(',');
                }
                context.Database.ExecuteSqlRaw(Query.ToString());

                var products = JsonSerializer.Deserialize<List<Product>>(File.ReadAllText($"{rootPath}/products.json"));

                if (products is not null)
                    foreach (var product in products)
                        context.Add(product);
            }

            // DeliveryMethods data seeding
            if (!context.DeliveryMethods.Any())
            {
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(File.ReadAllText($"{rootPath}/delivery.json"));

                foreach (var DM in DeliveryMethods)
                    context.DeliveryMethods.Add(DM);
            }

            context.SaveChanges();


        }
        public static void Seed(StoreContext context)
        {
            if (context is null)
                return;

            // products, categories & brands seeding
            if(!context.Products.Any())
            {
                var brands = JsonSerializer.Deserialize<List<Brand>>(File.ReadAllText($"{rootPath}/brands.json"));
                var categories = JsonSerializer.Deserialize<List<Category>>(File.ReadAllText($"{rootPath}/types.json"));
                if (brands is not null)
                    foreach (var brand in brands)
                        context.Add(brand);

                if (categories is not null)
                    foreach (var category in categories)
                        context.Add(category);

                context.SaveChanges();

                var products = JsonSerializer.Deserialize<List<Product>>(File.ReadAllText($"{rootPath}/products.json"));

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
