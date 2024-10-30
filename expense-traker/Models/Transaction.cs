using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace expense_traker.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        //CategoryId
        [Required(ErrorMessage = "La categoria è obbligatoria.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [DisplayName("Importo")]
        [Required(ErrorMessage = "L'importo è obbligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "L'importo deve essere maggiore di zero.")]
        [Column(TypeName = "decimal(18,2)")]
        [DataType(DataType.Currency)] //Usare DataType per specificare che si tratta di una valuta,(DataType.Currency) rappresenta un importo monetario
        public decimal Amount { get; set; }

        [DisplayName("Nome Categoria")]
        [NotMapped] 
        public string? AmountWithSymbol //formattazione importo per la visualizzazione, simbolo(+/-) + euro + importo 
        {
            get
            {
                // Definire il simbolo in base alla categoria
                // + = Income
                // - = Expense
                //string symbol = Category?.Type == "Expense" ? "-" : (Category?.Type == "Income" ? "+" : "");
                string symbol = "";

                if (Category != null)
                {
                    if (Category.Type == "Expense")
                    {
                        symbol = "-";
                    }
                    else if (Category.Type == "Income")
                    {
                        symbol = "+";
                    }
                }

                return $"{symbol} € {Amount:0.00}";

                //vecchia formattazione
                //return String.Format("{0:C}", Amount).Replace("€", "€ ");
                //return String.Format("€" + " " + Amount);
            }
        }

        [DisplayName("Data Transazione")]
        [NotMapped]
        public string FormattedDate // Formattazione delle data, per visualizzare in pagina solo la data e non le ore
        {
            get
            {
                return Date.ToString("dd/MM/yyyy");
            }
        }

        [MaxLength(75, ErrorMessage = "La nota non può superare i 75 caratteri.")]
        [Column(TypeName = "nvarchar(75)")]
        public string? Note { get; set; }

        [DisplayName("Data Transazione")]
        public DateTime Date {  get; set; } = DateTime.Now;


    }
}
