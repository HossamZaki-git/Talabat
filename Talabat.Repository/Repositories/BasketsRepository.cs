using Microsoft.AspNetCore.Http.Features;
using StackExchange.Redis;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Domain_Models;
using Talabat.Core.Domain_Models.Order_Module;
using Talabat.Core.GenericRepository;
using Product = Talabat.Core.Domain_Models.Product;

namespace Talabat.Repository.Repositories
{
    public class BasketsRepository : IBasketsRepository
    {
        IDatabase database;
        private readonly IUnitOfWork unitOfWork;

        // This IConnectionMultiplexer instance represents the connection with the redis server
        public BasketsRepository(IConnectionMultiplexer connectionMultiplexer, IUnitOfWork unitOfWork)
        {
            // IConnectionMultiplexer.GetDatabase() => returns an instance of type IDatabase which represents the DB
            database = connectionMultiplexer.GetDatabase();
            this.unitOfWork = unitOfWork;
        }
        public async Task<Basket?> Create_UpdateAsync(Basket basket)
        {
            #region My Code for correcting the items attributes
            List<BasketItem> BasketItems = new List<BasketItem>();
            if (basket.Items is not null)
                foreach (var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product>().GetFirstAsync_withSpecification(new ProductSpecification(P => P.ID == item.ID));
                    if (product is null)
                        continue;

                    // Correcting the items attributes
                    item.Name = product.Name;
                    item.Price = product.Price;
                    item.PictureURL = product.PictureURL;
                    item.Category = product.category?.Name ?? "";
                    item.Brand = product.brand?.Name ?? "";

                    BasketItems.Add(item);
                }
            basket.Items = BasketItems;
            basket.ShippingCost = basket.DeliveryMethodID is not null ?
                (await unitOfWork.Repository<DeliveryMethod>().GetByIDAsync(basket.DeliveryMethodID.Value))?.Cost ?? 0  : 0;
            #endregion

            // IDatabase.StringSetAsync("key",value as JSON, TimeSpan) => If that key already exists its value & spanning time will be overridden
            //                                                            while if it doesn't exist it will be created
            bool succeeded = await database.StringSetAsync(basket.ID, JsonSerializer.Serialize(basket), TimeSpan.FromDays(1));
            return succeeded ? (await GetBasketAsync(basket.ID)) : null;
        }

        public async Task<Basket?> GetBasketAsync(string ID)
        {
            var basket = await database.StringGetAsync(ID);
            if (basket.IsNull)
                return null;
            // IDatabase.StringGetAsync("key") => returns the key's corresponding value (as JSON) if the key exists or null if it's not found
            return JsonSerializer.Deserialize<Basket>(basket);
        }

        public async Task<bool> DeleteBasketAsync(string ID)
            => await database.KeyDeleteAsync(ID); // IDatabase.KeyDeleteAsync("key") => deletes the pair if the key exists and returns a bool indicating
                                                  // the status of the process
    }
}
