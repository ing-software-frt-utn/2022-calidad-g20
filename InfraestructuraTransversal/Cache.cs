using Microsoft.Extensions.Caching.Memory;
using System;

namespace InfraestructuraTransversal
{
    public class Cache
    {
        private static readonly object padlock = new Cache();
        private static Cache _instance = null;

        private IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        private const string ordenIdCacheKey = "ordenId";
        private const string empleadoIdCacheKey = "empleadoId";

        public static Cache Instance
        {
            get
            {
                lock (padlock)
                {
                    return _instance ??= new Cache();
                }
            }
        }

        public void GuardarOrdenID(int id)
        {
            if (!_cache.TryGetValue(ordenIdCacheKey, out int ordenId))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
                _cache.Set(ordenIdCacheKey, id, cacheEntryOptions);
            }
        }
        public void GuardarEmpleadoID(int id)
        {
            if (!_cache.TryGetValue(empleadoIdCacheKey, out int empleadoId))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromHours(8))
                        .SetAbsoluteExpiration(TimeSpan.FromHours(8))
                        .SetPriority(CacheItemPriority.Normal)
                        .SetSize(1024);
                _cache.Set(empleadoIdCacheKey, id, cacheEntryOptions);
            }
        }
        public int ObtenerOrdenID()
        {
            int ordenId = 0;
            _cache.TryGetValue(ordenIdCacheKey, out ordenId);
            return ordenId;
        }
        public int ObtenerEmpleadoID()
        {
            int empleadoId = 0;
            _cache.TryGetValue(empleadoIdCacheKey, out empleadoId);
            return empleadoId;
        }
        public void LiberarCache()
        {
            _cache.Remove(empleadoIdCacheKey);
            _cache.Remove(ordenIdCacheKey);
        }
        public void BorrarOPId()
        {
            _cache.Remove(ordenIdCacheKey);
        }
    }

    public class TransfersQueueWorker
    {
        private static readonly object padlock = new TransfersQueueWorker();
        private static TransfersQueueWorker _instance = null; 
        public static TransfersQueueWorker Instance
        {
            get
            {
                lock (padlock)
                {
                    return _instance ??= new TransfersQueueWorker();
                }
            }
        }
        private TransfersQueueWorker() { }
        //TransferQueueWorkers.Instance.Method();
    }
    }