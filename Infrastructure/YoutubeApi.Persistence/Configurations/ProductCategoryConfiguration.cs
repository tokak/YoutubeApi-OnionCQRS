using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YoutubeApi.Domain.Entities;

namespace YoutubeApi.Persistence.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            //  (Primary Key) ProductId ve CategoryId’den oluştur
            builder.HasKey(x => new { x.ProductId, x.CategoryId });

            // Product ile ilişki kur: 
            // Bir Product’ın birden çok ProductCategory’si olabilir
            // ProductCategory -> Product ilişkisini kur, silindiğinde ilişkili kayıtlar da silinsin
            builder.HasOne(p => p.Product)
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Category ile ilişki kur:
            // Bir Category’nin birden çok ProductCategory’si olabilir
            // ProductCategory -> Category ilişkisini kur, silindiğinde ilişkili kayıtlar da silinsin
            builder.HasOne(c => c.Category)
                .WithMany(pc => pc.ProductCategories)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
