using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios en el contenedor
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("EmployeeDb")); // Usa InMemoryDatabase para pruebas

var app = builder.Build();

// Configurar el pipeline de solicitud HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Employee/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Ruta para el controlador Employee
app.MapControllerRoute(
    name: "employee",
    pattern: "{controller=Employee}/{action=Index}/{id?}");

// Ruta de fallback para otras vistas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=Index}/{id?}"); // Asegúrate de que "Employee" sea el controlador predeterminado

app.Run();
