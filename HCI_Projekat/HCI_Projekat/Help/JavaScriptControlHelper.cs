using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace HCI_Projekat.help
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class JavaScriptControlHelper
    {
        MainWindow prozor;

        ManagerHomepage mh;

        ClientHomepage ch;

        public JavaScriptControlHelper(MainWindow w)
        {
            prozor = w;
        }

        public JavaScriptControlHelper(ManagerHomepage w)
        {
            mh = w;
        }

        public JavaScriptControlHelper(ClientHomepage w)
        {
            ch = w;
        }

        public void RunFromJavascript(string param)
        {
           // ...
        }
    }
}
