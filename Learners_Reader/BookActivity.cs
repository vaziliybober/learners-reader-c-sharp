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
        BookViewer bookViewer;
        Button nextChapterButton, prevChapterButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_book);

            ConfigureBookViewer();
            ConfugureChapterButtons();
        }

        private void ConfigureBookViewer()
        {
            Book book = GlobalData.CurrentBook;

            ChapterView chv1 = FindViewById<ChapterView>(Resource.Id.chapterView1);
            ChapterView chv2 = FindViewById<ChapterView>(Resource.Id.chapterView2);
            ChapterView chv3 = FindViewById<ChapterView>(Resource.Id.chapterView3);

            bookViewer = new BookViewer(book, chv1, chv2, chv3);
            bookViewer.ShowChapter(5);
        }

        private void ConfugureChapterButtons()
        {
            nextChapterButton = FindViewById<Button>(Resource.Id.nextChapterButton);
            nextChapterButton.Click += delegate (object sender, EventArgs e)
            {
                bookViewer.ShowNextChapter();
            };

            prevChapterButton = FindViewById<Button>(Resource.Id.previousChapterButton);
            prevChapterButton.Click += delegate (object sender, EventArgs e)
            {
                bookViewer.ShowPrevChapter();
            };
        }

    }
}