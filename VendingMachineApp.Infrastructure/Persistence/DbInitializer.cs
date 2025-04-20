using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VendingMachineApp.Core.Entities;

namespace VendingMachineApp.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new InventoryContext(
                serviceProvider.GetRequiredService<DbContextOptions<InventoryContext>>()))
            {
                if (context.Drinks.Any())
                {
                    return;
                }

                var brandCoca = new Brand { Name = "Coca-Cola" };
                var brandPepsi = new Brand { Name = "Pepsi" };
                var brandFanta = new Brand { Name = "Fanta" };
                var brandSprite = new Brand { Name = "Sprite" };

                var brands = new List<Brand>
                {
                    brandCoca, brandPepsi, brandFanta, brandSprite

                };

                context.Brands.AddRange(brands);
                context.SaveChanges();

                var drinks = new List<Drink>
                {
                    new Drink
                    {
                        Name = "Coca-Cola Classic",
                        Price = 53,
                        Quantity = 50,
                        ImageUrl = "https://aws.kiiiosk.store/uploads/shop/9065/uploads/product_image/image/819573/302c122b76ec379776e40b41a95da77320240816-971369-fb7qxp.png",
                        BrandId = brandCoca.Id
                    },
                    new Drink
                    {
                        Name = "Coca-Cola Zero",
                        Price = 74,
                        Quantity = 50,
                        ImageUrl = "https://mosnapitki.ru/upload/iblock/224/fu9o9crbubuv6r4g62q18x3q8k164w6m.jpg",
                        BrandId = brandCoca.Id
                    },
                    new Drink
                    {
                        Name = "Diet Coke",
                        Price = 41,
                        Quantity = 50,
                        ImageUrl = "https://mosnapitki.ru/upload/iblock/f31/r23qq8t53pqxf7rrx92wsbjyrnhb3354.jpg",
                        BrandId = brandCoca.Id
                    },
                    new Drink
                    {
                        Name = "Pepsi",
                        Price = 64,
                        Quantity = 50,
                        ImageUrl = "https://t4.ftcdn.net/jpg/03/03/98/01/360_F_303980198_G2TzNSk73Av3YIQ0W7EmoILxOaKDSrXZ.jpg",
                        BrandId = brandPepsi.Id
                    },
                    new Drink
                    {
                        Name = "Pepsi Max",
                        Price = 51,
                        Quantity = 50,
                        ImageUrl = "https://napitkistore.ru/wp-content/uploads/2024/08/pepsi-max-zero-bez-sahara-330.webp",
                        BrandId = brandPepsi.Id
                    },

                    new Drink
                    {
                        Name = "Fanta Orange",
                        Price = 85,
                        Quantity = 50,
                        ImageUrl = "https://s2.wine.style/images_gen/207/207752/0_0_695x600.webp",
                        BrandId = brandFanta.Id
                    },
                    new Drink
                    {
                        Name = "Sprite",
                        Price = 93,
                        Quantity = 50,
                        ImageUrl = "https://www.cebooze.com/app/uploads/2020/10/spritecan.jpg",
                        BrandId = brandSprite.Id
                    },
                    new Drink
                    {
                        Name = "Sprite Cherry",
                        Price = 93,
                        Quantity = 50,
                        ImageUrl = "https://s2.wine.style/images_gen/906/90636/0_0_695x600.webp",
                        BrandId = brandSprite.Id
                    },

                };
                if (!context.Coins.Any())
                {
                    var coins = new List<Coin>
                    {
                        new Coin { Nominal = 1, Quantity = 100 },
                        new Coin { Nominal = 2, Quantity = 100 },
                        new Coin { Nominal = 5, Quantity = 100 },
                        new Coin { Nominal = 10, Quantity = 100 }
                    };
                    context.Coins.AddRange(coins);
                    context.SaveChanges();
                }

                context.Drinks.AddRange(drinks);
                context.SaveChanges();
            }
        }
    }
}
