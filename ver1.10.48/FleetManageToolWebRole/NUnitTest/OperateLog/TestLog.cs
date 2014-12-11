using FleetManageTool.Models.Common;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.NUnitTest.OperateLog
{
    [TestFixture]
    public class TestLog
    {
        [Test]
        public void testAddOperateLog()
        {
            string companyid = "ABCSoft";
            Operate_Log log = new Operate_Log();
            log.explain = "Delete Geofence";
            log.logtime = DateTime.Parse("2014/02/17 10:35:00");
            log.type = (long)OperateType.LOGOUT;

            OperateLogDBInterface inter = new OperateLogDBInterface();
            long id = inter.AddOperateLog(companyid, log);
            Assert.AreEqual(8, id);
        }

        [Test]
        public void testGetLogByType()
        {
            string companyid = "ABCSoft";
            long typeid = 2;

            List<Operate_Log> logs = new List<Operate_Log>();

            OperateLogDBInterface inter = new OperateLogDBInterface();
            logs = inter.GetOperateLogByType(companyid, typeid);
            Assert.AreEqual(6, logs[0].pkid);
            Assert.AreEqual(2, logs[0].type);
        }

        [Test]
        public void testGetOperateLogByDate()
        {
            string companyid = "ABCSoft";
            DateTime starttime = DateTime.Parse("2014/02/17 10:00:00");
            DateTime endtime = DateTime.Parse("2014/02/18 20:00:00");

            List<Operate_Log> logs = new List<Operate_Log>();

            OperateLogDBInterface inter = new OperateLogDBInterface();
            logs = inter.GetOperateLogByDate(companyid, starttime, endtime);
            Assert.AreEqual(3, logs.Count);
            //Assert.AreEqual(1, logs[0].pkid);
            //Assert.AreEqual(1, logs[0].type);
            //Assert.AreEqual(6, logs[1].pkid);
            //Assert.AreEqual(2, logs[1].type);
            Assert.AreEqual(6, logs[0].pkid);
            Assert.AreEqual(2, logs[0].type);
            Assert.AreEqual(7, logs[1].pkid);
            Assert.AreEqual(3, logs[1].type);
            Assert.AreEqual(8, logs[2].pkid);
            Assert.AreEqual(4, logs[2].type);
        }

        [Test]
        public void testDeleteLogByType()
        {
            string companyid = "ABCSoft";
            long typeid = 4;

            List<Operate_Log> logs = new List<Operate_Log>();

            OperateLogDBInterface inter = new OperateLogDBInterface();
            inter.DeleteOperateLogByType(companyid, typeid);
        }

        [Test]
        public void testDeleteLogByDate()
        {
            string companyid = "ABCSoft";
            DateTime starttime = DateTime.Parse("2014/02/17 10:00:00");
            DateTime endtime = DateTime.Parse("2014/02/18 20:00:00");

            List<Operate_Log> logs = new List<Operate_Log>();

            OperateLogDBInterface inter = new OperateLogDBInterface();
            inter.DeleteOperateLogByDate(companyid, starttime, endtime);
        }
    }
}