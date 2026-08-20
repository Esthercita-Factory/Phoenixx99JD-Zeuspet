using Phoenixx99JD_Zeuspet.Web.Services;
using Phoenixx99JD_Zeuspet.Web.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<VeterinariaService>();
builder.Services.AddScoped<SesionService>();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
