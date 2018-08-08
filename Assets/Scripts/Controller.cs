﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Text.RegularExpressions;

public class Controller : MonoBehaviour
{
    public LoginPanelController loginPanel;
    public JoyStickController joyStick;
    public SocketIOComponent socket;
    public CameraCotroller camera;
    public Player playGameobj;
    public GameObject Player2;
    public string namePlayer;
    public string myId;
    public string idPlayer2;
    public static Controller instance;
    void Start()
    {
        StartCoroutine(ConnectServer());
        socket.On("USER_CONNECTED", OnUserConnected);
        socket.On("PLAY", OnUserPlay);
        _makeInstance();
        socket.On("MOVE", onUserMove);
        socket.On("USER_DISCONNECTED", OnUserDisConnected);
        joyStick.gameObject.SetActive(false);
        loginPanel.playBtn.onClick.AddListener(OnClickPlayBtn);
        joyStick.onCommanMove += OnCommandMove;
    }
    void _makeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void OnCommandMove(Vector2 vec2,int angle)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        Vector2 position = new Vector2(vec2.x, vec2.y);
        data["position"] = position.x + "," + position.y;
        data["angle"] = angle.ToString();
        data["id"] = idPlayer2;
        data["id"] = data["id"].Remove(0, 1);
        data["id"] = data["id"].Remove(data["id"].Length - 1, 1);
        socket.Emit("MOVE", new JSONObject(data));
    }

    void onUserMove(SocketIOEvent obj)
    {
        Debug.Log("GEt the message server: " + obj + "user connected");
        Debug.Log("Position enemy: " + JsontoVector2(JsonToString(obj.data.GetField("position").ToString(), "\"")));
     //   GameObject player = GameObject.Find(JsonToString(obj.data.GetField("name").ToString(), "\" ")) as GameObject;
        //player.transform.position = JsontoVector2(JsonToString(obj.data.GetField("position").ToString(), "\""));
        Player2.transform.position = JsontoVector2(JsonToString(obj.data.GetField("position").ToString(), "\""));
        string s = obj.data.GetField("angle").ToString();
        s = s.Remove(0, 1);
        s = s.Remove(s.Length - 1, 1);
        int n = int.Parse(s);
        Debug.Log("s ne: " + s.Length);
        Player2.transform.eulerAngles = new Vector3(0, 0, n);
        Debug.Log("name move: "+ obj.data.GetField("name").ToString());
    }

    void OnClickPlayBtn()
    {
        if(loginPanel.inputField.text != "")
        {
            Debug.Log("da nhan button");
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["name"] = loginPanel.inputField.text;
            namePlayer = loginPanel.inputField.text;
            socket.Emit("GETUSER", new JSONObject(data));
        }
        else
        {
            loginPanel.inputField.text = "Please enter your name again";
        }
    }

    string JsonToString(string target,string s)
    {
        string[] newString = Regex.Split(target, s);
        return newString[1];
    }

    Vector2 JsontoVector2(string target)
    {
        Vector2 newVector;
        string[] newString = Regex.Split(target, ",");
        newVector = new Vector2(float.Parse(newString[0]), float.Parse(newString[1]));
        return newVector;
    }

    void OnUserDisConnected(SocketIOEvent obj)
    {
        Destroy(GameObject.Find(JsonToString(obj.data.GetField("name").ToString(), "\"")));
    }

    private void OnUserConnected(SocketIOEvent evt)
    {
        Debug.Log("GEt the message server: " + evt + "user connected") ;
        GameObject otherPlayer = GameObject.Instantiate(playGameobj.gameObject, playGameobj.position, Quaternion.identity) as GameObject;
        Player otherPlayCom = otherPlayer.GetComponent<Player>();
        otherPlayCom.playerName = JsonToString(evt.data.GetField("name").ToString(), "\"");
      //  otherPlayer.transform.position = JsontoVector2(JsonToString(evt.data.GetField("position").ToString(), "\""));
       // otherPlayCom.id = JsonToString(evt.data.GetField("id").ToString(), "\"");
        Player2 = otherPlayer;
        Debug.Log("position Player2 : " + Player2.transform.position);
    }
    private void OnUserPlay(SocketIOEvent evt)
    {
        Debug.Log("GEt the message server: " + evt + "userplay");
        loginPanel.gameObject.SetActive(false);
        joyStick.gameObject.SetActive(true);
        joyStick.ActionJoystick();
        Vector3 test = new Vector3(3, 0, 0);
        GameObject player = GameObject.Instantiate(playGameobj.gameObject, test, Quaternion.identity) as GameObject;
        Player playerCom = player.GetComponent<Player>();
        
        playerCom.playerName = JsonToString(evt.data.GetField("name").ToString(), "\"");
        joyStick.playerObject = player;
       /* camera.player = player;
        camera.offset = camera.transform.position - player.transform.position;
        camera.m_OrthographicCamera.orthographicSize = 1.2f; */
    }

    

    IEnumerator ConnectServer()
    {
        yield return new WaitForSeconds(0.2f);
       
        socket.Emit("USER_CONNECT");
        yield return new WaitForSeconds(0.2f);
      //  Dictionary<string, string> data = new Dictionary<string, string>(); ;
     //   data["name"] = "khangkhang";
     //   Vector3 temp = new Vector3(0, 3, 4);
       // data["position"] = temp.x + "," + temp.y + "," + temp.z;
       // socket.Emit("PLAY", new JSONObject(data));
    }
}
