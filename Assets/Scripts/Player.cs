using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public string playerName;
    public Vector2 position;
    public string id;

    private void Start()
    {
        this.name = playerName;
    }

}
