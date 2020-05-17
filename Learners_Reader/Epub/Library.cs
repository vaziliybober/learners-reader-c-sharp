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

namespace Learners_Reader.Epub
{
    public class Library
    {
        readonly FastZip fastZip = new FastZip();

        public List<Book> Books { get; private set; }
        public string Path { get; }

        public Library(string path) : base()
        {
            this.Path = path;
            this.Books = new List<Book>();

            Load();
        }

        private void Load()
        {
            foreach (string directory in Directory.GetDirectories(this.Path))
            {
                this.Books.Add(new Book(directory));
            }
        }

        public List<string> GetAllBookNames()
        {
            List<string> result = new List<string>();

            foreach (Book book in this.Books)
            {
                result.Add(book.Title + " by " + book.Author);
            }

            return result;
        }

        public void AddBook(string path)
        {
            string filename = Functions.RemoveInvalidCharactersFromPath(System.IO.Path.GetFileNameWithoutExtension(path));
            string newfolderpath = System.IO.Path.Combine(this.Path, filename);
            newfolderpath = Functions.CreateDirectory(newfolderpath);
            

            try
            {
                fastZip.ExtractZip(path, newfolderpath, null);
                this.Books.Add(new Book(newfolderpath));
            }
            catch (Exception e)
            {
                System.IO.Directory.Delete(newfolderpath, true);
                throw e;
            }
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