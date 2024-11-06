using SW;
using System.Collections.Generic;

namespace GH
{

    [System.Serializable]
    public struct UserInfo
    {
        public int id;
        public string socialId;
        public string name;
        public int grade;
        public string birthday;
        public bool gender;
        public string email;
        public string password;
        public string phone;
        public List<string> interest;
        public int schoolId;
        public int level;
        public int exp;
        public int maxExp;
        public School school;
    }

    //public struct Get

    [System.Serializable]
    public struct UserInfoData
    {
        public UserInfo data;
    }

}