using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : MonoBehaviour {
    private bool first = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.tag != "normalBullet")
        {
            if(first)
            {
                first = false;
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
    }
}
