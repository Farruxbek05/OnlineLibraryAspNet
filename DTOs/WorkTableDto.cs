using OnlineLibraryAspNet.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineLibraryAspNet.DTOs
{
    public class WorkTableDto
    {
        public int CustomerId { get; set; }
        public int BookId { get; set; }
        public DateTime Time { get; set; }
    }
}
