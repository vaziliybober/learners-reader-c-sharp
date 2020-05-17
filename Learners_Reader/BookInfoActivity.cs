using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Learners_Reader.Epub;
using Learners_Reader.Utilities;

namespace Learners_Reader
{
    [Activity(Label = "Information")]
    public class BookInfoActivity : AppCompatActivity
    {
        Book book;

        TextView titleTextView;
        TextView authorTextView;
        TextView descriptionTextView;

        Button readBookButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_bookinfo);

            book = GlobalData.CurrentBook;

            ConfigureTextViews();
            ConfigureReadBookButton();
        }

        private void ConfigureTextViews()
        {
            titleTextView = FindViewById<TextView>(Resource.Id.titleTextView);
            titleTextView.Text = book.Title;

            authorTextView = FindViewById<TextView>(Resource.Id.authorTextView);
            authorTextView.Text = book.Author + $" ({book.Language})";

            descriptionTextView = FindViewById<TextView>(Resource.Id.descriptionTextView);
            descriptionTextView.Text = book.Description;
        }

        private void ConfigureReadBookButton()
        {
            readBookButton = FindViewById<Button>(Resource.Id.readBookButton);
            readBookButton.Click += delegate (object sender, EventArgs e)
            {
                Intent nextActivityIntent = new Intent(this, typeof(BookActivity));
                StartActivity(nextActivityIntent);
            };
        }
    }
}