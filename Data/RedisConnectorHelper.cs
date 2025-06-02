using StackExchange.Redis;

namespace MoviesAPI.Data
{
    public class RedisConnectorHelper
    {
        private static readonly Lazy<ConnectionMultiplexer> lazyConnection;

        static RedisConnectorHelper()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect("localhost:6379"); // adjust as needed
            });
        }

        public static ConnectionMultiplexer Connection => lazyConnection.Value;

    }
}
