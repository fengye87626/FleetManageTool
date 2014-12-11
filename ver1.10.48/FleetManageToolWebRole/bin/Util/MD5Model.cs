using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace FleetManageTool.Util
{
    public class MD5Model
    {
        public static string getMD5String(string oldString)
        {
            MD5 md5 = MD5.Create();
            string newPassword = "";
            byte[] bytes = md5.ComputeHash(Encoding.Unicode.GetBytes(oldString));
            for (int i = 0; i < bytes.Length; ++i)
            {
                newPassword = newPassword + bytes[i].ToString("x");
            }
            return newPassword;
        }
    }
}