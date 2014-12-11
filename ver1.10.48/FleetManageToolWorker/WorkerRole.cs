using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.ApplicationServer.Caching;
using FleetManageToolWebRole.Util;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models.API;
using FleetManageTool.WebAPI;
using Microsoft.WindowsAzure.Diagnostics;
using FleetManageToolWorker.Models;
using System.Threading.Tasks;

namespace FleetManageToolWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private bool onStopCalled = false;

        private bool returnedFromRunMethod = true;

        private bool bStoreDataToDBThreadRunningStatus = false;

        private int iSynThreadNumber = 10;

        private static Mutex muxConsoleCustomer = new Mutex();
        private static Mutex muxConsoleVehicle = new Mutex();

        public override void OnStop()
        {
            DebugLog.Debug("FleetManageToolWorker OnStop Start ");
            onStopCalled = true;
            while (false == returnedFromRunMethod)
            {
                DebugLog.Debug("FleetManageToolWorker OnStop returnedFromRunMethod ");
                CacheService service = new CacheService();
                service.CacheClear();
                DebugLog.Debug("FleetManageToolWorker OnStop End ");
                base.OnStop();
                System.Threading.Thread.Sleep(60000);
            }

        }

        /**
         * 初始化状态标志位
         */
        private void Initialize()
        {
            DebugLog.Debug("FleetManageToolWorker Initialize Start ");
            CacheService service = new CacheService();
            if (null == service.CacheGet(CacheKeyConstant.CustomerThreadRunningStatus))
            {
                service.CachePut(CacheKeyConstant.CustomerThreadRunningStatus, false);
            }
            if (null == service.CacheGet(CacheKeyConstant.VehicleThreadRunningStatus))
            {
                service.CachePut(CacheKeyConstant.VehicleThreadRunningStatus, false);
            }
            //if (null == service.CacheGet(CacheKeyConstant.StoreDataToDBThreadRunningStatus))
            //{
            //    service.CachePut(CacheKeyConstant.StoreDataToDBThreadRunningStatus, false);
            //}
            DebugLog.Debug("FleetManageToolWorker Initialize End ");
        }

        /**
         * 初始化已登录的租户
         */
        private List<FleetManageToolWebRole.Models.Tenant> InitializeLoginTenantsProcessStatus()
        {
            DebugLog.Debug("FleetManageToolWorker InitializeLoginTenantsProcessStatus Start");
            CacheService service = new CacheService();
            TenantDBInterface tenantDB = new TenantDBInterface();
            Dictionary<int, int> results = new Dictionary<int, int>();
            List<FleetManageToolWebRole.Models.Tenant> tenants = new List<FleetManageToolWebRole.Models.Tenant>();
            Object table = service.CacheGet(CacheKeyConstant.FleetCounterTable);
            if (null != table)
            {
                foreach (String companyid in ((Dictionary<String, int>)table).Keys)
                {
                    if (((Dictionary<String, int>)table)[companyid] > 0)
                    {
                        FleetManageToolWebRole.Models.Tenant tenant = tenantDB.GetTenantByCompanyID(companyid);
                        tenants.Add(tenant);
                    }
                }
                for (int i = 1; i <= tenants.Count; ++i)
                {
                    results.Add(i, 0);
                    DebugLog.Debug("FleetManageToolWorker InitializeLoginTenantsProcessStatus tenants.pkid = " + tenants[i-1].pkid);
                }
            }
            else
            {
                CacheUtil.InitFleetCounterTable();
                for (int i = 0; i <= 0; ++i)
                {
                    results.Add(i, 0);
                }
            }
            DebugLog.Debug("FleetManageToolWorker InitializeLoginTenantsProcessStatus Running results.Count = " + results.Count + " ;tenants.Count = " + tenants.Count);
            service.CachePut(CacheKeyConstant.LoginTenantsProcessStatusTable, results);
            service.CachePut(CacheKeyConstant.LoginTenants, tenants);
            DebugLog.Debug("FleetManageToolWorker InitializeLoginTenantsProcessStatus End");
            return tenants;
        }

        /**
         * 初始化所有租户
         */
        private List<FleetManageToolWebRole.Models.Tenant> InitializeTenantsProcessStatus()
        {
            DebugLog.Debug("FleetManageToolWorker InitializeTenantsProcessStatus Start ");
            CacheService service = new CacheService();
            TenantDBInterface tenantDB = new TenantDBInterface();
            List<FleetManageToolWebRole.Models.Tenant> tenants = tenantDB.GetTenants();
            service.CachePut(CacheKeyConstant.Tenants, tenants);
            Dictionary<int, int> results = new Dictionary<int, int>();
            for (int i = 1; i <= tenants.Count; ++i)
            {
                results.Add(i, 0);  //0是没有开始同步的，1是正在同步的，2是同步完成的。
                DebugLog.Debug("FleetManageToolWorker InitializeTenantsProcessStatus tenants.pkid = " + tenants[i-1].pkid);
            }
            service.CachePut(CacheKeyConstant.CustomerProcessStatusTable, results);
            DebugLog.Debug("FleetManageToolWorker InitializeTenantsProcessStatus End ");
            return tenants;
        }

        /**
         * 初始化同步数据的状态表
         */
        private List<FleetManageToolWebRole.Models.Tenant> InitializeStoreTenantsProcessStatus()
        {
            DebugLog.Debug("FleetManageToolWorker InitializeTenantsProcessStatus Start ");
            TenantDBInterface tenantDB = new TenantDBInterface();
            CacheService service = new CacheService();
            List<FleetManageToolWebRole.Models.Tenant> tenants = tenantDB.GetTenants();
            service.CachePut(CacheKeyConstant.StoreDataTenants, tenants);
            Dictionary<int, int> results = new Dictionary<int, int>();
            for (int i = 1; i <= tenants.Count; ++i)
            {
                results.Add(i, 0);//0是没有开始同步的，1是正在同步的，2是同步完成的。
                DebugLog.Debug("FleetManageToolWorker InitializeTenantsProcessStatus tenants.pkid = " + tenants[i-1].pkid);
            }
            service.CachePut(CacheKeyConstant.StoreDataToDBProcessStatusTable, results);
            DebugLog.Debug("FleetManageToolWorker InitializeTenantsProcessStatus End ");
            return tenants;
        }

        /**
         * 获取登录的租户
         */
        private List<FleetManageToolWebRole.Models.Tenant> GetLoginTenants()
        {
            DebugLog.Debug("FleetManageToolWorker GetLoginTenants Run Start ");
            CacheService service = new CacheService();
            List<FleetManageToolWebRole.Models.Tenant> tenants = (List<FleetManageToolWebRole.Models.Tenant>)service.CacheGet(CacheKeyConstant.LoginTenants);
            if (null == tenants)
            {
                tenants = InitializeLoginTenantsProcessStatus();
            }
            else if (!(Boolean)service.CacheGet(CacheKeyConstant.VehicleThreadRunningStatus))
            {
                tenants = InitializeLoginTenantsProcessStatus();
            }
            DebugLog.Debug("FleetManageToolWorker GetLoginTenants Run tenants = " + tenants.Count);
            DebugLog.Debug("FleetManageToolWorker GetLoginTenants Run End");
            return tenants;
        }

        /**
         * 获取所有租户
         */
        private List<FleetManageToolWebRole.Models.Tenant> GetTenants()
        {
            DebugLog.Debug("FleetManageToolWorker GetTenants Run Start ");
            CacheService service = new CacheService();
            List<FleetManageToolWebRole.Models.Tenant> tenants = (List<FleetManageToolWebRole.Models.Tenant>)service.CacheGet(CacheKeyConstant.Tenants);
            if (null == tenants)
            {
                tenants = InitializeTenantsProcessStatus();
            }
            else if (!(Boolean)service.CacheGet(CacheKeyConstant.CustomerThreadRunningStatus))
            {
                tenants = InitializeTenantsProcessStatus();
            }
            DebugLog.Debug("FleetManageToolWorker GetTenants Run tenants = " + tenants.Count);
            DebugLog.Debug("FleetManageToolWorker GetTenants Run End");
            return tenants;
        }

        /**
         * 获取同步数据的租户
         */
        public List<FleetManageToolWebRole.Models.Tenant> GetStoreTenants()
        {
            DebugLog.Debug("FleetManageToolWorker GetTenants Run Start ");
            //CacheService service = new CacheService();
            //List<FleetManageToolWebRole.Models.Tenant> tenants = (List<FleetManageToolWebRole.Models.Tenant>)service.CacheGet(CacheKeyConstant.StoreDataTenants);
            //if (null == tenants)
            //{
            //    tenants = InitializeStoreTenantsProcessStatus();
            //}
            //else if (!(Boolean)service.CacheGet(CacheKeyConstant.StoreDataToDBThreadRunningStatus))
            //{
            //    tenants = InitializeStoreTenantsProcessStatus();
            //}

            List<FleetManageToolWebRole.Models.Tenant> tenants = InitializeStoreTenantsProcessStatus();

            DebugLog.Debug("FleetManageToolWorker GetTenants Run tenants = " + tenants.Count);
            DebugLog.Debug("FleetManageToolWorker GetTenants Run End");
            return tenants;
        }

        /**
         * 同步租户的线程
         */
        private void ThreadOnSyncCustomers(Object state)
        {
            try
            {
                DebugLog.Debug("FleetManageToolWorker ThreadOnSyncCustomers Start, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                CacheService service = new CacheService();
                int exceptionCount = 0;
                List<FleetManageToolWebRole.Models.Tenant> tenants = (List<FleetManageToolWebRole.Models.Tenant>)service.CacheGet(CacheKeyConstant.Tenants);
                if (null == tenants)
                {
                    return;
                }
                DebugLog.Debug("FleetManageToolWorker ThreadOnSyncCustomers tenants.Count = " + tenants.Count + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                Dictionary<int, int> processStatus = null;// (Dictionary<int, bool>)service.CacheGet(CacheKeyConstant.CustomerProcessStatusTable);
                int indexValue = 0;// processStatus.FirstOrDefault(t => t.Value == false).Key;
                int indexValueSyning = 0;             
                while (0 < tenants.Count)
                {
                    System.Threading.Thread.Sleep(1);

                    //加锁
                    //DataCacheLockHandle lockHandleCustomers;
                    //Object myobject = service.GetAndLock(CacheKeyConstant.CustomerProcessStatusTable, new TimeSpan(0, 0, 10), out lockHandleCustomers);                   
                    muxConsoleCustomer.WaitOne(60000);
                    DebugLog.Debug("FleetManageToolWorker Lock For Customers, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                    processStatus = (Dictionary<int, int>)service.CacheGet(CacheKeyConstant.CustomerProcessStatusTable);

                    string strProcessStatus = "";
                    foreach (int key in processStatus.Keys)
                    {
                        strProcessStatus += ("" + key + " --" + processStatus[key] + " ; ");

                    }
                    DebugLog.Debug("FleetManageToolWorker ThreadOnSyncCustomers Running processStatus, " + strProcessStatus);

                    indexValue = processStatus.FirstOrDefault(t => t.Value == 0).Key;
                    indexValueSyning = processStatus.FirstOrDefault(t => t.Value == 1).Key;
                    if (0 == indexValue)
                    {
                        //解锁
                        //service.PutAndUnlock(CacheKeyConstant.CustomerProcessStatusTable, myobject, lockHandleCustomers, new TimeSpan(0, 0, 10));                       
                        //service.Unlock(CacheKeyConstant.CustomerProcessStatusTable, lockHandleCustomers);                    
                        muxConsoleCustomer.ReleaseMutex();
                        DebugLog.Debug("FleetManageToolWorker unLock For Customers, ThreadID = " + Thread.CurrentThread.ManagedThreadId);

                        if (0 == indexValueSyning)
                        {
                            break;
                        }

                        continue;      
                    }
                    DebugLog.Debug("FleetManageToolWorker ThreadOnSyncCustomers Running indexValue = " + indexValue + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                    
                    processStatus[indexValue] = 1;
                    service.CachePut(CacheKeyConstant.CustomerProcessStatusTable, processStatus);
                    //解锁
                    //service.PutAndUnlock(CacheKeyConstant.CustomerProcessStatusTable, myobject, lockHandleCustomers, new TimeSpan(0, 0, 10));                  
                    //service.Unlock(CacheKeyConstant.CustomerProcessStatusTable, lockHandleCustomers);                  
                    muxConsoleCustomer.ReleaseMutex();
                    DebugLog.Debug("FleetManageToolWorker unLock For Customers, ThreadID = " + Thread.CurrentThread.ManagedThreadId);

                    string companyid = "";
                    try
                    {
                        FleetManageToolWebRole.Models.Tenant tenant = tenants.ElementAt(indexValue - 1);
                        if (null != tenant)
                        {
                            DebugLog.Debug("FleetManageToolWorker ThreadOnSyncCustomers Running companyid = " + tenant.companyid + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                            companyid = tenant.companyid;
                            APIUtil.UpdateTenantData(companyid);
                        }
                        else
                        {
                            DebugLog.Debug("FleetManageToolWorker ThreadOnSyncCustomers Running tenant = null, indexValue - 1 = " + (indexValue - 1));
                        }

                        exceptionCount = 0;
                        DebugLog.Debug("FleetManageToolWorker ThreadOnSyncCustomers OK companyid = " + companyid);
                    }
                    catch (Exception exception)
                    {
                        DebugLog.Exception("FleetManageToolWorker ThreadOnSyncCustomers companyid = " + companyid);
                        DebugLog.Exception("FleetManageToolWorker ThreadOnSyncCustomers exception = " + exception.Message + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                        DebugLog.Exception("FleetManageToolWorker ThreadOnSyncCustomers exception = " + exception.StackTrace + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                        exceptionCount++;
                        //事不过三
                        if (exceptionCount > 3)
                        {
                            exceptionCount = 0;
                        }
                    }
                    finally
                    {
                        //processStatus = (Dictionary<int, bool>)service.CacheGet(CacheKeyConstant.CustomerProcessStatusTable);
                        //indexValue = processStatus.FirstOrDefault(t => t.Value == false).Key;
                        if (0 == exceptionCount)
                        {
                            //加锁
                            //DataCacheLockHandle lockHandleVehicleInfo;
                            //Object myobject = service.GetAndLock(CacheKeyConstant.CustomerProcessStatusTable, new TimeSpan(0, 0, 10), out lockHandleVehicleInfo);                 
                            muxConsoleCustomer.WaitOne(60000);
                            DebugLog.Debug("FleetManageToolWorker Lock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                            processStatus = (Dictionary<int, int>)service.CacheGet(CacheKeyConstant.CustomerProcessStatusTable);
                            processStatus[indexValue] = 2;
                            service.CachePut(CacheKeyConstant.CustomerProcessStatusTable, processStatus);
                            //解锁
                            //service.PutAndUnlock(CacheKeyConstant.CustomerProcessStatusTable, myobject, lockHandleVehicleInfo, new TimeSpan(0, 0, 10));
                            //service.Unlock(CacheKeyConstant.CustomerProcessStatusTable, lockHandleVehicleInfo);                                           
                            muxConsoleCustomer.ReleaseMutex();
                            DebugLog.Debug("FleetManageToolWorker unLock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                        }
                        else
                        {
                            //加锁
                            //DataCacheLockHandle lockHandleVehicleInfo;
                            //Object myobject = service.GetAndLock(CacheKeyConstant.CustomerProcessStatusTable, new TimeSpan(0, 0, 10), out lockHandleVehicleInfo);                 
                            muxConsoleCustomer.WaitOne(60000);
                            DebugLog.Debug("FleetManageToolWorker Lock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                            processStatus = (Dictionary<int, int>)service.CacheGet(CacheKeyConstant.CustomerProcessStatusTable);
                            processStatus[indexValue] = 0;
                            service.CachePut(CacheKeyConstant.CustomerProcessStatusTable, processStatus);
                            //解锁
                            //service.PutAndUnlock(CacheKeyConstant.CustomerProcessStatusTable, myobject, lockHandleVehicleInfo, new TimeSpan(0, 0, 10));
                            //service.Unlock(CacheKeyConstant.CustomerProcessStatusTable, lockHandleVehicleInfo);                                             
                            muxConsoleCustomer.ReleaseMutex();
                            DebugLog.Debug("FleetManageToolWorker unLock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                            DebugLog.Debug("FleetManageToolWorker ThreadOnSyncCustomers noce more companyid = " + companyid);
                        }
                    }
                }
                String time = System.Configuration.ConfigurationManager.AppSettings["CustomerTime"];
                DebugLog.Debug("FleetManageToolWorker ThreadOnSyncCustomers time = " + time + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(Int32.Parse(time));
                DebugLog.Debug("FleetManageToolWorker ThreadOnSyncCustomers End" + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
            }
            catch (Exception exception)
            {
                DebugLog.Exception("FleetManageToolWorker Syn ThreadOnSyncCustomers exception = " + exception.Message + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                DebugLog.Exception("FleetManageToolWorker Syn ThreadOnSyncCustomers exception = " + exception.StackTrace + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
            }
            finally
            {
                DebugLog.Debug("FleetManageToolWorker ThreadOnSyncCustomers finally - Line 1" + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                CacheService service = new CacheService();
                
                //加锁
                //DataCacheLockHandle lockHandleVehicleInfo;
                //Object myobject = service.GetAndLock(CacheKeyConstant.CustomerProcessStatusTable, new TimeSpan(0, 0, 10), out lockHandleVehicleInfo);                 
                muxConsoleCustomer.WaitOne(60000);
                DebugLog.Debug("FleetManageToolWorker Lock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                if ((Boolean)service.CacheGet(CacheKeyConstant.CustomerThreadRunningStatus))
                {
                    service.CachePut(CacheKeyConstant.CustomerThreadRunningStatus, false);
                }
                //解锁
                //service.PutAndUnlock(CacheKeyConstant.CustomerProcessStatusTable, myobject, lockHandleVehicleInfo, new TimeSpan(0, 0, 10));
                //service.Unlock(CacheKeyConstant.CustomerProcessStatusTable, lockHandleVehicleInfo);                                             
                muxConsoleCustomer.ReleaseMutex();
                DebugLog.Debug("FleetManageToolWorker unLock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                DebugLog.Debug("FleetManageToolWorker Syn Customers End............." + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
            }
        }

        /**
         * 同步车辆数据的线程
         */
        private void ThreadOnGetVehicleInfo(Object state)
        {
            try
            {
                DebugLog.Debug("FleetManageToolWorker ThreadOnGetVehicleInfo Start" + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                CacheService service = new CacheService();
                List<FleetManageToolWebRole.Models.Tenant> tenants = (List<FleetManageToolWebRole.Models.Tenant>)service.CacheGet(CacheKeyConstant.LoginTenants);
                if (null == tenants)
                {
                    return;
                }
                DebugLog.Debug("FleetManageToolWorker ThreadOnGetVehicleInfo tenants.Count = " + tenants.Count + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                Dictionary<int, int> processStatus = null;// (Dictionary<int, bool>)service.CacheGet(CacheKeyConstant.LoginTenantsProcessStatusTable);
                int indexValue = 0;// processStatus.FirstOrDefault(t => t.Value == false).Key;
                int indexValueSyning = 0;            
                while (0 < tenants.Count)
                {
                    System.Threading.Thread.Sleep(1);

                    //service.CachePut(CacheKeyConstant.VehicleThreadRunningStatus, true);
                    //加锁                    
                    muxConsoleVehicle.WaitOne(60000);
                    DebugLog.Debug("FleetManageToolWorker Lock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                    processStatus = (Dictionary<int, int>)service.CacheGet(CacheKeyConstant.LoginTenantsProcessStatusTable);

                    string strProcessStatus = "";
                    foreach (int key in processStatus.Keys)
                    {
                        strProcessStatus += ("" + key + " --" + processStatus[key] + " ; ");

                    }
                    DebugLog.Debug("FleetManageToolWorker ThreadOnGetVehicleInfo Running processStatus, " + strProcessStatus);

                    indexValue = processStatus.FirstOrDefault(t => t.Value == 0).Key;
                    indexValueSyning = processStatus.FirstOrDefault(t => t.Value == 1).Key;
                    if (0 == indexValue)
                    {
                        //解锁                                             
                        muxConsoleVehicle.ReleaseMutex();
                        DebugLog.Debug("FleetManageToolWorker unLock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);

                        if (0 == indexValueSyning)
                        {
                            break;
                        }

                        continue;                                                                 
                    }
                    DebugLog.Debug("FleetManageToolWorker ThreadOnGetVehicleInfo Running indexValue = " + indexValue + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);

                    //service.CachePut(CacheKeyConstant.CustomerThreadRunningStatus, true);
                    processStatus[indexValue] = 1;
                    service.CachePut(CacheKeyConstant.LoginTenantsProcessStatusTable, processStatus);
                    //解锁                                    
                    muxConsoleVehicle.ReleaseMutex();
                    DebugLog.Debug("FleetManageToolWorker unLock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                    string companyid = "";
                    try
                    {
                        FleetManageToolWebRole.Models.Tenant tenant = tenants.ElementAt(indexValue - 1);
                        if (null != tenant)
                        {
                            DebugLog.Debug("FleetManageToolWorker ThreadOnGetVehicleInfo Running companyid = " + tenant.companyid + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                            companyid = tenant.companyid;
                            APIUtil.GetVehiclesInfo(companyid);                         
                        }
                        else
                        {
                            DebugLog.Debug("FleetManageToolWorker ThreadOnGetVehicleInfo Running tenant = null, indexValue - 1 = " + (indexValue - 1));
                        }
                    }
                    catch (Exception exception)
                    {
                        DebugLog.Exception("FleetManageToolWorker ThreadOnGetVehicleInfo companyid = " + companyid);
                        DebugLog.Exception("FleetManageToolWorker ThreadOnGetVehicleInfo exception = " + exception.Message + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                        DebugLog.Exception("FleetManageToolWorker ThreadOnGetVehicleInfo exception = " + exception.StackTrace + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);                    
                    }
                    finally
                    {
                        //processStatus = (Dictionary<int, bool>)service.CacheGet(CacheKeyConstant.LoginTenantsProcessStatusTable);
                        //indexValue = processStatus.FirstOrDefault(t => t.Value == false).Key;

                        //加锁                       
                        muxConsoleVehicle.WaitOne(60000);
                        DebugLog.Debug("FleetManageToolWorker Lock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                        processStatus = (Dictionary<int, int>)service.CacheGet(CacheKeyConstant.LoginTenantsProcessStatusTable);
                        processStatus[indexValue] = 2;
                        service.CachePut(CacheKeyConstant.LoginTenantsProcessStatusTable, processStatus);
                        //解锁                                                         
                        muxConsoleVehicle.ReleaseMutex();
                        DebugLog.Debug("FleetManageToolWorker unLock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                        DebugLog.Debug("FleetManageToolWorker ThreadOnGetVehicleInfo OK companyid = " + companyid);
                    }
                }
                String time = System.Configuration.ConfigurationManager.AppSettings["VehicleTime"];
                DebugLog.Debug("FleetManageToolWorker ThreadOnGetVehicleInfo End time = " + time + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(Int32.Parse(time));
                DebugLog.Debug("FleetManageToolWorker ThreadOnGetVehicleInfo End" + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
            }
            catch (Exception exception)
            {
                DebugLog.Exception("FleetManageToolWorker Syn ThreadOnGetVehicleInfo exception = " + exception.Message + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                DebugLog.Exception("FleetManageToolWorker Syn ThreadOnGetVehicleInfo exception = " + exception.StackTrace + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
            }
            finally
            {
                DebugLog.Debug("FleetManageToolWorker ThreadOnGetVehicleInfo finally - Line1" + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                CacheService service = new CacheService();
                //加锁                       
                muxConsoleVehicle.WaitOne(60000);
                DebugLog.Debug("FleetManageToolWorker Lock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                if ((Boolean)service.CacheGet(CacheKeyConstant.VehicleThreadRunningStatus))
                {
                    service.CachePut(CacheKeyConstant.VehicleThreadRunningStatus, false);
                }
                //解锁                                                         
                muxConsoleVehicle.ReleaseMutex();
                DebugLog.Debug("FleetManageToolWorker unLock For Vehicle, ThreadID = " + Thread.CurrentThread.ManagedThreadId);
                DebugLog.Debug("FleetManageToolWorker Syn VehicleInfo End............." + ",  ThreadID = " + Thread.CurrentThread.ManagedThreadId);
            }
        }

        /**
         * 存储数据
         */
        public void StoreData(List<CustomerData> customerDatas)
        {
            DebugLog.Debug("FleetManageToolWorker StoreData Begin ");
            if (null == customerDatas)
            {
                DebugLog.Debug("FleetManageToolWorker StoreData End. customerDatas = null");
                return;
            }

            VehicleDBInterface vehicleDB = new VehicleDBInterface();
            DebugLog.Debug("FleetManageToolWorker StoreData Start customerDatas.Count = " + customerDatas.Count);
            foreach (CustomerData customerData in customerDatas)
            {
                foreach (FleetManageToolWebRole.Models.API.Vehicle vehicle in customerData.Vehicles)
                {
                    DebugLog.Debug("FleetManageToolWorker StoreData vehicle = " + vehicle.Id + ";vehicle.Trips = " + vehicle.Trips.Count + ";vehicle.Alerts = " + vehicle.Alerts.Count);
                    FleetManageToolWebRole.Models.Vehicle vehicledb = vehicleDB.GetVehicleByGUID(vehicle.Id);
                    FleetManageToolWebRole.Models.Trip recentTrip = vehicleDB.GetLastTrip(vehicledb.pkid);
                    APIUtil.ProcessTrip(vehicle.Trips, vehicledb, recentTrip, vehicle.status);

                    FleetManageToolWebRole.Models.Alert recentAlert = vehicleDB.GetLastAlert(vehicledb.pkid);
                    FleetManageToolWebRole.Models.Alert engineAlert = APIUtil.EngineAlertStatu(vehicle);
                    APIUtil.ProcessAlert(vehicle.Alerts, vehicledb, recentAlert, engineAlert);
                }
            }
            DebugLog.Debug("FleetManageToolWorker StoreData End ");
        }

        /**
         * 获取数据
         */
        public List<CustomerData> GetCustomerDatas(List<FleetManageToolWebRole.Models.Customer> customers)
        {
            DebugLog.Debug("FleetManageToolWorker GetCustomerDatas Start customers.Count = " + customers.Count);
            string customerGuid = "";
            try
            {
                TaskEngine<List<CustomerData>> task = new TaskEngine<List<CustomerData>>();
                task.AddPara("TaskName", "DataSyncTask");
                List<TaskParameter> paras = new List<TaskParameter>();
                foreach (FleetManageToolWebRole.Models.Customer customer in customers)
                {
                    TaskParameter para = new TaskParameter() { Name = "Customer", Value = customer.guid };
                    paras.Add(para);
                    customerGuid += customer.guid + " , ";
                }
                task.AddPara("Parameters", paras);

                List<CustomerData> result = task.execute().Result;
                DebugLog.Debug("FleetManageToolWorker GetCustomerDatas End");
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Exception("FleetManageToolWorker GetCustomerDatas exception customerGuid = " + customerGuid);
                DebugLog.Exception("FleetManageToolWorker GetCustomerDatas exception = " + e.Message);
                DebugLog.Exception("FleetManageToolWorker GetCustomerDatas exception = " + e.StackTrace);
                throw e;
            }
        }

        /**
         * 存储车辆数据
         */
        public void StoreFleetData(long tenantid)
        {
            try
            {
                DebugLog.Debug("FleetManageToolWorker StoreDataFleet Start tenantid = " + tenantid);
                TenantDBInterface tenantDB = new TenantDBInterface();
                List<FleetManageToolWebRole.Models.Customer> customers = tenantDB.GetCustomersByTenantID(tenantid);
                if (null == customers || 0 == customers.Count)
                {
                    return;
                }
                List<CustomerData> customerDatas = GetCustomerDatas(customers);
                StoreData(customerDatas);
                DebugLog.Debug("FleetManageToolWorker StoreDataFleet End ");
            }
            catch (Exception e)
            {
                DebugLog.Exception("FleetManageToolWorker StoreDataFleet exception = " + e.Message);
                DebugLog.Exception("FleetManageToolWorker StoreDataFleet exception = " + e.StackTrace);
                throw e;
            }
        }

        /**
         * 同步数据的线程
         */
        public void ThreadOnStoreDataToDB()
        {
            try
            {
                DebugLog.Debug("FleetManageToolWorker ThreadOnStoreDataToDB Start");
                CacheService service = new CacheService();
                int exceptionCount = 0;

                List<FleetManageToolWebRole.Models.Tenant> tenants = (List<FleetManageToolWebRole.Models.Tenant>)service.CacheGet(CacheKeyConstant.StoreDataTenants);
                if (null == tenants)
                {
                    return;
                }
                DebugLog.Debug("FleetManageToolWorker ThreadOnStoreDataToDB Running tenants.Count = " + tenants.Count);
                Dictionary<int, int> processStatus = null;// (Dictionary<int, int>)service.CacheGet(CacheKeyConstant.StoreDataToDBProcessStatusTable);
                int indexValue = 0;// processStatus.FirstOrDefault(t => t.Value == 0).Key;
                int indexValueSyning = 0;               
                while (0 < tenants.Count)
                {
                    System.Threading.Thread.Sleep(1);

                    processStatus = (Dictionary<int, int>)service.CacheGet(CacheKeyConstant.StoreDataToDBProcessStatusTable);

                    string strProcessStatus = "";
                    foreach (int key in processStatus.Keys)
                    {
                        strProcessStatus += ("" + key + " --" + processStatus[key] + " ; ");
                        
                    }
                    DebugLog.Debug("FleetManageToolWorker ThreadOnStoreDataToDB Running processStatus, " + strProcessStatus);

                    indexValue = processStatus.FirstOrDefault(t => t.Value == 0).Key;
                    indexValueSyning = processStatus.FirstOrDefault(t => t.Value == 1).Key;
                    if (0 == indexValue)
                    {                      
                        if (0 == indexValueSyning)
                        {
                            break;
                        }

                        continue;
                    }
                    DebugLog.Debug("FleetManageToolWorker ThreadOnStoreDataToDB Running indexValue = " + indexValue);

                    processStatus[indexValue] = 1;
                    service.CachePut(CacheKeyConstant.StoreDataToDBProcessStatusTable, processStatus);
                    long tenantPkid = -1;
                    try
                    {
                        FleetManageToolWebRole.Models.Tenant tenant = tenants.ElementAt(indexValue - 1);
                        if (null != tenant)
                        {
                            DebugLog.Debug("FleetManageToolWorker ThreadOnStoreDataToDB Running pkid = " + tenant.pkid);
                            tenantPkid = tenant.pkid;
                            StoreFleetData(tenantPkid);
                        }
                        else
                        {
                            DebugLog.Debug("FleetManageToolWorker ThreadOnStoreDataToDB Running tenant = null, indexValue - 1 = " + (indexValue - 1));
                        }

                        exceptionCount = 0;
                        DebugLog.Debug("FleetManageToolWorker ThreadOnStoreDataToDB  OK tenantPkid = " + tenantPkid);
                    }
                    catch (Exception exception)
                    {
                        DebugLog.Exception("FleetManageToolWorker ThreadOnStoreDataToDB tenantPkid = " + tenantPkid);
                        DebugLog.Exception("FleetManageToolWorker ThreadOnStoreDataToDB exception = " + exception.Message);
                        DebugLog.Exception("FleetManageToolWorker ThreadOnStoreDataToDB exception = " + exception.StackTrace);
                        exceptionCount++;
                        //事不过三
                        if (exceptionCount > 3)
                        {
                            exceptionCount = 0;
                        }
                    }
                    finally
                    {
                        if (0 == exceptionCount)
                        {
                            processStatus[indexValue] = 2;
                            service.CachePut(CacheKeyConstant.StoreDataToDBProcessStatusTable, processStatus);
                        }
                        else
                        {
                            processStatus[indexValue] = 0;
                            service.CachePut(CacheKeyConstant.StoreDataToDBProcessStatusTable, processStatus);
                            DebugLog.Debug("FleetManageToolWorker ThreadOnStoreDataToDB  once more tenantPkid = " + tenantPkid);
                        }
                    }
                }
                String time = System.Configuration.ConfigurationManager.AppSettings["StoreDataTime"];
                DebugLog.Debug("FleetManageToolWorker ThreadOnStoreDataToDB time = " + time);
                Thread.Sleep(Int32.Parse(time));
                DebugLog.Debug("FleetManageToolWorker ThreadOnStoreDataToDB End");
            }
            catch (Exception exception)
            {
                DebugLog.Exception("FleetManageToolWorker Syn ThreadOnStoreDataToDB exception = " + exception.Message);
                DebugLog.Exception("FleetManageToolWorker Syn ThreadOnStoreDataToDB exception = " + exception.StackTrace);
            }
            finally
            {
                //CacheService service = new CacheService();
                //if ((Boolean)service.CacheGet(CacheKeyConstant.StoreDataToDBThreadRunningStatus))
                //{
                //    service.CachePut(CacheKeyConstant.StoreDataToDBThreadRunningStatus, false);
                //}

                bStoreDataToDBThreadRunningStatus = false;

                DebugLog.Debug("FleetManageToolWorker Syn StoreData End.............");
            }
        }

        public override void Run()
        {
            while (true)
            {
                try
                {
                    DebugLog.Debug("FleetManageToolWorker Run Start");

                    CacheService service = new CacheService();
                    if (onStopCalled == true)
                    {
                        while (true)
                        {
                            if (!(Boolean)service.CacheGet(CacheKeyConstant.CustomerThreadRunningStatus) &&
                                !(Boolean)service.CacheGet(CacheKeyConstant.VehicleThreadRunningStatus) &&
                                !bStoreDataToDBThreadRunningStatus)
                            {
                                returnedFromRunMethod = false;
                                break;
                            }
                            System.Threading.Thread.Sleep(10000);
                        }
                        DebugLog.Debug("FleetManageToolWorker Run onStopCalled");
                        return;
                    }

                    Initialize();

                    //同步customer
                    if (!(Boolean)service.CacheGet(CacheKeyConstant.CustomerThreadRunningStatus))
                    {
                        DebugLog.Debug("FleetManageToolWorker Syn Customers Start.............");
                        GetTenants();
                        service.CachePut(CacheKeyConstant.CustomerThreadRunningStatus, true);

                        for (int i = 0; i < iSynThreadNumber; ++i)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadOnSyncCustomers));

                            //Thread threadCustomer = new Thread(() => ThreadOnSyncCustomers());
                            //threadCustomer.Start();
                        }
                    }

                    //获取车辆现状
                    if (!(Boolean)service.CacheGet(CacheKeyConstant.VehicleThreadRunningStatus))
                    {
                        DebugLog.Debug("FleetManageToolWorker Syn VehicleInfo Start.............");
                        GetLoginTenants();
                        service.CachePut(CacheKeyConstant.VehicleThreadRunningStatus, true);

                        for (int i = 0; i < iSynThreadNumber; ++i)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadOnGetVehicleInfo));

                            //Thread threadVehicle = new Thread(() => ThreadOnGetVehicleInfo());
                            //threadVehicle.Start();
                        }
                    }
                   
                    //抓取数据到数据库
                    if (!bStoreDataToDBThreadRunningStatus)
                    {
                        DebugLog.Debug("FleetManageToolWorker Syn StoreData Start.............");
                        bStoreDataToDBThreadRunningStatus = true;
                        try
                        {
                            GetStoreTenants();
                            Thread threadStore = new Thread(() => ThreadOnStoreDataToDB());
                            threadStore.Start();
                        }
                        catch (Exception exception)
                        {
                            DebugLog.Exception("FleetManageToolWorker Syn StoreData exception = " + exception.Message);
                            DebugLog.Exception("FleetManageToolWorker Syn StoreData exception = " + exception.StackTrace);
                            bStoreDataToDBThreadRunningStatus = false;
                        }
                    }
                    
                    DebugLog.Debug("FleetManageToolWorker Run End");
                    System.Threading.Thread.Sleep(10000);
                }
                catch (Exception exception)
                {
                    DebugLog.Exception("FleetManageToolWorker Run exception = " + exception.Message);
                    DebugLog.Exception("FleetManageToolWorker Run exception = " + exception.StackTrace);
                    System.Threading.Thread.Sleep(10000);
                }
            }
        }

        public override bool OnStart()
        {
            try
            {
                // 设置最大并发连接数
                ServicePointManager.DefaultConnectionLimit = 512;

                CacheService service = new CacheService();
                service.CacheClear();

                var diagnosticsConfig = DiagnosticMonitor.GetDefaultInitialConfiguration();
                diagnosticsConfig.Directories.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);

                diagnosticsConfig.Directories.DataSources.Add(
                        new DirectoryConfiguration
                        {
                            Path = RoleEnvironment.GetLocalResource("Log4Net").RootPath,
                            Container = "log4net",
                            DirectoryQuotaInMB = 1024
                        }
                );

                string crashLogPath = RoleEnvironment.GetLocalResource("CrashLogs").RootPath;
                CrashDumps.EnableCollectionToDirectory(crashLogPath, true);
                DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", diagnosticsConfig);

                UpdateSynThreadNumber();//设置线程数，需要重启实例。
                CreateThreadPool();
            }
            catch (Exception e)
            {
                DebugLog.Exception(e.Message);
                DebugLog.Exception(e.StackTrace);               
            }
            return base.OnStart();
        }

        private void UpdateSynThreadNumber()
        {
            string synThreadNumber = System.Configuration.ConfigurationManager.AppSettings["SynThreadNumber"];
            if (null != synThreadNumber)
            {
                try
                {
                    int isSynThreadNumber = Convert.ToInt32(synThreadNumber);
                    iSynThreadNumber = isSynThreadNumber;
                }
                catch (Exception exception)
                {
                    DebugLog.Exception("FleetManageToolWorker Convert.ToInt32 exception = " + exception.Message);
                }
            }

            DebugLog.Debug("FleetManageToolWorker UpdateSynThreadNumber = " + synThreadNumber);
        }

        private void CreateThreadPool()
        {
            int portTh = 0;
            ThreadPool.SetMaxThreads(2 * iSynThreadNumber, portTh);
            ThreadPool.SetMinThreads(2 * iSynThreadNumber, portTh);
            DebugLog.Debug("FleetManageToolWorker Create ThreadPool = " + 2 * iSynThreadNumber);
        }

    }
}
