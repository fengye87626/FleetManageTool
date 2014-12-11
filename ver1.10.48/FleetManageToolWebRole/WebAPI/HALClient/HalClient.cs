using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FleetManageTool.WebAPI.Exceptions;
using FleetManageTool.WebAPI.JSON;
using FleetManageToolWebRole.Models.API;
using System.Linq;
using Tavis.UriTemplates;
using Newtonsoft.Json;
using Microsoft.ApplicationServer.Caching;
using FleetManageToolWebRole.Util;

namespace FleetManageTool.WebAPI
{
    internal sealed class TokenInfo
    {

        public TokenInfo()
        {          
            UpdateInitToken();
        }

        public String GetClientID()
        {
            return clientID;
        }

        public String GetRefreshToken()
        {
            return refreshToken;
        }

        public void SetRefreshToken(String strRefreshToken)
        {
            refreshToken = strRefreshToken;
        }

        public DateTime GetTokenRefreshDateTime()
        {
            return tokenRefreshDateTime;
        }

        public void SetTokenRefreshDateTime(DateTime tokenDateTime)
        {
            tokenRefreshDateTime = tokenDateTime;
        }

        private void UpdateInitToken()
        {
            DebugLog.Debug("HalClient UpdateInitToken Start");
            string strInitToken = ConfigureManager.GetConfigure("InitToken");
            if (null != strInitToken)
            {
                clientID = strInitToken;
            }
            DebugLog.Debug("HalClient UpdateInitToken End，clientID = " + strInitToken);
        }

        private static String clientID = "3c6eaaf5-1ff7-4d41-8fc7-76922b2d5b7b";  
        private static String refreshToken = "";
        private static DateTime tokenRefreshDateTime = DateTime.Now;
    }

    public interface IHalClient
    {
        Task<T> Get<T>(HalLink link, Dictionary<string, object> parameters = null) where T : class;
        Task<T> Post<T>(HalLink link, Dictionary<string, object> parameters = null) where T : class;
        Task<IHalResult> Put(HalLink link, Dictionary<string, object> parameters = null);
        Task<IHalResult> Delete(HalLink link, Dictionary<string, object> parameters = null);
        HttpClient HttpClient { get; }
        Task<T> GetOrDefault<T>(HalLink link, Dictionary<string, object> parameters = null) where T : new();
    }

    
    public class HalClient : IHalClient
    {
        private static TokenInfo tokenInfo = new TokenInfo();
      
        private static int tokenTimeoutRangeMinValue = 40;
        private static int tokenTimeoutRangeMaxValue = 50;
           
        private static readonly object lockToken = new object();
        
        private volatile static HalClient instance = null;
        private static readonly object lockInstance = new object();

        public static HalClient GetInstance()
        {
            DebugLog.Debug("HalClient GetInstance Start");
            if(instance == null)
            {
                lock (HalClient.lockInstance)
                {
                    if(instance == null)
                    {                   
                        instance = new HalClient();
                        instance.GetTokenTimeoutRangeValue();                      
                    }                                          
                }
            }

            instance.SetAuthorization();
            DebugLog.Debug("HalClient GetInstance End");
            return instance;
        }

        private void GetTokenTimeoutRangeValue()
        {
            DebugLog.Debug("HalClient GetTokenTimeoutRangeValue Start");
            string strTokenTimeoutRangeMinValue = ConfigureManager.GetConfigure("TokenTimeoutRangeMinValue");
            if (null != strTokenTimeoutRangeMinValue)
            {
                try
                {
                    int iTokenTimeoutRangeMinValue = Convert.ToInt32(strTokenTimeoutRangeMinValue);
                    HalClient.tokenTimeoutRangeMinValue = iTokenTimeoutRangeMinValue;
                }
                catch (Exception exception)
                {
                    DebugLog.Exception(exception.Message);               
                }              
            }

            string strTokenTimeoutRangeMaxValue = ConfigureManager.GetConfigure("TokenTimeoutRangeMaxValue");
            if (null != strTokenTimeoutRangeMaxValue)
            {
                try
                {
                    int iTokenTimeoutRangeMaxValue = Convert.ToInt32(strTokenTimeoutRangeMaxValue);
                    HalClient.tokenTimeoutRangeMaxValue = iTokenTimeoutRangeMaxValue;
                }
                catch (Exception exception)
                {
                    DebugLog.Exception(exception.Message);
                }             
            }
            DebugLog.Debug("tokenTimeoutRangeMinValue = " + HalClient.tokenTimeoutRangeMinValue + ";tokenTimeoutRangeMaxValue = " + HalClient.tokenTimeoutRangeMaxValue);
            DebugLog.Debug("HalClient GetTokenTimeoutRangeValue End");
            return;
        }

