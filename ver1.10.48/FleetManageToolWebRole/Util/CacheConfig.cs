using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Util
{
    public class CacheConfig
    {
        public static void CacheSetting(HttpResponseBase response)
        {
            //chenyangwen 2014/3/8
            //if (!response.IsRequestBeingRedirected)
            //{
            //    response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            //    response.AddHeader("pragma", "no-cache");
            //    response.AddHeader("Cache-Control", "no-cache");
            //    response.CacheControl = "no-cache";
            //    response.Expires = -1;
            //    response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            //    response.Cache.SetNoStore();
            //}
        }
    }
}