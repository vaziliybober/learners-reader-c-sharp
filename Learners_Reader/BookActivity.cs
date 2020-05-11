using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Learners_Reader.Epub;
using Learners_Reader.Utilities;
using Learners_Reader.View;

namespace Learners_Reader
{
    [Activity(Label = "BookActivity")]
    public class BookActivity : AppCompatActivity
    {
        private Book book;

        private Database database;

        private ChapterView chv1;
        private ChapterView chv2;
        private ChapterView chv3;

        private ChapterView PrevChapterView { get; set; }
        private ChapterView CurrChapterView { get; set; }
        private ChapterView NextChapterView { get; set; }


        private event Action<int> RelativePageNumberChanged;

        private BookScanner bookScanner;

        private PageNumberView pageNumberView;


        private LinearLayout topMenuLayout;

        private MyNumberEditText pageNumberEditText;
        private Button moveToPageButton;
        private Button topMenuCloseButton;

        private Spinner dropDownChapterSpinner;
        private Button moveToChapterButton;

        private Button largerFontSizeButton;
        private Button smallerFontSizeButton;

        private Button showWordInfoListButton;


        /////////////////////////////////////////////////////////////////////


        public event Action<int> ChapterIndexChanged;
        private int chapterIndex;
        private int ChapterIndex
        {
            get
            {
                return chapterIndex;
            }

            set
            {
                chapterIndex = value;
                this.ChapterIndexChanged?.Invoke(value);
            }
        }


        public event Action<int> AbsolutePageNumberChanged;
        private int absolutePageNumber;
        public int AbsolutePageNumber
        {
            get
            {
                return absolutePageNumber;
            }

            private set
            {
                absolutePageNumber = value;
                this.AbsolutePageNumberChanged?.Invoke(value);
            }
        }


        /////////////////////////////////////////////////////////////////////


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_book);

            book = GlobalData.CurrentBook;

            ConfigureDatabase();

            ConfigureChapterViews();
            ConfigureBookScanner();
            ConfigurePageNumberView();
            ConfigureTopMenu();

            InitializeProperties();

