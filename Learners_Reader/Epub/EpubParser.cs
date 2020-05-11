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
using System.Xml;

namespace Learners_Reader.Epub
{
    public class EpubParser
    {
        public string Path { get; }
        private string RootFilePath { get; set; }
        public string RootFolderPath { get; private set; }
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Language { get; private set; }
        public string Description { get; private set; }
        public List<string> PathsToChaptersInReadingOrder { get; private set; }
        public List<string> ChapterTitles { get; private set; }

        public EpubParser(string path)
        {
            this.Path = path;
        }

        public void Parse()
        {
            ParseContainer();
            ParseRootFile();
            ParseChapters();
        }

        private void ParseContainer()
        {
            string containerPath = System.IO.Path.Combine(this.Path, "META-INF/container.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(containerPath);

            this.RootFilePath = System.IO.Path.Combine(this.Path, doc.GetElementsByTagName("rootfile")[0].Attributes.GetNamedItem("full-path").Value);
            this.RootFolderPath = System.IO.Path.GetDirectoryName(this.RootFilePath);
        }

        private void ParseRootFile()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.RootFilePath);

            this.Title = doc.GetElementsByTagName("title", "http://purl.org/dc/elements/1.1/")[0].InnerText;
            this.Author = doc.GetElementsByTagName("creator", "http://purl.org/dc/elements/1.1/")[0].InnerText;
            this.Language = doc.GetElementsByTagName("language", "http://purl.org/dc/elements/1.1/")[0].InnerText;
            this.Description = doc.GetElementsByTagName("description", "http://purl.org/dc/elements/1.1/")[0].InnerText;

            this.PathsToChaptersInReadingOrder = new List<string>();
            XmlNode spineNode = doc.GetElementsByTagName("spine")[0];
            XmlNode manifestNode = doc.GetElementsByTagName("manifest")[0];
            XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("d", "http://www.idpf.org/2007/opf");

            foreach (XmlNode itemRef in spineNode)
            {
                string sectionId = itemRef.Attributes["idref"].Value;
                
                string relativeChapterPath = manifestNode.SelectSingleNode($"d:item[@id='{sectionId}']", nsManager).Attributes["href"].Value;
                string chapterPath = System.IO.Path.Combine(this.RootFolderPath, relativeChapterPath);
                this.PathsToChaptersInReadingOrder.Add(chapterPath);
            }
        }

        private void ParseChapters()
        {
            this.ChapterTitles = new List<string>();

            for (int i = 0; i < this.PathsToChaptersInReadingOrder.Count; i++)
            {
                string path = this.PathsToChaptersInReadingOrder[i];

                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                string chapterTitle = doc.GetElementsByTagName("title")[0].InnerText;
                this.ChapterTitles.Add($"{i + 1}. " + chapterTitle);
            }
        }
    }
}