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
        public static void Log(string tag, object message)
        {
            if (message == null)
                message = "null";

            Android.Util.Log.Debug(tag, message.ToString());
        }

        public static void Log(object message)
        {
            Log("Default", message);
        }
    }
}