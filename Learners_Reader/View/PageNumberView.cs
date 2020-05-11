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
    public class PageNumberView : TextView
    {
        private int currentPageNumber;
        public int CurrentPageNumber
        {
            get
            {
                return currentPageNumber;
            }

            set
            {
                currentPageNumber = value;
                UpdateText();
            }
        }


        private int totalPageCount;
        public int TotalPageCount
        {
            get
            {
                return totalPageCount;
            }

            set
            {
                totalPageCount = value;
                UpdateText();
            }
        }


        private bool currentPageNumberIsLoading;
        public bool CurrentPageNumberIsLoading
        {
            get
            {
                return currentPageNumberIsLoading;
            }

            set
            {
                currentPageNumberIsLoading = value;
                UpdateText();
            }
        }

        private bool totalPageCountIsLoading;
        public bool TotalPageCountIsLoading
        {
            get
            {
                return totalPageCountIsLoading;
            }

            set
            {
                totalPageCountIsLoading = value;
                UpdateText();
            }
        }


        public PageNumberView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            currentPageNumber = 1;
            totalPageCount = 0;
            currentPageNumberIsLoading = false;
            totalPageCountIsLoading = false;
        }

        private void UpdateText()
        {
            string current = this.CurrentPageNumberIsLoading ? "..." : this.CurrentPageNumber.ToString();
            string total = this.TotalPageCountIsLoading ? "..." : this.TotalPageCount.ToString();
            Text = $"{current}/{total}";
        }
    }
}