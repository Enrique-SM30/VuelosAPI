using VuelosAPI.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<sistem21_salidasvuelosContext>(x=>x.UseMySql("server=sistemas19.com;database=sistem21_salidasvuelos;user=sistem21_Salidas;password=salidasvuelos123", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.17-mariadb")));

var app = builder.Build();

app.UseFileServer();

app.UseRouting();

app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();