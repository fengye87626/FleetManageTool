using Microsoft.ApplicationServer.Caching;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.Cache;
using Microsoft.Practices.TransientFaultHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Util
{
    public class CacheService
    {
        private DataCache cache;

        public CacheService()
        {
            CreateDataCache();
        }

        private void CreateDataCache()
        {
            this.cache = CacheUtil.GetDataCacheFactory().GetDefaultCache();
        }

        //封装Cache方法
        public object CacheGet(string key)
        {
            return this.SafeCallFunction(() => this.cache.Get(key));
        }

        public void CacheClear()
        {
            CacheRemove(CacheKeyConstant.CustomerThreadRunningStatus);
            CacheRemove(CacheKeyConstant.VehicleThreadRunningStatus);
            CacheRemove(CacheKeyConstant.StoreDataToDBThreadRunningStatus);
            CacheRemove(CacheKeyConstant.CustomerProcessStatusTable);
            CacheRemove(CacheKeyConstant.StoreDataToDBProcessStatusTable);
            CacheRemove(CacheKeyConstant.LoginTenantsProcessStatusTable);
            CacheRemove(CacheKeyConstant.Tenants);
            CacheRemove(CacheKeyConstant.StoreDataTenants);
            CacheRemove(CacheKeyConstant.LoginTenants);
            CacheRemove(CacheKeyConstant.FleetCounterTable);
        }

        public void CacheClearAll()
        {
            foreach (String regionName in cache.GetSystemRegions())
            {
                cache.ClearRegion(regionName);
            }
        }

        public void CachePut(string key, object cacheObject)
        {
            this.SafeCallFunction(() => this.cache.Put(key, cacheObject));
        }

        public void CacheRemove(string key)
        {
            this.SafeCallFunction(() => this.cache.Remove(key));
        }

        private object SafeCallFunction(Func<object> function)
        {
            object myObject = null;

            try
            {
                // Do some work that may result in a transient fault.
                CacheExceptionRetryStrategy.GetInstance().GetRetryPolicy().ExecuteAction(() =>
                {
                    // Call a method that uses Windows Azure storage and which may
                    // throw a transient exception.
                    myObject = function.Invoke();
                });
            }
            catch (Exception e)
            {
                DebugLog.Exception("CacheUtil DataCacheException = " + e);
                // All the retries failed.
               // throw e;
            }

            return myObject;
        }
    }
}