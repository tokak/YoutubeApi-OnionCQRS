using YoutubeApi.Domain.Common;

namespace YoutubeApi.Domain.Entities
{
    public class Detail :EntityBase
    {
        public  string Title { get; set; }
        public  string Description { get; set; }

        public  int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
