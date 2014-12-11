using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Util;

namespace FleetManageToolWebRole.DB_interface
{
    public class UserDBInterface
    {
        public FleetUser LoginIsUser(string UserName, string Password)
        {
            //modified by caoyandong
            FleetUser result = null;
            string newUserName = UserName.Trim();
            string newPassword = Password.Trim();
            //modified by caoyandong
            var db = new FleetManageToolDBContext();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
                DBExceptionRetryStrategy.GetInstance().GetRetryPolicy().ExecuteAction(() =>
                {
                    IEnumerable<FleetUser> users = from UserDB in db.FleetUser
                                                   where newUserName.Equals(UserDB.username.ToLower()) && newPassword.Equals(UserDB.password)
                                                   select UserDB;
                    foreach (FleetUser userTemp in users)
                    {
                        result = userTemp;
                    }
                });
              
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("租户登陆判定异常," + e.Message);
            }
        }

        //获取所有用户
        public List<FleetUser> GetAllUser(string companyID)
        {
            List<FleetUser> result = new List<FleetUser>();
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<FleetUser> users = from userDB in dbContext.FleetUser
                                               join tenantDB in dbContext.Tenant on userDB.tenantid equals tenantDB.pkid
                                               where tenantDB.companyid == companyID
                                               select userDB;
                foreach (FleetUser userTemp in users)
                {
                    FleetUser_Role role = GetUserRoleByUserID(userTemp.pkid);
                    userTemp.FleetUser_Role.Add(role);
                    result.Add(userTemp);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取所有用户异常," + e.Message);
            }
        }

        public FleetUser_Role GetUserRoleByUserID(long userID)
        {
            TenantDBInterface tenantInterface = new TenantDBInterface();
            FleetUser_Role result = null;
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<FleetUser_Role> useroles = from useroleDB in dbContext.FleetUser_Role
                                                       join userDB in dbContext.FleetUser on useroleDB.userid equals userDB.pkid
                                                       where userDB.pkid == userID
                                                       select useroleDB;
                foreach (FleetUser_Role useroleTemp in useroles)
                {
                    result = new FleetUser_Role();
                    result.roleid = useroleTemp.roleid;
                    result.userid = useroleTemp.userid;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("获取用户权限异常," + e.Message);
            }
        }

        //添加用户
        public long AddUser(string companyID, FleetUser newUser)
        {
            //给数据库插入数据
            TenantDBInterface dbInterface = new TenantDBInterface();
            var dbContext = new FleetManageToolDBContext();
            newUser.tenantid = dbInterface.GetTenantIDByCompanyID(companyID);
            try
            {
                FleetUser result = dbContext.FleetUser.Add(newUser);
                dbContext.SaveChanges();
                return result.pkid;
            }
            catch (Exception e)
            {
                throw new DBException("添加用户异常," + e.Message);
            }
        }

        //编辑用户信息和修改用户密码
        public void UpdateUser(FleetUser updateUser)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<FleetUser> users = from userDB in dbContext.FleetUser
                                               where userDB.pkid == updateUser.pkid
                                               select userDB;
                //从数据库获取数据
                foreach (FleetUser userTemp in users)
                {
                    userTemp.username = updateUser.username;
                    userTemp.email = updateUser.email;
                    UpdateUserRole(updateUser.FleetUser_Role.ElementAt(0));
                    userTemp.telephone = updateUser.telephone;
                    userTemp.password = updateUser.password;
                    break;
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("更新用户异常," + e.Message);
            }
        }
        //编辑用户信息不修改用户密码
        public void UpdateUserExpPsw(FleetUser updateUser)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<FleetUser> users = from userDB in dbContext.FleetUser
                                               where userDB.pkid == updateUser.pkid
                                               select userDB;
                //从数据库获取数据
                foreach (FleetUser userTemp in users)
                {
                    userTemp.username = updateUser.username;
                    userTemp.email = updateUser.email;
                    UpdateUserRole(updateUser.FleetUser_Role.ElementAt(0));
                    userTemp.telephone = updateUser.telephone;
                    break;
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("更新用户异常," + e.Message);
            }
        }
        public void UpdateUserRole(FleetUser_Role role)
        {
            var dbContext = new FleetManageToolDBContext();
            TenantDBInterface tenantInterface = new TenantDBInterface();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<FleetUser_Role> users = from useroleDB in dbContext.FleetUser_Role
                                                    where useroleDB.userid == role.userid
                                                    select useroleDB;
                //从数据库获取数据
                foreach (FleetUser_Role roleTemp in users)
                {
                    roleTemp.roleid = role.roleid;
                    break;
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("更新用户权限异常," + e.Message);
            }
        }

        //删除用户
        public void DeleteUser(FleetUser deleteUser)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                FleetUser_Role useroleTemp = dbContext.FleetUser_Role.First(user_role => user_role.userid == deleteUser.pkid);
                dbContext.FleetUser_Role.Remove(useroleTemp);
                dbContext.SaveChanges();

                FleetUser userTemp = dbContext.FleetUser.First(fleetuser => fleetuser.pkid == deleteUser.pkid);
                dbContext.FleetUser.Remove(userTemp);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("删除用户异常," + e.Message);
            }
        }

        //通过用户名和密码获取管理员
        public Administrator GetAdminstration(string username, string password)
        {
            //modified by caoyandong
            Administrator result = null;
            string newUserName = username.Trim();
            string newPassword = password.Trim();
            //modified by caoyandong
            var db = new FleetManageToolDBContext();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<Administrator> adminstrations = from adminstrationDB in db.Administrator
                                                            where newUserName.Equals(adminstrationDB.username.ToLower()) && newPassword.Equals(adminstrationDB.password)
                                                            select adminstrationDB;
                foreach (Administrator admin in adminstrations)
                {
                    result = new Administrator();
                    result.username = admin.username;
                    result.password = admin.password;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("后台管理员登陆判定异常," + e.Message);
            }
        }

        public FleetUser GetUserByID(string companyID, long userID)
        {
            FleetUser result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<FleetUser> adminstrations = from userDB in db.FleetUser
                                                        join tenantDB in db.Tenant on userDB.tenantid equals tenantDB.pkid
                                                        where userDB.pkid == userID && tenantDB.companyid == companyID
                                                        select userDB;
                foreach (FleetUser userTemp in adminstrations)
                {
                    FleetUser_Role role = GetUserRoleByUserID(userTemp.pkid);
                    result = userTemp;
                    result.FleetUser_Role.Add(role);
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过用户ID获取用户异常," + e.Message);
            }
        }

        public FleetUser GetUserByUserName(string username)
        {
            FleetUser result = null;
            var db = new FleetManageToolDBContext();
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<FleetUser> adminstrations = from userDB in db.FleetUser
                                                        where userDB.username == username
                                                        select userDB;
                foreach (FleetUser userTemp in adminstrations)
                {
                    result = userTemp;
                    break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new DBException("通过用户名获取用户异常," + e.Message);
            }
        }

        //修改Admin密码
        public void UpdateAdminPassword(string adminName, string newpassword)
        {
            var dbContext = new FleetManageToolDBContext();
            dbContext.Configuration.ProxyCreationEnabled = false;
            dbContext.Configuration.LazyLoadingEnabled = false;
            try
            {
                IEnumerable<Administrator> adminstrations = from adminstrationDB in dbContext.Administrator
                                                            where adminstrationDB.username == adminName
                                                            select adminstrationDB;
                //从数据库获取数据
                foreach (Administrator admin in adminstrations)
                {
                    admin.password = newpassword;
                    break;
                }
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DBException("更新用户异常," + e.Message);
            }
        }
    }
}