using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoListProject.Models
{
    [Table("Todos")]
    public class Todos
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string? Title { get; set; }

        [Required]
        [Column(TypeName ="varchar(100)")]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }

        [ForeignKey("Users")]
        [Required]
        public int UserId { get; set; }

        public virtual Users? User{ get; set; }

    }
}
