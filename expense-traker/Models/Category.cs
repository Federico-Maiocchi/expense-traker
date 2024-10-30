using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace expense_traker.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [DisplayName("Titolo")]
        [Required(ErrorMessage = "Il titolo è obbligatorio.")]
        [Column(TypeName = "nvarchar(50)")]
        [MaxLength(50, ErrorMessage = "Il titolo non può superare i 50 caratteri.")]
        public string Title { get; set; }

        [DisplayName("Icona")]
        [Column(TypeName = "nvarchar(5)")]
        [MaxLength(5, ErrorMessage = "L'icona non può superare i 5 caratteri.")]
        public string Icon { get; set; } = "";

        [DisplayName("Tipo")]
        [Required(ErrorMessage = "Il tipo è obbligatorio.")]
        [Column(TypeName = "nvarchar(10)")]
        [MaxLength(10, ErrorMessage = "Il tipo non può superare i 10 caratteri.")]
        [RegularExpression("^(Income|Expense)$", ErrorMessage = "Il tipo deve essere 'Income' o 'Expense'.")]
        public string Type { get; set; } = "Expense";

        [DisplayName("Titolo")]
        [NotMapped]
        public string? TitleWithIcon
        {
            get
            {
                return this.Icon + " " + this.Title;
            }
        }
    }
}
