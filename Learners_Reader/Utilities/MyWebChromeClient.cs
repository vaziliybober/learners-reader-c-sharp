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
        private readonly BookViewer bookViewer;

        public MyWebChromeClient(BookViewer bookViewer) : base()
        {
            this.bookViewer = bookViewer;
        }
        public override bool OnJsAlert(WebView view, string url, string message, JsResult result)
        {
            Logger.Log("JS", message);

            if (message.StartsWith("page count: "))
            {
                int pageCount = int.Parse(message.Split(": ")[1]);
                (view as ChapterView).PageCount = pageCount;
            }

            if (message == "swipe left")
            {
                bookViewer.ScrollToNextPage();
            }

            if (message == "swipe right")
            {
                bookViewer.ScrollToPrevPage();
            }

            result.Cancel();
            return true;
        }
        /*public override bool OnConsoleMessage(ConsoleMessage consoleMessage)
        {
            Logger.Log("JS", consoleMessage.Message());

            if (consoleMessage.Message().StartsWith("page count: ")) {
                int pageCount = int.Parse(consoleMessage.Message().Split(": ")[1]);
                chapterView.PageCount = pageCount;
            }

            if (consoleMessage.Message() == "swipe left") {
                chapterView.ScrollToNextPage();
            }

            if (consoleMessage.Message() == "swipe right")
            {
                
            }

            return base.OnConsoleMessage(consoleMessage);
        }*/
    }
}