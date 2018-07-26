using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class OnlineUserController : MonoBehaviour {
    public SocketIOComponent socket;
    public LoginPanelController loginPanel;
    public OnlineUser onlineUser;
    // Use this for initialization
    void Start () {
        socket.On("LISTWAITING", getListWaiting);
    }

    void getListWaiting(SocketIOEvent data)
    {
        onlineUser.gameObject.SetActive(true);
        loginPanel.gameObject.SetActive(false);
        Debug.Log(data.data.GetField("client").ToString());
    }
}
