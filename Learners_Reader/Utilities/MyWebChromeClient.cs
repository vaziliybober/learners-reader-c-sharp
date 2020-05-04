using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace Learners_Reader.Utilities
{
    public class MyWebChromeClient : WebChromeClient
    {
        public override bool OnConsoleMessage(ConsoleMessage consoleMessage)
        {
            if (consoleMessage.Message().StartsWith("<number of pages>:")) {
                //int nPages = int.Parse(consoleMessage.Message().Split(':')[1]);
                //Logger.Log("" + nPages);
                Logger.Log(consoleMessage.Message());
            }

            return base.OnConsoleMessage(consoleMessage);
        }
    }
}