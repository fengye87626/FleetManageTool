using System;
using NUnit.Framework;
using FleetManageToolWebRole.Controllers;
using System.Web.Mvc;
using FleetManageToolWebRole.Models;

namespace FleetManageToolWebRole.NUnitTest.Vehicles
{
    [TestFixture]
    //public class SampleTest
    public class SettingTest
    {
        [Test]
        public void TestSettingControllerTenantMethod()
        {
            SettingController setting = new SettingController();
            
            ActionResult expected = new ViewResult();//todo
            ActionResult actual = setting.Tenant();//fengpan #508 20140304
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerGetUserMethod()
        {
            SettingController setting = new SettingController();
            ActionResult expected = new ViewResult();//todo
            ActionResult actual = setting.GetUser();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerAddUserMethod()
        {
            SettingController setting = new SettingController();
            //ActionResult expected = new ViewResult();//todo
            //String username = "";
            //String role = "";
            //String email = "";
            //String telephone = "";
            //String password = "";
            //ActionResult actual = setting.AddUser(username,role,email,telephone,password);
            //Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerDelUserMethod()
        {
            SettingController setting = new SettingController();
            //ActionResult expected = new ViewResult();//todo
            //String username = "";
            //String role = "";
            //String email = "";
            //String telephone = "";
            //String password = "";
            //ActionResult actual = setting.AddUser(username,role,email,telephone,password);
            //Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerResetUserMethod()
        {
            SettingController setting = new SettingController();
            //ActionResult expected = new ViewResult();//todo
            //String username = "";
            //String role = "";
            //String email = "";
            //String telephone = "";
            //String password = "";
            //ActionResult actual = setting.AddUser(username,role,email,telephone,password);
            //Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerEditUserMethod()
        {
            SettingController setting = new SettingController();
            //ActionResult expected = new ViewResult();//todo
            //String username = "";
            //String role = "";
            //String email = "";
            //String telephone = "";
            //String password = "";
            //ActionResult actual = setting.AddUser(username,role,email,telephone,password);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSettingControllerGetGroupMethod()
        {
            SettingController setting = new SettingController();
            ActionResult expected = new ViewResult();//todo
            ActionResult actual = setting.GetGroup();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerGetGroupDataMethod()
        {
            SettingController setting = new SettingController();
            ActionResult expected = new ViewResult();//todo
            String groupID = "";
            ActionResult actual = setting.GetGroupData(groupID);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSettingControllerGetNotGroupDataMethod()
        {
            //SettingController setting = new SettingController();
            //ActionResult expected = new ViewResult();//todo
            //String groupID = "";
            //ActionResult actual = setting.GetNotGroupData(groupID);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSettingControllerGroupAddVehicleMethod()
        {
            SettingController setting = new SettingController();
            ActionResult expected = new ViewResult();//todo
            //String groupID = "";
            //String vehicle = "";
            //ActionResult actual = setting.GroupAddVehicle(vehicle,groupID);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSettingControllerGroupDelVehicleMethod()
        {
            SettingController setting = new SettingController();
            ActionResult expected = new ViewResult();//todo
            //String groupID = "";
            //String vehicle = "";
            //ActionResult actual = setting.GroupAddVehicle(vehicle,groupID);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSettingControllerGroupAddGroupMethod()
        {
            SettingController setting = new SettingController();
            ActionResult expected = new ViewResult();//todo
            //String groupID = "";
            //String vehicle = "";
            //ActionResult actual = setting.GroupAddVehicle(vehicle,groupID);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSettingControllerGroupDelGroupMethod()
        {
            SettingController setting = new SettingController();
            ActionResult expected = new ViewResult();//todo
            //String groupID = "";
            //String vehicle = "";
            //ActionResult actual = setting.GroupAddVehicle(vehicle,groupID);
            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestSettingControllerGroupEditGroupMethod()
        {
            SettingController setting = new SettingController();
            ActionResult expected = new ViewResult();//todo
            //String groupID = "";
            //String vehicle = "";
            //ActionResult actual = setting.GroupAddVehicle(vehicle,groupID);
            //Assert.AreEqual(expected, actual);
        }

        /*************** fengpan **************/
        [Test]
        public void TestSettingControllerGetAccunt_infoMethod()
        {
            SettingController setting = new SettingController();
            JsonResult expected = new JsonResult();//todo
            JsonResult actual = setting.GetAccunt_info();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerEditAccunt_infoMethod()
        {
            string password = "";
            string email = "";
            string tel = "";
            SettingController setting = new SettingController();
            string expected = "OK";//todo
            string actual = setting.EditAccunt_info(password,email,tel);
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerCheck_Accunt_passwordMethod()
        {
            string password = "";
            SettingController setting = new SettingController();
            string expected = "OK";//todo
            string actual = setting.Check_Accunt_password(password);
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerGetTenant_infoMethod()
        {
            SettingController setting = new SettingController();
            JsonResult expected = new JsonResult();//todo
            JsonResult actual = setting.GetTenant_info();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerEditTenant_infoMethod()
        {
            string companyName = "";
            string companyEmail = ""; 
            string companyTel = "";
            string companyIntro = "";
            SettingController setting = new SettingController();
            string expected = "OK";//todo
            
        }
        [Test]
        public void TestSettingControllerGetOBU_VehiclesMethod()
        {
            SettingController setting = new SettingController();
            JsonResult expected = new JsonResult();//todo
            JsonResult actual = setting.GetOBU_Vehicles();
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerRegistionOBUMethod()
        {
            SettingController setting = new SettingController();
            string expected = "OK";//todo
            //string actual = setting.RegistionOBU();
            //Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerAddOBU_VehiclesMethod()
        {
            string OBU_ESN_ID = "";
            string OBU_KEY_ID = "";
            SettingController setting = new SettingController();
            string expected = "OK";//todo
            //string actual = setting.AddOBU_Vehicles(OBU_ESN_ID, OBU_KEY_ID);
            //Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TestSettingControllerEditOBU_VehiclesMethod()
        {
            long vehicleID = 1;
            string vehicleName = "";
            string vehicleInfo = "";
            string vehicleLicence = "";
            SettingController setting = new SettingController();
            string expected = "OK";//todo
            //string actual = setting.EditOBU_Vehicles(vehicleID, vehicleName, vehicleInfo, vehicleLicence);
            //Assert.AreEqual(expected, actual);
        }
    }
}