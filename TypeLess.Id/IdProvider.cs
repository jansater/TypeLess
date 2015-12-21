using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeLess;

namespace TypeLess.Id
{
    public class IdProvider
    {
        private ICacheProvider _cache;
        private IIdSource _idSource;
        private const int FetchSize = 50;

        public IdProvider(ICacheProvider cache, IIdSource idSource)
        {
            cache.If("cache").IsNull.ThenThrow().Otherwise(x => _cache = x);
            idSource.If("idSource").IsNull.ThenThrow().Otherwise(x => _idSource = x);
        }

        private string GetCacheKey<T>() {
            return typeof(T).Name;
        }

        private string GetReservationKey<T>(int id)
        {
            return typeof(T).Name + "_Reserved_" + id;
        }

        public int ReserveId<T>(TimeSpan howLong) {

            var cacheKey = GetCacheKey<T>();
            var idGroup = _cache.Get(cacheKey);

            if (idGroup == null || !idGroup.Any()) { 
                //then we need to fill this group
                var idsFromSource = idSource.GetIds<T>(FetchSize);
                idGroup.AddRange(idsFromSource);
                
            }

            if (!idGroup.Any()) {
                throw new Exception("Failed to get new id:s from source");
            }

            var nextId = idGroup.FirstOrDefault();
            idGroup.Remove(nextId);

            _cache.Put(cacheKey, idGroup, TimeSpan.FromDays(365));

            var reserveKey = GetReservationKey<T>(nextId);
            if (_cache.Contains(reserveKey)) {
                throw new Exception("ID " + nextId + " has already been reserved");
            }

            _cache.Put(reserveKey, nextId, howLong);
            return nextId;
        }

        public void ClaimId<T>(int id) {
            var reserveKey = GetReservationKey<T>(id);
            if (!_cache.Contains(reserveKey))
            {
                throw new Exception("Id has not been reserved");
            }

            _cache.Remove(reserveKey);
        }

        public void ReleaseId<T>(int id) {
            var reserveKey = GetReservationKey<T>(id);
            if (!_cache.Contains(reserveKey))
            {
                throw new Exception("Id has not been reserved");
            }

            _cache.Remove(reserveKey);

            var cacheKey = GetCacheKey<T>();
            var idGroup = _cache.Get(cacheKey);

            if (idGroup == null || !idGroup.Any())
            {
                //then we need to fill this group
                idGroup = new List<int>();
            }

            idGroup.Add(id);
        }

    }
}
