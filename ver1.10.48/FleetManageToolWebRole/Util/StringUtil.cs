using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Util
{
    public class StringUtil
    {
        //字符串转成Unicode
        public static string StringToUnicode(string resStr)
        {
            string codeStr = String.Empty;
            for (int i = 0; i < resStr.Length; ++i)
            {
                int s = (int)resStr[i];
                codeStr += "%" + s;
            }
            return codeStr;
        }

        //Unicode转成String
        public static string UnicodeToString(string resStr)
        {
            string[] k = resStr.Split('%');
            string rs = String.Empty;
            if ("".Equals(resStr))
            {
                return rs;
            }
            for (int i = 0; i < k.Length; i++)
            {
                rs += (char)System.Int32.Parse(k[i]);
            }
            return rs;
        }
    }
}