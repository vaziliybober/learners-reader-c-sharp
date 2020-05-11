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

            foreach (string file in System.IO.Directory.GetFiles(this.Path))
            {
                WordInfo word = new WordInfo();
                word.ContextList = new List<string>();

                string[] lines = System.IO.File.ReadAllLines(file);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (i == 0)
                    {
                        word.Word = lines[i];
                    }

                    else if (i == 1)
                    {
                        word.Notes = lines[i];
                    }

                    else
                    {
                        word.ContextList.Add(lines[i]);
                    }
                }

                this.WordInfoList.Add(word);
            }
        }

        public void Save()
        {
            foreach (WordInfo word in this.WordInfoList)
            {
                string data = "";
                string path = System.IO.Path.Combine(this.Path, word.Word + ".txt");
                data += word.Word + "\n";
                data += word.Notes + "\n";

                foreach (string context in word.ContextList)
                {
                    data += context + "\n";
                }

                System.IO.File.WriteAllText(path, data);
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