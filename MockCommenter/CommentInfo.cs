using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MockCommenter
{
    public class CommentInfo
    {
        public BitmapImage UserIcon { get; }
        public String User { get; }
        public Boolean IsMember { get; }
        public String Comment { get; }
        public Int32 Pay { get; set; }

        public CommentInfo (BitmapImage icon, String comment, UserInfo user)
        {
            this.UserIcon = icon;
            this.User = user.Name;
            this.IsMember = user.IsMember;
            this.Comment = comment;
        }
    }
}
