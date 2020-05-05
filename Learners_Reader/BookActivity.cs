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
using Learners_Reader.Utilities;

namespace Learners_Reader
{
    [Activity(Label = "BookActivity")]
    public class BookActivity : Activity
    {
        MyWebView webView;
        Button nextChapterButton, prevChapterButton;

        Book book;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_book);

            book = GlobalData.CurrentBook;

            ConfigureWebView();
            ConfugureSectionButtons();
        }

        private void ConfigureWebView()
        {
            webView = FindViewById<MyWebView>(Resource.Id.webView);
            webView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            webView.Settings.JavaScriptEnabled = true;
            webView.SetWebViewClient(new MyWebViewClient());
            webView.SetWebChromeClient(new MyWebChromeClient(webView));
            webView.HorizontalScrollBarEnabled = false;
            webView.VerticalScrollBarEnabled = false;

            webView.LoadDataWithBaseURL("file://" + book.RootFolderPath + "/", book.ReadSection(7), "text/html", "UTF-8", null);

        }

        private void ConfugureSectionButtons()
        {
            nextChapterButton = FindViewById<Button>(Resource.Id.nextSectionButton);
            nextChapterButton.Click += delegate (object sender, EventArgs e)
            {
                webView.LoadDataWithBaseURL("file://" + book.RootFolderPath + "/", book.ReadNextSection(), "text/html", "UTF-8", null);
            };

            prevChapterButton = FindViewById<Button>(Resource.Id.previousSectionButton);
            prevChapterButton.Click += delegate (object sender, EventArgs e)
            {
                webView.LoadDataWithBaseURL("file://" + book.RootFolderPath + "/", book.ReadPreviousSection(), "text/html", "UTF-8", null);
            };
        }

    }
}