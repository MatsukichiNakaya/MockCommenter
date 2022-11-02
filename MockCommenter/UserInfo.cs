using System;
using System.Windows.Media.Imaging;

namespace MockCommenter
{
    public class UserInfo
    {
        public BitmapImage? UserIcon { get; }
        public String Name { get; set; }
        public Boolean IsMember { get; set; }

        public UserInfo(BitmapImage? icon, String name, Boolean isMember)
        {
            this.UserIcon = icon;
            this.Name = name;
            this.IsMember = isMember;
        }
    }
}
