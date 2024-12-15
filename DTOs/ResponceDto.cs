namespace OnlineLibraryAspNet.DTOs
{
   
        public class ResponceDto
        {
            public bool Success { get; set; }
            public int StatusCode { get; set; }
            public string? Message { get; set; }
        }

        public class ResponceDto<T>
        {
            public bool Success { get; set; }
            public int StatusCode { get; set; }
            public string? Message { get; set; }
            public T? Data { get; set; }
        }
    }

