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
using Learners_Reader.Epub;
using Learners_Reader.Utilities;

namespace Learners_Reader.View
{
    public class BookScanner : ChapterView
    {
        public bool IsComplete { get; private set; }
        public event Action FinishedScanning;
        public event Action StartedScanning;


        public event Action<int> TotalPageCountChanged;
        private int totalPageCount;
        public int TotalPageCount
        {
            get
            {
                return totalPageCount;
            }

            private set
            {
                totalPageCount = value;
                TotalPageCountChanged?.Invoke(value);
            }
        }


        public event Action<List<int>> FirstPageNumbersChanged;
        private List<int> firstPageNumbers;
        public List<int> FirstPageNumbers
        {
            get
            {
                return firstPageNumbers;
            }

            private set
            {
                firstPageNumbers = value;
                FirstPageNumbersChanged?.Invoke(value);
            }
        }


        public event Action<List<int>> LastPageNumbersChanged;
        private List<int> lastPageNumbers;
        public List<int> LastPageNumbers
        {
            get
            {
                return lastPageNumbers;
            }

            private set
            {
                lastPageNumbers = value;
                LastPageNumbersChanged?.Invoke(value);
            }
        }


        public int GetChapterIndex(int absolutePageNumber)
        {
            for (int i = 0; i < this.FirstPageNumbers.Count; i++)
            {
                if (absolutePageNumber >= this.FirstPageNumbers[i] && absolutePageNumber <= this.LastPageNumbers[i])
                {
                    return i;
                }
            }

            return -1;
        }

        public int GetRelativePageIndex(int absolutePageNumber)
        {
            for (int i = 0; i < this.FirstPageNumbers.Count; i++)
            {
                if (absolutePageNumber >= this.FirstPageNumbers[i] && absolutePageNumber <= this.LastPageNumbers[i])
                {
                    return absolutePageNumber - this.FirstPageNumbers[i];
                }
            }

            return -1;
        }

        public int GetAbsolutePageNumber(int chapterIndex, int pageIndex)
        {
            return this.FirstPageNumbers[chapterIndex] + pageIndex;
        }



        public BookScanner(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.IsComplete = true;

            this.StartedScanning += () =>
            {
                this.IsComplete = false;
            };

            this.FinishedScanning += () =>
            {
                this.IsComplete = true;
            };
        }

        public void ImitateScanning(int totalPageCount, List<int> firstPageNumbers, List<int> lastPageNumbers)
        {
            this.StartedScanning?.Invoke();

            this.TotalPageCount = totalPageCount;
            this.FirstPageNumbers = firstPageNumbers;
            this.LastPageNumbers = lastPageNumbers;

            this.FinishedScanning?.Invoke();
        }

        public int chapterIndex = 0;
        private bool flag = false;

        public void ScanThroughBook(Book book, Action onScanningFinished = null)
        {
            void ScanThroughBookRecursively()
            {
                if (flag)
                {
                    chapterIndex = 0;
                    this.TotalPageCount = 0;
                    this.FirstPageNumbers = new List<int>();
                    this.LastPageNumbers = new List<int>();
                    flag = false;
                }

                ShowChapter(book.ReadChapter(chapterIndex), (int pageCount) =>
                {
                    

                    firstPageNumbers.Add(this.TotalPageCount + 1);
                    lastPageNumbers.Add(this.TotalPageCount + pageCount);
                    totalPageCount += pageCount;

                    if (chapterIndex == book.ChapterCount - 1)
                    {
                        this.TotalPageCount = totalPageCount;
                        this.FirstPageNumbers = firstPageNumbers;
                        this.LastPageNumbers = lastPageNumbers;

                        onScanningFinished?.Invoke();
                        this.FinishedScanning?.Invoke();
                    }
                    else
                    {
                        chapterIndex++;
                        ScanThroughBookRecursively();
                    }

                });
            }

            chapterIndex = 0;
            this.TotalPageCount = 0;
            this.FirstPageNumbers = new List<int>();
            this.LastPageNumbers = new List<int>();

            if (this.IsComplete)
            {
                this.StartedScanning?.Invoke();
                ScanThroughBookRecursively();
            }
            else
            {
                flag = true;
            }
        }


    }
}