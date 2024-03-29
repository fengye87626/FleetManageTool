﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BaiduWGSPoint
{

    public class GeoPointDTO
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public static double pi = 3.14159265358979324;
        public static double x_pi = 3.14159265358979324 * 3000.0 / 180.0;
        public static double a = 6378245.0;
        public static double ee = 0.00669342162296594323;

        public static bool isOutsideOfChina(double lat, double lon)
        {
            if (lon < 72.004 || lon > 137.8347)
                return true;
            if (lat < 0.8293 || lat > 55.8271)
                return true;
            return false;
        }

        public static GeoPointDTO ChineseToBaidu(GeoPointDTO chineseGeoPoint)
        {
            double x = chineseGeoPoint.longitude, y = chineseGeoPoint.latitude;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);

            return new GeoPointDTO()
            {
                longitude = z * Math.Cos(theta) + 0.0065,
                latitude = z * Math.Sin(theta) + 0.006
            };
        }

        public static GeoPointDTO BaiduToChinese(GeoPointDTO baiduGeoPoint)
        {
            double x = baiduGeoPoint.longitude - 0.0065, y = baiduGeoPoint.latitude - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);

            return new GeoPointDTO()
            {
                longitude = z * Math.Cos(theta),
                latitude = z * Math.Sin(theta)
            };
        }

        public static double LatitudeConversionForChina(double x, double y)
        {
            double ret = -100.0 + 2.0 * x + 3.0 * y + 0.2 * y * y + 0.1 * x * y + 0.2 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(y * pi) + 40.0 * Math.Sin(y / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(y / 12.0 * pi) + 320 * Math.Sin(y * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        public static double LongitudeConversionForChina(double x, double y)
        {
            double ret = 300.0 + x + 2.0 * y + 0.1 * x * x + 0.1 * x * y + 0.1 * Math.Sqrt(Math.Abs(x));
            ret += (20.0 * Math.Sin(6.0 * x * pi) + 20.0 * Math.Sin(2.0 * x * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(x * pi) + 40.0 * Math.Sin(x / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(x / 12.0 * pi) + 300.0 * Math.Sin(x / 30.0 * pi)) * 2.0 / 3.0;
            return ret;
        }

        public static GeoPointDTO InternationalToChinese(GeoPointDTO internationalGeoPoint)
        {
            if (isOutsideOfChina(internationalGeoPoint.latitude, internationalGeoPoint.longitude))
            {
                return new GeoPointDTO()
                {
                    latitude = internationalGeoPoint.latitude,
                    longitude = internationalGeoPoint.longitude
                };
            }

            double dLat = LatitudeConversionForChina(internationalGeoPoint.longitude - 105.0, internationalGeoPoint.latitude - 35.0);
            double dLon = LongitudeConversionForChina(internationalGeoPoint.longitude - 105.0, internationalGeoPoint.latitude - 35.0);
            double radLat = internationalGeoPoint.latitude / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);

            return new GeoPointDTO()
            {
                latitude = internationalGeoPoint.latitude + dLat,
                longitude = internationalGeoPoint.longitude + dLon
            };
        }

        /// <summary>
        /// Convert an International Coordinate into a Chinese Coordinate
        /// </summary>
        public static GeoPointDTO ChineseToInternational(GeoPointDTO chineseGeoPoint)
        {
            if (isOutsideOfChina(chineseGeoPoint.latitude, chineseGeoPoint.longitude))
            {
                return new GeoPointDTO()
                {
                    latitude = chineseGeoPoint.latitude,
                    longitude = chineseGeoPoint.longitude
                };
            }

            double dLat = LatitudeConversionForChina(chineseGeoPoint.longitude - 105.0, chineseGeoPoint.latitude - 35.0);
            double dLon = LongitudeConversionForChina(chineseGeoPoint.longitude - 105.0, chineseGeoPoint.latitude - 35.0);
            double radLat = chineseGeoPoint.latitude / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);

            return new GeoPointDTO()
            {
                latitude = chineseGeoPoint.latitude - dLat,
                longitude = chineseGeoPoint.longitude - dLon
            };
        }

        //百度坐标转换成国际坐标 参数：经度、纬度
        public static GeoPointDTO BaiduToInternational(double Baidulng, double Baidulat)
        {
            try
            {
                GeoPointDTO tmp = new GeoPointDTO();
                tmp.longitude = Baidulng;
                tmp.latitude = Baidulat;

                //直接返回GeoPointDTO类型的对象。
                return GeoPointDTO.ChineseToInternational(GeoPointDTO.BaiduToChinese(tmp));
            }
            catch (Exception e)
            {
                throw new Exception("百度坐标转换成国际坐标有问题" + e.Message);
            }
        }

        //国际坐标转换成百度坐标 参数：经度、纬度
        public static GeoPointDTO InternationalToBaidu(double Internationallng, double Internationallat)
        {
            try
            {
                GeoPointDTO tmp = new GeoPointDTO();
                tmp.longitude = Internationallng;
                tmp.latitude = Internationallat;

                //直接返回GeoPointDTO类型的对象。
                return GeoPointDTO.ChineseToBaidu(GeoPointDTO.InternationalToChinese(tmp));
            }
            catch (Exception e)
            {
                throw new Exception("国际坐标转换成百度坐标有问题" + e.Message);
            }
        }
    }
}
