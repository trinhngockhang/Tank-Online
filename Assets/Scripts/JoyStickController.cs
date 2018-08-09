using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickController : MonoBehaviour {
    public delegate void OnMove(Vector2 vector2,int angle);
    public event OnMove onCommanMove;
    public WachButton Left;
    public WachButton Right;
    public WachButton BackWard;
    public WachButton Forward;

    [HideInInspector]
    public GameObject playerObject;
    private Rigidbody2D myBody;
    public bool leftMove;
    public bool rightMove;
    public bool backMove;
    public bool frontMove;
    public bool fire;
    void Start()
    {
        // playerObject = new GameObject();
        
    }
    public void ActionJoystick()
    {
        Left.OnPress += OnPress;
        Right.OnPress += OnPress;
        BackWard.OnPress += OnPress;
        Forward.OnPress += OnPress;
    }
    void OnPress(GameObject unit,bool state)
    {
        switch (unit.name)
        {
            case "Left":
                LeftMove(state);
            break;
            case "Right":
                RightMove(state);
            break;
            case "Down":
                BackMove(state);
            break;
            case "Foward":
                FrontMove(state);
            break;
        }
        Debug.Log(unit.name);
    }

    private void LeftMove(bool state)
    {
        leftMove = state;
    }
    private void RightMove(bool state)
    {
        rightMove = state;
    }
    private void BackMove(bool state)
    {
        backMove = state;
    }
    private void FrontMove(bool state)
    {
        frontMove = state;
    }

    private void Update()
    {
        Transform tranf = playerObject.transform;
        myBody = playerObject.GetComponent<Rigidbody2D>() as Rigidbody2D; ;
        if (leftMove)
        {
            myBody.velocity = new Vector2(-6f , 0);
            tranf.eulerAngles = new Vector3(0, 0, 0);
            Debug.Log(myBody.velocity + "left");

            if(onCommanMove != null)
            {
                onCommanMove(playerObject.transform.position,0);
            }
        }
        else if (rightMove)
        {
            myBody.velocity = new Vector2(6f,0);
            tranf.eulerAngles = new Vector3(0, 0, 180);
            Debug.Log(myBody.velocity + "left");
            if (onCommanMove != null)
            {
                onCommanMove(playerObject.transform.position,180);
            }
        }
        else if (backMove)
        {
            myBody.velocity = new Vector2(0,-6f);
            tranf.eulerAngles = new Vector3(0, 0, 90);
            Debug.Log(myBody.velocity + "left");
            if (onCommanMove != null)
            {
                onCommanMove(playerObject.transform.position,90);
            }
        }
        else if (frontMove)
        {
            myBody.velocity = new Vector2(0, 6f);
            Debug.Log(myBody.velocity + "left");
            tranf.eulerAngles = new Vector3(0, 0, 270);
            if (onCommanMove != null)
            {
                onCommanMove(playerObject.transform.position,270);
            }
        }
        else
        {
            myBody.velocity = new Vector2(0, 0);
        }
        
    }
}
