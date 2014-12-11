using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageToolWebRole.Models;

namespace FleetManageToolWebRole.DB_interface
{
    public class LogoDBInterface
    {
        //chenyangwen
        public long InsertLogoAndGetId(Logo Logo)
        {
            try
            {
                //给数据库插入数据
                var dbContext = new FleetManageToolDBContext();
                Logo result = dbContext.Logo.Add(Logo);
                dbContext.SaveChanges();

                return result.pkid;
            }
            catch (Exception)
            {
                throw new DBException("插入图标异常");
            }
        }

        public void UpdateLogo(long logoID, Logo updateLogo)
        {
            try
            {
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Logo> logos = from logoDB in dbContext.Logo
                                          where logoDB.pkid == logoID
                                          select logoDB;
                foreach (Logo logoTemp in logos)
                {
                    logoTemp.data = updateLogo.data;
                    break;
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("更新图标异常," + e.Message);
            }
        }

        public byte[] GetLogoDataByLogoID(long logoID)
        {
            try
            {
                byte[] result = null;
                var dbContext = new FleetManageToolDBContext();
                dbContext.Configuration.ProxyCreationEnabled = false;
                dbContext.Configuration.LazyLoadingEnabled = false;
                IEnumerable<Logo> logos = from logoDB in dbContext.Logo
                                          where logoDB.pkid == logoID
                                          select logoDB;
                foreach (Logo logo in logos)
                {
                    result = logo.data;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取图标异常," + e.Message);
            }
        }
    }
}