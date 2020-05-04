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

using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using Learners_Reader.Utilities;

namespace Learners_Reader.Model
{
    public class Book
    {
        public string Path { get; }

        public string RootFolderPath { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Language { get; private set; }
        public string Description { get; private set; }
        public List<string> PathsToSectionsInReadingOrder { get; private set; }

        public int CurrentSectionIndex { get; set; }

        public Book(string path)
        {
            this.Path = path;
            this.CurrentSectionIndex = 0;

            LoadBook();
        }

        private void LoadBook()
        {
            EpubParser parser = new EpubParser(this.Path);
            parser.Parse();

            this.RootFolderPath = parser.RootFolderPath;
            this.Title = parser.Title;
            this.Author = parser.Author;
            this.Language = parser.Language;
            this.Description = parser.Description;
            this.PathsToSectionsInReadingOrder = parser.PathsToSectionsInReadingOrder;
        }

        private string ModifySection(string section)
        {
            string js = @"<script>window.onload = function () {
      var d = document.getElementsByTagName('body')[0];
      d.style.height = window.innerHeight + 'px';
      d.style.webkitColumnCount = 1;
      d.style.columnFill = 'auto';
      d.style.columnGap = 0;
      d.style.margin = 0;
      console.log(""<number of pages>:"" + d.scrollWidth/d.clientWidth);
    }</script>";

            return section.Replace("</body>", js + "</body>");
        }

        public string ReadSection(int i)
        {
            if (i < 0 || i >= this.PathsToSectionsInReadingOrder.Count)
                return null;

            this.CurrentSectionIndex = i;
            return ModifySection(System.IO.File.ReadAllText(this.PathsToSectionsInReadingOrder[i]));
        }

        public string ReadCurrentSection()
        {
            return ReadSection(this.CurrentSectionIndex);
        }

        public string ReadNextSection()
        {
            return ReadSection(this.CurrentSectionIndex + 1);
        }

        public string ReadPreviousSection()
        {
            return ReadSection(this.CurrentSectionIndex - 1);
        }
    }
}