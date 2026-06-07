var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// HttpClient tipado para LoginService (API con certificado autofirmado)
builder.Services.AddHttpClient<ILoginService, LoginService>(client =>
{
    client.BaseAddress = new Uri("https://127.0.0.1:5010");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

// Servicio UserService con HttpClient (requiere certificado autofirmado)
builder.Services
    .AddHttpClient<IUserService, UserService>()
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

// HttpClient tipado para ClasificacionService (API Flask)
builder.Services.AddHttpClient<IClasificacionService, ClasificacionService>(client =>
{
    client.BaseAddress = new Uri("https://127.0.0.1:5001");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

// Configuración de sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout        = TimeSpan.FromHours(10);
    options.Cookie.HttpOnly    = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession(); // antes de UseAuthorization
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}")
    .WithStaticAssets();

app.Run();
