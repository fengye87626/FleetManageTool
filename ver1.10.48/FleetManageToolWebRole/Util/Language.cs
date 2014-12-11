using Resource.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace FleetManageToolWebRole.Util
{
    public class Language
    {
        public string LanguageScript()
        {
            System.Type stringType = typeof(string);
            var scripts = typeof(ihpleD_String_cn).GetProperties();
            System.Collections.Hashtable array = new System.Collections.Hashtable();
            foreach (var p in scripts)
            {
                if (p.PropertyType == stringType)
                    array.Add(p.Name, p.GetValue(p, null));
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string script = serializer.Serialize(array);
            script = "([{Shared:" + script + "}]);";

            return script;
        } 
    }
}