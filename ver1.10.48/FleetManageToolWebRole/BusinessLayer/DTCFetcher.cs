using FleetManageTool.WebAPI;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Models.API;
using FleetManageToolWebRole.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace FleetManageToolWebRole.BusinessLayer
{
    public class DTCFetcher
    {
        public long vehicleID;
        public string vehicleGUID;

        public void VehicleDTCScan()
        {
            try
            {
                DebugLog.Debug("[DTCFetcher] VehicleDTCScan() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ") Start");
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                vehicleGUID = vehicleInterface.GetVehicleByID(vehicleID).id;
                if (null == vehicleGUID)
                {
                    return;
                }
                DiagnosticDBInterface dtcInterface = new DiagnosticDBInterface();
                List<Diagnostic> lastResult = dtcInterface.GetDiagnosticByVehicleId(vehicleID);
                if (null != lastResult)
                {
                    TimeSpan backTimeZone = TimeZoneInfo.Local.BaseUtcOffset;
                    Diagnostic findProgress = lastResult.FirstOrDefault(t => t.status.Trim().Equals("InProgress")||t.status.Trim().Equals("Cleaning"));
                    if (null != findProgress)
                    {
                        double minus = (DateTime.Now.ToUniversalTime() - findProgress.lastReadDate.Add(backTimeZone).ToUniversalTime()).TotalMinutes;
                        if (minus < 3)
                        {
                            return;
                        }
                    }
                }

                DiagnosticCode DTCResult = PostDiagnostic();
                if (null == DTCResult)
                {
                    return;
                }
                Diagnostic dtcDB = new Diagnostic();
                dtcDB.code = null;
                dtcDB.lastReadDate = DTCResult.Created.ToUniversalTime();
                dtcDB.status = "InProgress";
                dtcDB.vehicleId = vehicleID;
                dtcInterface.AddProgressDiagnostic(vehicleID, dtcDB);

                DiagnosticCode findResult = null;
                while (true)
                {
                    DiagnosticList dtcList = GetDiagnostic();
                    if (null != dtcList && null != dtcList.self_diagnostic)
                    {
                        findResult = dtcList.self_diagnostic.FirstOrDefault(t => t.Id == DTCResult.Id);
                        if (null != findResult && !findResult.State.Equals("InProgress"))
                        {
                            break;
                        }
                    }

                    if (0 == dtcInterface.getProgressDiagnosticNum(vehicleID))
                    {
                        return;
                    }

                    Thread.Sleep(1000);
                }

                List<Diagnostic> codeResult = new List<Diagnostic>();
                if (findResult.State.Equals("Fail"))
                {
                    Diagnostic temp = new Diagnostic();
                    string[] arr = findResult.Message.Split(':');
                    temp.status = "Fail";
                    temp.code = arr[0];
                    temp.vehicleId = vehicleID;
                    temp.message = arr[1];
                    temp.lastReadDate = findResult.Created.ToUniversalTime();
                    codeResult.Add(temp);
                    dtcInterface.SetDiagnosticOfVehicle(vehicleID, codeResult);
                }
                else if (findResult.State.Equals("Success"))
                {
                    List<DiagnosticCode> codeList = GetVehicleDTCs();
                    if (codeList != null)
                    {
                        foreach (DiagnosticCode codeTemp in codeList)
                        {
                            Diagnostic temp = new Diagnostic();
                            temp.vehicleId = vehicleID;
                            temp.code = codeTemp.Code;
                            temp.status = "Success";
                            temp.message = null;
                            temp.lastReadDate = findResult.Created.ToUniversalTime();
                            codeResult.Add(temp);
                        }
                    }
                    dtcInterface.SetDiagnosticOfVehicle(vehicleID, codeResult);
                }
                DebugLog.Debug("[DTCFetcher] VehicleDTCScan() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ") End");
            }
            catch (Exception e)
            {
                DebugLog.Debug("[DTCFetcher] VehicleDTCScan() Exception" + e.Message);
                DebugLog.Exception(DebugLog.DebugType.OtherException, "VehicleDTCScan" + e.Message);
                return;
            }
        }

        private DiagnosticCode PostDiagnostic()
        {
            try
            {
                DebugLog.Debug("[DTCFetcher] PostDiagnostic() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ") Start");
                IHalClient client = HalClient.GetInstance();
                HalLink idLink = new HalLink { Href = URI.VEHICLEDIAGNOSTICS, IsTemplated = true };
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["ID"] = vehicleGUID;
                paras["DiagnosticType"] = "VEHICLE_DTC_SCAN";
                DebugLog.Debug("[DTCFetcher] PostDiagnostic() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ") End");
                return client.Post<DiagnosticCode>(idLink, paras).Result;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "PostDiagnostic" + e.Message);
                throw e;
            }
            
        }

        private DiagnosticList GetDiagnostic()
        {
            try
            {
                DebugLog.Debug("[DTCFetcher] GetDiagnostic() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ") Start");
                IHalClient client = HalClient.GetInstance();
                HalLink idLink = new HalLink { Href = URI.VEHICLEDIAGNOSTICS, IsTemplated = true };
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["ID"] = vehicleGUID;
                DebugLog.Debug("[DTCFetcher] GetDiagnostic() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ") End");
                DiagnosticList result =  client.Get<DiagnosticList>(idLink, paras).Result;
                if (null == result || null == result.self_diagnostic)
                {
                    return new DiagnosticList ();
                }
                HalLink nextHref = result.Links.FirstOrDefault(x => x.Rel == URI.LINK_NEXT);
                while (null != nextHref)
                {
                    HalLink link = new HalLink() { Href = nextHref.Href, IsTemplated = false };
                    DiagnosticList nextDiagnostic = client.Get<DiagnosticList>(link).Result;
                    if (null == nextDiagnostic || null == nextDiagnostic.self_diagnostic)
                    {
                        break;
                    }
                    result.self_diagnostic.AddRange(nextDiagnostic.self_diagnostic);
                    nextHref = nextDiagnostic.Links.FirstOrDefault(x => x.Rel == URI.LINK_NEXT);
                }
                return result;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "GetDiagnostic" + e.Message);
                throw e;
            }
        }

        private List<DiagnosticCode> GetVehicleDTCs()
        {
            try
            {
                DebugLog.Debug("[DTCFetcher] GetVehicleDTCs() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ") Start");
                IHalClient client = HalClient.GetInstance();
                HalLink idLink = new HalLink { Href = URI.VEHICLEDTCS, IsTemplated = true };
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["ID"] = vehicleGUID;
                DiagnosticCodes result = client.Get<DiagnosticCodes>(idLink, paras).Result;
                List<DiagnosticCode> resultCodes = new List<DiagnosticCode>();
                if (null == result || null == result.diagnosticCode)
                {
                    return resultCodes;
                }
                resultCodes.AddRange(result.diagnosticCode);
                HalLink nextHref = result.Links.FirstOrDefault(x => x.Rel == URI.LINK_NEXT);
                while (null != nextHref)
                {
                    HalLink link = new HalLink() { Href = nextHref.Href, IsTemplated = false };
                    DiagnosticCodes nextDiagnostic = client.Get<DiagnosticCodes>(link).Result;
                    if (null == nextDiagnostic || null == nextDiagnostic.diagnosticCode)
                    {
                        break;
                    }
                    resultCodes.AddRange(nextDiagnostic.diagnosticCode);
                    nextHref = nextDiagnostic.Links.FirstOrDefault(x => x.Rel == URI.LINK_NEXT);
                }
                DebugLog.Debug("[DTCFetcher] GetVehicleDTCs() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ") End");
                return resultCodes;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "GetVehicleDTCs" + e.Message);
                throw e;
            }
        }

        //Trigger a DTC clear (on Device)
        public bool ClearDeviceDiagnostic()
        {
            try
            {
                DiagnosticDBInterface dtcInterface = new DiagnosticDBInterface();
                List<Diagnostic> lastResult = dtcInterface.GetDiagnosticByVehicleId(vehicleID);
                if (null != lastResult)
                {
                    TimeSpan backTimeZone = TimeZoneInfo.Local.BaseUtcOffset;
                    Diagnostic findProgress = lastResult.FirstOrDefault(t => t.status.Trim().Equals("InProgress") || t.status.Trim().Equals("Cleaning"));
                    if (null != findProgress)
                    {
                        double minus = (DateTime.Now.ToUniversalTime() - findProgress.lastReadDate.Add(backTimeZone).ToUniversalTime()).TotalMinutes;
                        if (minus < 3)
                        {
                            return false;
                        }
                    }
                }

                //在数据库中插入一个正在清除的状态！
                Diagnostic dtcDB = new Diagnostic();
                dtcDB.code = null;
                dtcDB.lastReadDate = DateTime.Now.ToUniversalTime();
                dtcDB.status = "Cleaning";
                dtcDB.vehicleId = vehicleID;
                dtcInterface.AddProgressDiagnostic(vehicleID, dtcDB);

                DebugLog.Debug("[DTCFetcher] ClearDeviceDiagnostic() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ") Start");
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                vehicleGUID = vehicleInterface.GetVehicleByID(vehicleID).id;
                if (null == vehicleGUID)
                {
                    return false;
                }

                //调用api
                IHalClient client = HalClient.GetInstance();
                HalLink idLink = new HalLink { Href = URI.VEHICLEDIAGNOSTICS, IsTemplated = true };
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["ID"] = vehicleGUID;
                paras["DiagnosticType"] = "VEHICLE_DTC_CLEAR";
                DebugLog.Debug("[DTCFetcher] ClearDeviceDiagnostic() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ") Mid");
                DiagnosticCode ApiResult = client.Post<DiagnosticCode>(idLink, paras).Result;
                if (null == ApiResult)
                {
                    return false;
                }

                //清除本地数据库
                bool DbResult = dtcInterface.DeleteDiagnosticByVehicleId(vehicleID);
                DebugLog.Debug("[DTCFetcher] ClearDeviceDiagnostic() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ";DbResult=" + DbResult + ") End");

                return DbResult;

            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return false;
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return false;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return false;
            }
        }

        //Trigger a DTC clear (on Server)
        public bool ClearServerDiagnostic()
        {
            try
            {
                DiagnosticDBInterface dtcInterface = new DiagnosticDBInterface();
                List<Diagnostic> lastResult = dtcInterface.GetDiagnosticByVehicleId(vehicleID);
                if (null != lastResult)
                {
                    TimeSpan backTimeZone = TimeZoneInfo.Local.BaseUtcOffset;
                    Diagnostic findProgress = lastResult.FirstOrDefault(t => t.status.Trim().Equals("InProgress") || t.status.Trim().Equals("Cleaning"));
                    if (null != findProgress)
                    {
                        double minus = (DateTime.Now.ToUniversalTime() - findProgress.lastReadDate.Add(backTimeZone).ToUniversalTime()).TotalMinutes;
                        if (minus < 3)
                        {
                            return false;
                        }
                    }
                }

                //在数据库中插入一个正在清除的状态！
                Diagnostic dtcDB = new Diagnostic();
                dtcDB.code = null;
                dtcDB.lastReadDate = DateTime.Now.ToUniversalTime();
                dtcDB.status = "Cleaning";
                dtcDB.vehicleId = vehicleID;
                dtcInterface.AddProgressDiagnostic(vehicleID, dtcDB);

                DebugLog.Debug("[DTCFetcher] ClearServerDiagnostic() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ") Start");
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                vehicleGUID = vehicleInterface.GetVehicleByID(vehicleID).id;
                if (null == vehicleGUID)
                {
                    return false;
                }

                //调用api
                IHalClient client = HalClient.GetInstance();
                HalLink idLink = new HalLink { Href = URI.VEHICLEDIAGNOSTICS, IsTemplated = true };
                Dictionary<string, object> paras = new Dictionary<string, object>();
                paras["ID"] = vehicleGUID;
                paras["DiagnosticType"] = "SERVER_DTC_CLEAR";
                DebugLog.Debug("[DTCFetcher] ClearServerDiagnostic() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ") Mid");
                DiagnosticCode ApiResult = client.Post<DiagnosticCode>(idLink, paras).Result;
                if (null == ApiResult)
                {
                    return false;
                }

                //清除本地数据库
                bool DbResult = dtcInterface.DeleteDiagnosticByVehicleId(vehicleID);

                DebugLog.Debug("[DTCFetcher] ClearServerDiagnostic() para(vehicleID=" + vehicleID + ";vehicleGUID=" + vehicleGUID + ";DbResult=" + DbResult + ") End");
                return DbResult;

            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return false;
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return false;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return false;
            }
        }
    }
}