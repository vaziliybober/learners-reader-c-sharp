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
    public class WordInfo
    {
        public string Word { get; set; }
        public List<string> ContextList { get; set; }
        public string Notes { get; set; }

        public WordInfo()
        {
            this.ContextList = new List<string>();
        }

        public bool ContainsContext(string context)
        {
            return this.ContextList.Contains(context);
        }

        public void TryAddContext(string context)
        {
            if (!ContainsContext(context))
            {
                this.ContextList.Add(context);
            }
        }
    }
}