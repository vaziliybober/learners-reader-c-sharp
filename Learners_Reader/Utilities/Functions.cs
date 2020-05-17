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
        public static string RemoveInvalidCharactersFromFilename(string filename)
        {
            string allowedSymbols = "1234567890-qwertyuiop[]{}asdfghjkl;'\"zxcvbnm,./<>?!@#$%^&*()!№;%:\\йцукенгшщзхъфывапролджэячсмитьбюъхё. `~ЁЙЦУКЕНГШЩЗФЫВАПРОЛДЯЧСМИТЬБЮЪХQWERTYUIOPASDFGHJKLZXCVBNM";
            string result = "";

            foreach (char s in filename)
            {
                if (allowedSymbols.Contains(s))
                    result += s;
            }

            if (result.Length > 25)
            {
                return result.Substring(0, 25);
            }

            return result;
        }


        public static string RemoveInvalidCharactersFromPath(string filename)
        {
            string allowedSymbols = "1234567890-qwertyuiop[]{}asdfghjkl;'\"zxcvbnm,./<>?!@#$%^&*()!№;%:\\йцукенгшщзхъфывапролджэячсмитьбюъхё. `~ЁЙЦУКЕНГШЩЗФЫВАПРОЛДЯЧСМИТЬБЮЪХQWERTYUIOPASDFGHJKLZXCVBNM";
            string result = "";

            foreach (char s in filename)
            {
                if (allowedSymbols.Contains(s))
                    result += s;
            }

            if (result.Length > 25)
            {
                return result.Substring(0, 25);
            }

            return result;
        }

        public static string CreateDirectory(string path)
        {
            if (System.IO.Directory.Exists(path))
            {
                int i;
                for (i = 1; System.IO.Directory.Exists(path + $"({ i})"); i++) ;

                path += $"({ i})";
                System.IO.Directory.CreateDirectory(path);
            }
            else
            {
                System.IO.Directory.CreateDirectory(path);
            }

            return path;
        }

        public static string ListToString<T>(List<T> list, string sep = " ")
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