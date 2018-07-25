using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickController : MonoBehaviour {
    public delegate void OnMove(Vector2 vector2);
    public event OnMove onCommanMove;
    public WachButton Left;
    public WachButton Right;
    public WachButton BackWard;
    public WachButton Forward;

    [HideInInspector]
    public GameObject playerObject;

    public bool leftMove;
    public bool rightMove;
    public bool backMove;
    public bool frontMove;
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
        if (leftMove)
        {
            playerObject.transform.position = new Vector2(tranf.position.x - 6f * Time.deltaTime, tranf.position.y);
            if(onCommanMove != null)
            {
                onCommanMove(playerObject.transform.position);
            }
        }
        if (rightMove)
        {
            playerObject.transform.position = new Vector2(tranf.position.x + 6f * Time.deltaTime, tranf.position.y);
            if (onCommanMove != null)
            {
                onCommanMove(playerObject.transform.position);
            }
        }
        if (backMove)
        {
            playerObject.transform.position = new Vector2(tranf.position.x , tranf.position.y - 6f * Time.deltaTime);
            if (onCommanMove != null)
            {
                onCommanMove(playerObject.transform.position);
            }
        }
        if (frontMove)
        {
            playerObject.transform.position = new Vector2(tranf.position.x, tranf.position.y + 6f * Time.deltaTime);
            if (onCommanMove != null)
            {
                onCommanMove(playerObject.transform.position);
            }
        }
    }
}