        private void SetAuthorization()
        {
            DebugLog.Debug("HalClient SetAuthorization Start");
            if (IsTokenTimeout())
            {
                lock (HalClient.lockToken)
                {
                    if (IsTokenTimeout())
                    {
                        DebugLog.Debug("HalClient Create HttpClient Start");
                        Uri endpoint = new Uri(ConfigureManager.GetConfigure("EndPoint"));
                        HttpClient = new HttpClient(new HttpClientHandler()) { BaseAddress = endpoint };
                        SetCredentials();
                        DateTime currentDateTime = DateTime.Now;
                        Token token = GetToken();
                        HalClient.tokenInfo.SetTokenRefreshDateTime(currentDateTime);
                        HalClient.tokenInfo.SetRefreshToken(token.refresh_token);
                        HttpClient.DefaultRequestHeaders.Remove("Authorization");
                        HttpClient.DefaultRequestHeaders.Add("Authorization", (token.token_type + " " + token.access_token));
                        HttpClient.DefaultRequestHeaders.Remove("Accept");
                        HttpClient.DefaultRequestHeaders.Add("Accept", "application/hal+json");
                        DebugLog.Debug("HalClient Create HttpClient End");
                    }
                }
            }
            DebugLog.Debug("HalClient SetAuthorization End");
            return;
        }

        private Boolean IsTokenTimeout()
        {
            DebugLog.Debug("HalClient IsTokenTimeout Start");  
            Boolean bUpdateToken = false;
            if ("".Equals(HalClient.tokenInfo.GetRefreshToken()))
            {
                bUpdateToken = true;
            }
            else 
            {
                double dTimeDifference = (DateTime.Now.Subtract(HalClient.tokenInfo.GetTokenRefreshDateTime()).Duration()).TotalMinutes;
                int iTimeDifference = (int)(dTimeDifference + 1);
                if (iTimeDifference >= HalClient.tokenTimeoutRangeMinValue && iTimeDifference < HalClient.tokenTimeoutRangeMaxValue)
                {
                    bUpdateToken = true;
                }
                else if (iTimeDifference >= HalClient.tokenTimeoutRangeMaxValue)
                {
                    HalClient.tokenInfo.SetRefreshToken("");
                    bUpdateToken = true;
                }
                else
                {
                    //
                }      
            }
            DebugLog.Debug("HalClient IsTokenTimeout End");  
            return bUpdateToken;
        }

        private HalClient()
        {
            DebugLog.Debug("HalClient new HalClient Start");

            DebugLog.Debug("HalClient new HalClient End");  
        }

        private Token GetToken()
        {
            DebugLog.Debug("HalClient GetToken Start");  
            string grant_type = ConfigureManager.GetConfigure("grant_type");
            string username = ConfigureManager.GetConfigure("username");
            string password = ConfigureManager.GetConfigure("password");
            string jsonstr = grant_type + "&" + username + "&" + password;
            HttpContent httpContent = new StringContent(jsonstr);
            Uri uri = new Uri(URI.TOOLTOKEN, UriKind.Relative);
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/x-www-form-urlencoded");
            var getResult = HttpClient.PostAsync(uri, httpContent).Result;
            DebugLog.Debug("HalClient GetToken getResult = " + getResult.StatusCode);
            if (!getResult.IsSuccessStatusCode || HttpStatusCode.NoContent == getResult.StatusCode)
            {
                throw new HalException("" + getResult.StatusCode);
            }
            var body = getResult.Content.ReadAsStringAsync().Result;
            Token token = Parse<Token>(body);
            DebugLog.Debug("HalClient GetToken token.access_token = " + token.access_token + ";token.expires_in = " + token.expires_in + ";token.refresh_token = " + token.refresh_token);
            DebugLog.Debug("HalClient GetToken End");
            return token;
        }

