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
using ICSharpCode.SharpZipLib.Zip;
using Learners_Reader.Utilities;
using System.IO;

namespace Learners_Reader.Model
{
    public class Library
    {
        readonly FastZip fastZip = new FastZip();

        private List<Book> Books { get; set; }
        public string Path { get; }

        public Library(string path) : base()
        {
            this.Path = path;
            this.Books = new List<Book>();

            LoadLibrary();
        }

        private void LoadLibrary()
        {
            foreach (string directory in Directory.GetDirectories(this.Path))
            {
                AddBook(directory);
            }
        }

        private void AddBook(string path)
        {
            this.Books.Add(new Book(path));
        }

        public List<string> GetAllBookNames()
        {
            List<string> result = new List<string>();

            foreach (Book book in this.Books)
            {
                result.Add(book.Title);
            }

            return result;
        }

        public void ImportBook(string path)
        {
            string filename = System.IO.Path.GetFileNameWithoutExtension(path);
            string newfolderpath = this.Path + "/" + filename;
            System.IO.Directory.CreateDirectory(newfolderpath);
            fastZip.ExtractZip(path, newfolderpath, null);

            AddBook(newfolderpath);
        }

        public Book GetBook(string title)
        {
            foreach (Book book in this.Books)
            {
                if (book.Title == title)
                    return book;
            }

            return null;
        }
    }
}