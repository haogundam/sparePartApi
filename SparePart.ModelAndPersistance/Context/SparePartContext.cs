using SparePart.ModelAndPersistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace SparePart.ModelAndPersistance.Context
{
    public class SparePartContext : DbContext
    {
        public DbSet<Part> Parts { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<QuotationList> QuotationLists { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Supplier> Suppliers { get; set; } = null!;
        public DbSet<Warehouse> Warehouses { get; set; } = null!;
        public DbSet<Storage> Storages { get; set; } = null!;
        public DbSet<QuotationPart> QuotationParts { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public SparePartContext(DbContextOptions<SparePartContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Part and Category relationship
            modelBuilder.Entity<Part>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Parts)
                .HasForeignKey(p => p.CategoryId);

            // Configure Part and Supplier relationship
            modelBuilder.Entity<Part>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Parts)
                .HasForeignKey(p => p.SupplierId);

            // Configure Part and Storage relationship
            modelBuilder.Entity<Part>()
                .HasMany(p => p.Storages)
                .WithOne(s => s.Part)
                .HasForeignKey(s => s.PartId);

            // Configure Storage and Warehouse relationship
            modelBuilder.Entity<Storage>()
                .HasOne(s => s.Warehouse)
                .WithMany(w => w.Storages)
                .HasForeignKey(s => s.WarehouseId);

            // Configure QuotationList and QuotePart relationship
            modelBuilder.Entity<QuotationList>()
                .HasMany(q => q.QuotationParts)
                .WithOne(qp => qp.QuotationList)
                .HasForeignKey(qp => qp.QuoteNo);

            // Configure Part and QuotePart relationship
            modelBuilder.Entity<Part>()
                .HasMany(p => p.QuotationParts)
                .WithOne(qp => qp.Part)
                .HasForeignKey(qp => qp.PartId);

            // Configure QuotationList and Customer relationship
            modelBuilder.Entity<QuotationList>()
                .HasOne(q => q.Customer)
                .WithMany(c => c.QuotationLists)
                .HasForeignKey(q => q.CustomerId);

            // Configure QuotationList and User relationship
            modelBuilder.Entity<QuotationList>()
                .HasOne(q => q.User) 
                .WithMany(c => c.QuotationLists)
                .HasForeignKey(q => q.UserId);

            modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = 1,
                Email = "rmaict@gmail.com",
                PasswordHash = Convert.FromBase64String("GF8D8bWOBEgaPDB02XvmSlOw9TBcb1mp35knE/IVb/o858Q0Gt/YKP3gEqsKatGzQaHThSXLHN+7JdLcvp+mWQ=="),
                PasswordSalt = Convert.FromBase64String("qVz55B6jTAfoXc0ys+aXll1vs+Ld7+rgQCofm4TWBHbwSeZ1DEiF1dmKPJDEsaUMAHO3P1N7x1EQDpGn1HcxbY273DJlnWw1kW6SPdjqteX61Jbq0hX926PXI8JKBFic2IiJr/CeC2IkvhzQRLxwGyzMbmdbsmrWDuWQHbThskM="),
            }
            );

            modelBuilder.Entity<Supplier>().HasData(
            new Supplier
            {
                SupplierId = 1,
                SupplierName = "ABC Parts Inc.",
                SupplierContact = "1234567890",
                SupplierEmail = "info@abcparts.com",
                SupplierAddress = "456 Oak Avenue"
            },
                 new Supplier
                 {
                     SupplierId = 2,
                     SupplierName = "ABC Parts Inc.",
                     SupplierContact = "1234567890",
                     SupplierEmail = "info@abcparts.com",
                     SupplierAddress = "456 Oak Avenue"
                 }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Engine Parts",
                    Description = "Parts related to vehicle engines."
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Brake System",
                    Description = "Parts related to vehicle brake systems."
                }
            );

            modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                CustomerId = 1,
                CustomerName = "John Doe",
                CustomerContact = "1234567890",
                CustomerEmail = "john.doe@example.com",
                CustomerAddress = "123 Main Street"
            }
        );

            modelBuilder.Entity<Part>()
                .HasData(
                    new Part
                    {
                        PartId = 1,
                        SKU = "ENG123",
                        PartName = "Engine",
                        CategoryId = 1, // Assuming PartCategoryID is an integer
                        SellingPrice = 5000.00,
                        BuyingPrice = 12.00,
                        EntryDate = new DateOnly(2023, 01, 01),
                        SupplierId = 1,
                    },
                    new Part
                    {
                        PartId = 2,
                        SKU = "BP456",
                        PartName = "Brake Pads",
                        CategoryId = 2, // Assuming PartCategoryID is an integer
                        SellingPrice = 50.00,
                        BuyingPrice = 8.00,
                        EntryDate = new DateOnly(2023, 01, 05),
                        SupplierId = 2,
                    }
                   );



            modelBuilder.Entity<QuotationList>().HasData(
                new QuotationList
                {
                    QuoteNo = 1,
                    QuoteDate = new DateOnly(2023, 02, 01),
                    QuoteValidDate = new DateOnly(2023, 02, 15),
                    CustomerId = 1,
                    CustomerName = "John Doe",
                    UserId = 1,
                    TotalAmount = 150.00,
                    PaymentType = "Credit",
                    Status = Status.pending
                }
            );




            modelBuilder.Entity<Warehouse>().HasData(
                new Warehouse
                {
                    WarehouseId = 1,
                    WarehouseName = "Main Warehouse",
                    WarehouseAddress = "789 Elm Street"
                }
            );

            modelBuilder.Entity<Storage>().HasData(
                new Storage
                {
                    StorageId = 1,
                    PartId = 1,
                    WarehouseId = 1,
                    Location = "Section A",
                    Quantity = 50
                }
            );

            modelBuilder.Entity<QuotationPart>().HasData(
                new QuotationPart
                {
                    QuotePartId = 1,
                    QuoteNo = 1,
                    PartId = 1,
                    UnitPrice = 20.00,
                    Quantity = 5
                }
            );


            base.OnModelCreating(modelBuilder);
        }
    }
}