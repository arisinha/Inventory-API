using web_api.Repository;
using web_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//SERVICES
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IClientesService, ClientesService>();
builder.Services.AddScoped<IVentasService, VentasService>();

//REPOSITORY
builder.Services.AddScoped<IVentasRepository, VentasRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IClientesRepository, ClientesRepository>(_ =>
    new ClientesRepository(builder.Configuration.GetConnectionString("DefaultConnection")!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}
app.UseCors(policy =>
    policy.WithOrigins("http://localhost:8000") // o el puerto de Laravel
        .AllowAnyMethod()
        .AllowAnyHeader()
);

app.UseHttpsRedirection();

app.MapControllers();

app.Run();