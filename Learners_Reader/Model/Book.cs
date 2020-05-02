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

        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Language { get; private set; }
        public string Description { get; private set; }
        public List<string> PathsToSectionsInReadingOrder { get; private set; }

        public Book(string path)
        {
            this.Path = path;
            LoadBook();
        }

        private void LoadBook()
        {
            EpubParser parser = new EpubParser(this.Path);
            parser.Parse();

            this.Title = parser.Title;
            this.Author = parser.Author;
            this.Language = parser.Language;
            this.Description = parser.Description;
            this.PathsToSectionsInReadingOrder = parser.PathsToSectionsInReadingOrder;
        }

        public string ReadSection(int i)
        {
            return System.IO.File.ReadAllText(this.PathsToSectionsInReadingOrder[i]);
        }
    }
}