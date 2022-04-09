/*----------------------------------------------------------------
// 文件名：CacheHelper.cs
// 功能描述：缓存工具类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace HST.Utillity
{
    /// <summary>
    /// 缓存工具类
    /// </summary>
    public class CacheHelper
    {

        private static readonly object objLock = new object();
        public const int DEFAULT_CACHE_PERIOD = 20;

        /// <summary>
        /// 本地缓存获取
        /// </summary>
        /// <param name="name">key</param>
        /// <returns></returns>
        public static object Get(string name)
        {
            return HttpRuntime.Cache.Get(name);
        }

        /// <summary>
        /// 本地缓存移除
        /// </summary>
        /// <param name="name">key</param>
        public static void Remove(string name)
        {
            if (HttpRuntime.Cache[name] != null)
                HttpRuntime.Cache.Remove(name);
        }

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public static void RemoveAll()
        {
            Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }

        /// <summary>
        /// 本地缓存写入（默认缓存20min）
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        public static void Set(string name, object value)
        {
            Set(name, value, null);
        }

        /// <summary>
        /// 本地缓存写入（默认缓存20min）,依赖项
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        /// <param name="cacheDependency">依赖项</param>
        public static void Set(string name, object value, CacheDependency cacheDependency)
        {
            HttpRuntime.Cache.Insert(name, value, cacheDependency, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
        }

        /// <summary>
        /// 本地缓存写入
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        /// <param name="minutes">缓存分钟</param>
        public static void Set(string name, object value, int minutes)
        {
            HttpRuntime.Cache.Insert(name, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes));
        }

        /// <summary>
        /// 本地缓存写入，包括分钟，是否绝对过期及缓存过期的回调
        /// </summary>
        /// <param name="name">key</param>
        /// <param name="value">value</param>
        /// <param name="minutes"缓存分钟></param>
        /// <param name="isAbsoluteExpiration">是否绝对过期</param>
        /// <param name="onRemoveCallback">缓存过期回调</param>
        public static void Set(string name, object value, int minutes, bool isAbsoluteExpiration, CacheItemRemovedCallback onRemoveCallback)
        {
            if (isAbsoluteExpiration)
                HttpRuntime.Cache.Insert(name, value, null, DateTime.Now.AddMinutes(minutes), Cache.NoSlidingExpiration, CacheItemPriority.Normal, onRemoveCallback);
            else
                HttpRuntime.Cache.Insert(name, value, null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(minutes), CacheItemPriority.Normal, onRemoveCallback);
        }

        public static T GetOrAddToCache<T>(string cacheKey, Func<T> callback, int minuteToCache = DEFAULT_CACHE_PERIOD, bool skipNull = false) where T : class, new()
        {
            T returnValue = Get(cacheKey) as T;
            if (returnValue == null)
            {
                lock (objLock)
                {
                    returnValue = Get(cacheKey) as T;
                    if (returnValue == null)
                    {
                        returnValue = callback();
                        if (returnValue == null)
                        {
                            if (skipNull) return null;
                        }

                        Set(cacheKey, returnValue,  minuteToCache);
                    }
                }
            }

            return returnValue;
        }
    }
}
