using FleetManageToolWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.DB_interface
{
    public class OperateLogDBInterface
    {
        //add operator log
        public long AddOperateLog(String companyid, Operate_Log log)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                TenantDBInterface dbInterface = new TenantDBInterface();
                log.tenantid = dbInterface.GetTenantIDByCompanyID(companyid);
                dbContext.Operate_Log.Add(log);
                dbContext.SaveChanges();

                return log.pkid;
            }
            catch (Exception)
            {
                throw new DBException("添加操作LOG异常");
            }
        }

        //delete type's log
        public void DeleteOperateLogByType(string companyid, long typeid)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                TenantDBInterface dbInterface = new TenantDBInterface();
                long tenantid = dbInterface.GetTenantIDByCompanyID(companyid);
                IEnumerable<Operate_Log> logs;
                if (typeid <= 0)
                {
                    logs = dbContext.Operate_Log.Where(Operate_Log => Operate_Log.tenantid == tenantid);
                }
                else
                {
                    logs = dbContext.Operate_Log.Where(Operate_Log => Operate_Log.type == typeid && Operate_Log.tenantid == tenantid);
                }
                foreach (Operate_Log i in logs)
                {
                    dbContext.Operate_Log.Remove(i);
                }
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw new DBException("按类型删除操作LOG异常");
            }
        }

        //delete log by the date.
        public void DeleteOperateLogByDate(string companyid, DateTime stime, DateTime etime)
        {
            DateTime starttime;
            DateTime endtime;
            if (null == stime)
            {
                string sstime = "1900/01/01 00:00:00";
                starttime = DateTime.Parse(sstime);
            }
            else
            {
                starttime = stime;
            }

            if (null == etime)
            {
                DateTime now = DateTime.Now;
                string nowstr = now.ToString("yyyy/MM/dd HH:mm:ss");
                endtime = DateTime.Parse(nowstr);
            }
            else
            {
                endtime = etime;
            }

            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;

            try
            {
                TenantDBInterface dbInterface = new TenantDBInterface();
                long tenantid = dbInterface.GetTenantIDByCompanyID(companyid);
                var logs = dbContext.Operate_Log.Where(Operate_Log => Operate_Log.logtime >= starttime &&
                                                        Operate_Log.logtime <= endtime &&
                                                        Operate_Log.tenantid == tenantid);
                foreach (Operate_Log i in logs)
                {
                    dbContext.Operate_Log.Remove(i);
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("按日期删除操作LOG异常," + e.Message);
            }
        }

        //get log by type
        public List<Operate_Log> GetOperateLogByType(string companyid, long typeid)
        {
            List<Operate_Log> result = new List<Operate_Log>();
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;

            try
            {
                TenantDBInterface dbInterface = new TenantDBInterface();
                long tenantid = dbInterface.GetTenantIDByCompanyID(companyid);

                IEnumerable<Operate_Log> logs;
                if (0 >= typeid)
                {
                    logs = from logDB in dbContext.Operate_Log
                           where logDB.tenantid == tenantid
                           select logDB;
                }
                else
                {
                    logs = from logDB in dbContext.Operate_Log
                           where logDB.tenantid == tenantid && logDB.type == typeid
                           select logDB;
                }

                foreach (Operate_Log i in logs)
                {
                    result.Add(i);
                }
            }
            catch (Exception e)
            {
                throw new DBException("按类型获取操作LOG异常," + e.Message);
            }
            
            return result;
        }

        //get log by the date.
        public List<Operate_Log> GetOperateLogByDate(string companyid, DateTime stime, DateTime etime)
        {
            DateTime starttime;
            DateTime endtime;
            if (null == stime)
            {
                string sstime = "1900/01/01 00:00:00";
                starttime = DateTime.Parse(sstime);
            }
            else
            {
                starttime = stime;
            }

            if (null == etime)
            {
                DateTime now = DateTime.Now;
                string nowstr = now.ToString("yyyy/MM/dd HH:mm:ss");
                endtime = DateTime.Parse(nowstr);
            }
            else
            {
                endtime = etime;
            }

            List<Operate_Log> result = new List<Operate_Log>();
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;

            try
            {
                TenantDBInterface dbInterface = new TenantDBInterface();
                long tenantid = dbInterface.GetTenantIDByCompanyID(companyid);

                IEnumerable<Operate_Log> logs;
                    logs = from logDB in dbContext.Operate_Log
                           where logDB.tenantid == tenantid && logDB.logtime >= starttime &&
                           logDB.logtime <= endtime
                           select logDB;

                foreach (Operate_Log i in logs)
                {
                    result.Add(i);
                }
            }
            catch (Exception e)
            {
                throw new DBException("按日期获取操作LOG异常," + e.Message);
            }

            return result;
        }
    }
}