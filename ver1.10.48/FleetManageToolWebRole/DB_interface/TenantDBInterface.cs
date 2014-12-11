using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Util;
using System.Threading;
using System.Data.Linq;

namespace FleetManageToolWebRole.DB_interface
{
    public class TenantDBInterface
    {
        //测试OK
        //通过登录用户获取公司ID
        public string GetCompanyIDByUserID(long userID)
        {
            DebugLog.Debug("FleetManageToolWorker GetCompanyIDByUserID Start userID = " + userID);
            try
            {
                string result = "";
                var db = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Tenant> tenants = from tenantDB in db.Tenant
                                              join userDB in db.FleetUser on tenantDB.pkid equals userDB.tenantid
                                              where userDB.pkid == userID
                                              select tenantDB;
                foreach (Tenant tenantTemp in tenants)
                {
                    result = tenantTemp.companyid;
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker GetCompanyIDByUserID Start " + e.Message);
                throw new DBException("通过用户ID获取租户ID异常," + e.Message);
            }
        }

        public bool IsCustomerRegistered(String customerid)
        {
            DebugLog.Debug("FleetManageToolWorker IsCustomerRegistered Start tenantID = " + customerid);
            try
            {
                var db = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
                IEnumerable<FleetManageToolWebRole.Models.Customer> customers = from customerDB in db.Customer
                                                                                where customerDB.guid == customerid
                                                                                select customerDB;
                if (null == customers || 0 == customers.Count())
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker IsCustomerRegistered End " + e.Message);
                throw new DBException("通过租户ID获取租户异常," + e.Message);
            }
        }

        //Add by LiYing start for check Obu is existed
        public bool isOBUExist(string esn, string regkey) {
            DebugLog.Debug("FleetManageToolWorker isOBUExist Start esn = " + esn + ";regkey=" + regkey);
            try
            {
                var db = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
                IEnumerable<FleetManageToolWebRole.Models.Obu> obu = from obuDb in db.Obu
                                                                                where obuDb.id == esn && obuDb.regkey == regkey 
                                                                                select obuDb;
                if (null == obu || 0 == obu.Count())
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker isOBUExist End " + e.Message);
                throw new DBException("判断OBU是否存在异常" + e.Message);
            }
        }

        //Add by LiYing end for check Obu is existed
        public long AddACustomer(Customer customer)
        {
            DebugLog.Debug("FleetManageToolWorker IsCustomerRegistered Start tenantID = " + customer.guid);
            try
            {
                var dbContext = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
                Customer result = dbContext.Customer.Add(customer);
                dbContext.SaveChanges();
                return result.pkid;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker IsCustomerRegistered End " + e.Message);
                throw new DBException("通过租户ID获取租户异常," + e.Message);
            }
        }

        public List<Customer> GetCustomersByTenantID(long tenantID)
        {
            DebugLog.Debug("FleetManageToolWorker GetCustomersByTenantID Start tenantID = " + tenantID);
            try
            {
                List<Customer> result = new List<Customer>();
                DebugLog.Debug("FleetManageToolWorker GetCustomersByTenantID Running");
                var db = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                IEnumerable<FleetManageToolWebRole.Models.Customer> customers = from customerDB in db.Customer
                                                                                where customerDB.tenantid == tenantID
                                                                                select customerDB;
                foreach (FleetManageToolWebRole.Models.Customer customerTemp in customers)
                {
                    result.Add(customerTemp);
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker GetCustomersByTenantID Start " + e.Message);
                throw new DBException("通过租户ID获取租户异常," + e.Message);
            }
        }

        /// <summary>
        /// fengpan 20140507
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public List<Customer> GetCustomersByCompanyID(string companyID)
        {
            DebugLog.Debug("FleetManageToolWorker GetCustomersByTenantID Start tenantID = " + companyID);
            try
            {
                List<Customer> result = new List<Customer>();
                DebugLog.Debug("FleetManageToolWorker GetCustomersByTenantID Running");
                var db = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                IEnumerable<FleetManageToolWebRole.Models.Customer> customers = from customerDB in db.Customer
                                                                                join tenantDB in db.Tenant on customerDB.tenantid equals tenantDB.pkid 
                                                                                where tenantDB.companyid == companyID
                                                                                select customerDB;
                foreach (FleetManageToolWebRole.Models.Customer customerTemp in customers)
                {
                    result.Add(customerTemp);
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker GetCustomersByTenantID Start " + e.Message);
                throw new DBException("通过租户ID获取租户异常," + e.Message);
            }
        }

        //chenyangwen 2014/3/6
        public Tenant GetTenantByTenantID(long tenantID)
        {
            DebugLog.Debug("FleetManageToolWorker GetTenantByTenantID Start tenantID = " + tenantID);
            try
            {
                Tenant result = null;
                var db = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Tenant> tenants = from tenantDB in db.Tenant
                                              where tenantDB.pkid == tenantID
                                              select tenantDB;
                foreach (Tenant tenantTemp in tenants)
                {
                    result = tenantTemp;
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker GetTenantByTenantID Start " + e.Message);
                throw new DBException("通过租户ID获取租户异常," + e.Message);
            }
        }

        //通过公司ID判断公司是否注册
        public Tenant IsCompanyRegisted(Tenant tenant)
        {
            DebugLog.Debug("FleetManageToolWorker IsCompanyRegisted Start tenant.companyid = " + tenant.companyid);
            try
            {
                Tenant result = null;
                var db = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
                db.Configuration.LazyLoadingEnabled = false;
                db.Configuration.ProxyCreationEnabled = false;
                IEnumerable<Tenant> tenants = null;
                if (null != tenant.companyid)
                {
                    tenants = from tenantDB in db.Tenant
                              where tenantDB.companyid == tenant.companyid
                              select tenantDB;
                }
                else if (null != tenant.companyname)
                {
                    tenants = from tenantDB in db.Tenant
                              where tenantDB.companyname == tenant.companyname
                              select tenantDB;
                }
                foreach (Tenant tenantTemp in tenants)
                {
                    result = tenantTemp;
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker IsCompanyRegisted Start " + e.Message);
                throw new DBException("判断租户是否存在异常," + e.Message);
            }
        }

        //注册公司
        public long RegistTenant(Tenant register)
        {
            DebugLog.Debug("FleetManageToolWorker RegistTenant Start register.companyid = " + register.companyid);
            try
            {
                //给数据库插入数据
                var dbContext = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                Tenant result = dbContext.Tenant.Add(register);
                dbContext.SaveChanges();

                return result.pkid;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker RegistTenant Start " + e.Message);
                throw new DBException("插入租入异常," + e.Message);
            }
        }

        //修改公司
        public void UpdateTenant(Tenant updateTenant, long logoID)
        {
            var dbContext = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                long tenantID = GetTenantIDByCompanyID(updateTenant.companyid);
                IEnumerable<Tenant> tenants = from tenantDB in dbContext.Tenant
                                              where tenantDB.pkid == tenantID
                                              select tenantDB;
                //从数据库获取数据
                foreach (Tenant tenantTemp in tenants)
                {
                    tenantTemp.companyname = updateTenant.companyname;
                    //tenantTemp.email = updateTenant.email;
                    tenantTemp.telephone = updateTenant.telephone;
                    tenantTemp.introduction = updateTenant.introduction;
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker GetTenants Start " + e.Message);
                throw new DBException("更新租户异常," + e.Message);
            }
        }
        //fengpan 20140704 
        public void UpdateTenantEmail(Tenant updateTenant)
        {
            var dbContext = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                long tenantID = GetTenantIDByCompanyID(updateTenant.companyid);
                IEnumerable<Tenant> tenants = from tenantDB in dbContext.Tenant
                                              where tenantDB.pkid == tenantID
                                              select tenantDB;
                //从数据库获取数据
                foreach (Tenant tenantTemp in tenants)
                {
                    tenantTemp.email = updateTenant.email;
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                DebugLog.Debug("TenantDbInterface UpdateTenantEmail Exception= " + e.Message);
                throw new DBException("更新租户通知邮箱异常," + e.Message);
            }
        }

        //重新设置公司的密码
        public void UpdateTenantStatus(string tenantID, string status)
        {
            DebugLog.Debug("FleetManageToolWorker GetTenants tenantID =  " + tenantID + " status = " + status);
            var dbContext = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<Tenant> tenants = from tenantDB in dbContext.Tenant
                                              where tenantDB.companyid == tenantID
                                              select tenantDB;
                foreach (Tenant tenantTemp in tenants)
                {
                    tenantTemp.status = status;
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker GetTenants Start " + e.Message);
                throw new DBException("更新租户状态异常," + e.Message);
            }
        }

        //获取所有公司的信息
        public List<Tenant> GetTenants()
        {
            DebugLog.Debug("FleetManageToolWorker GetTenants Start ");
            var dbContext = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            DebugLog.Debug("FleetManageToolWorker GetTenants Running ");
            try
            {
                IEnumerable<FleetManageToolWebRole.Models.Tenant> tenants = from tenantDB in dbContext.Tenant
                                              select tenantDB;
                List<FleetManageToolWebRole.Models.Tenant> result = new List<FleetManageToolWebRole.Models.Tenant>();
                foreach (FleetManageToolWebRole.Models.Tenant tenantTemp in tenants)
                {
                    result.Add(tenantTemp);
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker GetTenants Start " + e.Message);
                throw new DBException("获取所有租户异常," + e.Message);
            }
        }

        //通过公司ID获取公司的ID
        public long GetTenantIDByCompanyID(string companyID)
        {
            DebugLog.Debug("FleetManageToolWorker GetTenantIDByCompanyID companyID = " + companyID);
            long tenantID = 0;
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<Tenant> tenants = from tenantDB in db.Tenant
                                              where tenantDB.companyid == companyID
                                              select tenantDB;
                foreach (Tenant tenantTemp in tenants)
                {
                    tenantID = tenantTemp.pkid;
                    break;
                }
                return tenantID;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker GetTenantIDByCompanyID Start " + e.Message);
                throw new DBException("通过公司ID获取租户ID异常," + e.Message);
            }
        }

        public Tenant GetTenantByCompanyID(string companyID)
        {
            DebugLog.Debug("FleetManageToolWorker GetTenantByCompanyID Start companyID = " + companyID);
            Tenant result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                IEnumerable<Tenant> tenants = from tenantDB in db.Tenant
                                              where tenantDB.companyid == companyID
                                              select tenantDB;
                foreach (Tenant tenantTemp in tenants)
                {
                    result = tenantTemp;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker GetTenantByCompanyID Start " + e.Message);
                throw new DBException("通过公司ID获取租户异常," + e.Message);
            }
        }

        //Add by LiYing Start
        //查询所有OBU信息
        public List<Obu_Check> SearchAllOBU()
        {
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                List<Obu_Check> list = new List<Obu_Check>();
                IEnumerable<Obu_Check> obu_checks = from obu_CheckDB in db.Obu_Check
                                                    select obu_CheckDB;
                foreach (Obu_Check obu in obu_checks)
                {
                    list.Add(obu);
                }
                return list;
            }
            catch(Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker SearchAllOBU Start " + e.Message);
                throw new DBException("查询所有OBU信息," + e.Message);
            }
        }


        //插入CSV数据到数据库中
        public void SaveCSVData(List<Obu_Check> list)
        {
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                DateTime now = DateTime.Now.ToUniversalTime();
                foreach(Obu_Check obu in list)
                {
                    obu.createdate = now;
                    obu.status = 0;
                    db.Obu_Check.Add(obu);
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker SaveCSVData Start " + e.Message);
                throw new DBException("插入CSV数据," + e.Message);
            }
        }

        //查询所有HW_Model信息
        public List<HW_Model> GetAllHW_Model()
        {
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                List<HW_Model> list = new List<HW_Model>();
                IEnumerable<HW_Model> hw_models = from hw_model in db.HW_Model
                                                   select hw_model;
                foreach (HW_Model model in hw_models)
                {
                    list.Add(model);
                }
                return list;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker GetAllHW_Model Start " + e.Message);
                throw new DBException("查询所有HW_Model信息," + e.Message);
            }
        }
		//Add by LiYing End
        //Add by LiYing Start for OBU

        //根据翻页页数查找OBUCheck
        public List<Obu_Check> getPageOBUData(int pageIndex, int pageSize)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                int skipCount = (pageIndex - 1) >= 0 ? (pageIndex - 1) * pageSize : 0;

                List<Obu_Check> result = new List<Obu_Check>();
                IEnumerable<Obu_Check> obus = (from obu_CheckDB in dbContext.Obu_Check
                                               orderby obu_CheckDB.createdate descending, obu_CheckDB.byteid ascending
                                               select obu_CheckDB).Skip(skipCount).Take(pageSize);
                foreach (Obu_Check obu in obus)
                {
                    TimeSpan backTimeZone = TimeZoneInfo.Local.BaseUtcOffset;
                    obu.createdate = obu.createdate.Add(backTimeZone).ToUniversalTime();
                    result.Add(obu);
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker getPageOBUData Start " + e.Message);
                throw new DBException("根据翻页页数查找OBUCheck," + e.Message);
            }
            
        }

        //删除OBU-Check
        public string deleteOBU(long id)
        {
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                // IEnumerable<Obu_Check> obuTemp = db.Obu_Check.Where(obu => obu.status != 1 && obu => obu.pkid == id);
                IEnumerable<Obu_Check> obuTemp = from obu_CheckDB in db.Obu_Check
                                                 where obu_CheckDB.status != 1 && obu_CheckDB.pkid == id
                                                 select obu_CheckDB;
                
                if (null != obuTemp && obuTemp.Count() > 0) 
                {
                    foreach(Obu_Check obu in obuTemp)
                    {
                        db.Obu_Check.Remove(obu);
                    }
                    
                    db.SaveChanges();
                    return "OK";
                }
                return "error";
                
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker deleteOBU Start " + e.Message);
                throw new DBException("删除OBU-Check," + e.Message);
            }
            
        }

        //根据ID获取ObuCheck
        public Obu_Check getOBUById(long id)
        {
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            Obu_Check result = null;
            try
            {
                IEnumerable<Obu_Check> obu = from obuDB in db.Obu_Check
                                    where obuDB.pkid == id
                                    select obuDB;
                foreach (Obu_Check obuTemp in obu)
                {
                    result = obuTemp;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker getOBUById Start " + e.Message);
                throw new DBException("根据ID获取ObuCheck," + e.Message);
            }
        }

        //根据关键字查询OBU
        public List<Obu_Check> searchOBUByKey(string key, int pageIndex)
        {
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            List<Obu_Check> result = new List<Obu_Check>();
            int pageSize = 100;
            int skipCount = (pageIndex - 1) >= 0 ? (pageIndex - 1) * pageSize : 0;
            
            try
            {
                IEnumerable<Obu_Check> obu = (from obuDB in db.Obu_Check
                                             where obuDB.byteid.Contains(key)
                                             || obuDB.labelid.Contains(key) || obuDB.regkey.Contains(key)
                                              orderby obuDB.createdate descending, obuDB.byteid ascending 
                                              select obuDB).Skip(skipCount).Take(pageSize);
                foreach (Obu_Check obuTemp in obu)
                {
                    TimeSpan backTimeZone = TimeZoneInfo.Local.BaseUtcOffset;
                    obuTemp.createdate = obuTemp.createdate.Add(backTimeZone).ToUniversalTime();
                    result.Add(obuTemp);
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker getOBUById Start " + e.Message);
                throw new DBException("根据ID获取ObuCheck," + e.Message);
            }
        }
        //获取符合搜索条件OBU的数量
        public int searchOBUByKeyCount(string key)
        {
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            int result = 0;

            try
            {
                int count = (from obuDB in db.Obu_Check
                                              where obuDB.byteid.Contains(key)
                                              || obuDB.labelid.Contains(key) || obuDB.regkey.Contains(key)
                                              select obuDB).Count();
                result = count;
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker getOBUById Start " + e.Message);
                throw new DBException("根据ID获取ObuCheck," + e.Message);
            }
        }
        //Add by LiYing End for OBU


        //caoyandong
        //获取数据库MMY数据
        public List<MMY> getMMYbylanguage(string language)
        {
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            List<MMY> result = new List<MMY>();

            try
            {
                IEnumerable<MMY> mmy = (from mmyDB in db.MMY
                                        where mmyDB.language == language
                                        select mmyDB);
                foreach (MMY mmyTemp in mmy)
                {
                    result.Add(mmyTemp);
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker getMMYbylanguage Start " + e.Message);
                throw new DBException("根据语言获取MMY," + e.Message);
            }
        }

        //caoyandong
        //插入MMY数据到数据库中
        public void SaveMMYData(List<string> list, long listnum)
        {
            long num = listnum;
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                foreach (string data in list)
                {
                    ++num;
                    string temp = data;
                    if (temp.Contains(','))
                    {
                        MMY mmy_en = new MMY();
                        MMY mmy_cn = new MMY();
                        if (temp.LastIndexOf(",") == temp.Length - 1)
                        {
                            temp = temp.Remove(temp.Length - 1);
                        }
                        string[] dataArray = temp.Split(',');
                        mmy_en.language = "ZH-EN";
                        mmy_en.mmyMake = dataArray[0];
                        mmy_en.mmyModel = dataArray[1];
                        mmy_en.mmyYear = dataArray[2];
                        mmy_en.mmyIndex = num;

                        mmy_cn.language = "ZH-CN";
                        mmy_cn.mmyMake = dataArray[3];
                        mmy_cn.mmyModel = dataArray[4];
                        mmy_cn.mmyYear = dataArray[5];
                        mmy_cn.mmyIndex = num;

                        db.MMY.Add(mmy_en);
                        db.MMY.Add(mmy_cn);
                    }

                }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker SaveMMYData Start " + e.Message);
                throw new DBException("插入MMY数据," + e.Message);
            }
        }

        //caoyandong
        //删除MMY数据
        public void DeleteMMYData(List<MMY> mmytemp)
        {
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                foreach (MMY m in mmytemp)
                {
                    IEnumerable<MMY> mmy = (from mmyDB in db.MMY
                                            where mmyDB.mmyIndex == m.mmyIndex
                                            select mmyDB);
                    foreach (MMY t in mmy)
                    {
                        db.MMY.Remove(t);
                    }
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker DeleteMMYData Start " + e.Message);
                throw new DBException("插入MMY数据," + e.Message);
            }
        }

        //Add by LiYing start
        //删除车队
        public void DeleteTenant(long tanantID)
        {
            var db = new FleetManageToolDBContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;

            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;

                var deleteOBU = "delete from Obu where tenantid =" + tanantID;
                var deleteCustomer = "delete from Customer where tenantid =" + tanantID;
                var deleteTanant = "delete from Tenant where pkid =" + tanantID;

                db.Database.ExecuteSqlCommand(deleteOBU);
                db.Database.ExecuteSqlCommand(deleteCustomer);
                db.Database.ExecuteSqlCommand(deleteTanant);

              
            }
            catch (Exception e)
            {
                DebugLog.Debug("FleetManageToolWorker DeleteTenant Start " + e.Message);
                throw new DBException("删除车队," + e.Message);
            
            }
        }
        //Add by LiYing end
    }
}