        private T Parse<T>(string content)
        {
            int index = content.IndexOf("_embedded");
            if (index > 0)
            {
                int listIndex = content.IndexOf("[", index);

                if (listIndex <= 0)
                {
                    int insertIndex = content.IndexOf(":", index + "_embedded\":".Length);
                    content = content.Insert(insertIndex + 1, "[");
                    int lastIndex = content.LastIndexOf("}");
                    content = content.Insert(lastIndex - 1, "]");
                }
                else
                {
                    String tempStr = content.Substring(index, listIndex - index);
                    int length = tempStr.Split('{').Length;
                    if (length > 2)
                    {
                        int insertIndex = content.IndexOf(":", index + "_embedded\":".Length);
                        content = content.Insert(insertIndex + 1, "[");
                        int lastIndex = content.LastIndexOf("}");
                        content = content.Insert(lastIndex - 1, "]");
                    }
                }
            }
            return JsonConvert.DeserializeObject<T>(content, new JsonConverter[] { new HalResourceConverter() });
        }

        private void SetCredentials()
        {
            DebugLog.Debug("HalClient SetCredentials Start");  
            String strClientId = "";
            if ("".Equals(HalClient.tokenInfo.GetRefreshToken())) //第一次启动和超时时，使用GUID
            {
                strClientId = HalClient.tokenInfo.GetClientID();
            }
            else
            {
                strClientId = HalClient.tokenInfo.GetRefreshToken();
            }

            HttpClient
                .DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", strClientId, strClientId))));
            DebugLog.Debug("HalClient SetCredentials End");  
        }

        public Task<T> Get<T>(HalLink link, Dictionary<string, object> parameters = null) where T : class
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();
            return Task<T>
                .Factory
                .StartNew(() =>
                {
                    Uri uri;
                    uri = link.IsTemplated ? ResolveTemplate(link, parameters) : new Uri(link.Href, UriKind.Relative);
                    string jsonstr = JsonConvert.SerializeObject(parameters);
                    DebugLog.Debug("HalClient Get jsonstr.Length = " + jsonstr.Length + " ; link = " + link.Href);
                    DebugLog.Debug("HalClient Get link = " + link.Href + "; jsonstr = " + jsonstr);
                    var getResult = HttpClient.GetAsync(uri).Result;
                    //DebugLog.Debug("HalClient Get getResult = " + getResult.StatusCode + " ;link = " + link.Href);
                    DebugLog.Debug("HalClient Get Result = " + getResult.StatusCode + " ;link = " + link.Href + ";ReasonPhrase = " + getResult.ReasonPhrase);
                    if (!getResult.IsSuccessStatusCode || HttpStatusCode.NoContent == getResult.StatusCode)
                    {
                        //chenyangwen 20140612 #1830
                        throw new HalException("" + getResult.StatusCode) { ReasonPhrase = getResult.ReasonPhrase };
                    }
                    var body = getResult.Content.ReadAsStringAsync().Result;
                    DebugLog.Debug("HalClient Get body.Length = " + body.Length + " ; link = " + link.Href);
                    DebugLog.Debug("HalClient Get body = " + body + " ; link = " + link.Href);
                    var ret = Parse<T>(body);
                    return ret;
                });
        }

