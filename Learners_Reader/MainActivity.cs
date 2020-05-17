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
    [Activity(Label = "Learner's Reader", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Database database;

        private FilePicker filePicker;
        private Button addBookButton;

        private ListView libraryListView;
        private Library library;
        ArrayAdapter<string> adapter;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            RequestPermissions();
        }

        private void RequestPermissions()
        {
            ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage }, 228);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            if (requestCode == 228)
            {
                if (grantResults[0] == Permission.Granted)
                {
                    StartApplication();
                }
                else
                {
                    this.Finish();
                }
            }
        }

        public void StartApplication()
        {
            SetContentView(Resource.Layout.activity_main);

            GlobalData.RootFolder = Application.ApplicationContext.GetExternalFilesDir(null).AbsolutePath;

            CreateDirectories();
            LoadLibrary();
            ConfigureLibraryListView();
            ConfigureFilePicker();
            ConfigureAddBookButton();

            TryShowLastBook();

            
        }

        private void CreateDirectories()
        {
            System.IO.Directory.CreateDirectory(GlobalData.LibraryFolder);
            System.IO.Directory.CreateDirectory(GlobalData.VocabularyFolder);
        }

        private void TryShowLastBook()
        {
            database = new Database(GlobalData.DatabaseFilePath);
            if (database.CurrentBookIndex != -1)
            {
                GlobalData.CurrentBook = library.Books[database.CurrentBookIndex];

                Intent nextActivityIntent = new Intent(this, typeof(BookActivity));
                StartActivity(nextActivityIntent);
            }
        }

        private void LoadLibrary()
        {
            string libraryPath = GlobalData.LibraryFolder;
            library = new Library(libraryPath);
        }

        private void ConfigureLibraryListView()
        {
            libraryListView = FindViewById<ListView>(Resource.Id.libraryListView);
            adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, library.GetAllBookNames());
            libraryListView.Adapter = adapter;

            libraryListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs args) =>
            {
                GlobalData.CurrentBook = library.Books[args.Position];

                database.CurrentBookIndex = args.Position;
                database.Save();

                Intent nextActivityIntent = new Intent(this, typeof(BookInfoActivity));
                StartActivity(nextActivityIntent);
            };
        }

        private void ConfigureAddBookButton()
        {
            addBookButton = FindViewById<Button>(Resource.Id.addBookButton);
            addBookButton.Click += (object sender, EventArgs e) =>
            {
                filePicker.Start();
            };
        }

        private void ConfigureFilePicker()
        {

            filePicker = new FilePicker(this);
            filePicker.Finished  += (object sender, FilePicker.FinishedEventArgs e) =>
            {
                if (e.PathToFile == null)
                {
                    Toast.MakeText(this, "Book adding canceled.", ToastLength.Short);
                }
                else if (e.PathToFile.StartsWith("content://"))
                {
                    Toast.MakeText(this, "Couldn't get the file by this path!", ToastLength.Short).Show();
                }
                else if (System.IO.Path.GetExtension(e.PathToFile) == ".epub")
                {
                    try
                    {
                        library.AddBook(e.PathToFile);
                        adapter.Clear();
                        adapter.AddAll(library.GetAllBookNames());
                        adapter.NotifyDataSetChanged();
                        Toast.MakeText(this, "Parsing the book...", ToastLength.Short).Show();
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(this, "Couldn't parse the file!", ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, "Not an epub file!", ToastLength.Short).Show();
                }

            };
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (database != null)
            {
                database.CurrentBookIndex = -1;
                database.Save();
            }
        }

        private void Test()
        {
            GlobalData.CurrentBook = library.GetBook("A game of thrones");

            Intent nextActivityIntent = new Intent(this, typeof(BookActivity));
            StartActivity(nextActivityIntent);
        }
    }
}