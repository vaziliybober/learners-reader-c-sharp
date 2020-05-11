using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Collections.Generic;
using Learners_Reader.Utilities;
using Learners_Reader.Epub;
using System;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using System.IO;
using Android.Content;

namespace Learners_Reader
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private FilePicker filePicker;
        private Button addBookButton;

        private ListView libraryListView;
        private Library library;
        ArrayAdapter<string> adapter;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            GlobalData.RootFolder = Application.ApplicationContext.GetExternalFilesDir(null).AbsolutePath;

            RequestPermissions();
            LoadLibrary();
            ConfigureLibraryListView();
            ConfigureFilePicker();
            ConfigureAddBookButton();

            Test();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void RequestPermissions()
        {
            // WriteExternalStorage includes ReadExternalStorage
            while (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Denied)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage }, 228);
            }
        }

        private void LoadLibrary()
        {
            string libraryPath = GlobalData.RootFolder + "/" + "Library";
            System.IO.Directory.CreateDirectory(libraryPath);
            library = new Library(libraryPath);
        }

        private void ConfigureLibraryListView()
        {
            libraryListView = FindViewById<ListView>(Resource.Id.libraryListView);
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, library.GetAllBookNames());
            libraryListView.Adapter = adapter;

            libraryListView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                string selectedBookTitle = library.GetAllBookNames()[args.Position];
                GlobalData.CurrentBook = library.GetBook(selectedBookTitle);
                Book t = GlobalData.CurrentBook;

                Intent nextActivityIntent = new Intent(this, typeof(BookInfoActivity));
                StartActivity(nextActivityIntent);
            };
        }

        private void ConfigureAddBookButton()
        {
            addBookButton = FindViewById<Button>(Resource.Id.addBookButton);
            addBookButton.Click += delegate (object sender, EventArgs e)
            {
                filePicker.Start();
            };
        }

        private void ConfigureFilePicker()
        {
            
            filePicker = new FilePicker(this);
            filePicker.Finished += delegate (object sender, FilePicker.FinishedEventArgs e)
            {
                Logger.Log("Adding a new book on path: " + e.PathToFile);

                if (e.PathToFile == null)
                {
                    Logger.Log("Book adding canceled");
                }
                else if (Functions.IsFileEpub(e.PathToFile))
                {
                    try
                    {
                        library.ImportBook(e.PathToFile);
                        adapter.Clear();
                        adapter.AddAll(library.GetAllBookNames());
                        adapter.NotifyDataSetChanged();
                        Toast.MakeText(this, "Parsing the book...", ToastLength.Short).Show();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Error", ex.Message);
                        Toast.MakeText(this, "Couldn't parse the file!", ToastLength.Short).Show();
                    }
                }
                else
                {
                    Logger.Log("Not an epub file chosen.");
                    Toast.MakeText(this, "Not an epub file!", ToastLength.Short).Show();
                }

            };
        }

        private void Test()
        {
            GlobalData.CurrentBook = library.GetBook("A game of thrones");

            Intent nextActivityIntent = new Intent(this, typeof(BookActivity));
            StartActivity(nextActivityIntent);
        }
    }
}