using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
[System.Serializable]
public class Boundary {
    public float xMin, xMax, yMin, yMax;
}
*/
public class PlayerController : MonoBehaviour {
    //Movement stuff
    private Rigidbody2D rb;
    public float speed;
    private Vector2 movement;
    //public Boundary boundary;

    //Upgrade stuff
   // private SpriteRenderer sr;
   // public Sprite characterUpgrade1;

    //Shot stuff
    public GameObject shot;
    public int NumberOfShots;
    //public GameObject shotUpgrade1;
    public Transform[] shotSpawn;
    public float fireRate;
    private float nextFire;

    private BasePlayerScript basePlayerScript;
    private LevelController levelController;

    private bool onlyTriggerOnce = false;
    // private bool gameOver;

    // Use this for initialization
    void Start () {
        //gameOver = false;
        rb = GetComponent<Rigidbody2D>();
       // sr = GetComponent<SpriteRenderer>();
        basePlayerScript = GetComponent<BasePlayerScript>();
        levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();

        SaveInfoObject temp = SaveInfo.saveInfo.SaveObject;
        fireRate = temp.AttackFrequency;
        basePlayerScript.health = temp.MaxHealth;
        NumberOfShots = temp.AttackType;
        
	}
	
	// Update is called once per frame
	void Update () {
        if (basePlayerScript.health <= 0 && !onlyTriggerOnce)
        {
            levelController.playerKilled(gameObject);
            onlyTriggerOnce = true;
        }

        else if (Input.GetButton("Fire1") && Time.time > nextFire) {
            nextFire = Time.time + fireRate;
            for (int i = 0; i < NumberOfShots; i++)
            {
                Instantiate(shot, shotSpawn[i].position, shotSpawn[i].rotation);
            }
        }
        /*
        float x = Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax);
        float y = Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax);
        transform.position = new Vector2(x, y);
        print(transform.position);*/
    }

    void FixedUpdate() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        /*
        //Problem: Player can build up speed to go out of the border for a little
        if (((transform.position.x < boundary.xMin) && (moveHorizontal < 0)) ||     //Player is on the left border and trying to move left, or
            ((boundary.xMax < transform.position.x) && (0 < moveHorizontal)))       //Player is on the right border and trying to move right
            moveHorizontal = 0;     //Don't allow the horizontal movement

        if (((transform.position.y < boundary.yMin) && (moveVertical < 0)) ||       //Player is on the bottom border and trying to move down, or
            ((boundary.yMax < transform.position.y) && (0 < moveVertical)))         //Player is on the top border and trying to move up
            moveVertical = 0;       //Don't allow the vertical movement
            */
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb.velocity = movement * speed;
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Upgrade() {
        /*sr.sprite = characterUpgrade1;
        transform.localScale = new Vector2(0.3f, 0.3f);
        shot = shotUpgrade1;*/
    }
}