        //当发生异常不抛出，返回新建空内容对象
        public Task<T> GetOrDefault<T>(HalLink link, Dictionary<string, object> parameters = null) where T : new()
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();
            return Task<T>
                .Factory
                .StartNew(() =>
                {
                    Uri uri;
                    uri = link.IsTemplated ? ResolveTemplate(link, parameters) : new Uri(link.Href, UriKind.Relative);
                    string jsonstr = JsonConvert.SerializeObject(parameters);
                    DebugLog.Debug("HalClient GetOrDefault jsonstr.Length = " + jsonstr.Length + " ; link = " + link.Href);
                    DebugLog.Debug("HalClient GetOrDefault link = " + link.Href + "; jsonstr = " + jsonstr);
                    var getResult = HttpClient.GetAsync(uri).Result;
                    //DebugLog.Debug("HalClient Get getResult = " + getResult.StatusCode + " ;link = " + link.Href);
                    DebugLog.Debug("HalClient GetOrDefault Result = " + getResult.StatusCode + " ;link = " + link.Href + ";ReasonPhrase = " + getResult.ReasonPhrase);
                    if (!getResult.IsSuccessStatusCode || HttpStatusCode.NoContent == getResult.StatusCode)
                    {
                        return new T();
                    }
                    var body = getResult.Content.ReadAsStringAsync().Result;
                    DebugLog.Debug("HalClient GetOrDefault body.Length = " + body.Length + " ; link = " + link.Href);
                    DebugLog.Debug("HalClient GetOrDefault body = " + body + " ; link = " + link.Href);
                    var ret = Parse<T>(body);
                    return ret;
                });
        }

        //chenyangwen add
        public Task<T> Post<T>(HalLink link, Dictionary<string, object> parameters = null) where T : class
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();
            return Task<T>
                .Factory
                .StartNew(() =>
                {
                    Uri uri;
                    uri = link.IsTemplated ? ResolveTemplate(link, parameters) : new Uri(link.Href, UriKind.Relative);
                    string jsonstr = JsonConvert.SerializeObject(parameters);
                    DebugLog.Debug("HalClient Post jsonstr.Length = " + jsonstr.Length + " ; link = " + link.Href);
                    DebugLog.Debug("HalClient Post link = " + link.Href + "; jsonstr = " + jsonstr);
                    HttpContent httpContent = new StringContent(jsonstr);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var getResult = HttpClient.PostAsync(uri, httpContent).Result;
                    //DebugLog.Debug("HalClient Post getResult = " + getResult.StatusCode + " ;link = " + link.Href);
                    DebugLog.Debug("HalClient Post Result = " + getResult.StatusCode + " ;link = " + link.Href + ";ReasonPhrase = " + getResult.ReasonPhrase);
                    if (!getResult.IsSuccessStatusCode || HttpStatusCode.NoContent == getResult.StatusCode)
                    {
                        //chenyangwen 20140612 #1830
                        throw new HalException("" + getResult.StatusCode) { ReasonPhrase = getResult.ReasonPhrase };
                    }
                    var body = getResult.Content.ReadAsStringAsync().Result;
                    DebugLog.Debug("HalClient Post body.Length = " + body.Length + " ; link = " + link.Href);
                    DebugLog.Debug("HalClient Post body = " + body + " ; link = " + link.Href);
                    var ret = Parse<T>(body);
                    return ret;
                });
        }

        public Task<IHalResult> Put(HalLink link, Dictionary<string, object> parameters = null)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();
            return Task<IHalResult>
                .Factory
                .StartNew(() =>
                {
                    Uri uri;
                    uri = link.IsTemplated ? ResolveTemplate(link, parameters) : new Uri(link.Href, UriKind.Relative);
                    string jsonstr = JsonConvert.SerializeObject(parameters);
                    DebugLog.Debug("HalClient Put jsonstr.Length = " + jsonstr.Length + " ; link = " + link.Href);
                    DebugLog.Debug("HalClient Put link = " + link.Href + "; jsonstr = " + jsonstr);
                    HttpContent httpContent = new StringContent(jsonstr);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var getResult = HttpClient.PutAsync(uri, httpContent).Result;
                    DebugLog.Debug("HalClient Put Result = " + getResult.StatusCode + " ;link = " + link.Href + ";ReasonPhrase = " + getResult.ReasonPhrase);
                    //chenyangwen 20140612 #1830
                    return new HalResult { Success = getResult.IsSuccessStatusCode, StatusCode = getResult.StatusCode.ToString(), ReasonPhrase = getResult.ReasonPhrase };
                });
        }

        public Task<IHalResult> Delete(HalLink link, Dictionary<string, object> parameters = null)
        {
            if (parameters == null)
                parameters = new Dictionary<string, object>();
            return Task<IHalResult>
                .Factory
                .StartNew(() =>
                {
                    Uri uri = link.IsTemplated ? ResolveTemplate(link, parameters) : new Uri(link.Href, UriKind.Relative);
                    string jsonstr = JsonConvert.SerializeObject(parameters);
                    DebugLog.Debug("HalClient Delete link = " + link.Href + "; jsonstr = " + jsonstr);
                    var getResult = HttpClient.DeleteAsync(uri).Result;
                    DebugLog.Debug("HalClient Delete Result = " + getResult.StatusCode + " ;link = " + link.Href + ";ReasonPhrase = " + getResult.ReasonPhrase);
                    //chenyangwen 20140612 #1830
                    return new HalResult { Success = getResult.IsSuccessStatusCode, StatusCode = getResult.StatusCode.ToString(), ReasonPhrase = getResult.ReasonPhrase };
                });
        }

        internal Uri ResolveTemplate(HalLink link, Dictionary<string, object> parameters)
        {
            var template = new UriTemplate(link.Href);
            foreach (var key in parameters.Keys)
            {
                template.SetParameter(key, parameters[key]);
            }
            return new Uri(template.Resolve(), UriKind.Relative);
        }

        public HttpClient HttpClient { get; internal set; }
    }
}
