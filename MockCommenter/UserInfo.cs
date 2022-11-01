using System;

namespace MockCommenter
{
    public class UserInfo
    {
        public String Name { get; set; }
        public Boolean IsMember { get; set; }

        public UserInfo(String name, Boolean isMember)
        {
            this.Name = name;
            this.IsMember = isMember;
        }
    }
}
