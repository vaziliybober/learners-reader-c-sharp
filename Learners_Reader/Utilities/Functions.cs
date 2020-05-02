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

namespace Learners_Reader.Utilities
{
    public static class Functions
    {
        public static bool IsFileEpub(string path)
        {
            if (path == null)
                return false;

            string[] splitPath = path.Split('.');

            if (splitPath.Length == 0)
                return false;

            // ^1 in C# is like [-1] in Python
            return splitPath[^1] == "epub";
        }
    }
}