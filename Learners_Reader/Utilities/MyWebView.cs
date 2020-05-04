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
using Android.Webkit;
using Android.Widget;

namespace Learners_Reader.Utilities
{
    public class MyWebView : WebView
    {
        public MyWebView(Context context, IAttributeSet attrs) : base(context, attrs)
        {

        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            return base.OnTouchEvent(e);
        }
    }
}