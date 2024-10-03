using API.Middleware;
using API.SignalR;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<StoreContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}); 
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoty<>));
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddCors();
builder.Services.AddSingleton<IConnectionMultiplexer>(config => {
    var connectionString = builder.Configuration.GetConnectionString("Redis")
        ?? throw new Exception("Cannot read Redis connection string");
    var configuration = ConfigurationOptions.Parse(connectionString,true);
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<IShoppingCartService,ShoppingCartService>();
builder.Services.AddSingleton<IResponseCachesService,ResponseCacheService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<AppUser>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<StoreContext>();


builder.Services.AddScoped<IPaymentService,PaymentService>();
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials()
    .WithOrigins("http://localhost:4200","https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapControllers();
app.MapGroup("api").MapIdentityApi<AppUser>();
app.MapHub<NotificationHub>("/hub/notifications");
app.MapFallbackToController("Index","Fallback");
try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoreContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    await StoreContextSeedData.SeedAsync(context, userManager);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
    throw;
}


app.Run();
