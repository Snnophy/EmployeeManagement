using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; } // El ID será autogenerado por la base de datos

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(50)]
        public required string Position { get; set; }

        [Required]
        [StringLength(50)]
        public required string Office { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Ingresa el salario.")]
        public decimal Salary { get; set; }
    }
}
