using Microsoft.EntityFrameworkCore;

namespace ExampleORM;

public class ShopApp : DbContext
{
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;port=3306;database=ShopApp;user=root;password=***********;",
            new MySqlServerVersion(new Version(8, 0, 2)));
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // One to Many Relation
        modelBuilder.Entity<Seller>()
            .HasMany(s => s.Products)
            .WithOne(p => p.Seller)
            .HasForeignKey(p => p.SellerId);

        modelBuilder.Entity<Buyer>()
            .HasMany(b => b.Orders)
            .WithOne(o => o.Buyer)
            .HasForeignKey(o => o.BuyerId);

        modelBuilder.Entity<Seller>()
            .HasMany(s => s.Orders)
            .WithOne(o => o.Seller)
            .HasForeignKey(o => o.SellerId);

        // Many to Many Relation
        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderId, op.ProductId });

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderId);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductId);

    }
}
public class Seller
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public List<Product> Products { get; set; } // Bir tane satıcının birden fazla ürünü olabilir. One to Many
    public List<Order> Orders { get; set; } // Bir tane satıcının birden fazla siparişi olabilir. One to Many
}
public class Buyer
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public List<Order> Orders { get; set; } // Buyer'ın bir sürü order'ı olabilir.
}
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int SellerId { get; set; } // Ürünün hangi satıcıya ait olduğunu gösterir
    public Seller Seller { get; set; } // İlgili Seller
    public List<OrderProduct> OrderProducts { get; set; }
}
public class Order
{
    public int Id { get; set; }
    public DateTime OrderTime { get; set; } // ikinci migrationda ismi düzeltildi.
    public int BuyerId { get; set; } // Siparişin hangi alıcıya ait olduğunu gösterir
    public Buyer Buyer { get; set; } // İlgili Buyer
    public int SellerId { get; set; } // Siparişin hangi satıcıya ait olduğunu gösterir
    public Seller Seller { get; set; } // İlgili Seller
    public List<OrderProduct> OrderProducts { get; set; }
}
public class OrderProduct
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
}
class Program
{
    static void Main(string[] args)
    {
        
    }
    static void AddOrderAndRelation()
    {
        using (var db = new ShopApp())
        {
            var buyers = db.Buyers.ToList();
            var sellers = db.Sellers.ToList();
            var products = db.Products.ToList();

            var orders = new List<Order>
            {
                new Order
                    {
                        OrderTime = DateTime.Now,
                        BuyerId = buyers[0].Id,
                        SellerId = sellers[0].Id,
                        OrderProducts = new List<OrderProduct>
                        {
                            new OrderProduct { ProductId = products[0].Id },
                            new OrderProduct { ProductId = products[1].Id },
                            new OrderProduct { ProductId = products[2].Id },
                            new OrderProduct { ProductId = products[3].Id },
                            new OrderProduct { ProductId = products[4].Id }
                        }
                    },

                new Order
                    {
                        OrderTime = DateTime.Now,
                        BuyerId = buyers[1].Id,
                        SellerId = sellers[1].Id,
                        OrderProducts = new List<OrderProduct>
                        {
                            new OrderProduct { ProductId = products[5].Id },
                            new OrderProduct { ProductId = products[6].Id },
                            new OrderProduct { ProductId = products[7].Id },
                            new OrderProduct { ProductId = products[8].Id },
                            new OrderProduct { ProductId = products[9].Id }
                        }
                    },
                new Order
                    {
                        OrderTime = DateTime.Now,
                        BuyerId = buyers[2].Id,
                        SellerId = sellers[2].Id,
                        OrderProducts = new List<OrderProduct>
                        {
                            new OrderProduct { ProductId = products[10].Id },
                            new OrderProduct { ProductId = products[11].Id },
                            new OrderProduct { ProductId = products[12].Id },
                            new OrderProduct { ProductId = products[13].Id },
                            new OrderProduct { ProductId = products[14].Id }
                        }
                    }
            };

            db.Orders.AddRange(orders);
            db.SaveChanges();
            Console.WriteLine("Siparişler eklendi.");
        }
    }
    static void AddProducts()
    {
        using (var db = new ShopApp())
        {
            var products = new List<Product>
            {
                new Product { Name = "Ürün 1", Price = 10, SellerId = 1},
                new Product { Name = "Ürün 2", Price = 11, SellerId = 1},
                new Product { Name = "Ürün 3", Price = 12, SellerId = 1},
                new Product { Name = "Ürün 4", Price = 13, SellerId = 1},
                new Product { Name = "Ürün 5", Price = 14, SellerId = 1},
                new Product { Name = "Ürün 6", Price = 20, SellerId = 2},
                new Product { Name = "Ürün 7", Price = 21, SellerId = 2},
                new Product { Name = "Ürün 8", Price = 22, SellerId = 2},
                new Product { Name = "Ürün 9", Price = 23, SellerId = 2},
                new Product { Name = "Ürün 10", Price = 30, SellerId = 3},
                new Product { Name = "Ürün 11", Price = 35, SellerId = 3},
                new Product { Name = "Ürün 12", Price = 39, SellerId = 3},
                new Product { Name = "Ürün 13", Price = 40, SellerId = 4},
                new Product { Name = "Ürün 14", Price = 45, SellerId = 4},
                new Product { Name = "Ürün 15", Price = 50, SellerId = 5}
            };
            db.Products.AddRange(products);
            db.SaveChanges();
            Console.WriteLine("Eklendi");

        }
    }
    static void AddBuyers()
    {
        using (var db = new ShopApp())
        {
            var buyers = new List<Buyer>
            {
                new Buyer { FullName="Alıcı 1"},
                new Buyer { FullName="Alıcı 2"},
                new Buyer { FullName="Alıcı 3"},
                new Buyer { FullName="Alıcı 4"},
                new Buyer { FullName="Alıcı 5"}

            };
            db.Buyers.AddRange(buyers);
            db.SaveChanges();
            Console.WriteLine("Eklendi");

        }
    }
    static void AddSellers()
    {
        using (var db = new ShopApp())
        {
            var sellers = new List<Seller>
            {
                new Seller { CompanyName="Satıcı 1"},
                new Seller { CompanyName="Satıcı 2"},
                new Seller { CompanyName="Satıcı 3"},
                new Seller { CompanyName="Satıcı 4"},
                new Seller { CompanyName="Satıcı 5"}
            };
            db.Sellers.AddRange(sellers);
            db.SaveChanges();

        }
    }
}
