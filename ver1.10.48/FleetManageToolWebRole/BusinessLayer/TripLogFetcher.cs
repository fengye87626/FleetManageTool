using BaiduWGSPoint;
using FleetManageTool.Models.Common;
using FleetManageTool.Models.page;
using FleetManageTool.WebAPI;
using FleetManageToolWebRole.DB_interface;
using FleetManageToolWebRole.Models;
using FleetManageToolWebRole.Models.Common;
using FleetManageToolWebRole.Models.Constant;
using FleetManageToolWebRole.Util;
using Microsoft.ApplicationServer.Caching;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace FleetManageToolWebRole.BusinessLayer
{
    public class TripLogFetcher
    {
        //获取Vehicle Detail画面中 TripLog
        public List<TripLog> GetTripLogs(long vehicleID, TripLog lasttrip)
        {
            List<TripLog> tripLogs = new List<TripLog>();
            try
            {
                if (0 == vehicleID % 2)
                {
                    if (null == lasttrip || "" == lasttrip.type)
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            TripLog temp = new TripLog();
                            temp.alerts = new List<AlertType>();
                            temp.geofenceInfo = new List<string>();
                            if (0 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 21:56:00");//"201401231956";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 22:00:00");//"201401232000";
                                temp.endLocation = "前门";
                                temp.distance = 2;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            }
                            if (1 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 19:56:00");//"201401231956";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 20:00:00");//"201401232000";
                                temp.endLocation = "西单";
                                temp.distance = 2;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                                for (int j = 0; j < 1; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    tempalert = AlertType.SPEEDALERT;
                                    temp.alerts.Add(tempalert);
                                }
                                for (int j = 0; j < 1; j++)
                                {
                                    string tempgeo = null;
                                    tempgeo = "进入: 家";
                                    temp.geofenceInfo.Add(tempgeo);
                                }
                                
                            }
                            if (2 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 17:23:00");//"201401231723";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 18:48:00");//"201401231848";
                                temp.endLocation = "长安街";
                                temp.distance = 5;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                            }
                            if (3 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 14:48:00");//"201401231448";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 16:00:00");//"201401231600";
                                temp.endLocation = "东单";
                                temp.distance = 15;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                                for (int j = 0; j < 1; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    tempalert = AlertType.HIGHPRMALERT;
                                    temp.alerts.Add(tempalert);
                                }
                            }
                            if (4 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 12:23:00");//"201401231223";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 13:01:00");//"201401232201";
                                temp.endLocation = "人民大会堂";
                                temp.distance = 20;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                                for (int j = 0; j < 3; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    if (0 == j)
                                    {
                                        tempalert = AlertType.MOTIONALERT;
                                    }
                                    if (2 == j)
                                    {
                                        tempalert = AlertType.HIGHPRMALERT;
                                    }
                                    if (1 == j)
                                    {
                                        tempalert = AlertType.SPEEDALERT;
                                    }
                                    temp.alerts.Add(tempalert);
                                }
                            }
                            if (5 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 11:00:00");//"201401231100";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 12:00:00");//"201401231200";
                                temp.endLocation = "王府井";
                                temp.distance = 20;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                                for (int j = 0; j < 2; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    if (0 == j)
                                    {
                                        tempalert = AlertType.MOTIONALERT;
                                    }
                                    if (1 == j)
                                    {
                                        tempalert = AlertType.HIGHPRMALERT;
                                    }
                                    temp.alerts.Add(tempalert);
                                }

                                for (int j = 0; j < 3; j++)
                                {
                                    string tempgeo = null;
                                    if (0 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (1 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (2 == j)
                                    {
                                        tempgeo = "离开: 动物园";
                                    }
                                    temp.geofenceInfo.Add(tempgeo);
                                }
                            }
                            if (6 == i)
                            {
                                //temp.StartTime = "";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 6:00:00");//"201401221200";
                                temp.endLocation = "北京朝阳区霄云路20号";
                                temp.type = "Final";
                                temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                            }
                            tripLogs.Add(temp);
                        }
                    }
                    /*else if ("Normal" == lasttrip.type)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            TripLog temp = new TripLog();
                            temp.alerts = new List<AlertType>();
                            temp.geofenceInfo = new List<string>();
                            if (0 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-22 09:01:00");//"201401220901";
                                temp.EndTime = Convert.ToDateTime("2014-01-22 12:00:00");//"201401221200";
                                temp.endLocation = "北京朝阳区霄云路25号";
                                temp.distance = 18;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                                for (int j = 0; j < 3; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    if (0 == j)
                                    {
                                        tempalert = AlertType.MOTIONALERT;
                                    }
                                    if (2 == j)
                                    {
                                        tempalert = AlertType.HIGHPRMALERT;
                                    }
                                    if (1 == j)
                                    {
                                        tempalert = AlertType.SPEEDALERT;
                                    }
                                    temp.alerts.Add(tempalert);
                                }

                                for (int j = 0; j < 12; j++)
                                {
                                    string tempgeo = null;
                                    if (0 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (1 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (2 == j)
                                    {
                                        tempgeo = "离开: 公司";
                                    }
                                    if (3 == j)
                                    {
                                        tempgeo = "进入: 公司";
                                    }
                                    if (4 == j)
                                    {
                                        tempgeo = "离开: 体育场";
                                    }
                                    if (5 == j)
                                    {
                                        tempgeo = "进入: 体育场";
                                    }
                                    if (6 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (7 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (8 == j)
                                    {
                                        tempgeo = "离开: 公司";
                                    }
                                    if (9 == j)
                                    {
                                        tempgeo = "进入: 公司";
                                    }
                                    if (10 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (11 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    temp.geofenceInfo.Add(tempgeo);
                                }
                            }
                            if (1 == i)
                            {
                                //temp.StartTime = Convert.ToDateTime("2014-01-22 07:32:00");//"";
                                temp.EndTime = Convert.ToDateTime("2014-01-22 08:00:00"); //"201401220800";
                                temp.endLocation = "北京朝阳区霄云路25号";
                                temp.type = "Trailer";
                                temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                            }
                            tripLogs.Add(temp);
                        }
                    }
                    else if ("Trailer" == lasttrip.type)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            TripLog temp = new TripLog();
                            temp.alerts = new List<AlertType>();
                            temp.geofenceInfo = new List<string>();
                            if (0 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-22 02:01:00");//"201401220201";
                                //temp.EndTime = "";
                                temp.endLocation = "北京朝阳区霄云路25号";
                                temp.distance = 18;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                                for (int j = 0; j < 3; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    if (0 == j)
                                    {
                                        tempalert = AlertType.MOTIONALERT;
                                    }
                                    if (2 == j)
                                    {
                                        tempalert = AlertType.HIGHPRMALERT;
                                    }
                                    if (1 == j)
                                    {
                                        tempalert = AlertType.SPEEDALERT;
                                    }
                                    temp.alerts.Add(tempalert);
                                }

                                for (int j = 0; j < 12; j++)
                                {
                                    string tempgeo = null;
                                    if (0 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (1 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (2 == j)
                                    {
                                        tempgeo = "离开: 公司";
                                    }
                                    if (3 == j)
                                    {
                                        tempgeo = "进入: 公司";
                                    }
                                    if (4 == j)
                                    {
                                        tempgeo = "离开: 体育场";
                                    }
                                    if (5 == j)
                                    {
                                        tempgeo = "进入: 体育场";
                                    }
                                    if (6 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (7 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (8 == j)
                                    {
                                        tempgeo = "离开: 公司";
                                    }
                                    if (9 == j)
                                    {
                                        tempgeo = "进入: 公司";
                                    }
                                    if (10 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (11 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    temp.geofenceInfo.Add(tempgeo);
                                }
                            }
                            if (1 == i)
                            {
                                //temp.StartTime = "";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 20:00:00");//"201401221200";
                                temp.endLocation = "北京朝阳区霄云路25号";
                                temp.type = "Final";
                                temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                            }
                            tripLogs.Add(temp);
                        }
                    }*/
                }
                else
                {
                    if (null == lasttrip || "" == lasttrip.type)
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            TripLog temp = new TripLog();
                            temp.alerts = new List<AlertType>();
                            temp.geofenceInfo = new List<string>();
                            if (0 == i)
                            {
                                temp.type = "Driving";
                                temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                            }
                            if (1 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 19:56:00");//"201401231956";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 20:00:00");//"201401232000";
                                temp.endLocation = "西单";
                                temp.distance = 2;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                            }
                            if (2 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 17:23:00");//"201401231723";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 18:48:00");//"201401231848";
                                temp.endLocation = "长安街";
                                temp.distance = 5;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                                for (int j = 0; j < 1; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    tempalert = AlertType.SPEEDALERT;
                                    temp.alerts.Add(tempalert);
                                }
                                for (int j = 0; j < 1; j++)
                                {
                                    string tempgeo = null;
                                    tempgeo = "进入: 家";
                                    temp.geofenceInfo.Add(tempgeo);
                                }
                            }
                            if (3 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 14:48:00");//"201401231448";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 16:00:00");//"201401231600";
                                temp.endLocation = "西单";
                                temp.distance = 15;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                                for (int j = 0; j < 1; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    tempalert = AlertType.HIGHPRMALERT;
                                    temp.alerts.Add(tempalert);
                                }
                            }
                            if (4 == i)
                            {
                                temp.type = "Day";
                                temp.startLocation = "昨天";
                            }
                            if (5 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 12:23:00");//"201401231223";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 22:01:00");//"201401232201";
                                temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                temp.distance = 20;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                                for (int j = 0; j < 3; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    if (0 == j)
                                    {
                                        tempalert = AlertType.MOTIONALERT;
                                    }
                                    if (2 == j)
                                    {
                                        tempalert = AlertType.HIGHPRMALERT;
                                    }
                                    if (1 == j)
                                    {
                                        tempalert = AlertType.SPEEDALERT;
                                    }
                                    temp.alerts.Add(tempalert);
                                }

                                for (int j = 0; j < 3; j++)
                                {
                                    string tempgeo = null;
                                    if (0 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (1 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (2 == j)
                                    {
                                        tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                    }
                                    temp.geofenceInfo.Add(tempgeo);
                                }
                            }
                            if (6 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 11:00:00");//"201401231100";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 12:00:00");//"201401231200";
                                temp.endLocation = "王府井";
                                temp.distance = 20;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                                for (int j = 0; j < 2; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    if (0 == j)
                                    {
                                        tempalert = AlertType.MOTIONALERT;
                                    }
                                    if (1 == j)
                                    {
                                        tempalert = AlertType.HIGHPRMALERT;
                                    }
                                    temp.alerts.Add(tempalert);
                                }

                                for (int j = 0; j < 3; j++)
                                {
                                    string tempgeo = null;
                                    if (0 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (1 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (2 == j)
                                    {
                                        tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                    }
                                    temp.geofenceInfo.Add(tempgeo);
                                }
                            }
                            if (7 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 10:00:00");//"201401231000";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 10:30:00");//"201401231030";
                                temp.endLocation = "建国门";
                                temp.distance = 20;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                                for (int j = 0; j < 3; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    if (0 == j)
                                    {
                                        tempalert = AlertType.MOTIONALERT;
                                    }
                                    if (2 == j)
                                    {
                                        tempalert = AlertType.HIGHPRMALERT;
                                    }
                                    if (1 == j)
                                    {
                                        tempalert = AlertType.SPEEDALERT;
                                    }
                                    temp.alerts.Add(tempalert);
                                }

                                for (int j = 0; j < 3; j++)
                                {
                                    string tempgeo = null;
                                    if (0 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (1 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (2 == j)
                                    {
                                        tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                    }
                                    temp.geofenceInfo.Add(tempgeo);
                                }
                            }
                            if (8 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-23 06:20:00");//"201401230620";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 08:51:00");//"201401230851";
                                temp.endLocation = "后海";
                                temp.distance = 20;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                                for (int j = 0; j < 3; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    if (0 == j)
                                    {
                                        tempalert = AlertType.MOTIONALERT;
                                    }
                                    if (2 == j)
                                    {
                                        tempalert = AlertType.HIGHPRMALERT;
                                    }
                                    if (1 == j)
                                    {
                                        tempalert = AlertType.SPEEDALERT;
                                    }
                                    temp.alerts.Add(tempalert);
                                }

                                for (int j = 0; j < 3; j++)
                                {
                                    string tempgeo = null;
                                    if (0 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (1 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (2 == j)
                                    {
                                        tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                    }
                                    temp.geofenceInfo.Add(tempgeo);
                                }
                            }
                            tripLogs.Add(temp);
                        }
                    }
                    else if ("Normal" == lasttrip.type)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            TripLog temp = new TripLog();
                            temp.alerts = new List<AlertType>();
                            temp.geofenceInfo = new List<string>();
                            if (0 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-22 09:01:00");//"201401220901";
                                temp.EndTime = Convert.ToDateTime("2014-01-22 12:00:00");//"201401221200";
                                temp.endLocation = "北京朝阳区霄云路25号";
                                temp.distance = 18;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                                for (int j = 0; j < 3; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    if (0 == j)
                                    {
                                        tempalert = AlertType.MOTIONALERT;
                                    }
                                    if (2 == j)
                                    {
                                        tempalert = AlertType.HIGHPRMALERT;
                                    }
                                    if (1 == j)
                                    {
                                        tempalert = AlertType.SPEEDALERT;
                                    }
                                    temp.alerts.Add(tempalert);
                                }

                                for (int j = 0; j < 12; j++)
                                {
                                    string tempgeo = null;
                                    if (0 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (1 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (2 == j)
                                    {
                                        tempgeo = "离开: 公司";
                                    }
                                    if (3 == j)
                                    {
                                        tempgeo = "进入: 公司";
                                    }
                                    if (4 == j)
                                    {
                                        tempgeo = "离开: 体育场";
                                    }
                                    if (5 == j)
                                    {
                                        tempgeo = "进入: 体育场";
                                    }
                                    if (6 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (7 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (8 == j)
                                    {
                                        tempgeo = "离开: 公司";
                                    }
                                    if (9 == j)
                                    {
                                        tempgeo = "进入: 公司";
                                    }
                                    if (10 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (11 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    temp.geofenceInfo.Add(tempgeo);
                                }
                            }
                            if (1 == i)
                            {
                                //temp.StartTime = Convert.ToDateTime("2014-01-22 07:32:00");//"";
                                temp.EndTime = Convert.ToDateTime("2014-01-22 08:00:00"); //"201401220800";
                                temp.endLocation = "北京朝阳区霄云路25号";
                                temp.type = "Trailer";
                                temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                            }
                            tripLogs.Add(temp);
                        }
                    }
                    else if ("Trailer" == lasttrip.type)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            TripLog temp = new TripLog();
                            temp.alerts = new List<AlertType>();
                            temp.geofenceInfo = new List<string>();
                            if (0 == i)
                            {
                                temp.StartTime = Convert.ToDateTime("2014-01-22 02:01:00");//"201401220201";
                                //temp.EndTime = "";
                                temp.endLocation = "北京朝阳区霄云路25号";
                                temp.distance = 18;
                                temp.type = "Normal";
                                temp.healthStatus = HealthStatus.ENGINELIGHTON;
                                for (int j = 0; j < 3; j++)
                                {
                                    AlertType tempalert = new AlertType();
                                    if (0 == j)
                                    {
                                        tempalert = AlertType.MOTIONALERT;
                                    }
                                    if (2 == j)
                                    {
                                        tempalert = AlertType.HIGHPRMALERT;
                                    }
                                    if (1 == j)
                                    {
                                        tempalert = AlertType.SPEEDALERT;
                                    }
                                    temp.alerts.Add(tempalert);
                                }

                                for (int j = 0; j < 12; j++)
                                {
                                    string tempgeo = null;
                                    if (0 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (1 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (2 == j)
                                    {
                                        tempgeo = "离开: 公司";
                                    }
                                    if (3 == j)
                                    {
                                        tempgeo = "进入: 公司";
                                    }
                                    if (4 == j)
                                    {
                                        tempgeo = "离开: 体育场";
                                    }
                                    if (5 == j)
                                    {
                                        tempgeo = "进入: 体育场";
                                    }
                                    if (6 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (7 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    if (8 == j)
                                    {
                                        tempgeo = "离开: 公司";
                                    }
                                    if (9 == j)
                                    {
                                        tempgeo = "进入: 公司";
                                    }
                                    if (10 == j)
                                    {
                                        tempgeo = "离开: 家";
                                    }
                                    if (11 == j)
                                    {
                                        tempgeo = "进入: 家";
                                    }
                                    temp.geofenceInfo.Add(tempgeo);
                                }
                            }
                            if (1 == i)
                            {
                                //temp.StartTime = "";
                                temp.EndTime = Convert.ToDateTime("2014-01-23 20:00:00");//"201401221200";
                                temp.endLocation = "北京朝阳区霄云路25号";
                                temp.type = "Final";
                                temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                            }
                            tripLogs.Add(temp);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return tripLogs;
        }

        //获取Vehicle Detail画面中 TripLogList
        public List<TripLog> GetTripLogLists(long vehicleID, TripLog lasttrip)
        {
            List<TripLog> tripLogs = new List<TripLog>();
            try
            {
                if (null == lasttrip || "" == lasttrip.type || null == lasttrip.type)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        TripLog temp = new TripLog();
                        temp.alerts = new List<AlertType>();
                        temp.geofenceInfo = new List<string>();
                        if (0 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-23 18:48:00");//"201401231848";
                            temp.EndTime = Convert.ToDateTime("2014-01-23 19:56:00");//"201401231956";
                            temp.startLocation = "长安街";
                            temp.endLocation = "西单";
                            temp.distance = 2;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                        }
                        if (1 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-23 16:00:00");//"201401231600";
                            temp.EndTime = Convert.ToDateTime("2014-01-23 17:23:00");//"201401231723";
                            temp.endLocation = "长安街";
                            temp.startLocation = "西单";
                            temp.distance = 5;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 1; j++)
                            {
                                AlertType tempalert = new AlertType();
                                tempalert = AlertType.SPEEDALERT;
                                temp.alerts.Add(tempalert);
                            }
                            for (int j = 0; j < 1; j++)
                            {
                                string tempgeo = null;
                                tempgeo = "进入: 家";
                                temp.geofenceInfo.Add(tempgeo);
                            }
                        }
                        if (2 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-23 14:01:00");//"201401222201";
                            temp.EndTime = Convert.ToDateTime("2014-01-23 14:48:00");//"201401231448";
                            temp.endLocation = "西单";
                            temp.startLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.distance = 15;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                            for (int j = 0; j < 1; j++)
                            {
                                AlertType tempalert = new AlertType();
                                tempalert = AlertType.HIGHPRMALERT;
                                temp.alerts.Add(tempalert);
                            }
                        }
                        if (3 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-23 12:00:00");//"201401231200";
                            temp.EndTime = Convert.ToDateTime("2014-01-23 12:23:00");//"201401231223";
                            temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.startLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.distance = 20;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }

                        }
                        if (4 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-23 02:01:00");//"201401230201";
                            temp.EndTime = Convert.ToDateTime("2014-01-23 12:00:00");//"201401231200";
                            temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.startLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.distance = 20;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }

                        }
                        if (5 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-22 22:53:00");//"201401222253";
                            temp.EndTime = Convert.ToDateTime("2014-01-23 02:01:00");//"201401230201";
                            temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.startLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.distance = 20;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }

                        }
                        if (6 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-22 21:00:00");//"201401222100";
                            temp.EndTime = Convert.ToDateTime("2014-01-22 22:53:00");//"201401222253";
                            temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.startLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.distance = 20;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }

                        }
                        if (7 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-22 20:30:00");//"201401222030";
                            temp.EndTime = Convert.ToDateTime("2014-01-22 21:00:00");//"201401222100";
                            temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.startLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.distance = 20;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }

                        }
                        if (8 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-22 19:00:00");//"201401221900";
                            temp.EndTime = Convert.ToDateTime("2014-01-22 20:30:00");//"201401222030";
                            temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.startLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.distance = 20;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }

                        }
                        if (9 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-22 18:00:00");//"201401221800";
                            temp.EndTime = Convert.ToDateTime("2014-01-22 19:00:00");//"201401221900";
                            temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.startLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.distance = 20;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }

                        }
                        if (10 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-22 16:32:00");//"201401221632";
                            temp.EndTime = Convert.ToDateTime("2014-01-22 18:00:00");//"201401221800";
                            temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.startLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.distance = 20;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }

                        }
                        if (11 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-22 12:01:00");//"201401221201";
                            temp.EndTime = Convert.ToDateTime("2014-01-22 16:32:00");//"201401221632";
                            temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.startLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.distance = 20;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }

                        }
                        if (12 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-22 09:12:00");//"201401220912";
                            temp.EndTime = Convert.ToDateTime("2014-01-22 10:23:00");//"201401221023";
                            temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.startLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.distance = 20;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }

                        }
                        if (13 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-22 07:49:00");//"201401220749";
                            temp.EndTime = Convert.ToDateTime("2014-01-22 08:16:00");//"201401220816";
                            temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.startLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.distance = 20;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }

                        }
                        if (14 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-22 05:23:00");//"201401220523";
                            temp.EndTime = Convert.ToDateTime("2014-01-22 07:00:00");//"201401220700";
                            temp.endLocation = "沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                            temp.startLocation = "北京朝阳区霄云路25号";
                            temp.distance = 20;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;
                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 3; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 沈阳市浑南新区沈营路新秀街阳光新嘉园十五期";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }

                        }
                        tripLogs.Add(temp);
                    }
                }
                else if ("Normal" == lasttrip.type)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        TripLog temp = new TripLog();
                        temp.alerts = new List<AlertType>();
                        temp.geofenceInfo = new List<string>();
                        if (0 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-22 03:01:00");//"201401211301";
                            temp.EndTime = Convert.ToDateTime("2014-01-22 05:00:00");//"201401211600";
                            temp.endLocation = "北京朝阳区霄云路25号";
                            temp.startLocation = "北京朝阳区霄云路25号";
                            temp.distance = 18;
                            temp.type = "Normal";
                            temp.healthStatus = HealthStatus.ENGINELIGHTON;

                            for (int j = 0; j < 3; j++)
                            {
                                AlertType tempalert = new AlertType();
                                if (0 == j)
                                {
                                    tempalert = AlertType.MOTIONALERT;
                                }
                                if (2 == j)
                                {
                                    tempalert = AlertType.HIGHPRMALERT;
                                }
                                if (1 == j)
                                {
                                    tempalert = AlertType.SPEEDALERT;
                                }
                                temp.alerts.Add(tempalert);
                            }

                            for (int j = 0; j < 12; j++)
                            {
                                string tempgeo = null;
                                if (0 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (1 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (2 == j)
                                {
                                    tempgeo = "离开: 公司";
                                }
                                if (3 == j)
                                {
                                    tempgeo = "进入: 公司";
                                }
                                if (4 == j)
                                {
                                    tempgeo = "离开: 体育场";
                                }
                                if (5 == j)
                                {
                                    tempgeo = "进入: 体育场";
                                }
                                if (6 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (7 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                if (8 == j)
                                {
                                    tempgeo = "离开: 公司";
                                }
                                if (9 == j)
                                {
                                    tempgeo = "进入: 公司";
                                }
                                if (10 == j)
                                {
                                    tempgeo = "离开: 家";
                                }
                                if (11 == j)
                                {
                                    tempgeo = "进入: 家";
                                }
                                temp.geofenceInfo.Add(tempgeo);
                            }
                        }
                        if (1 == i)
                        {
                            temp.StartTime = Convert.ToDateTime("2014-01-22 01:00:00");//"20140121900";
                            temp.EndTime = Convert.ToDateTime("2014-01-22 02:00:00");//"201401211200";
                            temp.endLocation = "北京朝阳区霄云路25号";
                            temp.startLocation = "北京朝阳区霄云路25号";
                            temp.type = "Trailer";
                            temp.healthStatus = HealthStatus.ENGINELIGHTOFF;
                        }
                        tripLogs.Add(temp);
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return tripLogs;
        }

        //获取Vehicle Detail画面中 TripLogDetail
        public TripLogDetail GetTripLogDetail(string vehicleID, string endtime)
        {
            TripLogDetail detail = new TripLogDetail();

            detail.StartTime = Convert.ToDateTime("2014-03-31 05:26:00");//"0526";
            detail.EndTime = Convert.ToDateTime("2014-03-31 08:39:00");//"0839";

            detail.startLocation = "西单";
            detail.endLocation = "天安门东";
            detail.distance = 3;
            detail.idleTime = 348;//"000348";
            detail.DriveTime = detail.EndTime - detail.StartTime;//"0313";

            detail.linePoint = new List<LocationPoint>();

            for (int i = 0; i < 8; i++)
            {
                LocationPoint point = new LocationPoint();
                if (0 == i)
                {
                    point.latitude = 39.913285;
                    point.longitude = 116.381777;
                }
                if (1 == i)
                {
                    point.latitude = 39.91326;
                    point.longitude = 116.381777;
                }
                if (2 == i)
                {
                    point.latitude = 39.913398;
                    point.longitude = 116.386735;
                }
                if (3 == i)
                {
                    point.latitude = 39.913398;
                    point.longitude = 116.388424;
                }
                if (4 == i)
                {
                    point.latitude = 39.913453;
                    point.longitude = 116.391622;
                }
                if (5 == i)
                {
                    point.latitude = 39.913578;
                    point.longitude = 116.394479;
                }
                if (6 == i)
                {
                    point.latitude = 39.913689;
                    point.longitude = 116.396796;
                }
                if (7 == i)
                {
                    point.latitude = 39.914145;
                    point.longitude = 116.398252;
                }
                detail.linePoint.Add(point);
            }

            return detail;
        }

        //获取trip 条数至少20条
        public List<TripLog> GetTrips(long vehicleID, DateTime time,Boolean flag,string companyID,int timeZone)
        {
            DebugLog.Debug("[TripLogFetcher] GetTrips(long vehicleID, DateTime time,Boolean flag) Start Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
            DebugLog.Debug("para(vehicleID：" + vehicleID + ";time.ToString():" + time.ToString("yyyy-MM-dd") + ";flag:" + flag + ")");
            try
            {
                List<Alert> alertsFromApi = new List<Alert>();
                List<TripLog> trips = GetTripsFromCache(vehicleID, time, flag, companyID,alertsFromApi);
                DateTime endTime = time;
                if (trips.Count >= TripConstant.TripsNumber)  //每次取20条以上 不够再从DB中取
                {
                    return trips;
                }
                else if (trips.Count > 0)
                {
                    TripLog tripTemp = trips[trips.Count - 1];
                    endTime = (tripTemp.StartTime == new DateTime(0) ? tripTemp.EndTime : tripTemp.StartTime);
                }
                List<TripLog> tripsFromDB = GetTripsFromDB(vehicleID, endTime, (TripConstant.TripsNumber - trips.Count), timeZone, alertsFromApi);
                if (tripsFromDB != null && 0 != tripsFromDB.Count)
                {
                    trips.AddRange(tripsFromDB);
                }
                DebugLog.Debug("[TripLogFetcher] GetTrips(long vehicleID, DateTime time,Boolean flag) End Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
                return trips;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new List<TripLog>();
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new List<TripLog>();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new List<TripLog>();
            }
        }

        //获取trip 条数至少20条
        public List<TripLog> GetTrips(long vehicleID, DateTime nearTime, DateTime oldTime, Boolean flag, string companyID, int timeZone)
        {
            try
            {
                List<TripLog> trips = GetTrips(vehicleID, nearTime, flag, companyID, timeZone);
                List<TripLog> result = trips.FindAll(t => (t.StartTime != null ? t.StartTime.ToUniversalTime() :
                                    (t.EndTime != null ? t.EndTime.ToUniversalTime() : new DateTime())).CompareTo(oldTime) >= 0);
                return result;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new List<TripLog>();
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new List<TripLog>();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new List<TripLog>();
            }
        }

        //cache中获取trip
        public List<TripLog> GetTripsFromCache(long vehicleID, DateTime startTime, Boolean flag, string companyID, List<Alert> alertsFromApi)
        {
            try
            {
                DebugLog.Debug("[TripLogFetcher] GetTripsFromCache(long vehicleID, DateTime time,Boolean flag) Start Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
                DebugLog.Debug("para(vehicleID：" + vehicleID + ";startTime.ToString():" + startTime.ToString("yyyy-MM-dd") + ";flag:" + flag + ")");
                List<VehicleInfo> allVehicles = PickUpTripsFromCache(companyID);
                List<TripLog> results = new List<TripLog>();
                if (null == allVehicles || 0 == allVehicles.Count)
                {
                    return results;
                }
                VehicleInfo vehicleInfo = null;
                foreach (VehicleInfo vehicleTemp in allVehicles)
                {
                    if (vehicleTemp.primarykey == vehicleID)
                    {
                        vehicleInfo = vehicleTemp;
                        break;
                    }
                }
                if (null == vehicleInfo)
                {
                    return results;
                }


                if (null != vehicleInfo.alerts)
                {
                    foreach (Models.API.Alert alertTemp in vehicleInfo.alerts)
                    {
                        Alert alertStor = new Alert();
                        alertStor.guid = alertTemp.Id;
                        alertStor.AlertType = alertTemp.AlertType;
                        alertStor.TriggeredDateTime = alertTemp.TriggeredDateTime.ToUniversalTime();
                        alertStor.Value = alertTemp.Details.ToString();
                        alertsFromApi.Add(alertStor);
                    }
                }
                if (null != vehicleInfo.trips)
                {
                    //GetTripDetailListFromApi(vehicleInfo.trips);//mabiao  20140507
                    vehicleInfo.trips.Sort(
                        (y,x) =>  
                            (x.StartDateTime != null ?  x.StartDateTime.Value.ToUniversalTime() : 
                                (x.EndDateTime != null ? x.EndDateTime.Value.ToUniversalTime() : new DateTime ())).CompareTo(
                                    y.StartDateTime != null ? y.StartDateTime.Value.ToUniversalTime() :
                                        (y.EndDateTime != null ? y.EndDateTime.Value.ToUniversalTime() : new DateTime ()))
                                );
                    results.AddRange(PickUpTripByTime(vehicleID, startTime, vehicleInfo.trips, flag, alertsFromApi));
                }
                DebugLog.Debug("[TripLogFetcher] GetTripsFromCache(long vehicleID, DateTime time,Boolean flag) End Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
                return results;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new List<TripLog>();
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new List<TripLog>();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new List<TripLog>();
            }
        }

        //通过Trip GUID获取TripDetail
        public void GetTripDetailListFromApi(List<TripLog> trips)
        {
            DebugLog.Debug("[TripLogFetcher] GetTripDetailListFromApiStart Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
            DebugLog.Debug("para(trips.count：" + trips.Count+ ")");
            try
            {
                IHalClient client = HalClient.GetInstance();
                List<Task<Models.API.Trip>> tripTasks = new List<Task<Models.API.Trip>>();
                foreach (TripLog tripTemp in trips)
                {
                    HalLink tripLink = new HalLink { Href = Models.API.URI.TRIPS, IsTemplated = true };
                    Dictionary<string, object> idPara = new Dictionary<string, object>();
                    idPara[CommonConstant.ID] = tripTemp.id;
                    Task<Models.API.Trip> tripTask = client.Get<Models.API.Trip>(tripLink, idPara);
                    tripTasks.Add(tripTask);
                }
                int count = 0;
                foreach (Models.API.Trip tripTemp in Task.WhenAll(tripTasks).Result)
                {
                    //trips.Find(t => t.Id == tripTemp.Id).TripRoute = tripTemp.TripRoute;//To Test
                    trips[count] = PickUpLocationFromApiRoute(tripTemp, trips[count]);
                    count++;
                }
                DebugLog.Debug("[TripLogFetcher] GetTripDetailListFromApiEND Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message + "GetTripDetailListFromApi()");
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message + "GetTripDetailListFromApi()");
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message + "GetTripDetailListFromApi()");
            }
        }


        //提取在startTime 之前的Trip并转化为 UI Model
        public List<TripLog> PickUpTripByTime(long vehicleID, DateTime startTime, List<Models.API.Trip> tripsApi, Boolean flag, List<Alert> alertsFromApi)
        {
            try
            {
                DebugLog.Debug("[TripLogFetcher] PickUpTripByTime(long vehicleID, DateTime startTime ,List<Models.API.Trip> tripsApi,Boolean flag) Start Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
                DebugLog.Debug("para(vehicleID：" + vehicleID + ";startTime.ToString():" + startTime.ToString("yyyy-MM-dd") + ";flag:" + flag + ")");
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                List<TripLog> trips = new List<TripLog>();
                Models.API.Trip tripEnd = tripsApi.Find(t => t.EndDateTime != null);
                DateTime tripsEnd = tripEnd == null ? new DateTime () : tripEnd.EndDateTime.Value.ToUniversalTime();
                Models.API.Trip tripStart = tripsApi.FindLast(t => t.StartDateTime != null);
                DateTime tripsStart = tripStart == null ? new DateTime() : tripStart.StartDateTime.Value.ToUniversalTime();
                List<Alert> allAlerts = new List<Alert> ();
                List<Alert> engnieAlerts = new List<Alert>();
                if (null != tripEnd && null != tripStart)
                {
                    allAlerts.AddRange(alertsFromApi);
                    List<Alert> DBAlerts = vehicleInterface.GetAlertByVehicleIDAndTime(vehicleID, tripsStart, tripsEnd);
                    if (null != DBAlerts)
                    {
                        allAlerts.AddRange(DBAlerts);
                    }
                    engnieAlerts = GetEngineAlertFromCacheAndDB(vehicleID, tripsStart, tripsEnd);
                }

                DebugLog.Debug("[TripLogFetcher] PickUpTripByTime(long vehicleID, DateTime startTime ,List<Models.API.Trip> tripsApi,Boolean flag) Mid01");
                foreach (Models.API.Trip tripTemp in tripsApi)
                {
                    DateTime? tripTime = tripTemp.StartDateTime;
                    if (tripTime == null)                                           //取trip中不空的时间  已开始时间为主
                    {
                        tripTime = tripTemp.EndDateTime;
                    }
                    if (tripTime == null || DateTime.Compare(startTime, tripTime.Value.ToUniversalTime()) < 1)     //都为空或首次取trip或time之后发生的trip不取出
                    {
                        continue;
                    }
                    TripLog trip = new TripLog();
                    trip.id = tripTemp.Id;
                    trip.StartTime = tripTemp.StartDateTime == null ? new DateTime(0) : tripTemp.StartDateTime.Value.ToUniversalTime();//mabiao 0409 utc
                    trip.startLocation = tripTemp.StartLocation;
                    trip.EndTime = tripTemp.EndDateTime == null ? new DateTime(0) : tripTemp.EndDateTime.Value.ToUniversalTime();//mabiao 0409 utc
                    trip.endLocation = tripTemp.EndLocation;
                    //trip = PickUpLocationFromApiRoute(tripTemp, trip);
                    trip.distance = Math.Round(tripTemp.Distance, 1);
                    trip.healthStatus = HealthStatus.ENGINELIGHTOFF;//默认制OFF
                    List<Alert> alerts = allAlerts.FindAll(t => t.TriggeredDateTime != null && t.TriggeredDateTime < trip.EndTime && t.TriggeredDateTime > trip.StartTime);
                    List<Alert> alertsEngine = engnieAlerts.FindAll(t => t.TriggeredDateTime!=null && t.EngineEndTime!=null && !(t.TriggeredDateTime < trip.StartTime && t.EngineEndTime < trip.StartTime) && !(t.TriggeredDateTime > trip.EndTime && t.EngineEndTime > trip.EndTime));
                    trip = PickUpAlertsInfo(trip, alerts);
                    if (null != alertsEngine && 0 < alertsEngine.Count)
                    {
                        trip.healthStatus = HealthStatus.ENGINELIGHTON;
                    }
                    trips.Add(trip);
                    if (trips.Count == TripConstant.TripsNumber) { break; }
                }

                DebugLog.Debug("[TripLogFetcher] PickUpTripByTime(long vehicleID, DateTime startTime ,List<Models.API.Trip> tripsApi,Boolean flag) Mid03");
                GetTripDetailListFromApi(trips);
                DebugLog.Debug("[TripLogFetcher] PickUpTripByTime(long vehicleID, DateTime startTime ,List<Models.API.Trip> tripsApi,Boolean flag) End Time:" + DateTime.Now.ToString("HH:mm:ss:fff"));
                return trips;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new List<TripLog> ();
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new List<TripLog> ();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new List<TripLog> ();
            }
        }

        //启停点经纬度 并转换成百度坐标
        public TripLog  PickUpLocationFromApiRoute(Models.API.Trip tripTemp ,TripLog trip)
        {
            try
            {
                DebugLog.Debug("[TripLogFetcher] PickUpLocationFromApiRoute(Models.API.Trip tripTemp ,TripLog trip) Start");
                if (null == tripTemp.TripRoute || tripTemp.TripRoute.Count == 0)
                {
                    trip.startlocationGPSLng = 0;
                    trip.startlocationGPSLat = 0;
                    trip.startlocationLng = 0;
                    trip.startlocationLat = 0;
                    trip.isFirstFlag = 0;
                    trip.isLastFlag = 0;
                    trip.endlocationGPSLng = 0 ;
                    trip.endlocationGPSLat = 0;
                    trip.endlocationLng = 0;
                    trip.endlocationLat = 0;
                    return trip;
                }
                Models.API.TripRouteDetail startLocation = tripTemp.TripRoute.Find(t => !t.location.latitude.Equals(0) && !t.location.longitude.Equals(0));
                if (null == startLocation)
                {
                    trip.startlocationGPSLng = 0;
                    trip.startlocationGPSLat = 0;
                    trip.startlocationLng = 0;
                    trip.startlocationLat = 0;
                    trip.isFirstFlag = 0;
                }
                else
                {
                    trip.startlocationGPSLng = startLocation.location.longitude;
                    trip.startlocationGPSLat = startLocation.location.latitude;

                    //非估计的
                    trip.isFirstFlag = 1;
                    GeoPointDTO point = new GeoPointDTO();
                    point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(startLocation.location.longitude, startLocation.location.latitude);
                    trip.startlocationLng = point.longitude;
                    trip.startlocationLat = point.latitude;
                    if (tripTemp.TripRoute[0].location.longitude.Equals(0) || tripTemp.TripRoute[0].location.latitude.Equals(0))
                    {
                        //估计的
                        trip.isFirstFlag = 0;
                    }
                }
                Models.API.TripRouteDetail endLocation = tripTemp.TripRoute.FindLast(t => !t.location.latitude.Equals(0) && !t.location.longitude.Equals(0));
                if (null == endLocation)
                {
                    trip.isLastFlag = 0;
                    trip.endlocationGPSLng = 0;
                    trip.endlocationGPSLat = 0;
                    trip.endlocationLng = 0;
                    trip.endlocationLat = 0;
                }
                else
                {
                    trip.endlocationGPSLng = endLocation.location.longitude;
                    trip.endlocationGPSLat = endLocation.location.latitude;
                    trip.isLastFlag = 1;
                    GeoPointDTO point = new GeoPointDTO();
                    point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(endLocation.location.longitude, endLocation.location.latitude);
                    trip.endlocationLng = point.longitude;
                    trip.endlocationLat = point.latitude;
                    if (tripTemp.TripRoute[tripTemp.TripRoute.Count - 1].location.longitude.Equals(0) || tripTemp.TripRoute[tripTemp.TripRoute.Count - 1].location.latitude.Equals(0))
                    {
                        trip.isLastFlag = 0;
                    }
                }
                DebugLog.Debug("[TripLogFetcher] PickUpLocationFromApiRoute(Models.API.Trip tripTemp ,TripLog trip) End");
                return trip;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new TripLog ();
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new TripLog ();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new TripLog ();
            }
        }

        //获取引擎灯Alert从Cache和DB
        public List<Alert> GetEngineAlertFromCacheAndDB(long vehicleID, DateTime tripsStart, DateTime tripsEnd)
        {
            try
            {
                DebugLog.Debug("[TripLogFetcher] GetEngineAlertFromCacheAndDB(long vehicleID, DateTime tripsStart, DateTime tripsEnd) Start");
                DebugLog.Debug("para(vehicleID：" + vehicleID + ";tripsStart.ToString():" + tripsStart.ToString("yyyy-MM-dd") + ";tripsEnd.ToString():" + tripsEnd.ToString("yyyy-MM-dd") + ")");
                List<Alert> engineAlert = new List<Alert>();
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                CacheService service = new CacheService();
                string vehicleGUID = vehicleInterface.GetVehicleByID(vehicleID).id;

                if (null != service.CacheGet(vehicleGUID + "_engine"))
                {
                    Boolean isEngineLight = (Boolean)service.CacheGet(vehicleGUID + "_engine");
                    if (isEngineLight)
                    {
                        Alert engineTemp = new Alert();
                        engineTemp.vehicleId = vehicleID;
                        engineTemp.AlertType = TripConstant.Engine;
                        engineTemp.TriggeredDateTime = (DateTime?)service.CacheGet(vehicleGUID + "_engineTime");
                        engineTemp.EngineEndTime = tripsEnd;
                        engineAlert.Add(engineTemp);
                    }
                }
                //Todo From Cache
                engineAlert.AddRange(vehicleInterface.GetEngineAlertByVehicleIDAndTime(vehicleID, tripsStart, tripsEnd));
                DebugLog.Debug("[TripLogFetcher] GetEngineAlertFromCacheAndDB(long vehicleID, DateTime tripsStart, DateTime tripsEnd) End");
                return engineAlert;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new List<Alert> ();
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new List<Alert> ();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new List<Alert> ();
            }
        }
        //从DB中提取在startTime 之前的制定数目的Trip
        public List<TripLog> GetTripsFromDB(long vehicleID, DateTime startTime, int count, int timeZone, List<Alert> alertsFromApi)
        {
            try
            {
                DebugLog.Debug("[TripLogFetcher] GetTripsFromDB(long vehicleID, DateTime startTime, int count) Start");
                DebugLog.Debug("para(vehicleID：" + vehicleID + ";startTime.ToString():" + startTime.ToString("yyyy-MM-dd") + ";count:" + count + ")");
                List<TripLog> tripLogs = new List<TripLog>();
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                List<Trip> tripsDB = vehicleInterface.GetTripsByVehicleIDAndTime(vehicleID, startTime, count);
                if (tripsDB == null || 0 == tripsDB.Count)
                {
                    return null;
                }
                Trip tripEnd = tripsDB.Find(t => t.endtime != null);
                DateTime tripsEnd = tripEnd == null ? new DateTime() : tripEnd.endtime.Value;
                Trip tripStart = tripsDB.FindLast(t => t.startTime != null);
                DateTime tripsStart = tripStart == null ? new DateTime() : tripStart.startTime.Value;
                List<Alert> allAlerts = new List<Alert> ();
                List<Alert> engnieAlerts = new List<Alert>();
                if (null != tripEnd && null != tripStart)
                {
                    allAlerts.AddRange(alertsFromApi);
                    List<Alert> DBAlerts = vehicleInterface.GetAlertByVehicleIDAndTime(vehicleID, tripsStart, tripsEnd);
                    if (null != DBAlerts)
                    {
                        allAlerts.AddRange(DBAlerts);
                    }
                    engnieAlerts = GetEngineAlertFromCacheAndDB(vehicleID, tripsStart, tripsEnd);
                }
                TimeSpan backTimeZone = TimeZoneInfo.Local.BaseUtcOffset;

                foreach (Trip tripTemp in tripsDB)
                {
                    TripLog tripLog = new TripLog();
                    List<Alert> alerts = new List<Alert>();
                    tripLog.id = tripTemp.guid;
                    tripLog.StartTime = tripTemp.startTime == null ? new DateTime() : tripTemp.startTime.Value.Add(backTimeZone).ToUniversalTime();
                    tripLog.startLocation = tripTemp.startlocation;
                    tripLog.EndTime = tripTemp.endtime == null ? new DateTime() : tripTemp.endtime.Value.Add(backTimeZone).ToUniversalTime();
                    tripLog.endLocation = tripTemp.endlocation;
                    tripLog = PickUpLocationFromDB(tripTemp, tripLog);
                    tripLog.distance = tripTemp.distance == null ? 0 : Math.Round(tripTemp.distance.Value, 1);
                    tripLog.healthStatus = HealthStatus.ENGINELIGHTOFF;     //默认制OFF
                    if (null != tripTemp.endtime && null != tripTemp.startTime)
                    {
                        alerts = allAlerts.FindAll(t => t.TriggeredDateTime < tripTemp.endtime && t.TriggeredDateTime > tripTemp.startTime);
                    }
                    tripLog = PickUpAlertsInfo(tripLog, alerts);
                    List<Alert> alertsEngine = engnieAlerts.FindAll(t => !(t.TriggeredDateTime < tripTemp.startTime && t.EngineEndTime < tripTemp.startTime) && !(t.TriggeredDateTime > tripTemp.endtime && t.EngineEndTime > tripTemp.endtime));
                    if (null != alertsEngine && 0 < alertsEngine.Count)
                    {
                        tripLog.healthStatus = HealthStatus.ENGINELIGHTON;
                    }
                    tripLogs.Add(tripLog);
                }
                DebugLog.Debug("[TripLogFetcher] GetTripsFromDB(long vehicleID, DateTime startTime, int count) End");
                return tripLogs;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new List<TripLog>();
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new List<TripLog>();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new List<TripLog>();
            }
        }

        //启停点经纬度 并转换成百度坐标
        public TripLog PickUpLocationFromDB(Trip tripTemp, TripLog trip)
        {
            try
            {
                DebugLog.Debug("[TripLogFetcher] PickUpLocationFromDB(Trip tripTemp, TripLog trip) Start");
                if (tripTemp.startlocationLng == null && tripTemp.startlocationLng.Value.Equals(0) && tripTemp.startlocationLat == null && tripTemp.startlocationLat.Value.Equals(0))
                {
                    trip.startlocationGPSLng = 0;
                    trip.startlocationGPSLat = 0;
                    trip.startlocationLng = 0;
                    trip.startlocationLat = 0;
                    trip.isFirstFlag = 0;
                }
                else
                {
                    trip.startlocationGPSLng = tripTemp.startlocationLng.Value;
                    trip.startlocationGPSLat = tripTemp.startlocationLat.Value;
                    GeoPointDTO point = new GeoPointDTO();
                    point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(tripTemp.startlocationLng.Value, tripTemp.startlocationLat.Value);
                    trip.startlocationLng = point.longitude;
                    trip.startlocationLat = point.latitude;
                    trip.isFirstFlag = tripTemp.isFirstFlag == null ? 0 : tripTemp.isFirstFlag.Value;
                }
                if (tripTemp.endlocationLng == null && tripTemp.endlocationLng.Value.Equals(0) && tripTemp.endlocationLat == null && tripTemp.endlocationLat.Value.Equals(0))
                {
                    trip.isLastFlag = 0;
                    trip.endlocationGPSLng = 0;
                    trip.endlocationGPSLat = 0;
                    trip.endlocationLng = 0;
                    trip.endlocationLat = 0;
                }
                else
                {
                    trip.endlocationGPSLng = tripTemp.endlocationLng.Value;
                    trip.endlocationGPSLat = tripTemp.endlocationLat.Value;
                    GeoPointDTO point = new GeoPointDTO();
                    point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(tripTemp.endlocationLng.Value, tripTemp.endlocationLat.Value);
                    trip.endlocationLng = point.longitude;
                    trip.endlocationLat = point.latitude;
                    trip.isLastFlag = tripTemp.isLastFlag == null ? 0 : tripTemp.isLastFlag.Value;
                }
                DebugLog.Debug("[TripLogFetcher] PickUpLocationFromDB(Trip tripTemp, TripLog trip) End");
                return trip;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return trip;
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return trip;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return trip;
            }
        }

        //摘取三种alert
        public TripLog PickUpAlertsInfo(TripLog tripLog, List<Alert> alerts)
        {
            try
            {
                DebugLog.Debug("[TripLogFetcher] PickUpAlertsInfo(TripLog tripLog, List<Alert> alerts) Start");
                tripLog.alerts = new List<AlertType>();
                tripLog.geofenceInfo = new List<string>();
                alerts = alerts.Distinct(new AlertComparer()).ToList();
                alerts.Sort((y,x) => x.TriggeredDateTime.Value.CompareTo(y.TriggeredDateTime.Value));
                foreach (Alert alertTemp in alerts)
                {
                    if (alertTemp.AlertType.Trim().Equals(TripConstant.Motion) || alertTemp.AlertType.Trim().Equals("Motion"))
                    {
                        tripLog.alerts.Add(AlertType.MOTIONALERT);
                    }
                    if (alertTemp.AlertType.Trim().Equals(TripConstant.Rpm) || alertTemp.AlertType.Trim().Equals("Engine RPM"))
                    {
                        tripLog.alerts.Add(AlertType.HIGHPRMALERT);
                    }
                    if (alertTemp.AlertType.Trim().Equals(TripConstant.Speed) || alertTemp.AlertType.Trim().Equals("Speed"))
                    {
                        tripLog.alerts.Add(AlertType.SPEEDALERT);
                    }
                    if (alertTemp.AlertType.Trim().Equals(TripConstant.GeofenceType.GEO_1.ToString()) ||
                        alertTemp.AlertType.Trim().Equals(TripConstant.GeofenceType.GEO_2.ToString()) ||
                        alertTemp.AlertType.Trim().Equals(TripConstant.GeofenceType.GEO_3.ToString()) ||
                        alertTemp.AlertType.Trim().Equals(TripConstant.GeofenceType.GEO_4.ToString()) ||
                        alertTemp.AlertType.Trim().Equals(TripConstant.GeofenceType.GEO_5.ToString()) ||
                        alertTemp.AlertType.Trim().Equals(TripConstant.GeofenceType.GEO_6.ToString()) ||
                        alertTemp.AlertType.Trim().Equals("Geo1")                                     ||
                        alertTemp.AlertType.Trim().Equals("Geo2")                                     ||
                        alertTemp.AlertType.Trim().Equals("Geo3")                                     ||
                        alertTemp.AlertType.Trim().Equals("Geo4")                                     ||
                        alertTemp.AlertType.Trim().Equals("Geo5")                                     ||
                        alertTemp.AlertType.Trim().Equals("Geo6")                                       )
                    {
                        dynamic alertDetail = JsonConvert.DeserializeObject(alertTemp.Value);
                        string geoEvent = alertDetail["Event"] == null ? "" : alertDetail["Event"].Value;
                        string name = alertDetail["Name"] == null ? "" : alertDetail["Name"].Value;
                        string info = geoEvent.Replace(TripConstant.Exited, "离开").Replace(TripConstant.Enter, "进入") + ":" + name;
                        tripLog.geofenceInfo.Add(info);
                    }
                }
                DebugLog.Debug("[TripLogFetcher] PickUpAlertsInfo(TripLog tripLog, List<Alert> alerts) End");
                return tripLog;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return tripLog;
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return tripLog;
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return tripLog;
            }
        }

        //通过trip guid获取tripdetail数据
        public TripLogDetail GetTripDetail(string tripGUID, int timeZone)
        {
            try
            {
                DebugLog.Debug("[TripLogFetcher] GetTripDetail(string tripGUID) Start");
                DebugLog.Debug("para(tripGUID:" + tripGUID + ")");
                VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                TripLogDetail tripDetail = new TripLogDetail();
                tripDetail.linePoint = new List<LocationPoint>();

                Models.API.Trip tripApi = GetTripFromApi(tripGUID);
                Trip tripDB = vehicleInterface.GetTripByGuid(tripGUID);
                tripDetail.distance = Math.Round(tripApi.Distance, 1);
                tripDetail.StartTime = tripApi.StartDateTime == null ? new DateTime(0) : tripApi.StartDateTime.Value.ToUniversalTime();//mabiao 0409 utc
                tripDetail.EndTime = tripApi.EndDateTime == null ? new DateTime() : tripApi.EndDateTime.Value.ToUniversalTime();//mabiao 0409 utc
                tripDetail.DriveTime = (tripDetail.StartTime == null || tripDetail.EndTime == null || tripDetail.StartTime == new DateTime() || tripDetail.EndTime == new DateTime()) ? new TimeSpan() : (TimeSpan)(tripDetail.EndTime - tripDetail.StartTime);
                tripDetail.idleTime = tripApi.IdleTime;
                tripDetail.startLocation = tripDB.startlocation == null ? null : tripDB.startlocation.Trim();
                tripDetail.endLocation = tripDB.endlocation == null ? null : tripDB.endlocation.Trim();
                if (tripApi.TripRoute != null)
                {
                    foreach (Models.API.TripRouteDetail locationTemp in tripApi.TripRoute)
                    {
                        if (locationTemp.location.longitude.Equals(0) || locationTemp.location.latitude.Equals(0))
                        {
                            continue;
                        }
                        GeoPointDTO point = new GeoPointDTO();
                        LocationPoint pointLocation = new LocationPoint();
                        point = BaiduWGSPoint.GeoPointDTO.InternationalToBaidu(locationTemp.location.longitude, locationTemp.location.latitude);
                        pointLocation.longitude = point.longitude;
                        pointLocation.latitude = point.latitude;
                        tripDetail.linePoint.Add(pointLocation);
                    }
                }
                DebugLog.Debug("[TripLogFetcher] GetTripDetail(string tripGUID) End");
                return tripDetail;
            }
            catch (DBException e)
            {
                DebugLog.Exception(DebugLog.DebugType.DBException, e.Message);
                return new TripLogDetail();
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new TripLogDetail();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new TripLogDetail();
            }
        }

        //通过guid获取api中route
        public Models.API.Trip GetTripFromApi(string tripGUID)
        {
            try
            {
                DebugLog.Debug("[TripLogFetcher] GetTripFromApi(string tripGUID) Start");
                DebugLog.Debug("para(tripGUID:" + tripGUID + ")");
                IHalClient client = HalClient.GetInstance();
                HalLink tripLink = new HalLink { Href = Models.API.URI.TRIPS, IsTemplated = true };
                Dictionary<string, object> idPara = new Dictionary<string, object>();
                idPara[CommonConstant.ID] = tripGUID;
                DebugLog.Debug("[TripLogFetcher] GetTripFromApi(string tripGUID) End");
                return client.Get<Models.API.Trip>(tripLink, idPara).Result;
            }
            catch (HttpException e)
            {
                DebugLog.Exception(DebugLog.DebugType.HttpException, e.Message);
                return new Models.API.Trip ();
            }
            catch (Exception e)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, e.Message);
                return new Models.API.Trip ();
            }
        }

        public List<VehicleInfo> PickUpTripsFromCache(string companyID)
        {
            DebugLog.Debug("[TripLogFetcher] PickUpTripsFromCache()  paras[companyID = " + companyID + "Time:"+ DateTime.Now.ToString("HH:mm:ss:fff") +"]");
            List<VehicleInfo> allVehicle = new List<VehicleInfo>();
            try
            {
                CacheService service = new CacheService();

                List<Models.API.CustomerData> customers = (List<Models.API.CustomerData>)service.CacheGet(companyID + "_Cache");
                //chenyangwen 20140705
                int i = 0;
                while (customers == null || customers.Count == 0)
                {
                    Thread.Sleep(200);
                    customers = (List<Models.API.CustomerData>)service.CacheGet(companyID + "_Cache");
                    //chenyangwen 20140705
                    i++;
                    if (i >= 3)
                        break;
                    DebugLog.Debug("[TripLogFetcher] PickUpTripsFromCache()  END(return allVehicle)[return null customers becauseOf null cache]");
                }
                //chenyangwen 20140705
                if (customers != null)
                {
                    foreach (Models.API.CustomerData customerTemp in customers)
                    {
                        if (customerTemp.Vehicles == null || customerTemp.Vehicles.Count == 0)
                        {
                            //...todo
                            continue;
                        }
                        //apiVehicleInfos.AddRange(customerTemp.Vehicles);
                        foreach (Models.API.Vehicle vehicleTemp in customerTemp.Vehicles)
                        {
                            VehicleDBInterface vehicleInterface = new VehicleDBInterface();
                            if (null != vehicleTemp.ConnectedDeviceId)
                            {
                                VehicleInfo vehicle = new VehicleInfo();
                                Models.Vehicle dbvehicle = vehicleInterface.GetVehicleByGUID(vehicleTemp.Id);
                                vehicle.primarykey = dbvehicle.pkid;
                                vehicle.trips = vehicleTemp.Trips;
                                vehicle.alerts = vehicleTemp.Alerts;
                                allVehicle.Add(vehicle);
                            }
                        }
                    }
                }
                DebugLog.Debug("[TripLogFetcher] PickUpTripsFromCache()  END(return allVehicle)[allVehicle.Count=" + allVehicle.Count + "Time:" + DateTime.Now.ToString("HH:mm:ss:fff") + "]");
                return allVehicle;
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.Message);
                DebugLog.Debug("[TripLogFetcher] PickUpTripsFromCache()  END(return allVehicle)[return null allVehicle becauseOf Exception]");
                return allVehicle;
            }
        }

        public string IsAlertVehicle(string companyID, long vehicleID, DateTime startTime)
        {
            try
            {
                DebugLog.Debug("[TripLogFetcher] IsAlertVehicle()  paras[companyID = " + companyID + "vehicleID = " + vehicleID + "startTime.toString() = " + startTime.ToString("yyyy-MM-dd") + "]");
                List<VehicleInfo> vehicleInfo = PickUpTripsFromCache(companyID);
                if (null == vehicleInfo || 0 == vehicleInfo.Count)
                {
                    return "OK";
                }
                VehicleInfo infoVehicle = new VehicleInfo();
                foreach (VehicleInfo infoTemp in vehicleInfo)
                {
                    if (infoTemp.primarykey.Equals(vehicleID))
                    {
                        infoVehicle = infoTemp;
                    }
                }
                if (null == infoVehicle.alerts || 0 == infoVehicle.alerts.Count)
                {
                    return "OK";
                }
                else
                {
                    Models.API.Alert alert = infoVehicle.alerts.Find(t => t.TriggeredDateTime.ToUniversalTime() > startTime);
                    if (alert != null)
                    {
                        return "Alert";
                    }
                    else
                    {
                        return "OK";
                    }
                }
                DebugLog.Debug("[TripLogFetcher] IsAlertVehicle()  End");
            }
            catch (Exception e)
            {
                DebugLog.Debug(e.Message);
                DebugLog.Debug("[TripLogFetcher] PickUpTripsFromCache()  END(return allVehicle)[return null allVehicle becauseOf Exception]");
                return "OK";
            }
        }
    }
}