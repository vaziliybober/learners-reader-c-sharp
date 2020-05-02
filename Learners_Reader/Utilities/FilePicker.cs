using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;

namespace Learners_Reader.Utilities
{
    public class FilePicker
    {
        private FileData fileData = null;
        private Context context;

        // A class to store event arguments
        public class FinishedEventArgs : EventArgs
        {
            public string PathToFile { get; set; }
        }

        public FilePicker(Context context)
        {
            this.context = context;
        }

        public event EventHandler<FinishedEventArgs> Finished;

        public async void Start()
        {
            try
            {
                fileData = await CrossFilePicker.Current.PickFile();
            }
            catch (Exception ex)
            {
                Logger.Log("Error", ex.Message);
            }


            string path = null;
            if (fileData != null)
            {
                Android.Net.Uri potentialUri = Android.Net.Uri.Parse(fileData.FilePath);
                if (DocumentsContract.IsDocumentUri(context, potentialUri))
                {
                    string docId = DocumentsContract.GetDocumentId(potentialUri);
                    path = "/storage/" + docId.Replace(':', '/');
                }
                else
                {
                    path = fileData.FilePath;
                }
            }

            Finished.Invoke(this, new FinishedEventArgs() { PathToFile = path });
        }
    }
}