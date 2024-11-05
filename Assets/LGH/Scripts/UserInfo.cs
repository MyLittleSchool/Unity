using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GH
{
    [System.Serializable]
    public struct UserInfo
    {
        public string socialId;
        public string email;
        public string name;
        public string birthday;
        public bool gender;
        public string password;
        public List<string> interest;
        public int grade;
        public string phone;
        public int level;
        public int exp;
        public int maxExp;
    }

    [System.Serializable]
    public struct UserInfoArray
    {
        public List<UserInfoArray> data;
    }

}