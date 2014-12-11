using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetManageToolWebRole.Models.API
{
    public class URI
    {
        //OK
        public const string TASK = @"/api/v1/tasks";

        public const string TASKRESULTS = @"/api/v1/tasks/{ID}/result";

        //OK
        public const string TENANTS = @"/api/v1/tenants";

        //OK
        public const string TENANT = @"/api/v1/tenants/{ID}";

        //ok
        public const string ADDCUSTOMER = @"/api/v1/tenants/{ID}/customers";

        //OK
        public const string CUSTOMERS = @"/api/v1/customers/{ID}";

        //OK
        public const string CUSTOMERVEHICLES = @"/api/v1/customers/{ID}/vehicles";

        //OK
        public const string CUSTOMERDEVICES = @"/api/v1/customers/{ID}/devices";

        //OK
        public const string VEHICLES = @"/api/v1/vehicles/{ID}";

        //OK
        public const string VEHICLESODOMETER = @"/api/v1/vehicles/{ID}/state";

        //OK
        public const string VEHICLETRIPS = @"/api/v1/vehicles/{ID}/trips";

        //OK
        public const string VEHICLEDTCS = @"/api/v1/vehicles/{ID}/diagnostic-codes";

        //OK
        public const string VEHICLEALERTS = @"/api/v1/vehicles/{ID}/alerts";

        //OK
        public const string VEHICLEALERTCONFIGURATIONS = @"/api/v1/vehicles/{ID}/alert-configurations";

        //OK
        public const string VEHICLEGEOFENCES = @"/api/v1/vehicles/{ID}/geo-fences";

        //OK
        public const string TRIPS = @"/api/v1/trips/{ID}";

        //OK
        public const string ALERTCONFIGURATIONS = @"/api/v1/alert-configurations/{ID}";

        //OK
        public const string ALERTS = @"/api/v1/alerts/{ID}";

        //OK
        public const string FINDDEVICEIDBYCARRIERID = @"/api/v1/devices/find/carrierid/{id}";

        //OK
        public const string FINDDEVICEIDBYREGKEY = @"/api/v1/devices/find/regkey/{id}";

        //OK
        public const string TOOLTOKEN = @"/api/v1/token";

        //500 
        public const string VEHICLEDIAGNOSTICS = @"/api/v1/vehicles/{ID}/diagnostics";

        //page
        public const string VEHICLETRIPPAGE = @"api/v1/vehicles/{ID}/trips?page={page}";

        //page
        public const string VEHICLEALERTPAGE = @"api/v1/vehicles/{ID}/alerts?page={page}";

        public const string VEHICLEALERTTIME = @"api/v1/vehicles/{ID}/alerts?startTime={startTime}";
	
	    public const string GEOFENCES = @"/api/v1/geo-fences/{ID}";

        public const string LINK_SELE = "self";

        public const string LINK_NEXT = "next";

        public const string LINK_TENANTCUSTOMERLIST = "tenant-customer-list";

        public const string LINK_VEHICLETRIPLIST = "vehicle-trip-list";

        public const string LINK_TASKLIST = "list-tasks";

        public const string LINK_TASKRESULT = "task-result";

        public const string LINK_DEVICE = "device";
    }
}
