using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace travel_app
{
   
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        [ComVisible(true)]
        public class JavaScriptControlHelper
        {
            Login prozor;
            public JavaScriptControlHelper(Login w)
            {
                prozor = w;
            }

            public void RunFromJavascript(string param)
            {
                prozor.doThings(param);
            }
        }
    
}
