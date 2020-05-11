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
using Learners_Reader.Epub;
using Learners_Reader.Utilities;

namespace Learners_Reader.Utilities
{
    public class MyWebChromeClient : WebChromeClient
    {

        public event Action<int> ChapterLoaded;

        public event Action SwipeLeft;
        public event Action SwipeRight;
        public event Action SwipeDown;


        public override bool OnJsAlert(WebView view, string url, string message, JsResult result)
        {
            Logger.Log("JS", message);

            if (message.StartsWith("page count: "))
            {
                int pageCount = int.Parse(message.Split(": ")[1]);

                ChapterLoaded?.Invoke(pageCount);
            }

            if (message == "swipe left")
            {
                SwipeLeft?.Invoke();
            }

            if (message == "swipe right")
            {
                SwipeRight?.Invoke();
            }

            if (message == "swipe down")
            {
                SwipeDown?.Invoke();
            }

            result.Cancel();
            return true;
        }
    }
}