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
using Learners_Reader.Epub;
using Learners_Reader.Utilities;

namespace Learners_Reader
{
    [Activity(Label = "WordInfoActivity")]
    public class WordInfoActivity : Activity
    {
        private WordInfoDatabase wordDatabase;

        private WordInfo wordInfo;

        private TextView contextListTextView;

        private EditText notesEditText;

        private Button saveButton;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_wordinfo);

            ConfigureWordInfoDatabase();
            ConfigureContextListTextView();
            ConfigureNotesEditText();
            ConfigureSaveButton();

            
        }

        private void ConfigureWordInfoDatabase()
        {
            wordDatabase = new WordInfoDatabase(System.IO.Path.Combine(GlobalData.RootFolder, "Words"));
            wordDatabase.Load();
            wordInfo = wordDatabase.GetWordInfo(GlobalData.CurrentWord);
            if (wordInfo == null)
            {
                wordInfo = new WordInfo();
                wordInfo.Word = GlobalData.CurrentWord;
            }
        }

        private void ConfigureContextListTextView()
        {
            contextListTextView = FindViewById<TextView>(Resource.Id.contextListTextView);
            Logger.Log(contextListTextView == null);
            contextListTextView.Text = Functions.ListToString<string>(wordInfo.ContextList, "\n\n");
        }

        private void ConfigureNotesEditText()
        {
            notesEditText = FindViewById<EditText>(Resource.Id.notesEditText);
            notesEditText.Text = wordInfo.Notes;
        }

        private void ConfigureSaveButton()
        {
            saveButton = FindViewById<Button>(Resource.Id.saveButton);
            saveButton.Click += (object sender, EventArgs e) =>
            {
                wordInfo.Notes = notesEditText.Text;
                if (wordDatabase.ContainsWordInfo(wordInfo.Word))
                {
                    wordDatabase.Save();
                    Toast.MakeText(this, "Saving...", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "The word must be saved first.", ToastLength.Short).Show();
                }

                
            };
        }
    }
}