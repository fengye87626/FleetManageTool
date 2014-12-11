using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

namespace Resource
{
    public class ResourceLoader
    {
        public static void SetCurrentThreadCulture(HttpSessionStateBase session)
        {
            if (session != null && session["Culture"] != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = (System.Globalization.CultureInfo)session["Culture"];
                System.Threading.Thread.CurrentThread.CurrentUICulture = (System.Globalization.CultureInfo)session["Culture"];
            }
        }
    }
}
