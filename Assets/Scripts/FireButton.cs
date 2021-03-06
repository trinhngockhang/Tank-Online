﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FireButton : MonoBehaviour {

    void Awake()
    {
        Button b = gameObject.GetComponent<Button>();
        b.onClick.AddListener(() => Controller.instance.playerCom.FireNormalBullet());
        b.onClick.AddListener(() => Controller.instance.playerFiretoSever());
    }
    private void Update()
    {
        if (Input.GetKey("space"))
        {
            Controller.instance.playerCom.FireNormalBullet();
            Controller.instance.playerFiretoSever();
        }
    }
}
