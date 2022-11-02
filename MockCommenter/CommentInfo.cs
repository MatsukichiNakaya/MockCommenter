using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MockCommenter
{
    public class CommentInfo
    {
        public BitmapImage? UserIcon { get; }
        public String User { get; }
        public Boolean IsMember { get; }
        public String Comment { get; }
        public Int32 Pay { get; set; }

        public SolidColorBrush Header { get; set; }
        public SolidColorBrush Body { get; set; }
        public SolidColorBrush FontColor { get; set; }

        public CommentInfo(String comment, UserInfo user)
        {
            this.UserIcon = user.UserIcon;
            this.User = user.Name;
            this.IsMember = user.IsMember;
            this.Comment = comment;
            this.Header = new SolidColorBrush(Colors.White);
            this.Body = new SolidColorBrush(Colors.White);
            this.FontColor = new SolidColorBrush(Colors.Black);
        }

        public void SetPayColor(Int32 pay)
        {
            this.Pay = pay;
            if (pay < 200) { // 青
                this.Header = new SolidColorBrush(Colors.DarkBlue);
                this.Body = new SolidColorBrush(Colors.Blue);
                this.FontColor = new SolidColorBrush(Colors.White);
                return;
            }
            if (pay < 500) {　// 水
                this.Header = new SolidColorBrush(Color.FromRgb(0, 206, 209));
                this.Body = new SolidColorBrush(Color.FromRgb(0, 255, 255));
                this.FontColor = new SolidColorBrush(Colors.Black);
                return;
            }
            if (pay < 1000) { // 黄緑
                this.Header = new SolidColorBrush(Color.FromRgb(0, 255, 127));
                this.Body = new SolidColorBrush(Color.FromRgb(124, 252, 0));
                this.FontColor = new SolidColorBrush(Colors.Black);
                return;
            }
            if (pay < 2000) { // 黄
                this.Header = new SolidColorBrush(Color.FromRgb(240, 230, 140));
                this.Body = new SolidColorBrush(Colors.Yellow);
                this.FontColor = new SolidColorBrush(Colors.Black);
                return;
            }
            if (pay < 5000) { // 橙
                this.Header = new SolidColorBrush(Color.FromRgb(255, 140, 0));
                this.Body = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                this.FontColor = new SolidColorBrush(Colors.White);
                return;
            }
            if (pay < 10000) { // マゼンタ
                this.Header = new SolidColorBrush(Color.FromRgb(199, 21, 133));
                this.Body = new SolidColorBrush(Colors.Magenta);
                this.FontColor = new SolidColorBrush(Colors.White);
                return;
            }
            this.Header = new SolidColorBrush(Color.FromRgb(220, 20, 60));
            this.Body = new SolidColorBrush(Colors.Red);
            this.FontColor = new SolidColorBrush(Colors.White);
        }
    }
}
