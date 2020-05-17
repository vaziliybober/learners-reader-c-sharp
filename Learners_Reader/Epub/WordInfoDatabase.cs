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
    public class WordInfoDatabase
    {
        public string Path { get; }

        public List<WordInfo> WordInfoList { get; set; }

        public WordInfoDatabase(string path)
        {
            this.Path = path;
            System.IO.Directory.CreateDirectory(this.Path);
        }

        public void Load()
        {
            this.WordInfoList = new List<WordInfo>();

            foreach (string directory in System.IO.Directory.GetDirectories(this.Path))
            {
                WordInfo word = new WordInfo();

                word.Word = System.IO.File.ReadAllText(System.IO.Path.Combine(directory, "word.txt"));
                word.Notes = System.IO.File.ReadAllText(System.IO.Path.Combine(directory, "notes.txt"));

                word.ContextList = new List<string>(System.IO.File.ReadAllLines(System.IO.Path.Combine(directory, "context_list.txt")));

                WordInfoList.Add(word);
            }
        }

        public void Save()
        {
            foreach (WordInfo word in this.WordInfoList)
            {
                string path = System.IO.Path.Combine(this.Path, word.Word);
                System.IO.Directory.CreateDirectory(path);

                string wordPath = System.IO.Path.Combine(path, "word.txt");
                string notesPath = System.IO.Path.Combine(path, "notes.txt");
                string contextListPath = System.IO.Path.Combine(path, "context_list.txt");

                System.IO.File.WriteAllText(wordPath, word.Word);
                System.IO.File.WriteAllText(notesPath, word.Notes);
                

                string data = "";

                foreach (string context in word.ContextList)
                {
                    data += context + "\n";
                }

                System.IO.File.WriteAllText(contextListPath, data);
            }
        }

        public WordInfo GetWordInfo(string word)
        {
            foreach (WordInfo w in this.WordInfoList)
            {
                if (w.Word == word)
                    return w;
            }

            return null;
        }

        public bool ContainsWordInfo(string word)
        {
            foreach (WordInfo w in this.WordInfoList)
            {
                if (w.Word == word)
                    return true;
            }

            return false;
        }

        public void AddWordInfo(WordInfo wordInfo)
        {
            this.WordInfoList.Add(wordInfo);
        }

        public void TryAddContext(string word, string context)
        {
            if (ContainsWordInfo(word))
            {
                WordInfo wordInfo = GetWordInfo(word);
                wordInfo.TryAddContext(context);
            }
            else
            {
                WordInfo wordInfo = new WordInfo();
                wordInfo.Word = word;
                wordInfo.TryAddContext(context);
                this.WordInfoList.Add(wordInfo);
            }
        }

        public void TryChangeNotes(string word, string notes)
        {
            if (ContainsWordInfo(word))
            {
                GetWordInfo(word).Notes = notes;
            }
        }
    }
}