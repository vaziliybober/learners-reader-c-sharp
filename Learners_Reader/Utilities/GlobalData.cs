using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Learners_Reader.Epub;

namespace Learners_Reader.Utilities
{
    public static class GlobalData
    {
        public static string RootFolder { get; set; }
        public static string DatabaseFilePath { get { return Path.Combine(RootFolder, "current_book_index.txt"); } }
        public static string LibraryFolder { get { return Path.Combine(RootFolder, "Books"); } }
        public static string VocabularyFolder { get { return Path.Combine(RootFolder, "Words"); } }

        public static Book CurrentBook { get; set; }

        public static string CurrentWord { get; set; }
        public static string CurrentContext { get; set; }
        public static string CurrentNotes { get; set; }
    }
}