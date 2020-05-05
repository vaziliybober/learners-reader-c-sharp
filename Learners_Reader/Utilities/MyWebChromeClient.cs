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
using Learners_Reader.Model;

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
            Logger.Log("js" + consoleMessage.Message());

            if (consoleMessage.Message().StartsWith("<number of pages>:")) {
                int pageCount = int.Parse(consoleMessage.Message().Split(':')[1]);
                webView.PageCount = pageCount;
            }

            if (consoleMessage.Message() == "<event>:last_page_turn") {
                Book book = GlobalData.CurrentBook;
                webView.LoadDataWithBaseURL("file://" + book.RootFolderPath + "/", book.ReadNextSection(), "text/html", "UTF-8", null);
            }

            return base.OnConsoleMessage(consoleMessage);
        }
    }
}