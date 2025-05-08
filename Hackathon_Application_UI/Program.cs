using Hackathon_Application_UI.Interface;
using Hackathon_Application_UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add HttpClient services
builder.Services.AddHttpClient<IMatterService, MatterService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services.AddHttpClient<IDocumentService, DocumentService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Register services
builder.Services.AddScoped<IMatterService, MatterService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
