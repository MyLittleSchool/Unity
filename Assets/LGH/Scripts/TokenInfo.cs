using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TokenInfo
{
    public string userEmail;
    public string accessToken;
    public string refreshToken;
    public string password;
}

[System.Serializable]
public struct TokenData
{
    public TokenInfo data;
}

