using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Caching.Memory;  //IMemoryCache
using Microsoft.Extensions.Primitives;

namespace MarketMvcTest.Fakes
{
    class FakeCacheEntry : ICacheEntry
    {
        object _Key = null;
        public object Key => _Key;

        object _Value = null;
        public object Value
        {
            get => _Value;
            set => _Value = value;
        }

        DateTimeOffset? _AbsoluteExpiration = null;
        public DateTimeOffset? AbsoluteExpiration
        {
            get => _AbsoluteExpiration;
            set => _AbsoluteExpiration = value;
        }

        TimeSpan? _AbsoluteExpirationRelativeToNow = null;
        public TimeSpan? AbsoluteExpirationRelativeToNow
        {
            get => _AbsoluteExpirationRelativeToNow;
            set => _AbsoluteExpirationRelativeToNow = value;
        }

        TimeSpan? _SlidingExpiration = null;
        public TimeSpan? SlidingExpiration
        {
            get => _SlidingExpiration;
            set => _SlidingExpiration = value;
        }

        public IList<IChangeToken> ExpirationTokens => throw new NotImplementedException();

        public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks => throw new NotImplementedException();

        CacheItemPriority _Priority = CacheItemPriority.Normal;
        public CacheItemPriority Priority
        {
            get => _Priority;
            set => _Priority = value;
        }

        long? _Size = null;
        public long? Size
        {
            get => _Size;
            set => _Size = value;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
