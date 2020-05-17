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

namespace Learners_Reader.Epub
{
    public class Database
    {
        public string Path { get; }
        public int CurrentBookIndex { get; set; }

        public Database(string path)
        {
            this.Path = path;
            Load();
        }

        public void Load()
        {
            try
            {
                this.CurrentBookIndex = int.Parse(System.IO.File.ReadAllText(this.Path));
            }
            catch(Exception)
            {
                this.CurrentBookIndex = -1;
            }
        }

        public void Save()
        {
            System.IO.File.WriteAllText(this.Path, this.CurrentBookIndex.ToString());
        }
    }
}