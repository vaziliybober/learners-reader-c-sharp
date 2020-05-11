using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Learners_Reader.View
{
    public class MyNumberEditText : EditText
    {
        private string oldText;

        public int MaxValue { get; set; }
        public int MinValue { get; set; }

        public MyNumberEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            oldText = Text;
            this.MaxValue = int.MaxValue;
            this.MinValue = int.MinValue;

            this.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                if (e.Text.ToString() == "" || int.TryParse(e.Text.ToString(), out int value) && value <= this.MaxValue && value >= this.MinValue)
                {
                    oldText = e.Text.ToString();
                }
                else
                {
                    Text = oldText;
                    SetSelection(Text.Length);
                }
            };
        }

        
    }
}