using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineLibraryAspNet.Models
{
    public class WorkTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("BookId")]
        public Books? Books { get; set; }
        public int BookId { get; set; }
        public DateTime Time { get; set; }
    }
}
