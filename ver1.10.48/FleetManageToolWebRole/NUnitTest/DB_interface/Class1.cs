using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models;

namespace FleetManageToolWebRole.NUnitTest.DB_interface
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void testTenantDBInterface()
        {
            
        }

        [Test]
        public void testUserDBInterface()
        {
            UserDBInterface userInterface = new UserDBInterface();
            FleetUser user = userInterface.LoginIsUser("ABCSoft","85cc4219b2d9c9ef45e5c4ffd2c7a4");
            Assert.IsNotNull(user);
            List<FleetUser> users = userInterface.GetAllUser("ABCSoft");
            Assert.AreEqual(3, users.Count);
            FleetUser_Role role = userInterface.GetUserRoleByUserID(29);
            Assert.AreEqual(1, role.roleid);
            Administrator admin = userInterface.GetAdminstration("admin", "d4572bad4fe25b7d11a240de12c3a7");
            Assert.IsNotNull(admin);
            user = userInterface.GetUserByID("ABCSoft", 30);
            Assert.IsNotNull(user);
        }

        [Test]
        public void testVehicleDBInterface()
        {
            //VehicleDBInterface vehicleDB = new VehicleDBInterface();
            //List<Vehicle> vehicles = vehicleDB.GetTenantVehiclesByCompannyID("ABCSoft");
            //Assert.AreEqual(5, vehicles.Count);
            //List<VehicleGroup> groups = vehicleDB.GetGroupsByCompannyID("ABCSoft");
            //Assert.AreEqual(2, groups.Count);
            //Vehicle vehicle = vehicleDB.GetVehicleByID(1);
            //Assert.IsNotNull(vehicle);
            //vehicles = vehicleDB.GetGroupVehiclesByGroupId(31);
            //Assert.AreEqual(4, vehicles.Count);
        }
    }
}