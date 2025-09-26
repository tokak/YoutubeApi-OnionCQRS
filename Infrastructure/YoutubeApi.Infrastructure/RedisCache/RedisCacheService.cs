using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using YoutubeApi.Application.Interfaces.RedisCache;

namespace YoutubeApi.Infrastructure.RedisCache
{
    public class RedisCacheService : IRedisCacheService
    {
        // Redis sunucusuna bağlantı kurmak için kullanılır.
        private readonly ConnectionMultiplexer redisConnection;

        //  veritabanına erişmek için kullanılır.
        private readonly IDatabase database;

        //  bağlantı ayarlarını tutar.
        private readonly RedisCacheSettings settings;

        // Constructor: Ayarları alır, Redis'e bağlanır ve veritabanını hazırlar.
        public RedisCacheService(IOptions<RedisCacheSettings> options)
        {
            settings = options.Value;
            var opt = ConfigurationOptions.Parse(settings.ConnectionString);
            redisConnection = ConnectionMultiplexer.Connect(opt);
            database = redisConnection.GetDatabase();
        }

        // Redis'ten verilen "key" ile veriyi okur ve istenen tipe çevirip döner.
        public async Task<T> GetAsync<T>(string key)
        {
            var value = await database.StringGetAsync(key);
            if (value.HasValue)
                return JsonConvert.DeserializeObject<T>(value); // JSON'u objeye çevirir

            return default; // Veri yoksa varsayılan değeri döner
        }

        // Verilen "key" ile objeyi JSON formatında Redis'e kaydeder ve istenirse süre belirler.
        public async Task SetAsync<T>(string key, T value, DateTime? expirationTime = null)
        {
            TimeSpan timeUnitExpiration = expirationTime.Value - DateTime.Now; // Ne kadar süre saklanacak hesapla
            await database.StringSetAsync(key, JsonConvert.SerializeObject(value), timeUnitExpiration); // Veriyi kaydet
        }
    }

}
