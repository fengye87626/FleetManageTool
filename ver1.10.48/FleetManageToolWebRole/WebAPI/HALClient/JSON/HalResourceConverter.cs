using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FleetManageTool.WebAPI.Attributes;
using FleetManageTool.WebAPI.Exceptions;
using FleetManageToolWebRole.Models.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FleetManageToolWebRole.Util;

namespace FleetManageTool.WebAPI.JSON
{
    class HalResourceConverter : JsonConverter
    {
        public HalResourceConverter(Type type = null)
        {
            ObjectType = type;
        }

        protected Type ObjectType { get; set; }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new HalException("json写入异常");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                var obj = JToken.ReadFrom(reader);
                var ret = JsonConvert.DeserializeObject(obj.ToString(), ObjectType ?? objectType, new JsonConverter[] { });
                if (obj["_embedded"] != null && obj["_embedded"].HasValues)
                {
                    var enumerator = ((JObject)obj["_embedded"]).GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        var rel = enumerator.Current.Key;
                        foreach (var property in objectType.GetProperties())
                        {
                            var attribute = property.GetCustomAttributes(true).FirstOrDefault(attr => attr is HalEmbeddedAttribute && ((HalEmbeddedAttribute)attr).Rel == rel);
                            if (attribute != null)
                            {
                                var type = (attribute as HalEmbeddedAttribute).Type ?? property.PropertyType;
                                property.SetValue(ret, JsonConvert.DeserializeObject(enumerator.Current.Value.ToString(), type, new JsonConverter[] { new HalResourceConverter((attribute as HalEmbeddedAttribute).CollectionMemberType) }), null);
                            }
                        }
                    }
                }
                if (obj["_links"] != null && obj["_links"].HasValues && typeof(IHalResource).IsAssignableFrom(objectType))
                {
                    ((HalResource)ret).Links = JsonConvert.DeserializeObject<HalLinkCollection>(obj["_links"].ToString(), new JsonConverter[] { new HalLinkCollectionConverter() });
                }
                return ret;
            }
            catch (JsonReaderException jsonException)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "JsonReaderException 转化异常" + jsonException.Message);
                return null;
            }
            catch (JsonSerializationException jsonException)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "JsonSerializationException 转化异常" + jsonException.Message);
                return null;
            }
            catch (Exception exception)
            {
                DebugLog.Exception(DebugLog.DebugType.OtherException, "ReadJson 转化异常" + exception.Message);
                return null;
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof (IHalResource).IsAssignableFrom(objectType);
        }
    }
}
