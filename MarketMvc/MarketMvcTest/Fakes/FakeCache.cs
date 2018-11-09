using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;  //IMemoryCache
using MarketMvc.Entities;

namespace MarketMvcTest.Fakes
{
    class FakeCache : IMemoryCache
    {
        //use dictionary to fake cache
        ICacheEntry _cacheEntry = null;
        private bool _hasValue = false;

        public FakeCache(bool hasValue)
        {
            _hasValue = hasValue;
        }

        ICacheEntry IMemoryCache.CreateEntry(object key)
        {
            _cacheEntry = new FakeCacheEntry();
//            _cacheEntry.Key = key;
            return _cacheEntry;
        }

        void IDisposable.Dispose()
        {
            //throw new NotImplementedException();
        }

        void IMemoryCache.Remove(object key)
        {
            //throw new NotImplementedException();
        }

        bool IMemoryCache.TryGetValue(object key, out object value)
        {
            if (_hasValue == false)
            {
                value = null;
                return false;
            }
            else
            {
                IList<Category> Categories = new List<Category>();
                value = Categories;
                return true;
            }
        }
    }
}
