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

        public static string ListToString<T>(List<T> list, string sep=" ")
        {
            string result = "";

            foreach (T item in list)
            {
                result += item.ToString() + sep;
            }

            return result.Trim();
        }

        public static List<int> StringToIntList(string str)
        {


            List<int> result = new List<int>();

            if (str.Trim() == "")
                return result;

            foreach (string number in str.Trim().Split())
            {
                result.Add(int.Parse(number));
            }

            return result;
        }

    }
}