using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcMovie.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MvcMovieContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcMovieContext") ?? throw new InvalidOperationException("Connection string 'MvcMovieContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the validation services
builder.Services.AddValidation();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// utilize projection to return only the required fields for the API response rather than returning the entire entity.
// This can help reduce the amount of data sent over the network and improve performance.

app.MapGet("/api/directors/{id:int}", async (int id, MvcMovieContext context) =>
{
    // Use projection to select only the required fields for the director and their movies
    var director = await context.Director
    .Where(d => d.Id == id)
    .Select(d => new
    {
        d.Id,
        d.Name,
        d.BirthDate,
        Movies = d.Movies.Select(m => new
        {
            m.Id,
            m.Title,
            m.ReleaseDate
        }).ToList()
    }).FirstOrDefaultAsync();
    // Return the director if found, otherwise return a 404 Not Found response
    return director is not null ? Results.Ok(director) : Results.NotFound();

});

app.Run();