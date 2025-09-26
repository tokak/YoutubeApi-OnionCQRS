using MediatR;
using YoutubeApi.Application.Interfaces.RedisCache;

namespace YoutubeApi.Application.Beheviors
{
    public class RedisCacheBehevior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        // Redis'e veri yazma/okuma işlemleri yapan servisi tutar.
        private readonly IRedisCacheService redisCacheService;

        // Constructor: Redis servisini dependency injection ile alır.
        public RedisCacheBehevior(IRedisCacheService redisCacheService)
        {
            this.redisCacheService = redisCacheService;
        }

        // MediatR isteği çalışmadan önce cache kontrolü yapar.
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Eğer gelen istek cache'lenebilir bir sorguysa (ICacheableQuery uygulanmışsa)
            if (request is ICacheableQuery query)
            {
                var cacheKey = query.CacheKey;       // Cache için kullanılacak anahtar
                var cacheTime = query.CacheTime;     // Cache’in kaç dakika geçerli olacağı

                // 1️⃣ Önce Redis’te bu anahtar var mı diye kontrol et
                var cacheData = await redisCacheService.GetAsync<TResponse>(cacheKey);
                if (cacheData is not null)
                    return cacheData; // Varsa cache’ten al ve işlemi bitir

                // 2️⃣ Yoksa normal akışa devam et (veriyi handler’dan çek)
                var response = await next();

                // 3️⃣ Gelen veriyi Redis’e kaydet, belirtilen süre kadar sakla
                if (response is not null)
                    await redisCacheService.SetAsync(cacheKey, response, DateTime.Now.AddMinutes(cacheTime));

                return response; // Yeni veriyi döndür
            }

            // Eğer istek cache'lenebilir değilse normal şekilde çalıştır
            return await next();
        }
    }

}
