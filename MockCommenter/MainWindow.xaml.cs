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
        private const String IMAGE_DIR = "img";

        public MainWindow()
        {
            InitializeComponent();

            // データ読込み
            ExistsFiles();
            this.Users = ReadUserData();
            this.UserSelectBox.ItemsSource = this.Users;
            if(0 < this.Users.Length) {
                this.UserSelectBox.SelectedIndex = 0;
            }
            this.Comments = TextFile.ReadLines(COMMENT_FILE)
                                    .Where(x => !String.IsNullOrEmpty(x)).ToArray();
            // 初期データ設定
            this.Rnd = new XorShift();
            var items = new List<CommentInfo>();
            for (var i = 0; i < 20; i++) {
                items.Add(SelectRndComment());
            }
            this.CommentList.ItemsSource = items;
            this.LoopTimer = new DispatcherTimer() {
                Interval = new TimeSpan(0, 0, 1),
            };
            this.LoopTimer.Tick += LoopTimer_Tick;
            
            this.LoopTimer.Start();
        }

        /// <summary>
        /// ファイル有無確認(無ければ作る)
        /// </summary>
        private static void ExistsFiles()
        {
            if (!System.IO.Directory.Exists(@$".\{IMAGE_DIR}")) {
                System.IO.Directory.CreateDirectory(IMAGE_DIR);
            }
            var files = new String[] { COMMENT_FILE, USER_FILE };
            foreach (var f in files) {
                if (!System.IO.File.Exists(f)) {
                    System.IO.File.CreateText(f);
                }
            }
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
            SetComment(SelectRndComment());   
        }

        private void SetComment(CommentInfo cmt)
        {
            this.LoopTimer.Stop();
            var items = this.CommentList.ItemsSource as List<CommentInfo>;
            // コメントは100個まで
            if (100 <= (items?.Count ?? 0)) {
                items?.RemoveAt(0);
            }
            items?.Add(cmt);
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
            var uIdx = -1;

            foreach (var line in readData) {
                uIdx++;
                var sp = line.Split(',');
                if(sp.Length <= 1) { continue; }
                var path = System.IO.Path.GetFullPath(@$".\{IMAGE_DIR}\{uIdx:000}.png");
                result.Add(new UserInfo(ReadImage(path), sp[0], sp[1] == "1"));
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
            var cmt = new CommentInfo(this.Comments[cIdx], this.Users[uIdx]);
            //var paySeed = this.Rnd.Next(0, 100);
            //if(95 <= paySeed) {
            //    cmt.SetPayColor(this.Rnd.Next(1, 100) * 100);
            //}
            return cmt;
        }

        /// <summary>
        /// 任意コメント書き込み
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendButton_Click(Object sender, RoutedEventArgs e)
        {
            var user = this.UserSelectBox.SelectedItem as UserInfo;
            if (user == null) { return; }
            var cmt = new CommentInfo(this.CommentBox.Text, user);
            if (Int32.TryParse(this.PayBox.Text, out var pay)) {
                cmt.SetPayColor(pay);
            }
            SetComment(cmt);
        }
    }
}
