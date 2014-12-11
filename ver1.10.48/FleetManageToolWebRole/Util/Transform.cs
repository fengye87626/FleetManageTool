using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FleetManageToolWebRole.Util
{
    public class Transform
    {
        public List<long> StringToList(string data)
        {
            try
            {
                if (null == data)
                {
                    return null;
                }
                List<long> list = new List<long>();
                if (data.Equals(""))
                {
                    return list;
                }
                string[] arr = data.Split(',');
                foreach (string temp in arr)
                {
                    list.Add(System.Int64.Parse(temp));
                }
                return list;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}