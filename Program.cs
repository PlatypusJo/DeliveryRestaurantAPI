using BackAPI.Models1;
using BackAPI.Data;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
    builder =>
    {
        builder.WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod();

    });
});

builder.Services.AddControllers().AddJsonOptions(x =>
x.JsonSerializerOptions.ReferenceHandler =
ReferenceHandler.IgnoreCycles);
// Add services to the container.
builder.Services.AddIdentity<User, IdentityRole>()
.AddEntityFrameworkStores<RestaurantDeliveryContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RestaurantDeliveryContext>();



var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var restaurantDeliveryContext =
    scope.ServiceProvider.GetRequiredService<RestaurantDeliveryContext>();
    await RestaurantDeliveryContextSeed.Initialize(restaurantDeliveryContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();