using FleetManageToolWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FleetManageToolWebRole.DB_interface
{
    public class DomainDBInterface : Controller
    {
        public string GetResourceUrl(string DomainUrl)
        {
            
            string result = "default";
            string domainUrl = DomainUrl.Trim();
            var db = new FleetManageToolDBContext();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;

            IEnumerable<DomainSetting> domainSettings = from DomainSettingDB in db.DomainSetting
                                          where domainUrl.Equals(DomainSettingDB.domainurl)
                                          select DomainSettingDB;

            foreach (DomainSetting domainSetting in domainSettings)
            {
               // result = domainSetting.g;
            }
                return result;
            
        }



        //删除domain
        public void DeleteDomainSetting(long pkid)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                DomainSetting DomainSettingTemp = dbContext.DomainSetting.First(DomainSetting => DomainSetting.pkid == pkid);
                dbContext.DomainSetting.Remove(DomainSettingTemp);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("删除用户异常," + e.Message);
            }
        }


        public void editDomainSetting(long pkid,DomainSetting editedDomainSetting,string logoflag) {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {                
                       IEnumerable<DomainSetting> domainSettings = from DomainSettingDB in dbContext.DomainSetting
                                                  
                                               where DomainSettingDB.pkid == pkid
                                                   select DomainSettingDB;
                //从数据库获取数据
                foreach (DomainSetting domain in domainSettings)
                {
                    domain.domainurl = editedDomainSetting.domainurl;
                    if("1"==logoflag){
                     domain.loginlogo=editedDomainSetting.loginlogo;
                    }
                }
                dbContext.SaveChanges();
                
                
              
            }
            catch (Exception e)
            {
                throw new DBException("删除用户异常," + e.Message);
            }
        }


        //获取所有Domain的信息
        public List<DomainSetting> GetDomainSettings()
        {
          
            var dbContext = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
       
            try
            {
                IEnumerable<FleetManageToolWebRole.Models.DomainSetting> tenants = from DomainSettingDB in dbContext.DomainSetting
                                                                                   select DomainSettingDB;
                List<FleetManageToolWebRole.Models.DomainSetting> result = new List<FleetManageToolWebRole.Models.DomainSetting>();
                foreach (FleetManageToolWebRole.Models.DomainSetting tenantTemp in tenants)
                {
                    result.Add(tenantTemp);
                }
                return result;
            }
            catch (Exception e)
            {  
                throw new DBException("获取所有Domain异常," + e.Message);
            }
        }


        //获取所有Domain的信息
        public String CheckDomainUrl(string pkid,string url)
        {
       
            var dbContext = new FleetManageToolWebRole.Models.FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            IEnumerable<FleetManageToolWebRole.Models.DomainSetting> tenants = null;
            try
            {
                if (null != pkid && "" != pkid.Trim())
                {
                    long id = System.Int64.Parse(pkid);
                    tenants = from DomainSettingDB in dbContext.DomainSetting
                              where (DomainSettingDB.pkid != id) && DomainSettingDB.domainurl == url
                              select DomainSettingDB;
                }
                else {
                    tenants = from DomainSettingDB in dbContext.DomainSetting
                              where  DomainSettingDB.domainurl == url
                              select DomainSettingDB;
                }


              
             
                foreach (FleetManageToolWebRole.Models.DomainSetting tenantTemp in tenants)
                {       
                            return "NG";
         
                }
                return "OK";
            }
            catch (Exception e)
            {
                throw new DBException("检验Domain异常," + e.Message);
            }
        }

        public void AddDomainSetting(DomainSetting domainSetting)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.DomainSetting.Add(domainSetting);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("添加DomainSetting异常," + e.Message);
            }
        }


        public DomainSetting GetResource(string DomainUrl)
        {

            DomainSetting result = null;
            string domainUrl = DomainUrl.Trim();
            var db = new FleetManageToolDBContext();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;

            IEnumerable<DomainSetting> domainSettings = from DomainSettingDB in db.DomainSetting
                                                        where domainUrl.Equals(DomainSettingDB.domainurl) 
                                                        select DomainSettingDB;

            foreach (DomainSetting domainSetting in domainSettings)
            {
                result = domainSetting;
            }
            return result;

        }
    }
}
