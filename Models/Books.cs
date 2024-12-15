using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineLibraryAspNet.Models
{
    public class Books
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Jenre { get; set; }
        public int Price { get; set; }
        [ForeignKey("AuthorId")]
        public Author? Author { get; set; }
        public int AuthorId { get; set; }

    }
}
