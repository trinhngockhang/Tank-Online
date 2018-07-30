using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class OnlineUserController : MonoBehaviour {
    public static OnlineUserController instance;
    public SocketIOComponent socket;
    public LoginPanelController loginPanel;
    public OnlineUser onlineUser;
    public User user;
    public FightPanel fightPanel;
    private string nameEnemy;
    private string idEnemy;
    // Use this for initialization
    void Start () {
        _makeInstance();
        socket.On("LISTWAITING", getListWaiting);
        socket.On("BEFIGHT", beFight);
        socket.On("OTHERPLAYOK", otherPlayerOk);
    }

    void _makeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void otherPlayerOk(SocketIOEvent data)
    {
        Debug.Log("thang kia dong y r");
        fightPanel.gameObject.SetActive(false);
        onlineUser.gameObject.SetActive(false);
        Dictionary<string, string> dataSend = new Dictionary<string, string>();
        dataSend["name"] = Controller.instance.namePlayer;
        Vector2 position = new Vector2(0, 0);
        dataSend["position"] = position.x + "," + position.y;
        dataSend["enemyid"] = Controller.instance.idPlayer2;
        dataSend["enemyid"] = dataSend["enemyid"].Remove(0, 1);
        dataSend["enemyid"] = dataSend["enemyid"].Remove(dataSend["enemyid"].Length - 1, 1);
        Debug.Log("id enemy gui len: " + dataSend["enemyid"]);
        socket.Emit("PLAY", new JSONObject(dataSend));
    }

    void beFight(SocketIOEvent data)
    {
        Controller.instance.idPlayer2 = data.data.GetField("enemyid").ToString();
        Text[] newText;
        newText = fightPanel.GetComponentsInChildren<Text>();
        Debug.Log("vao day");
        fightPanel.gameObject.SetActive(true);
        Debug.Log(data);
        newText[0].text = data.data.GetField("name").ToString();
    }

    void getListWaiting(SocketIOEvent data)
    {
        onlineUser.gameObject.SetActive(true);
        loginPanel.gameObject.SetActive(false);
        // Debug.Log("id " + socket);
        //Debug.Log(data.data.GetField("client").ToString());
        // string test = data.data.GetField("client")[0].GetField("name").ToString();
        //Debug.Log(data.data.GetField("length"));
        // Debug.Log(test);
        Controller.instance.myId = data.data.GetField("userid").ToString();
        Debug.Log("my id is : " + Controller.instance.myId);
        int n = int.Parse(data.data.GetField("length").ToString());
        int m = 0;
        for (int i = 1;i <= n ; i++)
         {
             Vector2 temp = new Vector2(640, 360 - i * 120 );
             user = Instantiate(user, temp, Quaternion.identity);
             user.transform.parent = onlineUser.transform;
             Text textUser = user.GetComponentInChildren(typeof(Text)) as Text;
             string s = data.data.GetField("client")[i - 1].GetField("name").ToString();
             idEnemy = data.data.GetField("client")[i - 1].GetField("id").ToString();
             s = s.Remove(0, 1);
             s = s.Remove(s.Length - 1, 1);
            textUser.text = s;
             ButtonFight fightButton = user.GetComponentInChildren(typeof(ButtonFight)) as ButtonFight;
             fightButton.id = idEnemy;
        } 
    }

    public void sayNo()
    {
        fightPanel.gameObject.SetActive(false);
    }

    public void sayYes()
    {
        onlineUser.gameObject.SetActive(false);
        fightPanel.gameObject.SetActive(false);
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["name"] = Controller.instance.namePlayer;
        Vector2 position = new Vector2(1, 3);
        data["position"] = position.x + "," + position.y;
        data["enemyid"] = Controller.instance.idPlayer2;      
        socket.Emit("AGREE",new JSONObject(data["enemyid"]));
        data["enemyid"] = data["enemyid"].Remove(0, 1);
        data["enemyid"] = data["enemyid"].Remove(data["enemyid"].Length - 1, 1);
        socket.Emit("PLAY", new JSONObject(data));
    }

    public void sendRq(string idEnemy,string nameUser)
    {
        Controller.instance.idPlayer2 = idEnemy;
        Dictionary<string, string> rq = new Dictionary<string, string>();
        idEnemy = idEnemy.Remove(0,1);
        idEnemy = idEnemy.Remove(idEnemy.Length-1, 1);
        rq["enemyid"] = idEnemy;
        rq["myid"] = Controller.instance.myId;
        rq["myid"] = rq["myid"].Remove(0, 1);
        rq["myid"] = rq["myid"].Remove(rq["myid"].Length - 1, 1);
        rq["name"] = nameUser;
        Debug.Log("enemyid ne: " + rq["enemyid"]);
        Debug.Log("myid ne: " + rq["myid"]);
        Debug.Log("name ne: " + rq["name"]);
        socket.Emit("SENDREQUEST", new JSONObject(rq));
    }
}
