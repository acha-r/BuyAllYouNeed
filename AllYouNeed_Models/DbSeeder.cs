using AllYouNeed_Models.Models;
using Microsoft.AspNetCore.Builder;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AllYouNeed_Models
{
    public class DbSeeder
    {
        private readonly IMongoCollection<Product> _product;
        public DbSeeder(IMongoDatabase db)
        {
            _product = db.GetCollection<Product>("Products");
        }
        public async Task SeedData()
        {
            var data = new List<Product>
            {
                new Product
                {
                    Name = "Carton",
                    Price = 500,
                    Quantity = 200
                },
                new Product
                {
                    Name = "Cellotape",
                    Price = 250,
                    Quantity = 90
                },
                new Product
                {
                    Name = "Balloon",
                    Price = 50,
                    Quantity = 230
                },
                new Product
                {
                    Name = "Life",
                    Price = 220000,
                    Quantity = 5
                }
            };
            await _product.InsertManyAsync(data);
        }
       
    }
}
