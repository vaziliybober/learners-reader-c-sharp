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
using Learners_Reader.Utilities;

namespace Learners_Reader.Epub
{
    public class Database
    {
        public string Directory { get; }


        public Database(string path)
        {
            this.Directory = path;
        }


        public string ChapterIndexPath { get { return System.IO.Path.Combine(this.Directory, "chapter_index.txt"); } }
        public void OnChapterIndexChanged(int value)
        {
            System.IO.File.WriteAllText(this.ChapterIndexPath, value.ToString());
        }
        public int LoadChapterIndex()
        {
            try
            {
                return int.Parse(System.IO.File.ReadAllText(this.ChapterIndexPath));
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public string RelaivePageIndexPath { get { return System.IO.Path.Combine(this.Directory, "relative_page_index.txt"); } }
        public void OnRelativePageIndexChanged(int value)
        {
            System.IO.File.WriteAllText(this.RelaivePageIndexPath, value.ToString());
        }
        public int LoadRelativePageIndex()
        {
            try
            {
                return int.Parse(System.IO.File.ReadAllText(this.RelaivePageIndexPath));
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public string AbsolutePageNumberPath { get { return System.IO.Path.Combine(this.Directory, "absolute_page_number.txt"); } }
        public void OnAbsolutePageNumberChanged(int value)
        {
            System.IO.File.WriteAllText(this.AbsolutePageNumberPath, value.ToString());
        }
        public int LoadAbsolutePageNumber()
        {
            try
            {
                return int.Parse(System.IO.File.ReadAllText(this.AbsolutePageNumberPath));
            }
            catch (Exception)
            {
                return 1;
            }
        }


        public string TotalPageCountPath { get { return System.IO.Path.Combine(this.Directory, "total_page_count.txt"); } }
        public void OnTotalPageCountChanged(int value)
        {
            System.IO.File.WriteAllText(this.TotalPageCountPath, value.ToString());
        }
        public int LoadTotalPageCount()
        {
            try
            {
                return int.Parse(System.IO.File.ReadAllText(this.TotalPageCountPath));
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public string FirstPageNumbersPath { get { return System.IO.Path.Combine(this.Directory, "first_page_numbers.txt"); } }
        public void OnFirstPageNumbersChanged(List<int> value)
        {
            System.IO.File.WriteAllText(this.FirstPageNumbersPath, Functions.ListToString<int>(value));
        }
        public List<int> LoadFirstPageNumbers()
        {
            try
            {
                return Functions.StringToIntList(System.IO.File.ReadAllText(this.FirstPageNumbersPath));
            }
            catch (Exception)
            {
                return new List<int>();
            }
        }


        public string LastPageNumbersPath { get { return System.IO.Path.Combine(this.Directory, "last_page_numbers.txt"); } }
        public void OnLastPageNumbersChanged(List<int> value)
        {
            System.IO.File.WriteAllText(this.LastPageNumbersPath, Functions.ListToString<int>(value));
        }
        public List<int> LoadLastPageNumbers()
        {
            try
            {
                return Functions.StringToIntList(System.IO.File.ReadAllText(this.LastPageNumbersPath));
            }
            catch (Exception)
            {
                return new List<int>();
            }
        }


        public string FontSizePath { get { return System.IO.Path.Combine(this.Directory, "font_size.txt"); } }
        public void OnFontSizeChanged(int value)
        {
            System.IO.File.WriteAllText(this.FontSizePath, value.ToString());
        }
        public int LoadFontSize()
        {
            try
            {
                return int.Parse(System.IO.File.ReadAllText(this.FontSizePath));
            }
            catch (Exception)
            {
                return 100;
            }
        }
    }
}