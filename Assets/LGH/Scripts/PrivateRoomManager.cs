using GH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivateRoomManager : MonoBehaviour
{
    public static PrivateRoomManager instance;
    public List<PrivateRoom> privateRooms;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    void Start()
    {

    }

    void Update()
    {

    }
}
