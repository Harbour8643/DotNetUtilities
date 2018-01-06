using System;
using System.Web;
using System.Web.Caching;
using System.Collections;

namespace DotNet.Utilities
{
    /// <summary>
    /// Cache缓存帮助类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="CacheKey">缓存键</param>
        public static object GetCache(string CacheKey)
        {
            return HttpRuntime.Cache[CacheKey];
        }
       
        /// <summary>
        /// 设置数据缓存(默认绝对时间1小时)
        /// </summary>
        /// <param name="CacheKey">缓存键</param>
        /// <param name="objObject">缓存值</param>
        public static void SetCacheAbsEx(string CacheKey, object objObject)
        {
            SetCache(CacheKey, objObject, DateTime.Now.AddSeconds(3600));
        }
        /// <summary>
        /// 设置数据缓存(默认滑动时间20分钟)
        /// </summary>
        /// <param name="CacheKey">缓存键</param>
        /// <param name="objObject">缓存值</param>
        public static void SetCacheSlidEx(string CacheKey, object objObject)
        {
            SetCache(CacheKey, objObject, TimeSpan.FromMinutes(20));
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="CacheKey">缓存键</param>
        /// <param name="objObject">缓存值</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration)
        {
            SetCache(CacheKey, objObject, null, absoluteExpiration);
        }
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="CacheKey">缓存键</param>
        /// <param name="objObject">缓存值</param>
        /// <param name="slidingExpiration">滑动过期时间</param>
        public static void SetCache(string CacheKey, object objObject, TimeSpan slidingExpiration)
        {
            SetCache(CacheKey, objObject, null, slidingExpiration);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="CacheKey">缓存键</param>
        /// <param name="objObject">缓存值</param>
        /// <param name="dependencies">缓存依赖项</param>
        /// <param name="absoluteExpiration">绝对过期时间</param>
        public static void SetCache(string CacheKey, object objObject, CacheDependency dependencies, DateTime absoluteExpiration)
        {
            SetCache(CacheKey, objObject, dependencies, absoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
        }
        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="CacheKey">缓存键</param>
        /// <param name="objObject">缓存值</param>
        /// <param name="dependencies">缓存依赖项</param>
        /// <param name="slidingExpiration">滑动过期时间</param>
        public static void SetCache(string CacheKey, object objObject, CacheDependency dependencies, TimeSpan slidingExpiration)
        {
            SetCache(CacheKey, objObject, dependencies, Cache.NoAbsoluteExpiration, slidingExpiration, CacheItemPriority.High, null);
        }


        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="CacheKey">缓存键</param>
        /// <param name="objObject">缓存值</param>
        /// <param name="dependencies">缓存依赖项</param>
        /// <param name="absoluteExpiration">绝对过期时间：请使用 System.DateTime.UtcNow 而不是 System.DateTime.Now 作为此参数值。如果使用绝对到期，则 slidingExpiration 参数必须为Cache.NoSlidingExpiration</param>
        /// <param name="slidingExpiration">滑动过期时间：最后一次访问所插入对象时与该对象到期时之间的时间间隔。如果该值等效于 20 分钟，则对象在最后一次被访问 20 分钟之后将到期并被从缓存中移除。如果使用可调到期，则absoluteExpiration 参数必须为Cache.NoAbsoluteExpiration</param>
        /// <param name="priority">内存自动回收优先级</param>
        /// <param name="onRemoveCallback">缓存移除回调通知（委托）</param>
        public static void SetCache(string CacheKey, object objObject, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            HttpRuntime.Cache.Insert(CacheKey, objObject, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }


        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="CacheKey">缓存键</param>
        public static void Remove(string CacheKey)
        {
            HttpRuntime.Cache.Remove(CacheKey);
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            Cache objCache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = objCache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                objCache.Remove(CacheEnum.Key.ToString());
            }
        }
    }
}