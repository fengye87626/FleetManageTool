using FleetManageTool.Models.page;
using FleetManageTool.WebAPI;
using FleetManageTool.WebAPI.Exceptions;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Models.Common;
using FleetManageToolWebRole.Models.Constant;
using FleetManageToolWebRole.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FleetManageToolWebRole.BusinessLayer
{
    public class GeofenceApiInterface
    {
        //Flag 
        private static Boolean  upDateFlag = true;
        //获取Geofence 已添加和未添加车辆
        //mabiao 20140308 
        public GeofenceInfo GetTenantVehiclesByCompannyID(string companyID, long geofenceID)
        {
            try
            {
                DebugLog.Debug("[GeofenceApiInterface] GetTenantVehiclesByCompannyID()  para( companyID=" + companyID + ", geofenceID=" + geofenceID + ") Start");
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
                List<long> allHistoryVehicleids = vehicleInterface.GetHistoryVehicleidsByCompannyID(companyID);

                GeofenceInfo results = new GeofenceInfo();
                List<Vehicle> hasvehicles = new List<Vehicle>();
                List<Vehicle> addvehicles = new List<Vehicle>();
                if (-1 == geofenceID)
                {
                    addvehicles = vehicleInterface.GetTenantVehiclesByCompannyID(companyID);

                    //排除所有历史车辆 by zhangbo 20140708
                    foreach (Vehicle temp in addvehicles)
                    {
                        if(true == ListHasItem(allHistoryVehicleids, temp.pkid))
                        {
                            addvehicles.Remove(temp);
                        }
                    }

                    results.addvehicles = addvehicles;
                    results.hasvehicles = new List<Vehicle>();
                    DebugLog.Debug("[GeofenceApiInterface] GetTenantVehiclesByCompannyID() End(return results)[results.addvehicles.Count=" + results.addvehicles.Count + "]");
                    return results;
                }
                else
                {
                    List<Vehicle> vehicles = vehicleInterface.GetTenantVehiclesByCompannyID(companyID);
                    List<Geofence_Vehicle> geofencevehicles = geofenceInterface.GetGeofenceVehicles(geofenceID);
                    foreach (Geofence_Vehicle geofencevehicle in geofencevehicles)
                    {
                        Vehicle vehicleTemp = vehicleInterface.GetVehicleByID(geofencevehicle.vehicleid);

                        //排除所有历史车辆 by zhangbo 20140708
                        if (false == ListHasItem(allHistoryVehicleids, vehicleTemp.pkid))
                        {
                            hasvehicles.Add(vehicleTemp);
                        }
                        
                        Vehicle removeVehicle = vehicles.First(Vehicle => Vehicle.pkid == vehicleTemp.pkid);
                        vehicles.Remove(removeVehicle);
                    }

                    results.addvehicles = vehicles;
                    foreach (Vehicle temp in results.addvehicles)
                    {
                        //排除所有历史车辆 by zhangbo 20140708
                        if (true == ListHasItem(allHistoryVehicleids, temp.pkid))
                        {
                            results.addvehicles.Remove(temp);
                        }
                    }
                    results.hasvehicles = hasvehicles;
                    DebugLog.Debug("[GeofenceApiInterface] GetTenantVehiclesByCompannyID() End(return results)[results.addvehicles.Count=" + results.addvehicles.Count + "]");
                    DebugLog.Debug("[GeofenceApiInterface] GetTenantVehiclesByCompannyID() End(return results)[results.hasvehicles.Count=" + results.hasvehicles.Count + "]");
                    return results;
                }
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new GeofenceInfo();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new GeofenceInfo();
            }
        }


        //通过GeoFenceID取得GeoFence信息
        //mabiao 20140308
        public GeofenceInfo GetGeoFenceInfoByGeoFenceId(string companyID, long geofenceID)
        {
            try
            {
                DebugLog.Debug("[GeofenceApiInterface] GetGeoFenceInfoByGeoFenceId()  para( companyID=" + companyID + ", geofenceID=" + geofenceID + ") Start");
                long SelectGeoFenceID = geofenceID;

                GeofenceInfo result = new GeofenceInfo();
                //非新规GeoFence
                if ((-1) != (long)SelectGeoFenceID)
                {
                    GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
                    VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                    Geofence geofence = geofenceInterface.GetGeofenceByGeofenceID(SelectGeoFenceID);
                    List<Geofence_Vehicle> geofencevehicles = geofenceInterface.GetGeofenceVehicles(SelectGeoFenceID);
                    List<long> allHistoryVehicleids = vehicleInterface.GetHistoryVehicleidsByCompannyID(companyID);
                    result.geofence = geofence;
                    result.hasvehicles = new List<Vehicle>();
                    foreach (Geofence_Vehicle geofencevehicle in geofencevehicles)
                    {
                        Vehicle vehicleTemp = vehicleInterface.GetVehicleByID(geofencevehicle.vehicleid);

                        //排除所有历史车辆 by zhangbo 20140708
                        if (false == ListHasItem(allHistoryVehicleids, vehicleTemp.pkid))
                        {
                            result.hasvehicles.Add(vehicleTemp);
                        }
                    }
                }
                DebugLog.Debug("[GeofenceApiInterface] GetGeoFenceInfoByGeoFenceId() End(return results)[result.hasvehicles.Count=" + result.hasvehicles.Count + "]");
                return result;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new GeofenceInfo();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new GeofenceInfo();
            }
        }

        //获取编辑的Geofence信息
        //mabiao 20140308 
        public string GetEditGeoFenceInfo(Geofence geofence, int EditGeoFenceZoom)
        {
            try
            {
                DebugLog.Debug("[GeofenceApiInterface] GetEditGeoFenceInfo()  para( geofence.pkid=" + geofence.pkid + ", geofence.tenantid=" + geofence.tenantid + ", EditGeoFenceZoom=" + EditGeoFenceZoom + ") Start");
                //打算重写
                //当GeoFenceID为(-1)时，为新规电子围栏，默认半径设定为（-1）
                if ((-1) == geofence.pkid)
                {
                    //if it is a new fence, the radius is set to -1
                    geofence.radiu = -1;
                    DebugLog.Debug("[GeofenceApiInterface] GetEditGeoFenceInfo() End(return string)[" + "{\"GeoFenceID\": " + geofence.pkid + ",\"GeoFencelng\": " + geofence.Baidulng + ",\"GeoFencelat\": " + geofence.Baidulat + ",\"GeoFenceradius\": " + geofence.radiu + ",\"zoom\": " + EditGeoFenceZoom + "}" + "]");
                    return "{\"GeoFenceID\": " + geofence.pkid + ",\"GeoFencelng\": " + geofence.Baidulng + ",\"GeoFencelat\": " + geofence.Baidulat + ",\"GeoFenceradius\": " + geofence.radiu + ",\"zoom\": " + EditGeoFenceZoom + "}";
                }
                //否者为：被选中编辑的GeoFence
                else
                {
                    DebugLog.Debug("[GeofenceApiInterface] GetEditGeoFenceInfo() End(return string)[" + "{\"GeoFenceID\": " + geofence.pkid + ",\"GeoFenceName\": " + geofence.name + ",\"GeoFencelng\": " + geofence.Baidulng + ",\"GeoFencelat\": " + geofence.Baidulat + ",\"GeoFenceradius\": " + geofence.radiu + ",\"zoom\": " + EditGeoFenceZoom + ",\"location\": " + geofence.location + "}" + "]");
                    //通过GeoFenceID从WebService中取得半径信息
                    return "{\"GeoFenceID\": " + geofence.pkid + ",\"GeoFenceName\":\" " + geofence.name + "\",\"GeoFencelng\": " + geofence.Baidulng + ",\"GeoFencelat\": " + geofence.Baidulat + ",\"GeoFenceradius\": " + geofence.radiu + ",\"zoom\": " + EditGeoFenceZoom + ",\"location\":\" " + geofence.location + "\"}";
                }
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return null;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return null;
            }
        }

        //获取GeoFence 信息
        //mabiao 20140308 
        public List<GeofenceInfo> GetGeofencesInfo(string companyID, long group_id)
        {
            try
            {
                DebugLog.Debug("[GeofenceApiInterface] GetGeofencesInfo()  para( companyID =" + companyID + ", group_id=" + group_id + ") Start");
                GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                List<Geofence> geofences = new List<Geofence>();
                List<long> allHistoryVehicleids = vehicleInterface.GetHistoryVehicleidsByCompannyID(companyID);

                if (-1 == group_id)
                {
                    geofences = geofenceInterface.GetGeofences(companyID);
                }
                else
                {
                    List<Vehicle> vehicles = vehicleInterface.GetGroupVehiclesByGroupId(group_id);
                    foreach (Vehicle vehicleTemp in vehicles)
                    {
                        List<Geofence_Vehicle> geofencevehicles = geofenceInterface.GetGeofenceVehiclesByVehicleID(vehicleTemp.pkid);
                        foreach (Geofence_Vehicle geofencevehicleTemp in geofencevehicles)
                        {
                            Geofence geofence = geofenceInterface.GetGeofenceByGeofenceID(geofencevehicleTemp.geofenceid);
                            if (null == geofences.Find(Geofence => Geofence.pkid == geofence.pkid))
                            {
                                geofences.Add(geofence);
                            }
                        }
                    }
                }
                List<GeofenceInfo> result = new List<GeofenceInfo>();
                foreach (Geofence geofenceTemp in geofences)
                {
                    GeofenceInfo model = new GeofenceInfo();
                    model.geofence = geofenceTemp;
                    model.hasvehicles = new List<Vehicle>();
                    foreach (Geofence_Vehicle geofencevehicle in geofenceTemp.Geofence_Vehicle)
                    {
                        Vehicle vehicle = vehicleInterface.GetVehicleByID(geofencevehicle.vehicleid);

                        //排除所有历史车辆 by zhangbo 20140708
                        if (false == ListHasItem(allHistoryVehicleids, vehicle.pkid))
                        {
                            model.hasvehicles.Add(vehicle);
                        }
                    }
                    result.Add(model);
                }
                DebugLog.Debug("[GeofenceApiInterface] GetGeofencesInfo() End(return results)[result.Count=" + result.Count + "]");
                return result;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return null;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return null;
            }
        }

        //请求根据车辆的id取得已经绑定的Geo的数量
        //mabiao 20140308 
        public long GetGeofenceNumByVehicleID(string companyID, long vehicleID)
        {
            try
            {
                DebugLog.Debug("[GeofenceApiInterface] GetGeofenceNumByVehicleID()  para( companyID =" + companyID + ", vehicleID=" + vehicleID + ") Start");
                GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
                List<Geofence_Vehicle> Geos = geofenceInterface.GetGeofenceVehiclesByVehicleID(vehicleID);
                DebugLog.Debug("[GeofenceApiInterface] GetGeofenceNumByVehicleID() End(return results)[Geos.Count=" + Geos.Count + "]");
                return Geos.Count;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return GeofenceConstant.MaxGeofence;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return GeofenceConstant.MaxGeofence;
            }
        }

        //删除GeoFence 车辆
        //mabiao 20140308 
        public bool DeleteGeoVehicle(string companyID, long geofenceID, long vehicleID)
        {
            try
            {
                DebugLog.Debug("[GeofenceApiInterface] DeleteGeoVehicle()  para( companyID =" + companyID + ", geofenceID=" + geofenceID + ", vehicleID=" + vehicleID + ") Start");
                GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
                Hashtable hshVehicleTable = new Hashtable();
                hshVehicleTable.Add(geofenceInterface.GetGUIDByGeofenceIDAndVehicleID(geofenceID, vehicleID), vehicleID);

                List<long> result = DeleteGeofenceFromApi(companyID, hshVehicleTable);
                if (0 == result.Count)
                {
                    return false;
                }
                //删除成功后 运行下方代码
                geofenceInterface.DeleteGeofenceVehicle(geofenceID, vehicleID);
                DebugLog.Debug("[GeofenceApiInterface] DeleteGeoVehicle() End(return true)");
                return true;
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

        //请求（停用/激活）GeoFence
        //mabiao 20140308 
        public bool InactiveOrActiveGeo(string companyID, long geofenceID, string status)
        {
            DebugLog.Debug("[GeofenceApiInterface] InactiveOrActiveGeo()  para( companyID =" + companyID + ", geofenceID=" + geofenceID + ", status=" + status + ") Start");
            try
            {
                GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
                List<string> ihpleDgeofenceIDs = new List<string> ();
                Geofence updateGeofence = geofenceInterface.GetGeofenceByGeofenceID(geofenceID);
                updateGeofence.status = status;
                List<Geofence_Vehicle> vehicles = geofenceInterface.GetGeofenceVehicles(geofenceID);
                foreach(Geofence_Vehicle vehicleTemp in vehicles)
                {
                    ihpleDgeofenceIDs.Add(vehicleTemp.ihpleDgeofenceid);
                    if(GetGeofenceNumByVehicleID(companyID,vehicleTemp.pkid) >= GeofenceConstant.MaxGeofence && GeofenceConstant.Active.Equals(status)){return false;}
                }
                List<string> updatevehicles =  UpdateGeofenceFromApi(companyID, updateGeofence, ihpleDgeofenceIDs);
                if (updatevehicles.Count == ihpleDgeofenceIDs.Count)
                {
                    geofenceInterface.UpdateGeofence(updateGeofence);
                    DebugLog.Debug("[GeofenceApiInterface] InactiveOrActiveGeo() End(return true)");
                    return true;
                }
                else
                {
                    //TO DO??
                    DebugLog.Debug("[GeofenceApiInterface] InactiveOrActiveGeo() End(return false)");
                    return false;
                }
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

        //请求（删除）GeoFence
        //mabiao 20140308
        public bool DeleteGeo(string companyID, long geofenceID)
        {
            DebugLog.Debug("[GeofenceApiInterface] DeleteGeo()  para( companyID =" + companyID + ", geofenceID=" + geofenceID + ") Start");
            try
            {
                GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
                Hashtable deletevehicleIDsTable = new Hashtable();
                List<Geofence_Vehicle> vehicles = geofenceInterface.GetGeofenceVehicles(geofenceID);
                foreach (Geofence_Vehicle vehicleTemp in vehicles)
                {
                    deletevehicleIDsTable.Add(vehicleTemp.ihpleDgeofenceid, vehicleTemp.vehicleid);
                }
                List<long> result = DeleteGeofenceFromApi(companyID, deletevehicleIDsTable);
                if (result.Count == vehicles.Count)
                {
                    geofenceInterface.DeleteGeofence(geofenceID);
                    DebugLog.Debug("[GeofenceApiInterface] DeleteGeo() End(return true)");
                    return true;
                }
                else
                {
                    foreach (long resultTemp in result)
                    {
                        geofenceInterface.DeleteGeofenceVehicle(geofenceID, resultTemp);
                    }
                    //对于那些失败的怎么办？
                    //TO DO
                    DebugLog.Debug("[GeofenceApiInterface] DeleteGeo() End(return false)");
                    return false;
                }
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

        //编辑电子围栏(编辑、添加)
        //mabiao 20140308
        public string UpdateGeofence(string companyID, Geofence geofence, string newVehicleIDs, string oldVehicleIDs)
        {
            DebugLog.Debug("[GeofenceApiInterface] UpdateGeofence()  para( companyID =" + companyID + ", newVehicleIDs=" + newVehicleIDs + "oldVehicleIDs=" + oldVehicleIDs + ") Start");
            try
            {
                string result = "OK";
                GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                Transform trans = new Transform();
                List<long> newVehicles = trans.StringToList(newVehicleIDs);
                List<long> oldVehicles = trans.StringToList(oldVehicleIDs);
                List<Geofence_Vehicle> addsuccess = new List<Geofence_Vehicle>();
                //添加电子围栏
                if (-1 == geofence.pkid)                      //-1为新规电子围栏，由前台传入
                {
                    geofence.pkid = geofenceInterface.AddGeofence(companyID, geofence);
                    result = AddVehiclesTOGeoFence(companyID, geofence, newVehicles, oldVehicles, addsuccess);
                    if (result != "OK")
                    {
                        geofenceInterface.DeleteGeofence(geofence.pkid);
                    }
                }
                else
                {
                    //向围栏添加车辆
                    DebugLog.Debug("[GeofenceApiInterface] UpdateGeofence()  step 1" );
                    result = AddVehiclesTOGeoFence(companyID, geofence, newVehicles, oldVehicles, addsuccess);

                    //从围栏中删掉车辆
                    DebugLog.Debug("[GeofenceApiInterface] UpdateGeofence()  step 2");
                    List<long> deleteSuccess = DeleteVehiclesFromGeoFence(companyID, geofence, newVehicles, oldVehicles);

                    //更新车辆及电子围栏
                    DebugLog.Debug("[GeofenceApiInterface] UpdateGeofence()  step 3");
                    List<string> updateSuccess = UpdateGeoFence(companyID, geofence, newVehicles, oldVehicles);
                }
                return result;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return "ERROR";
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return "ERROR";
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return "ERROR"; ;
            }
        }

        //添加车辆
        public string AddVehiclesTOGeoFence(string companyID, Geofence newGeofence, List<long> newVehicleIDs, List<long> oldVehicleIDs,List<Geofence_Vehicle> resultVehicle)
        {
            try
            {
                string result = "OK";
                DebugLog.Debug("[GeofenceApiInterface] AddVehiclesTOGeoFence()  para( companyID =" + companyID + ", newVehicleIDs.Count=" + newVehicleIDs.Count + "oldVehicleIDs.Count=" + oldVehicleIDs.Count + ") Start");
                GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                List<string> vehicleGuids = new List<string>();
                List<Geofence_Vehicle> addresult = new List<Geofence_Vehicle>();
                List<long> addvehicleIDs = new List<long>();
                //找出添加的车辆ID并添加
                if (null == oldVehicleIDs)
                {
                    addvehicleIDs.AddRange(newVehicleIDs);
                }
                else if (0 == newVehicleIDs.Count)
                {
                    DebugLog.Debug("[GeofenceApiInterface] AddVehiclesTOGeoFence() End(return addresult)[addresult.Count" + addresult.Count + "]");
                    return "OK";
                }
                else
                {
                    IEnumerable<long> vehicleIDs = newVehicleIDs.Except(oldVehicleIDs);
                    addvehicleIDs.AddRange(vehicleIDs);
                }

            if (0 != addvehicleIDs.Count)
            {
                List<Vehicle> Vehicles = vehicleInterface.GetVehicleByIDs(addvehicleIDs);
                Hashtable hashvehicleIDsTable = new Hashtable(); //  创建哈希表

                foreach (Vehicle vehicleIDTemp in Vehicles)
                {
                    hashvehicleIDsTable.Add(vehicleIDTemp.id, vehicleIDTemp.pkid);
                }

                //返回成功添加车辆
                result = AddGeofenceFromApi(companyID, newGeofence, hashvehicleIDsTable, addresult);
                DebugLog.Debug("[GeofenceApiInterface] AddVehiclesTOGeoFence()  AddGeofenceFromApi(companyID, newGeofence, hashvehicleIDsTable, addresult)");

                foreach (Geofence_Vehicle addresultTemp in addresult)
                {
                    addresultTemp.geofenceid = newGeofence.pkid;
                }
                if (!addvehicleIDs.Count.Equals(addresult.Count))
                {
                    upDateFlag = false;
                }

                    geofenceInterface.AddGeofenceVehicles(addresult);
                }
                DebugLog.Debug("[GeofenceApiInterface] AddVehiclesTOGeoFence() End(return addresult)[addresult.Count=" + addresult.Count + "]");
                resultVehicle.AddRange(addresult);
                return result;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return "ERROR";
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return "ERROR";
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return "ERROR";
            }
        }

        //删除车辆
        public List<long> DeleteVehiclesFromGeoFence(string companyID, Geofence newGeofence, List<long> newVehicleIDs, List<long> depositVehicleIDs)
        {
            try
            {
                DebugLog.Debug("[GeofenceApiInterface] DeleteVehiclesFromGeoFence()  para( companyID =" + companyID + ", newVehicleIDs.Count=" + newVehicleIDs.Count + "depositVehicleIDs.Count=" + depositVehicleIDs.Count + ") Start");
                GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
                List<long> deleteresult = new List<long>();
                List<long> deletevehicleIDs = new List<long>();
                //找出删除车辆ID并删除
                IEnumerable<long> vehicleIDs = depositVehicleIDs.Except(newVehicleIDs);

            deletevehicleIDs.AddRange(vehicleIDs);

            if (0 != deletevehicleIDs.Count)
            {
                Hashtable deletevehicleIDsTable = new Hashtable();

                    foreach (long vehicleIDTemp in deletevehicleIDs)
                    {
                        deletevehicleIDsTable.Add(geofenceInterface.GetGUIDByGeofenceIDAndVehicleID(newGeofence.pkid, vehicleIDTemp), vehicleIDTemp);
                    }
                    //返回删除成功的车辆ID
                    deleteresult = DeleteGeofenceFromApi(companyID, deletevehicleIDsTable);
                    foreach (long deleteresultTemp in deleteresult)
                    {
                        geofenceInterface.DeleteGeofenceVehicle(newGeofence.pkid, deleteresultTemp);
                    }
                    if (!deletevehicleIDs.Count.Equals(deleteresult.Count))
                    {
                        upDateFlag = false;
                    }
                    //TO DO
                }
                DebugLog.Debug("[GeofenceApiInterface] DeleteVehiclesFromGeoFence() End(return addresult)[deleteresult.Count=" + deleteresult.Count + "]");
                return deleteresult;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new List<long>();
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new List<long>();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new List<long>();
            }
        }

        //更新车辆及电子围栏
        public List<string> UpdateGeoFence(string companyID, Geofence newGeofence, List<long> newVehicleIDs, List<long> depositVehicleIDs)
        {
            DebugLog.Debug("[GeofenceApiInterface] UpdateGeoFence()  para( companyID =" + companyID + ", newVehicleIDs.Count=" + newVehicleIDs.Count + "depositVehicleIDs.Count=" + depositVehicleIDs.Count + ") Start");
            GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
            List<string> unionihpleDGeofenceIDs = new List<string>();
            List<string> unionresult = new List<string>();
            List<long> unionvehicleIDs = new List<long>();
            //找出交集
            IEnumerable<long> vehicleIDs = newVehicleIDs.Intersect(depositVehicleIDs);

            unionvehicleIDs.AddRange(vehicleIDs);

            //注意还有该围栏的历史车辆！
            unionvehicleIDs.AddRange(GetGeofenceHistoryVehicleids(companyID, newGeofence.pkid));

            if (0 != unionvehicleIDs.Count)
            {
                foreach (long vehicleIDTemp in unionvehicleIDs)
                {
                    unionihpleDGeofenceIDs.Add(geofenceInterface.GetGUIDByGeofenceIDAndVehicleID(newGeofence.pkid, vehicleIDTemp));
                }
                unionresult = UpdateGeofenceFromApi(companyID, newGeofence, unionihpleDGeofenceIDs);
                if (!unionvehicleIDs.Count.Equals(unionresult.Count))
                {
                    upDateFlag = false;
                }
                //TO DO 异常时如何处理？
            }
            geofenceInterface.UpdateGeofence(newGeofence);
            DebugLog.Debug("[GeofenceApiInterface] UpdateGeoFence() End(return addresult)[unionresult.Count=" + unionresult.Count + "]");
            return unionresult;
        }
        //API删除车辆
        //mabiao 2014/3/24
        public List<long> DeleteGeofenceFromApi(string companyID, Hashtable deleteVehicleIDsTable)
        {
            DebugLog.Debug("[GeofenceApiInterface] DeleteGeofenceFromApi()  para( companyID =" + companyID + ", deleteVehicleIDsTable.Count=" + deleteVehicleIDsTable.Count + ") Start");
            //到时候还要删除的！
            if ("ABCSoft".Equals(companyID) || "ihpleD".Equals(companyID))
            {
                List<long> deletevehicleIDs = new List<long>();

                foreach (DictionaryEntry de in deleteVehicleIDsTable)
                {
                    deletevehicleIDs.Add(long.Parse(de.Value.ToString()));
                }
                return deletevehicleIDs;
            }

            try
            {
                IHalClient client = HalClient.GetInstance();
                List<long> deleteSuccessIDs = new List<long>();
                int count = 0;

                List<Task<IHalResult>> geofenceTasks = new List<Task<IHalResult>>();
                foreach (DictionaryEntry de in deleteVehicleIDsTable)
                {
                    HalLink geofenceLink = new HalLink { Href = Models.API.URI.GEOFENCES, IsTemplated = true };
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters[GeofenceConstant.ID] = de.Key;
                    Task<IHalResult> geofenceTask = client.Delete(geofenceLink, parameters);
                    geofenceTasks.Add(geofenceTask);
                }
                foreach (DictionaryEntry de in deleteVehicleIDsTable)
                {
                    //获取删除成功的ID
                    if (Task.WhenAll(geofenceTasks).Result[count].Success.Equals(true))
                    {
                        deleteSuccessIDs.Add(long.Parse(de.Value.ToString()));
                    }
                    count++;
                }
                DebugLog.Debug("[GeofenceApiInterface] DeleteGeofenceFromApi() End(return addresult)[deleteSuccessIDs.Count=" + deleteSuccessIDs.Count + "]");
                return deleteSuccessIDs;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new List<long>();
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new List<long>();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new List<long>();
            }
        }

        //API添加车辆
        //mabiao 2014/3/24
        public string AddGeofenceFromApi(string companyID, Geofence newGeofence, Hashtable hashvehicleIDsTable,List<Geofence_Vehicle> returnVehicle)
        {
            DebugLog.Debug("[GeofenceApiInterface] AddGeofenceFromApi()  para( companyID =" + companyID + ", hashvehicleIDsTable.Count=" + hashvehicleIDsTable.Count + ") Start");
            //到时候还要删除的！

            if ("ABCSoft".Equals(companyID) || "ihpleD".Equals(companyID))
            {
                int count = 0;
                List<Geofence_Vehicle> addvehicles = new List<Geofence_Vehicle>();

                foreach (DictionaryEntry de in hashvehicleIDsTable)
                {
                    Geofence_Vehicle newvehicle = new Geofence_Vehicle();
                    newvehicle.geofenceid = newGeofence.pkid;
                    newvehicle.vehicleid = long.Parse(de.Value.ToString());
                    newvehicle.ihpleDgeofenceid = newGeofence.pkid.ToString() + de.Value.ToString();
                    addvehicles.Add(newvehicle);
                    returnVehicle.Add(newvehicle);
                    count++;
                }
                DebugLog.Debug("[GeofenceApiInterface] AddGeofenceFromApi() End(return addresult)[addvehicles.Count=" + addvehicles.Count + "]");
                return "OK";
            }

            try
            {
                int count = 0;
                IHalClient client = HalClient.GetInstance();
                List<Geofence_Vehicle> addvehicles = new List<Geofence_Vehicle>();
                List<Task<Models.API.Geofence>> geofenceTasks = new List<Task<Models.API.Geofence>>();

                foreach (DictionaryEntry de in hashvehicleIDsTable)
                {
                    HalLink geofenceLink = new HalLink { Href = Models.API.URI.VEHICLEGEOFENCES, IsTemplated = true };
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    List<Models.API.Location> locations = new List<Models.API.Location>();

                    //必须将百度坐标转成国际坐标
                    double WGSlng = BaiduWGSPoint.GeoPointDTO.BaiduToInternational(newGeofence.Baidulng, newGeofence.Baidulat).longitude;
                    double WGSlat = BaiduWGSPoint.GeoPointDTO.BaiduToInternational(newGeofence.Baidulng, newGeofence.Baidulat).latitude;

                    Models.API.Location location = new Models.API.Location() { latitude = WGSlat, longitude = WGSlng };
                    locations.Add(location);
                    parameters.Add(GeofenceConstant.Coordinates, locations);
                    parameters.Add(GeofenceConstant.Radius, newGeofence.radiu);
                    parameters.Add(GeofenceConstant.GeoFenceType, Models.API.Geofence.GeofenceType.CIRCLE.ToString());
                    parameters.Add(GeofenceConstant.Name, newGeofence.name);
                    if (newGeofence.status.Equals(GeofenceConstant.Active))
                    {
                        parameters.Add(GeofenceConstant.Enabled, true);
                    }
                    else
                    {
                        parameters.Add(GeofenceConstant.Enabled, false);
                    }
                    parameters.Add(GeofenceConstant.CoordinateType, "WGS");

                    parameters.Add(GeofenceConstant.ID, de.Key);
                    Task<Models.API.Geofence> geofenceTask = client.Post<Models.API.Geofence>(geofenceLink, parameters);
                    geofenceTasks.Add(geofenceTask);
                    
                }

                foreach (DictionaryEntry de in hashvehicleIDsTable)
                {
                    if (null != Task.WhenAll(geofenceTasks).Result[count])
                    {
                        Geofence_Vehicle newvehicle = new Geofence_Vehicle();
                        newvehicle.geofenceid = newGeofence.pkid;
                        newvehicle.vehicleid = long.Parse(de.Value.ToString());
                        newvehicle.ihpleDgeofenceid = Task.WhenAll(geofenceTasks).Result[count].Id;
                        addvehicles.Add(newvehicle);
                        returnVehicle.Add(newvehicle);
                    }
                    count++;
                }
                if (count != 0)
                {
                    DebugLog.Debug("[GeofenceApiInterface] AddGeofenceFromApi() End(return addresult)[addvehicles.Count=" + addvehicles.Count + "]");
                    return "OK";
                }
                else
                {
                    DebugLog.Debug("[GeofenceApiInterface] AddGeofenceFromApi() End(return addresult)[addvehicles.Count=" + addvehicles.Count + "]");
                    return "ERROR";
                }
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return "ERROR";
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return "ERROR";
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                if (e.InnerException is HalException)
                {
                    string[] ecode = ((HalException)(e.InnerException)).ReasonPhrase.Split(':');
                    return ecode[0];
                }
                else
                {
                    return "ERROR";
                }
            }
        }

        //API更新车辆
        //mabiao 2014/3/24
        public List<string> UpdateGeofenceFromApi(string companyID, Geofence newGeofence, List<string> ihpleDgeofenceIDs)
        {
            DebugLog.Debug("[GeofenceApiInterface] UpdateGeofenceFromApi()  para( companyID =" + companyID + ", ihpleDgeofenceIDs.Count=" + ihpleDgeofenceIDs.Count + ") Start");
            //到时候还要删除的！
            if ("ABCSoft".Equals(companyID) || "ihpleD".Equals(companyID))
            {
                return ihpleDgeofenceIDs;
            }

            try
            {
                IHalClient client = HalClient.GetInstance();
                List<string> addsuccessIDs = new List<string>();
                int count = 0;
                List<Task<IHalResult>> geofenceTasks = new List<Task<IHalResult>>();

                foreach (string IDtemp in ihpleDgeofenceIDs)
                {
                    HalLink geofenceLink = new HalLink { Href = Models.API.URI.GEOFENCES, IsTemplated = true };
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    List<Models.API.Location> locations = new List<Models.API.Location>();

                    //必须将百度坐标转成国际坐标
                    double WGSlng = BaiduWGSPoint.GeoPointDTO.BaiduToInternational(newGeofence.Baidulng, newGeofence.Baidulat).longitude;
                    double WGSlat = BaiduWGSPoint.GeoPointDTO.BaiduToInternational(newGeofence.Baidulng, newGeofence.Baidulat).latitude;

                    Models.API.Location location = new Models.API.Location() { latitude = WGSlat, longitude = WGSlng };
                    locations.Add(location);

                    parameters.Add(GeofenceConstant.Radius, newGeofence.radiu);
                    parameters.Add(GeofenceConstant.GeoFenceType, Models.API.Geofence.GeofenceType.CIRCLE.ToString());
                    parameters.Add(GeofenceConstant.Name, newGeofence.name.Trim());
                    parameters.Add(GeofenceConstant.Coordinates, locations);
                    parameters.Add(GeofenceConstant.CoordinateType, "WGS");
                    if (newGeofence.status.Equals(GeofenceConstant.Active))
                    {
                        parameters.Add(GeofenceConstant.Enabled, true);
                    }
                    else
                    {
                        parameters.Add(GeofenceConstant.Enabled, false);
                    }

                    parameters.Add(GeofenceConstant.ID, IDtemp);
                    Task<IHalResult> geofenceTask = client.Put(geofenceLink, parameters);
                    geofenceTasks.Add(geofenceTask);
                }
                foreach (IHalResult resultTemp in Task.WhenAll(geofenceTasks).Result)
                {
                    if (resultTemp.Success.Equals(true))
                    {
                        addsuccessIDs.Add(ihpleDgeofenceIDs[count]);
                    }
                    count++;
                }
                DebugLog.Debug("[GeofenceApiInterface] UpdateGeofenceFromApi() End(return addresult)[addsuccessIDs.Count=" + addsuccessIDs.Count + "]");
                return addsuccessIDs;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new List<string>();
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new List<string>();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new List<string>();
            }
        }

        //该围栏的历史车辆
        public List<long> GetGeofenceHistoryVehicleids(string companyID, long geofenceid) 
        {
            DebugLog.Debug("[GeofenceApiInterface] GetGeofenceHistoryVehicleids()  para( companyID =" + companyID + ", geofenceid=" + geofenceid + ") Start");
            try 
            {
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                GeofenceDBInterface geofenceInterface = new GeofenceDBInterface();
                List<long> allHistoryVehicleids = vehicleInterface.GetHistoryVehicleidsByCompannyID(companyID);
                List<long> geofenceVehicleids = geofenceInterface.GetGeofenceVehicleids(geofenceid);
                List<long> result = new List<long>();
                IEnumerable<long> temp = allHistoryVehicleids.Intersect(geofenceVehicleids);
                result.AddRange(temp);
                return result;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new List<long>();
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new List<long>();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new List<long>();
            }
        }

        // by zhangbo 20140708
        public bool ListHasItem(List<long> longlist, long Item)
        {
            bool bFlag = false;
            if (longlist == null || Item == null)
            {
                return false;
            }
            foreach (long temp in longlist)
            {
                if (temp == Item)
                {
                    bFlag = true;
                    break;
                }
            }
            return bFlag;
        }
    }
}