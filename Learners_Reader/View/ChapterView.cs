using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Learners_Reader.Utilities;

namespace Learners_Reader.View
{
    public class ChapterView : WebView
    {
        public int PageCount { get; set; }
        public string BaseURL { get; set; }

        public event Action<int> CurrentPageIndexChanged;
        private int currentPageIndex;
        public int CurrentPageIndex
        {
            get
            {
                return currentPageIndex;
            }

            set
            {
                currentPageIndex = value;
                CurrentPageIndexChanged?.Invoke(value);
            }
        }


        public ChapterView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            currentPageIndex = 0;
        }

        public void ShowChapter(string chapter, Action<int> onChapterLoaded = null)
        {
            LoadDataWithBaseURL(this.BaseURL, chapter, "text/html", "UTF-8", null);

            void SingleTimeOnChapterLoaded(int pageCount)
            {
                this.PageCount = pageCount;
                onChapterLoaded?.Invoke(pageCount);
                (MyWebChromeClient as MyWebChromeClient).ChapterLoaded -= SingleTimeOnChapterLoaded;
            }

            (MyWebChromeClient as MyWebChromeClient).ChapterLoaded += SingleTimeOnChapterLoaded;
        }

        public void ScrollToPage(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex >= this.PageCount)
                return;

            Logger.Log(Width + " " + MeasuredWidth);
            int visibleWidth = Math.Max(ComputeHorizontalScrollRange() / this.PageCount + 1, this.MeasuredWidth + 1);

            ScrollTo(visibleWidth * pageIndex, 0);
            CurrentPageIndex = pageIndex;
        }

        public void ScrollToNextPage()
        {
            ScrollToPage(CurrentPageIndex + 1);
        }

        public void ScrollToPrevPage()
        {
            ScrollToPage(CurrentPageIndex - 1);
        }

        public void ScrollToStart()
        {
            ScrollToPage(0);
        }

        public void ScrollToEnd()
        {
            ScrollToPage(this.PageCount - 1);
        }

        public MyWebChromeClient MyWebChromeClient { get; private set; }

        public override void SetWebChromeClient(WebChromeClient client)
        {
            if (client.GetType() == typeof(MyWebChromeClient))
                MyWebChromeClient = client as MyWebChromeClient;
            base.SetWebChromeClient(client);
        }
    }
}