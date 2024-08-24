using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Models;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar la vista principal
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees); // Pasa la lista de empleados a la vista
        }

        // Acción para obtener la lista de empleados en formato JSON
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _context.Employees.ToListAsync();
            return Json(employees);
        }

        // Acción para agregar un nuevo empleado
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return Json(employee); // Devuelve el empleado recién creado en formato JSON
            }
            return BadRequest(ModelState); // Devuelve errores del modelo en caso de fallo
        }

        // Acción para actualizar un empleado
        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(employee).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Json(employee); // Retorna el empleado actualizado en formato JSON
            }
            return BadRequest(ModelState); // Retorna un error si el modelo no es válido
        }

        // Acción para eliminar un empleado
        
        [HttpPost]
        public async Task<IActionResult> DeleteEmployee([FromBody] int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return Ok(); // Retorna éxito si el empleado fue eliminado
            }
            return NotFound(); // Retorna error si el empleado no fue encontrado
        }

        // Acción para obtener un empleado por ID (para la edición)
        [HttpGet]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound(); // Empleado no encontrado
            }
            return Json(employee); // Retorna el empleado en formato JSON
        }
    }
}
