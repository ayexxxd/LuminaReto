using LuminaReto.Services.Formularios;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// HttpClient tipado para LoginService
builder.Services
    .AddHttpClient<ILoginService, LoginService>()
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

builder.Services
    .AddHttpClient<IHomeService, HomeService>()
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

builder.Services.AddHttpClient<IFormularioService, FormularioService>(client =>
{
    client.BaseAddress = new Uri("https://10.22.194.109:8002");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});

// Servicio TokensService con HttpClient
builder.Services
    .AddHttpClient<ITokensService, TokensService>(client =>
    {
        client.Timeout = TimeSpan.FromSeconds(10);
    })
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

// HttpClient tipado para ClasificacionService
builder.Services.AddHttpClient<IClasificacionService, ClasificacionService>(client =>
{
    client.BaseAddress = new Uri("https://10.14.255.45:5001");
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
    options.IdleTimeout = TimeSpan.FromHours(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configuración MIME para Unity WebGL
var provider = new FileExtensionContentTypeProvider();

provider.Mappings[".data"] = "application/octet-stream";
provider.Mappings[".wasm"] = "application/wasm";
provider.Mappings[".br"] = "application/octet-stream";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider,
    OnPrepareResponse = ctx =>
    {
        var path = ctx.File.PhysicalPath ?? "";

        if (path.EndsWith(".wasm.br"))
        {
            ctx.Context.Response.Headers["Content-Encoding"] = "br";
            ctx.Context.Response.Headers["Content-Type"] = "application/wasm";
        }
        else if (path.EndsWith(".js.br"))
        {
            ctx.Context.Response.Headers["Content-Encoding"] = "br";
            ctx.Context.Response.Headers["Content-Type"] = "application/javascript";
        }
        else if (path.EndsWith(".data.br"))
        {
            ctx.Context.Response.Headers["Content-Encoding"] = "br";
            ctx.Context.Response.Headers["Content-Type"] = "application/octet-stream";
        }
    }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();