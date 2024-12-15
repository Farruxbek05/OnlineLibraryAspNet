using OnlineLibraryAspNet.Enum;

namespace OnlineLibraryAspNet.Models
{
    public class DownloadLinks
    {

        public string path { get; set; }
        public LinkPermission link_prmission { get; set; }
        public Guid? owner_id { get; set; }


      
    }
}
