using OnlineLibraryAspNet.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineLibraryAspNet.DTOs
{
    public class BooksDto
    {
        public string Name { get; set; }
        public string Jenre { get; set; }
        public int Price { get; set; }
        public int AuthorId { get; set; }
    }
}
