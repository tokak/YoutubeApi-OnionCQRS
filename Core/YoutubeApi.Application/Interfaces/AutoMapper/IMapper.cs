namespace YoutubeApi.Application.Interfaces.AutoMapper
{
    //  bir nesneyi başka bir nesneye dönüştürmek (maplemek) için kullanılacak metodları tanımlar
    public interface IMapper
    {
        // Tek bir nesneyi, kaynak tipinden hedef tipe dönüştürür
        // 'ignore' parametresi, dönüştürürken bazı özellikleri atlamak için isteğe bağlıdır
        TDestination Map<TDestination, TSource>(TSource source, string? ignore = null);

   
        // Yani birden fazla nesneyi aynı anda mapler
        IList<TDestination> Map<TDestination, TSource>(IList<TSource> source, string? ignore = null);

        // Kaynak tipi belli değilse, tek bir nesneyi hedef tipe dönüştürür
        TDestination Map<TDestination>(object source, string? ignore = null);

        // Kaynak tipi belli değilse, bir nesne listesini hedef tipe dönüştürür
        IList<TDestination> Map<TDestination>(IList<object> source, string? ignore = null);
    }

}
