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
    [Activity(Label = "Vocabulary")]
    public class WordInfoListActivity : AppCompatActivity
    {
        private WordInfoDatabase wordDatabase;

        private ListView wordListView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_wordinfolist);

            ConfigureWordInfoDatabase();
            ConfigureWordListView();
        }

        private void ConfigureWordInfoDatabase()
        {
            wordDatabase = new WordInfoDatabase(System.IO.Path.Combine(GlobalData.RootFolder, "Words"));
            wordDatabase.Load();
        }

        private void ConfigureWordListView()
        {
            wordListView = FindViewById<ListView>(Resource.Id.wordListView);

            List<string> words = new List<string>();
            foreach (WordInfo wordInfo in wordDatabase.WordInfoList)
            {
                words.Add(wordInfo.Word);
            }

            ArrayAdapter adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, words);
            wordListView.Adapter = adapter;

            wordListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs args) =>
            {
                string word = words[args.Position];
                GlobalData.CurrentWord = word;
                GlobalData.CurrentContext = null;
                GlobalData.CurrentNotes = null;

                Intent nextActivityIntent = new Intent(this, typeof(TranslationActivity));
                StartActivity(nextActivityIntent);
            };
        }
    }
}