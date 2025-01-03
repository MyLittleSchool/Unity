using SW;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;

namespace GH
{

    [System.Serializable]
    public struct UserInfo
    {
        //юс╫ц..?(╠т)
        public string username;

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
        public string statusMesasge;
        public int schoolId;
        public int level;
        public int exp;
        public int maxExp;
        public bool isOnline;
        public int mapId;
        public string mapType;
        public School school;
    }

    //public struct Get

    [System.Serializable]
    public struct UserInfoData
    {
        public UserInfo data;
    }

}