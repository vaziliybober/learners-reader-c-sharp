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
        private List<string> PathsToChaptersInReadingOrder { get; set; }

        public int ChapterCount { get { return this.PathsToChaptersInReadingOrder.Count; } }


        public Book(string path)
        {
            this.Path = path;

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
            this.PathsToChaptersInReadingOrder = parser.PathsToChaptersInReadingOrder;
        }

        private string InjectJavascriptIntoChapter(string section)
        {
            string js = @"<script>
      var startX, startY, startTime;
var distXThreshold = 50;
var distYThreshold = 100;
var timeThreshold = 200;

window.onload = function () {
  var d = document.getElementsByTagName('body')[0];
  d.style.height = window.innerHeight + 'px';
  d.style.webkitColumnCount = 1;
  d.style.columnFill = 'auto';
  d.style.columnGap = 0;
  d.style.margin = 0;
  var pageCount = Math.round(d.scrollWidth / d.clientWidth);
  alert('page count: ' + pageCount);


  function onSwipeLeft() {
    alert('swipe left');
  }

  function onSwipeRight() {
    alert('swipe right');
  }


  window.addEventListener('touchstart', function (e) {
    var touchObj = e.changedTouches[0];
    startX = touchObj.pageX;
    startY = touchObj.pageY;
    startTime = new Date().getTime()
    e.preventDefault();
  }, {passive: false});
  
  window.addEventListener('touchend', function (e) {
    var touchObj = e.changedTouches[0];
    var distX = touchObj.pageX - startX;
    var distY = touchObj.pageY - startY;
    var time = new Date().getTime() - startTime;

    if (Math.abs(distX) > distXThreshold && Math.abs(distY) < distYThreshold && time <= timeThreshold) {
      if (distX > 0) onSwipeRight();
      else onSwipeLeft();
    }

    e.preventDefault();
  }, {passive: false});

  window.addEventListener('touchmove', function(e) {
    e.preventDefault();
  }, {passive: false});

}
      </script>";

            return section.Replace("</body>", js + "</body>");
        }

        public string ReadChapter(int i)
        {
            if (i < 0 || i >= this.PathsToChaptersInReadingOrder.Count)
                return null;

            return InjectJavascriptIntoChapter(System.IO.File.ReadAllText(this.PathsToChaptersInReadingOrder[i]));
        }
    }
}