using Microsoft.EntityFrameworkCore;
using Odin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add Entity Framework with In-Memory database for demo
builder.Services.AddDbContext<DemoDbContext>(options =>
    options.UseInMemoryDatabase("DemoDatabase"));

// Register Odin services
builder.Services.RegisterOdinServices();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Register Odin authentication and routes
app.RegisterOdinAuth();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Register Odin routes
app.RegisterOdinRoutes();

// Seed some demo data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DemoDbContext>();
    SeedData(context);
}

app.Run();

static void SeedData(DemoDbContext context)
{
    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User { Name = "John Doe", Email = "john@example.com", CreatedAt = DateTime.Now },
            new User { Name = "Jane Smith", Email = "jane@example.com", CreatedAt = DateTime.Now }
        );
    }

    if (!context.Products.Any())
    {
        context.Products.AddRange(
            new Product { Name = "Laptop", Price = 999.99m, InStock = true },
            new Product { Name = "Mouse", Price = 29.99m, InStock = true },
            new Product { Name = "Keyboard", Price = 79.99m, InStock = false }
        );
    }

    context.SaveChanges();
}

// Demo entities
public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public DateTime CreatedAt { get; set; }
}

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public bool InStock { get; set; }
}

// Demo DbContext
public class DemoDbContext : DbContext
{
    public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
}