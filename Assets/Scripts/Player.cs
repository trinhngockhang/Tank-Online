using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public Rigidbody2D myBody;
    public string playerName;
    public Vector2 position;
    public string id;

    private void Start()
    {
        this.name = playerName;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "player")
        {
            myBody.velocity = new Vector2(0, 0);
            Debug.Log("da cham player");
        }
    }

}
