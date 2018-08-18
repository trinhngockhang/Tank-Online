using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour {
    public Rigidbody2D myBody;
    public string playerName;
    public Vector2 position;
    public string id;
    public int direct = 0; // director of tank,1234 up right down left
    public NormalBullet normalbulletUp;
    public NormalBullet normalbulletRight;
    public NormalBullet normalbulletDown;
    public NormalBullet normalbulletLeft;
    public static Player instance;
    private float health = 100f;
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;
    public bool enoughTime = true;
    private bool firstPlayer;
    private Text nameUser;
   
    private HealthBar HealthImage;
    

    public void setName(bool first,Text textPlayer1,Text Player2,HealthBar HealthBar1,HealthBar HealthBar2)
    {
        Debug.Log("la ng dau toen : " + first);
        Debug.Log("ten la " + playerName);
        //firstPlayer = first;
        if (first)
        {
            Debug.Log("da vao first la ng dau tien");
            nameUser = textPlayer1;
            HealthImage = HealthBar1;
        }
        else
        {
            Debug.Log("da vao day deo phai ng dau ten");
            nameUser = Player2;
            HealthImage = HealthBar2;
        }
        nameUser.text = playerName;
        Debug.Log("ten la " + playerName);
    }

    public float getHealth()
    {
        return health;
    }

    private void Start()
    {
        this.name = playerName;
        _makeInstance();
    }

    void _makeInstance()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "player")
        {
            myBody.velocity = new Vector2(0, 0);
            Debug.Log("da cham player");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "normalBulletReady") 
        {
            beFirebyNormalBulletEnemy();
        }
    }

    public void beFirebyNormalBulletEnemy()
    {
        if(health > 0)
        {
            health -= 20;
            Debug.Log(playerName + " bi dinh dan,con lai: " + health);
            HealthImage.transform.localScale = new Vector2(health / 100, 1);
        }        
    }

    public void FireNormalBullet()
    {

       
        Debug.Log("fire " + direct);
        float velocityBullet = 6f;
        if (enoughTime)
        {
            nextFire = Time.time + fireRate;
            //up
            if (direct == 270)
            {
                GameObject bulletUp = GameObject.Instantiate(normalbulletUp.gameObject, myBody.position, Quaternion.identity);
                Rigidbody2D bulletUpBody = bulletUp.GetComponent<Rigidbody2D>();
                bulletUpBody.velocity = new Vector2(0, velocityBullet);
            }
            else if (direct == 180) // right
            {
                GameObject bulletRight = GameObject.Instantiate(normalbulletRight.gameObject, myBody.position, Quaternion.identity);
                Rigidbody2D normalbulletRightBody = bulletRight.GetComponent<Rigidbody2D>();
                normalbulletRightBody.velocity = new Vector2(velocityBullet, 0);
            }
            else if (direct == 90) //down
            {
                GameObject bulletDown = GameObject.Instantiate(normalbulletDown.gameObject, myBody.position, Quaternion.identity);
                Rigidbody2D normalbulletDownBody = bulletDown.GetComponent<Rigidbody2D>();
                normalbulletDownBody.velocity = new Vector2(0, -velocityBullet);
            }
            else if (direct == 0) //left
            {
                GameObject bulletLeft = GameObject.Instantiate(normalbulletLeft.gameObject, myBody.position, Quaternion.identity);
                Rigidbody2D normalbulletLeftBody = bulletLeft.GetComponent<Rigidbody2D>();
                normalbulletLeftBody.velocity = new Vector2(-velocityBullet, 0);
            }
        }
       
    }

    private void Update()
    {

        if ( Time.time > nextFire)
        {
            enoughTime = true;
        }
        else
        {
            enoughTime = false;
        }
        if (myBody.velocity.x > 0)
        {
            direct = 180;
            
        }
        else if(myBody.velocity.x < 0)
        {
            direct = 0;
        }
        else if (myBody.velocity.y > 0)
        {
            direct = 270;
        }
        else if(myBody.velocity.y < 0)
        {
            direct = 90;
        }
    }

}
