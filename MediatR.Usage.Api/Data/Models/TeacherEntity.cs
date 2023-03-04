using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediatR.Usage.Api.Data.Models
{
    [Table("Teachers")]
    public class TeacherEntity
    {
        [Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        [NotMapped]
        public List<string> ClassesTaught { get; set; }
    }
}
