using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers; 
using System.Text;

namespace FleetManageToolWebRole.BusinessLayer
{
    public class WebServiceException : Exception
    {
        private int code;
        private Object msg;
        public int Code { get; set; }
        public Object Msg { get; set; }
        public WebServiceException(int code, Object msg)
        {
            this.code = code;
            this.msg = msg;
        }
    }
    public class WebServiceUndefineException : Exception
    {
        private string msg;
        public string Msg { get; set; }
        public WebServiceUndefineException(string msg)
        {
            this.msg = msg;
        }
    }
    public class WebDate
    {
        DateTime date;
        public WebDate(DateTime date)
        {
            this.date = date;
        }
        public DateTime Date { get; set; }
    }
    public class WebTime
    {
        DateTime time;
        public WebTime(DateTime time)
        {
            this.time = time;
        }
        public DateTime Time { get; set; }
    }
    class ReturnData
    {
        bool result = false;
        int code = 0;
        string data = string.Empty;

        public bool Result { get; set; }
        public int Code { get; set; }
        public string Data { get; set; }
    }

    public class WebService
    {
        public const string ROOT_URL = @"http://localhost:8080/";

        public const string OBD_STATUS = @"OBD/status";
        public const string NOTIFICATION_MAIL = @"notification/mail";
        public const string GEOFENCES_CREATE = @"geofences/create";
        public const string GEOFENCES_UPDATE = @"geofences/update";
        public const string GEOFENCES_DELETE = @"geofences/delete";
        public const string GEOFENCES_VEHICLES_CREATE = @"geofences/vehicles/create";
        public const string GEOFENCES_VEHICLES_DELETE = @"geofences/vehicles/delete";
        public const string GEOFENCES = @"geofences";
        public const string VEHICLES_VINS = @"vehicles/vins";
        public const string VEHICLES_STATUS = @"vehicles/status";
        public const string VEHICLES_TRIPLOG = @"vehicles/triplog";
        public const string VEHICLES_TRIPLOG_DETAILS = @"vehicles/triplog/details";
        public const string VEHICLES_ALERT = @"vehicles/alert";
        public const string VEHICLES_USAGE = @"vehicles/usage";

        private Dictionary<string, string> paras = new Dictionary<string, string>();
        private string url = string.Empty;

        public WebService(string resource)
        {
            this.url = ROOT_URL + resource;
        }

        private string GetUrl()
        {
            string thisUrl = url;
            if (paras.Count > 0)
            {
                List<string> list = new List<string>();
                foreach (KeyValuePair<string, string> kvp in paras)
                {
                    list.Add(kvp.Key + "=" + kvp.Value);
                }
                thisUrl += "?";
                thisUrl += string.Join("&", list.ToArray());
            }
            return thisUrl;
        }

        private ReturnData parseJson(string json)
        {
            return null;
            //TODO
        }

        public T Get<T>()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(GetUrl()).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    ReturnData rd = parseJson(json);
                    if (rd.Result)
                    {
                        return JsonConvert.DeserializeObject<T>(rd.Data);
                    }
                    if (999 == rd.Code)
                    {
                        throw new WebServiceUndefineException("UndefineException!");
                    }
                    else
                    {
                        throw new WebServiceException(rd.Code, JsonConvert.DeserializeObject<T>(rd.Data));
                    }
                }
                else
                {
                    throw new WebServiceUndefineException("http error!");
                }
            }
            catch (Exception e)
            {
                throw new WebServiceUndefineException(e.Message);
            }
        }

        public T Post<T>()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.PostAsync(GetUrl(), null).Result;
                if (response.IsSuccessStatusCode)
                {
                    string json = response.Content.ReadAsStringAsync().Result;
                    ReturnData rd = parseJson(json);
                    if (rd.Result)
                    {
                        return JsonConvert.DeserializeObject<T>(rd.Data);
                    }
                    if (999 == rd.Code)
                    {
                        throw new WebServiceUndefineException("UndefineException!");
                    }
                    else
                    {
                        throw new WebServiceException(rd.Code, JsonConvert.DeserializeObject<T>(rd.Data));
                    }
                }
                else
                {
                    throw new WebServiceUndefineException("http error!");
                }
            }
            catch (Exception e)
            {
                throw new WebServiceUndefineException(e.Message);
            }
        }

        public void AddPara(string key, string value)
        {
            paras.Add(key, value);
        }

        public void AddPara(string key, long value)
        {
            paras.Add(key, value.ToString());
        }

        public void AddPara(string key, double value)
        {
            paras.Add(key, value.ToString());
        }

        public void AddPara(string key, int value)
        {
            paras.Add(key, value.ToString());
        }

        public void AddPara(string key, WebDate value)
        {
            paras.Add(key, value.Date.ToString("yyyy-MM-dd"));
        }

        public void AddPara(string key, WebTime value)
        {
            paras.Add(key, value.Time.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}