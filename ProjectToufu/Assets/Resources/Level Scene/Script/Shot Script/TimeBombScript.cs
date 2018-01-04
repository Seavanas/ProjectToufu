using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeBombScriptNS
{
    public class TimeBombScript : MonoBehaviour
    {
        private Animator animator;

        public bool active;
        public int NumberOfShots;
        public float SpeedOfCountDown;//bad name
        public GameObject gameObjectThing;

        bool TriggerOnceOnly = false;
        public GameObject[] ListOfShots;


        public bool GetTriggerOnceOnly()
        {
            return TriggerOnceOnly;
        }

        void Start()
        {

            animator = gameObject.GetComponent<Animator>();
            animator.speed = SpeedOfCountDown;
            ListOfShots = new GameObject[NumberOfShots];
        }

        // Update is called once per frame
        void Update()
        {
            if (active)
            {
                animator.SetInteger("State", 1);
                if (!TriggerOnceOnly)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("Explosion"))
                    {
                        TriggerOnceOnly = true;
                        animator.speed = 1.8f;
                        gameObject.transform.localScale /= 2;

                        //Extract method
                        float angle = 0;
                        for (int i = 0; i < NumberOfShots; i++)
                        {
                            GameObject shotInstance = Instantiate(gameObjectThing, gameObject.transform.position, Quaternion.identity);//Inline Temp
                            ListOfShots[i] = shotInstance;


                            shotInstance.GetComponent<Mover>().SetSpeed(4);
                            angle = incrementAngle(angle);
                            shotInstance.transform.eulerAngles = new Vector3(0, 0, angle);

                        }

                    }
                }

            }
        }


        //takes a 0<= angle < 360 and turns into a normal vector (magnitude = 1)
        Vector2 angleToNormVector(float angle)
        {
            if (angle != 90 && angle != 270)
            {
                return new Vector2((angle >= 180 ? 1 : -1), Mathf.Tan(angle / 180 * Mathf.PI) * (angle >= 180 ? 1 : -1)).normalized;
            }
            else if (angle == 90)
            {
                Vector2 answer = new Vector2(0, 1);//Inline Temp
                return answer;
            }
            else
            {
                Vector2 answer = new Vector2(0, -1);//Inline Temp
                return answer;
            }
        }

        float incrementAngle(float angle)
        {
            return angle + (360 / NumberOfShots);
        }

        public void AnimationEvent_Destroy()
        {
            Destroy(gameObject);
        }
    }
}



/*
 * 
 */
