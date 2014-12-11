using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Filters;
using System.IO;
using FleetManageToolWebRole.Util;

namespace FleetManageToolWebRole.Controllers
{
	//chenyangwen 2014/03/04
    [SessionFilter]
    [ReuqestFilter]
    public class LogoController : Controller
    {
        public ActionResult DrawImage(long vehicleID, string type)
        {
            try
            {
                LogoDBInterface logoInterface = new LogoDBInterface();
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                TenantDBInterface tenantInterface = new TenantDBInterface();
                long logoID = 0;
                if ("vehicleLogo".Equals(type))
                {
                    logoID = vehicleInterface.GetLogoIDByVehicleID(vehicleID);
                }
                var bytes = logoInterface.GetLogoDataByLogoID(logoID);
                if (bytes != null)
                {
                    return File(bytes, @"image/jpeg");
                }
                else
                {
                    
                    FileStream file = new FileStream( AppDomain.CurrentDomain.BaseDirectory + "/Images/vehicle.png", FileMode.Open, FileAccess.Read);
                    BinaryReader binaryReader = new BinaryReader(file);
                    binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
                    bytes = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                    return File(bytes, @"image/jpeg");
                }
            }
            catch (Exception exception)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, exception.Message);
            }
            return null;
        }
    }
}
