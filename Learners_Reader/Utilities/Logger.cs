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
    public static class Logger
    {
        public static void Log(string tag, string message)
        {
            Android.Util.Log.Debug(tag, message);
        }

        public static void Log(string message)
        {
            Log("Default", message);
        }
    }
}