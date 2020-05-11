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

namespace Learners_Reader
{
    [Activity(Label = "TranslationActivity")]
    public class TranslationActivity : Activity
    {
        private WordInfoDatabase wordDatabase;

        private WebView webView;

        private EditText wordEditText;
        private TextView contextTextView;

        private Button saveButton;
        private Button notesButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_translation);

            ConfigureWordInfoDatabase();
            ConfigureWebView();
            ConfigureWordEditText();
            ConfigureContextTextView();
            ConfigureSaveButton();
            ConfigureNotesButton();
        }

        private void ConfigureWordInfoDatabase()
        {
            wordDatabase = new WordInfoDatabase(System.IO.Path.Combine(GlobalData.RootFolder, "Words"));
            wordDatabase.Load();
        }
        private void ConfigureWebView()
        {
            webView = FindViewById<WebView>(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;
            webView.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            webView.SetWebViewClient(new WebViewClient());
            webView.SetWebChromeClient(new WebChromeClient());
            webView.LoadUrl("http://www.learnersdictionary.com/definition/" + GlobalData.CurrentWord);
        }

        private void ConfigureWordEditText()
        {
            wordEditText = FindViewById<EditText>(Resource.Id.wordEditText);
            wordEditText.Text = GlobalData.CurrentWord;
        }

        private void ConfigureContextTextView()
        {
            contextTextView = FindViewById<TextView>(Resource.Id.contextTextView);
            contextTextView.Text = GlobalData.CurrentContext;
        }

        private void ConfigureSaveButton()
        {
            saveButton = FindViewById<Button>(Resource.Id.saveButton);
            saveButton.Click += (object sender, EventArgs e) =>
            {
                if (GlobalData.CurrentNotes != null)
                {
                    wordDatabase.TryChangeNotes(wordEditText.Text, GlobalData.CurrentNotes);
                    GlobalData.CurrentNotes = null;
                }


                if (GlobalData.CurrentNotes != null)
                    wordDatabase.TryAddContext(wordEditText.Text, GlobalData.CurrentContext);

                wordDatabase.Save();
                Toast.MakeText(this, "Saving...", ToastLength.Short).Show();

            };
        }

        private void ConfigureNotesButton()
        {
            notesButton = FindViewById<Button>(Resource.Id.notesButton);
            notesButton.Click += (object sender, EventArgs e) =>
            {
                GlobalData.CurrentWord = wordEditText.Text;
                Intent nextActivityIntent = new Intent(this, typeof(WordInfoActivity));
                StartActivity(nextActivityIntent);
            };
        }
    }
}