using System.ComponentModel.DataAnnotations.Schema;

namespace HR.Model
{
    public class Vacation
    {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Notes { get; set; }


        [ForeignKey("Emp")]
        public long? EmployeeId { get; set; }
        public Employee? Emp { get; set; }

        [ForeignKey("Type")]
        public long? TypeId { get; set; }
        public Lookup? Type {  get; set; }
    }
}
