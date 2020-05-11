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

namespace Learners_Reader.Utilities
{
    public static class GlobalData
    {
        public static string RootFolder { get; set; }
        public static Book CurrentBook { get; set; }
        public static string CurrentWord { get; set; }
        public static string CurrentContext { get; set; }
        public static string CurrentNotes { get; set; }
    }
}