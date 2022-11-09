using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MockCommenter
{
    public class CommentInfo
    {
		/// <summary>ユーザアイコン</summary>
        public BitmapImage? UserIcon { get; }
		/// <summary>ユーザ名</summary>
		public String User { get; }
		/// <summary>メンバーシップ加入</summary>
		public Boolean IsMember { get; }
		/// <summary>コメント本文</summary>
		public String Comment { get; }
		/// <summary>スパチャ額</summary>
		public UInt32 Pay { get; set; }
		/// <summary>スパチャ枠ヘッダ色</summary>
		public SolidColorBrush Header { get; set; }
		/// <summary>スパチャ枠本文色</summary>
		public SolidColorBrush Body { get; set; }
		/// <summary>スパチャ時の文字色</summary>
		public SolidColorBrush FontColor { get; set; }

        public CommentInfo(String comment, UserInfo user)
        {
            this.UserIcon = user.UserIcon;
            this.User = user.Name;
            this.IsMember = user.IsMember;
            this.Comment = comment;
            this.Header = new SolidColorBrush(Color.FromRgb(21, 100, 192));
			this.Body = new SolidColorBrush(Color.FromRgb(42, 119, 208));
			this.FontColor = new SolidColorBrush(Colors.White);
		}

		/// <summary>
		/// 投入額によるスパチャ色設定
		/// </summary>
		/// <param name="pay">スパチャ投入額</param>
        public void SetPayColor(UInt32 pay)
        {
            this.Pay = pay;
            if (pay < 200) { // 青
                this.Header = new SolidColorBrush(Color.FromRgb(21, 100, 192));
                this.Body = new SolidColorBrush(Color.FromRgb(42, 119, 208));
                this.FontColor = new SolidColorBrush(Colors.White);
                return;
            }
            if (pay < 500) {　// 水
                this.Header = new SolidColorBrush(Color.FromRgb(0, 184, 212));
                this.Body = new SolidColorBrush(Color.FromRgb(0, 229, 255));
                this.FontColor = new SolidColorBrush(Colors.Black);
                return;
            }
            if (pay < 1000) { // 黄緑
                this.Header = new SolidColorBrush(Color.FromRgb(1, 191, 165));
                this.Body = new SolidColorBrush(Color.FromRgb(29, 233, 182));
                this.FontColor = new SolidColorBrush(Colors.Black);
                return;
            }
            if (pay < 2000) { // 黄
                this.Header = new SolidColorBrush(Color.FromRgb(255, 178, 0));
                this.Body = new SolidColorBrush(Color.FromRgb(254, 202, 40));
                this.FontColor = new SolidColorBrush(Colors.Black);
                return;
            }
            if (pay < 5000) { // 橙
                this.Header = new SolidColorBrush(Color.FromRgb(230, 81, 1));
                this.Body = new SolidColorBrush(Color.FromRgb(242, 122, 5));
                this.FontColor = new SolidColorBrush(Colors.White);
                return;
            }
            if (pay < 10000) { // マゼンタ
                this.Header = new SolidColorBrush(Color.FromRgb(193, 30, 94));
                this.Body = new SolidColorBrush(Color.FromRgb(229, 38, 102));
                this.FontColor = new SolidColorBrush(Colors.White);
                return;
            }
			// 赤
            this.Header = new SolidColorBrush(Color.FromRgb(208, 0, 0));
            this.Body = new SolidColorBrush(Color.FromRgb(230, 33, 24));
            this.FontColor = new SolidColorBrush(Colors.White);
        }
    }
}
