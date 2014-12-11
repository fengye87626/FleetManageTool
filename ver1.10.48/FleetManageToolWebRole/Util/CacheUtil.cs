using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ApplicationServer.Caching;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models;

namespace FleetManageToolWebRole.Util
{
    public class CacheUtil
    {
        private static DataCacheFactory factory = null;

        private static Object lockObj = new Object();

        private static object lockObject = new object();

        public static DataCacheFactory GetDataCacheFactory()
        {
            if (null == factory)
            {
                lock (lockObject)
                {
                    if (null == factory)
                    {
                        factory = new DataCacheFactory();
                    }
                }
            }
            return factory;
        }

        //chenyangwen worker
        public static void InitFleetCounterTable()
        {
            TenantDBInterface tenantDB = new TenantDBInterface();
            CacheService service = new CacheService();
            List<Tenant> tenants = tenantDB.GetTenants();
            Dictionary<String, int> tenantCounter = new Dictionary<string, int>();
            foreach (Tenant tenant in tenants)
            {
                tenantCounter.Add(tenant.companyid, 0);
            }
            service.CachePut(CacheKeyConstant.FleetCounterTable, tenantCounter);
        }

        public static void AddFleetCounter(String companyID)
        {
            lock (lockObj)
            {
                CacheService service = new CacheService();
                Object table = service.CacheGet(CacheKeyConstant.FleetCounterTable);
                if (null != table)
                {
                    if (((Dictionary<String, int>)table).ContainsKey(companyID.ToString()))
                    {
                        int count = ((Dictionary<String, int>)table)[companyID.ToString()];
                        ((Dictionary<String, int>)table)[companyID.ToString()] = count + 1;
                    }
                    else
                    {
                        ((Dictionary<String, int>)table).Add(companyID.ToString(), 1);
                    }
                }
                else
                {
                    CacheUtil.InitFleetCounterTable();
                    table = service.CacheGet(CacheKeyConstant.FleetCounterTable);
                    if (((Dictionary<String, int>)table).ContainsKey(companyID.ToString()))
                    {
                        ((Dictionary<String, int>)table)[companyID.ToString()] = 1;
                    }
                }
                service.CachePut(CacheKeyConstant.FleetCounterTable, table);
            }
        }

        public static void MinusFleetCounter(String companyID)
        {
            lock (lockObj)
            {
                CacheService service = new CacheService();
                Object table = service.CacheGet(CacheKeyConstant.FleetCounterTable);
                if (null != table)
                {
                    int count = ((Dictionary<String, int>)table)[companyID.ToString()];
                    if (count > 1)
                    {
                        ((Dictionary<String, int>)table)[companyID.ToString()] = count - 1;
                    }
                    else
                    {
                        ((Dictionary<String, int>)table)[companyID.ToString()] = 0;
                        service.CacheRemove(companyID + "_Cache");
                    }
                    service.CachePut(CacheKeyConstant.FleetCounterTable, table);
                }
                else
                {
                    CacheUtil.InitFleetCounterTable();
                }
            }
        }

        //public static void Print()
        //{
        //    CacheService service = new CacheService();
        //    Object table = service.CacheGet(CacheKeyConstant.FleetCounterTable);
        //    if (null != table)
        //    {
        //        foreach (String companyid in ((Dictionary<String, int>)table).Keys)
        //        {
        //            DebugLog.Debug("Print Table Companyid = " + companyid + ";Counter = " + ((Dictionary<String, int>)table)[companyid]);
        //        }
        //    }
        //}

        public static String GetTokenStr()
        {
            CacheService service = new CacheService();
            Object result = service.CacheGet(CacheKeyConstant.Token);
            if (null == result)
            {
                result = ConfigureManager.GetConfigure("InitToken");
                service.CachePut(CacheKeyConstant.Token, result);
            }
            return result.ToString();
        }

        public static void SetTokenStr(String tokenStr)
        {
            CacheService service = new CacheService();
            service.CachePut(CacheKeyConstant.Token, tokenStr);
        }

    }
}