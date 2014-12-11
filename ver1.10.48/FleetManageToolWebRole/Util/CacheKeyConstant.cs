using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManageToolWebRole.Util
{
    public class CacheKeyConstant
    {
        public static String CustomerThreadRunningStatus = "CustomerThreadRunningStatus";

        public static String VehicleThreadRunningStatus = "VehicleThreadRunningStatus";

        public static String StoreDataToDBThreadRunningStatus = "StoreDataToDBThreadRunningStatus";

        public static String CustomerProcessStatusTable = "CustomerProcessStatusTable";

        public static String StoreDataToDBProcessStatusTable = "StoreDataToDBProcessStatusTable";

        public static String LoginTenantsProcessStatusTable = "LoginTenantsProcessStatusTable";

        public static String Tenants = "Tenants";

        public static String StoreDataTenants = "StoreDataTenants";

        public static String LoginTenants = "LoginTenants";

        public static String FleetCounterTable = "FleetCounterTable";

        public static String Token = "Token";

        //public static String AuthorizationStr = "AuthorizationStr";
    }
}
