using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System.Text.RegularExpressions;
using UnityEngine.UI;
public class Controller : MonoBehaviour
{
    public LoginPanelController loginPanel;
    public JoyStickController joyStick;
    public SocketIOComponent socket;
    public Text textNamePlayer1;
    public Text textNamePlayer2;
    public HealthBar HealthBar1;
    public HealthBar HealthBar2;
    public CameraCotroller camera;
    public Player playGameobj;
    public Player playerCom;
    public Player otherPlayCom;
    public bool firstPlayerinRoom;
    public GameObject Player2;
    public string namePlayer;
    public string myId;
    public string idPlayer2;
    public static Controller instance;
    Vector2 spawnPositionFirst = new Vector2(-3, 0);
    Vector2 spawnPositionSecond = new Vector2(5, 4);
    Vector2 temp;
    void Start()
    {
        StartCoroutine(ConnectServer());
        socket.On("USER_CONNECTED", OnUserConnected);
        socket.On("PLAY", OnUserPlay);
        _makeInstance();
        socket.On("MOVE", onUserMove);
        socket.On("USER_DISCONNECTED", OnUserDisConnected);
        socket.On("OTHERPLAYERFIRE", otherPlayerFire);
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
        otherPlayCom.direct = n;
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
            socket.On("LISTWAITING", OnlineUserController.instance.getListWaiting);
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

    private void otherPlayerFire(SocketIOEvent obj)
    {
        otherPlayCom.FireNormalBullet();
    }

    private void OnUserConnected(SocketIOEvent evt)
    {
        if (firstPlayerinRoom)
        {
            temp = spawnPositionSecond;
        }
        else
        {
            temp = spawnPositionFirst;
        }       
        Debug.Log("GEt the message server: " + evt + "user connected") ;
        GameObject otherPlayer = GameObject.Instantiate(playGameobj.gameObject, temp, Quaternion.identity) as GameObject;
        otherPlayCom = otherPlayer.GetComponent<Player>();
        otherPlayCom.playerName = JsonToString(evt.data.GetField("name").ToString(), "\"");
        //  otherPlayer.transform.position = JsontoVector2(JsonToString(evt.data.GetField("position").ToString(), "\""));
        // otherPlayCom.id = JsonToString(evt.data.GetField("id").ToString(), "\"");
        otherPlayCom.setName(!firstPlayerinRoom, textNamePlayer1, textNamePlayer2,HealthBar1,HealthBar2);
        Player2 = otherPlayer;
        Debug.Log("position Player2 : " + Player2.transform.position);
    }
    private void OnUserPlay(SocketIOEvent evt)
    {
        if (!firstPlayerinRoom)
        {
            temp = spawnPositionSecond;
        }
        else
        {
            temp = spawnPositionFirst;
        }
        Debug.Log("GEt the message server: " + evt + "userplay");
        loginPanel.gameObject.SetActive(false);
        joyStick.gameObject.SetActive(true);
        joyStick.ActionJoystick();
        GameObject player = GameObject.Instantiate(playGameobj.gameObject, temp, Quaternion.identity) as GameObject;
        playerCom = player.GetComponent<Player>();
        playerCom.playerName = JsonToString(evt.data.GetField("name").ToString(), "\"");
        // playerCom.setName();        
        joyStick.playerObject = player;
        playerCom.setName(firstPlayerinRoom, textNamePlayer1, textNamePlayer2, HealthBar1, HealthBar2);
    }
    public void playerFiretoSever()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["enemyid"] = idPlayer2;
        data["enemyid"] = data["enemyid"].Remove(0, 1);
        data["enemyid"] = data["enemyid"].Remove(data["enemyid"].Length - 1, 1);
        socket.Emit("PLAYERFIRE", new JSONObject(data));
    }

    IEnumerator ConnectServer()
    {
        yield return new WaitForSeconds(0.2f);
       
        socket.Emit("USER_CONNECT");
        yield return new WaitForSeconds(0.2f);
    }
}
