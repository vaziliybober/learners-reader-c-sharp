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
        private readonly MyWebView webView;

        public MyWebChromeClient(MyWebView webView) : base()
        {
            this.webView = webView;
        }

        public override bool OnConsoleMessage(ConsoleMessage consoleMessage)
        {
            if (consoleMessage.Message().StartsWith("<number of pages>:")) {
                int pageCount = int.Parse(consoleMessage.Message().Split(':')[1]);
                webView.PageCount = pageCount;
                Logger.Log(webView.PageCount + "");
            }

            return base.OnConsoleMessage(consoleMessage);
        }
    }
}