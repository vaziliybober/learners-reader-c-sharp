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

namespace Learners_Reader.Utilities
{
    public class ChapterView : WebView
    {
        public int PageCount { get; set; }
        public string BaseURL { get; set; }

        public int CurrentPageIndex { get; private set; }

        public ChapterView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.CurrentPageIndex = 0;
        }

        public void ShowChapter(string chapter)
        {
            LoadDataWithBaseURL(this.BaseURL, chapter, "text/html", "UTF-8", null);
        }

        public void ScrollToPage(int i)
        {
            if (i < 0 || i >= this.PageCount)
                return;


            int visibleWidth = this.MeasuredWidth + 2;

            ScrollTo(visibleWidth * i, 0);
            CurrentPageIndex = i;
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

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.Action == MotionEventActions.Move)
            {
                return true;
            }

            return base.OnTouchEvent(e);
        }
    }
}