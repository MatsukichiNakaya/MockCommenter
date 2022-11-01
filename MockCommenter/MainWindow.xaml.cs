using Project.Common;
using Project.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MockCommenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer LoopTimer { get; set; }
        private UserInfo[] Users { get; set; }
        private String[] Comments { get; set; }
        private XorShift Rnd { get; set; }

        private const String COMMENT_FILE = @".\Comments.txt";
        private const String USER_FILE = @".\User.txt";

        public MainWindow()
        {
            InitializeComponent();
            this.LoopTimer = new DispatcherTimer() {
                Interval = new TimeSpan(0, 0, 1),
            };
            this.LoopTimer.Tick += LoopTimer_Tick;
            this.Rnd = new XorShift();

            this.Users = ReadUserData();
            this.Comments = TextFile.ReadLines(COMMENT_FILE)
                                    .Where(x => !String.IsNullOrEmpty(x)).ToArray();
            var items = new List<CommentInfo>();
            for (var i = 0; i < 20; i++) {
                items.Add(SelectRndComment());
            }
            this.CommentList.ItemsSource = items;
            this.LoopTimer.Start();
        }

        #region WindowBaseFunctions
        private IntPtr WndProc(IntPtr hwnd, Int32 msg,
                        IntPtr wParam, IntPtr lParam, ref Boolean handled)
        {
            switch (msg) {
                default:
                    break;
            }
            return IntPtr.Zero;
        }

        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            // メッセージ受信イベントを自身に追加する
            var src = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            src.AddHook(new HwndSourceHook(WndProc));
        }

        private void Window_MouseLeftButtonDown(Object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MinimumButton_Click(Object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Minimized
                                    ? WindowState.Normal
                                    : WindowState.Minimized;
        }

        private void MaximumButton_Click(Object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized
                                    ? WindowState.Normal
                                    : WindowState.Maximized;
        }

        private void CloseButton_Click(Object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        /// <summary>
        /// コメント表示追加タイマ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoopTimer_Tick(Object? sender, EventArgs e)
        {
            this.LoopTimer.Stop();
            var items = this.CommentList.ItemsSource as List<CommentInfo>;
            // コメントは100個まで
            if (100 <= (items?.Count ?? 0)) {
                items?.RemoveAt(0);
            }
            items?.Add(SelectRndComment());
            // 更新周期をランダムに設定
            var sec = this.Rnd.Next(0, 2);
            var milisec = this.Rnd.Next(0, 999);
            this.LoopTimer.Interval = new TimeSpan(0, 0, 0, sec, milisec);
            // 描画更新
            this.CommentList.Items.Refresh();
            if (items is not null) {
                this.CommentList.SelectedIndex = items.Count - 1;
                this.CommentList.ScrollIntoView(this.CommentList.SelectedItem);
            }
            this.LoopTimer.Start();
        }

        /// <summary>
        /// ユーザデータ取得
        /// </summary>
        /// <returns></returns>
        private static UserInfo[] ReadUserData()
        {
            var readData = TextFile.ReadLines(USER_FILE);
            var result = new List<UserInfo>();
            foreach (var line in readData) {
                var sp = line.Split(',');
                if(sp.Length <= 1) { continue; }
                result.Add(new UserInfo(sp[0], sp[1] == "1"));
            }
            return result.ToArray();
        }

        /// <summary>
        /// ユーザ画像取得
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static BitmapImage ReadImage(String path)
        {
            var bmpImg = new BitmapImage();

            bmpImg.BeginInit();
            bmpImg.CacheOption = BitmapCacheOption.OnLoad;
            bmpImg.DecodePixelWidth = 20;
            bmpImg.CreateOptions = BitmapCreateOptions.None;
            bmpImg.UriSource = new Uri(path);
            bmpImg.EndInit();
            bmpImg.Freeze();

            return bmpImg;
        }
        
        /// <summary>
        /// コメントランダム生成
        /// </summary>
        /// <returns></returns>
        private CommentInfo SelectRndComment()
        {
            var uIdx = this.Rnd.Next(0, this.Users.Length);
            var cIdx = this.Rnd.Next(0, this.Comments.Length);
            var path = System.IO.Path.GetFullPath(@$".\img\{uIdx:000}.png");
            var cmt = new CommentInfo(ReadImage(path), this.Comments[cIdx], this.Users[uIdx]);
            return cmt;
        }
    }
}
