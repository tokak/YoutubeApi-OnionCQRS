using YoutubeApi.Domain.Common;

namespace YoutubeApi.Domain.Entities
{
    public class Category : EntityBase
    {
        public required int ParentId { get; set; }
        public required string Name { get; set; }
        public required int Priorty { get; set; } //Öncelik sırası
        public ICollection<Detail> Details { get; set; }
        public ICollection<Product> Products { get; set; }

    }
   
}