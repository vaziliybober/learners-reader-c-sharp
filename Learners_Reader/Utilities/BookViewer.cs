using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Learners_Reader.Model;

namespace Learners_Reader.Utilities
{
    public class BookViewer
    {
        private readonly Book book;

        private ChapterView prevChapterView;
        private ChapterView currChapterView;
        private ChapterView nextChapterView;

        public int CurrentChapterIndex { get; private set; }

        public BookViewer(Book book, ChapterView chv1, ChapterView chv2, ChapterView chv3)
        {
            this.book = book;

            this.prevChapterView = chv1;
            this.currChapterView = chv2;
            this.nextChapterView = chv3;

            ConfigureChapterView(prevChapterView);
            ConfigureChapterView(currChapterView);
            ConfigureChapterView(nextChapterView);

            UpdateChapterViewVisibilities();

            ShowChapter(0);
        }

        private void ConfigureChapterView(ChapterView chv)
        {
            chv.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            chv.Settings.JavaScriptEnabled = true;
            
            chv.HorizontalScrollBarEnabled = false;
            chv.VerticalScrollBarEnabled = false;

            chv.SetWebViewClient(new MyWebViewClient());
            chv.SetWebChromeClient(new MyWebChromeClient(this));

            chv.BaseURL = "file://" + book.RootFolderPath + "/";
        }

        private void UpdateChapterViewVisibilities()
        {
            prevChapterView.Visibility = ViewStates.Invisible;
            currChapterView.Visibility = ViewStates.Visible;
            nextChapterView.Visibility = ViewStates.Invisible;

            //prevChapterView.SetBackgroundColor(Android.Graphics.Color.Red);
            //currChapterView.SetBackgroundColor(Android.Graphics.Color.White);
            //nextChapterView.SetBackgroundColor(Android.Graphics.Color.Blue);

            prevChapterView.Enabled = false;
            currChapterView.Enabled = true;
            nextChapterView.Enabled = false;
        }

        public void ShowChapter(int i)
        {
            if (i < 0 || i >= book.ChapterCount)
                return;

            this.CurrentChapterIndex = i;
            currChapterView.ShowChapter(book.ReadChapter(i));

            if (i != 0)
                prevChapterView.ShowChapter(book.ReadChapter(i - 1));
            
            if (i != book.ChapterCount - 1)
                nextChapterView.ShowChapter(book.ReadChapter(i + 1));
        }

        public void ShowNextChapter()
        {
            if (this.CurrentChapterIndex == book.ChapterCount - 1)
                return;

            prevChapterView.ScrollToStart();

            this.CurrentChapterIndex++;

            ChapterView temp = prevChapterView;
            prevChapterView = currChapterView;
            currChapterView = nextChapterView;
            nextChapterView = temp;

            UpdateChapterViewVisibilities();

            if (this.CurrentChapterIndex == book.ChapterCount - 1)
                return;

            nextChapterView.ShowChapter(book.ReadChapter(this.CurrentChapterIndex + 1));
        }

        public void ShowPrevChapter()
        {
            if (this.CurrentChapterIndex == 0)
                return;

            prevChapterView.ScrollToEnd();

            this.CurrentChapterIndex--;

            ChapterView temp = nextChapterView;
            nextChapterView = currChapterView;
            currChapterView = prevChapterView;
            prevChapterView = temp;

            UpdateChapterViewVisibilities();

            if (this.CurrentChapterIndex == 0)
                return;

            prevChapterView.ShowChapter(book.ReadChapter(this.CurrentChapterIndex - 1));
        }

        public void ScrollToNextPage()
        {
            if (currChapterView.CurrentPageIndex == currChapterView.PageCount - 1)
            {
                ShowNextChapter();
            }
            else
            {
                currChapterView.ScrollToNextPage();
            }

            Logger.Log(currChapterView.CurrentPageIndex);
            Logger.Log(currChapterView.PageCount);
        }

        public void ScrollToPrevPage()
        {
            if (currChapterView.CurrentPageIndex == 0)
            {
                ShowPrevChapter();
            }
            else
            {
                currChapterView.ScrollToPrevPage();
            }

            Logger.Log(currChapterView.CurrentPageIndex);
            Logger.Log(currChapterView.PageCount);
        }
    }
}