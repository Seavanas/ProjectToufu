using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile2Script : MonoBehaviour
{
    bool exploding = false;
    private GameObject playerReference = null;
    public float turnSpeed, missileSpeed;
    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (playerReference == null)
        {
            playerReference = GameObject.FindGameObjectWithTag("Player");
        }
        if (playerReference != null && GetComponent<Animator>().GetInteger("State") == 2)
        {
            Vector2 difference = playerReference.transform.position - transform.position;
            float angle = Vector2.Angle(difference, transform.up);
            float tempAngle = angle;
            transform.Rotate(0, 0, angle);
            if (Vector2.Angle(transform.up, difference) > 0.1) // calculates angle between direction turret is facing and difference, accomodates for the information lost due to float rounding
            {
                //Debug.Log("Switching Sign since angle is "  + Vector2.Angle(transform.up, difference));
                angle *= -1;
            }
            transform.Rotate(0, 0, -tempAngle);

            transform.Rotate(0, 0, Mathf.Clamp(angle, -turnSpeed, turnSpeed));

            GetComponent<Rigidbody2D>().velocity = (transform.up/transform.up.magnitude) * missileSpeed;
        }
    }

    void AnimationEvent_Destroy()//Triggered by Explosion animation, deletes object when animation over
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !exploding)
        {
            exploding = true;
            gameObject.GetComponent<Mover>().SetSpeed(0);
            transform.localScale *= 3f;
        }
    }
}
