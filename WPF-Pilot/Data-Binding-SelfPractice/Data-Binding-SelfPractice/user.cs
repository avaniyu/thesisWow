using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Binding_SelfPractice
{
    public class User
    {
        private string name;
        private int rating;
        private DateTime memberSince;

        #region Property Getters and Setters
        public string Name
        {
            get { return this.name; }
        }
        public int Rating
        {
            get { return this.rating; }
            set { this.rating = value; }
        }
        public DateTime MemberSince
        {
            get { return this.memberSince; }
        }
        #endregion

        public User(string name, int rating, DateTime memberSince)
        {
            this.name = name;
            this.rating = rating;
            this.memberSince = memberSince;
        }
    }
}
