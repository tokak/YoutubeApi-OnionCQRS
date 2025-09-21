using YoutubeApi.Domain.Common;

namespace YoutubeApi.Domain.Entities
{
    public class Category : EntityBase
    {
        public  int ParentId { get; set; }
        public  string Name { get; set; }
        public  int Priorty { get; set; } //Öncelik sırası
        public ICollection<Detail> Details { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }

    }
   
}