            ShowContents();
        }

        private void ConfigureDatabase()
        {
            string pathToDatabase = System.IO.Path.Combine(book.Path, "Data");
            System.IO.Directory.CreateDirectory(pathToDatabase);
            database = new Database(pathToDatabase);
        }

        private void ConfigureChapterViews()
        {
            chv1 = FindViewById<ChapterView>(Resource.Id.chapterView1);
            PrevChapterView = chv1;

            chv2 = FindViewById<ChapterView>(Resource.Id.chapterView2);
            CurrChapterView = chv2;

            chv3 = FindViewById<ChapterView>(Resource.Id.chapterView3);
            NextChapterView = chv3;

            this.RelativePageNumberChanged += database.OnRelativePageIndexChanged;

            ConfigureChapterView(PrevChapterView);
            ConfigureChapterView(CurrChapterView);
            ConfigureChapterView(NextChapterView);

            UpdateChapterViewVisibilities();
        }

        private void ConfigureBookScanner()
        {
            bookScanner = FindViewById<BookScanner>(Resource.Id.bookScanner);
            ConfigureChapterView(bookScanner);

            bookScanner.Visibility = ViewStates.Invisible;
            bookScanner.Enabled = false;
        }

        private void ConfigurePageNumberView()
        {
            pageNumberView = FindViewById<PageNumberView>(Resource.Id.pageNumberView);
            pageNumberView.SetBackgroundColor(Android.Graphics.Color.Black);
            pageNumberView.SetTextColor(Android.Graphics.Color.White);

            bookScanner.StartedScanning += () =>
            {
                pageNumberView.CurrentPageNumberIsLoading = true;
                pageNumberView.TotalPageCountIsLoading = true;
            };

            bookScanner.FinishedScanning += () =>
            {
                pageNumberView.CurrentPageNumberIsLoading = false;
                pageNumberView.TotalPageCountIsLoading = false;
            };

            this.AbsolutePageNumberChanged += (int value) =>
            {
                pageNumberView.CurrentPageNumber = value;
            };

            bookScanner.TotalPageCountChanged += (int value) =>
            {
                pageNumberView.TotalPageCount = value;
            };
        }

        private void ConfigureTopMenu()
        {
            void ConfigurePageNavigation()
            {
                topMenuLayout = FindViewById<LinearLayout>(Resource.Id.topMenuLayout);

                pageNumberEditText = FindViewById<MyNumberEditText>(Resource.Id.pageNumberEditText);
                pageNumberEditText.MinValue = 1;
                pageNumberEditText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
                {
                    moveToPageButton.Enabled = e.Text.ToString() != "";
                };

                moveToPageButton = FindViewById<Button>(Resource.Id.moveToPageButton);
                moveToPageButton.Click += (object sender, EventArgs e) =>
                {
                    ScrollToPage(int.Parse(pageNumberEditText.Text.ToString()));
                };

                bookScanner.StartedScanning += () =>
                {
                    pageNumberEditText.Enabled = false;
                    moveToPageButton.Enabled = false;
                };

                bookScanner.FinishedScanning += () =>
                {
                    pageNumberEditText.Enabled = true;
                    moveToPageButton.Enabled = true;
                    pageNumberEditText.Text = "";
                    pageNumberEditText.MaxValue = bookScanner.TotalPageCount;
                };

                topMenuCloseButton = FindViewById<Button>(Resource.Id.topMenuCloseButton);
                topMenuCloseButton.Click += (object sender, EventArgs e) =>
                {
                    topMenuLayout.Visibility = ViewStates.Invisible;
                };
            }

            void ConfigureChapterNavigation()
            {
                dropDownChapterSpinner = FindViewById<Spinner>(Resource.Id.dropDownChapterSpinner);
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, book.ChapterTitles);
                dropDownChapterSpinner.Adapter = adapter;
                moveToChapterButton = FindViewById<Button>(Resource.Id.moveToChapterButton);

                moveToChapterButton.Click += (object sender, EventArgs e) =>
                {
                    ShowChapter(dropDownChapterSpinner.SelectedItemPosition);
                };
            }

            void ConfigureFontSizeChanging()
            {
                largerFontSizeButton = FindViewById<Button>(Resource.Id.largerFontSizeButton);
                largerFontSizeButton.Click += (object sender, EventArgs e) =>
                {
                    book.FontSize += 10;
                    bookScanner.ScanThroughBook(book, () =>
                    {
                        this.AbsolutePageNumber = bookScanner.GetAbsolutePageNumber(this.ChapterIndex, this.CurrChapterView.CurrentPageIndex);
                    });
                    ShowChapter(this.ChapterIndex);
                };

                smallerFontSizeButton = FindViewById<Button>(Resource.Id.smallerFontSizeButton);
                smallerFontSizeButton.Click += (object sender, EventArgs e) =>
                {
                    book.FontSize -= 10;
                    bookScanner.ScanThroughBook(book, () =>
                    {
                        this.AbsolutePageNumber = bookScanner.GetAbsolutePageNumber(this.ChapterIndex, this.CurrChapterView.CurrentPageIndex);
                    });
                    ShowChapter(this.ChapterIndex);
                };

            }

            void ConfigureShowWordInfoList()
            {
                showWordInfoListButton = FindViewById<Button>(Resource.Id.showWordInfoListButton);
                showWordInfoListButton.Click += (object sender, EventArgs e) =>
                {
                    Intent nextActivityIntent = new Intent(this, typeof(WordInfoListActivity));
                    StartActivity(nextActivityIntent);
                };
            }

            ConfigurePageNavigation();
            ConfigureChapterNavigation();
            ConfigureFontSizeChanging();
            ConfigureShowWordInfoList();
        }

        private void InitializeProperties()
        {
            this.ChapterIndex = database.LoadChapterIndex();
            this.ChapterIndexChanged += database.OnChapterIndexChanged;


            this.AbsolutePageNumber = database.LoadAbsolutePageNumber();
            this.AbsolutePageNumberChanged += database.OnAbsolutePageNumberChanged;


            int totalPageCount = database.LoadTotalPageCount();
            bookScanner.TotalPageCountChanged += database.OnTotalPageCountChanged;

            List<int> firstPageNumbers = database.LoadFirstPageNumbers();
            bookScanner.FirstPageNumbersChanged += database.OnFirstPageNumbersChanged;

            List<int> lastPageNumbers = database.LoadLastPageNumbers();
            bookScanner.LastPageNumbersChanged += database.OnLastPageNumbersChanged;

            book.FontSize = database.LoadFontSize();
            book.FontSizeChanged += database.OnFontSizeChanged;



            if (totalPageCount != 0 && firstPageNumbers.Count != 0 && lastPageNumbers.Count != 0)
            {
                bookScanner.ImitateScanning(totalPageCount, firstPageNumbers, lastPageNumbers);
            }
            else
            {
                bookScanner.ScanThroughBook(book, () =>
                {
                    this.AbsolutePageNumber = bookScanner.GetAbsolutePageNumber(this.ChapterIndex, this.CurrChapterView.CurrentPageIndex);
                });
                pageNumberView.CurrentPageNumberIsLoading = false;
            }
        }

        private void ShowContents()
        {
            int relativePageIndex = database.LoadRelativePageIndex();
            ShowChapter(this.ChapterIndex, relativePageIndex);
        }


        /////////////////////////////////////////////////////////////////////


        private void ConfigureChapterView(ChapterView chv)
        {
            chv.Settings.JavaScriptCanOpenWindowsAutomatically = true;
            chv.Settings.JavaScriptEnabled = true;

            chv.HorizontalScrollBarEnabled = false;
            chv.VerticalScrollBarEnabled = false;

            chv.SetWebViewClient(new WebViewClient());

            chv.SetWebChromeClient(new MyWebChromeClient());

            (chv.MyWebChromeClient).SwipeLeft += () =>
            {
                ScrollToNextPage();
            };

            (chv.MyWebChromeClient).SwipeRight += () =>
            {
                ScrollToPrevPage();
            };

            chv.MyWebChromeClient.SwipeDown += () =>
            {
                topMenuLayout.Visibility = ViewStates.Visible;
            };

            chv.MyWebChromeClient.WordSelected += (string word, string sentence) =>
            {
                GlobalData.CurrentWord = word;
                GlobalData.CurrentContext = sentence;
                Intent nextActivityIntent = new Intent(this, typeof(TranslationActivity));
                StartActivity(nextActivityIntent);
            };

            chv.BaseURL = "file://" + book.RootFolderPath + "/";
        }

        private void UpdateChapterViewVisibilities()
        {

            PrevChapterView.Visibility = ViewStates.Invisible;
            CurrChapterView.Visibility = ViewStates.Visible;
            NextChapterView.Visibility = ViewStates.Invisible;

            PrevChapterView.Enabled = false;
            CurrChapterView.Enabled = true;
            NextChapterView.Enabled = false;

            void RedirectInvoke(int pageCount)
            {
                this.RelativePageNumberChanged?.Invoke(pageCount);
            }

            PrevChapterView.CurrentPageIndexChanged -= RedirectInvoke;
            CurrChapterView.CurrentPageIndexChanged += RedirectInvoke;
            NextChapterView.CurrentPageIndexChanged -= RedirectInvoke;
        }


        /////////////////////////////////////////////////////////////////////


        private void ShowChapter(int chapterIndex, int pageIndex = 0, Action<int> onChapterLoaded = null)
        {
            if (chapterIndex < 0 || chapterIndex >= book.ChapterCount)
                return;

            this.ChapterIndex = chapterIndex;

            if (bookScanner.IsComplete)
            {
                this.AbsolutePageNumber = bookScanner.GetAbsolutePageNumber(chapterIndex, pageIndex);
            }

            CurrChapterView.ShowChapter(book.ReadChapter(chapterIndex), (int pageCount) =>
            {
                CurrChapterView.ScrollToPage(pageIndex);
                onChapterLoaded?.Invoke(pageCount);
            });

            if (chapterIndex != 0)
            {
                PrevChapterView.ShowChapter(book.ReadChapter(chapterIndex - 1), (int pageCount) =>
                {
                    PrevChapterView.ScrollToEnd();
                });
            }

            if (chapterIndex != book.ChapterCount - 1)
            {
                NextChapterView.ShowChapter(book.ReadChapter(chapterIndex + 1), (int pageCount) =>
                {
                    NextChapterView.ScrollToStart();
                });
            }
        }

        private void ShowNextChapter()
        {
            if (this.ChapterIndex == book.ChapterCount - 1)
                return;

            this.ChapterIndex++;

            ChapterView temp = PrevChapterView;
            PrevChapterView = CurrChapterView;
            CurrChapterView = NextChapterView;
            NextChapterView = temp;

            UpdateChapterViewVisibilities();
            CurrChapterView.ScrollToStart();

            if (this.ChapterIndex == book.ChapterCount - 1)
                return;

            NextChapterView.ShowChapter(book.ReadChapter(this.ChapterIndex + 1));
        }

        private void ShowPrevChapter()
        {
            if (this.ChapterIndex == 0)
                return;

            this.ChapterIndex--;

            ChapterView temp = NextChapterView;
            NextChapterView = CurrChapterView;
            CurrChapterView = PrevChapterView;
            PrevChapterView = temp;

            UpdateChapterViewVisibilities();
            CurrChapterView.ScrollToEnd();

            if (this.ChapterIndex == 0)
                return;

            PrevChapterView.ShowChapter(book.ReadChapter(this.ChapterIndex - 1));
        }

        private void ScrollToPage(int absolutePageNumber, Action<int> onChapterLoaded = null)
        {
            int chapterIndex = bookScanner.GetChapterIndex(absolutePageNumber);
            int relPageIndex = bookScanner.GetRelativePageIndex(absolutePageNumber);
            ShowChapter(chapterIndex, relPageIndex, onChapterLoaded);
        }

        private void ScrollToNextPage()
        {
            if (this.AbsolutePageNumber == bookScanner.TotalPageCount)
                return;

            if (CurrChapterView.CurrentPageIndex == CurrChapterView.PageCount - 1)
            {
                ShowNextChapter();
            }

            else
            {
                CurrChapterView.ScrollToNextPage();
            }

            this.AbsolutePageNumber++;
        }

        private void ScrollToPrevPage()
        {
            if (this.AbsolutePageNumber == 1)
                return;

            if (CurrChapterView.CurrentPageIndex == 0)
            {
                ShowPrevChapter();
            }
            else
            {
                CurrChapterView.ScrollToPrevPage();
            }

            this.AbsolutePageNumber--;
        }
    }
}