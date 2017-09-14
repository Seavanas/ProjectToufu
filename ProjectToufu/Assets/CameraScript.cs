using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    public GameObject PlayerObject;

    public float CameraAcceleration = 2;
    public float currentAcceleration = 0.1f;
    public Vector3 Offset = new Vector3(-2, 0);

    public float xBoundS, xBoundL, yBoundS, yBoundL; 
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerObject == null)
        {
            PlayerObject = GameObject.FindGameObjectWithTag("Player");
        }
        if (PlayerObject == null)
        {
            //transform.position = new Vector3(0, 0, -10);
        }
        else
        {
            //print(Vector2.Distance(PlayerObject.transform.position, transform.position+Offset));
            
            Vector3 Camera = transform.position + Offset;
            if (Vector2.Distance(PlayerObject.transform.position, Camera) >= 0.2f)//if distance is too far
            {
                if (Mathf.Abs((PlayerObject.transform.position - Camera).x) > 0)
                {
                    float slope = (PlayerObject.transform.position - Camera).y / ((PlayerObject.transform.position - Camera).x);
                    //float b = Camera.y - slope * Camera.x;
                    Vector2 Option1 = new Vector2(0.1f, (slope * 0.1f));
                    if (Vector2.Distance(Option1 + (Vector2)transform.position, PlayerObject.transform.position) < Vector2.Distance(-Option1 + (Vector2)transform.position, PlayerObject.transform.position))
                        transform.position = new Vector3((Option1.normalized).x, (Option1.normalized).y) * Vector2.Distance(PlayerObject.transform.position, Camera)/30 + transform.position;
                    else transform.position = new Vector3((-Option1.normalized).x, (-Option1.normalized).y) * Vector2.Distance(PlayerObject.transform.position, Camera)/30 + transform.position;
                }

                else
                {
                    //Debug.Log(PlayerObject.transform.position + "," + Camera);
                    Vector2 Option1 = new Vector2(0, 1);
                    if (Vector2.Distance(Option1 + (Vector2)transform.position, PlayerObject.transform.position) < Vector2.Distance(-Option1 + (Vector2)transform.position, PlayerObject.transform.position))
                        transform.position = new Vector3((Option1.normalized * Vector2.Distance(PlayerObject.transform.position, Camera)/30).x, (Option1.normalized * Vector2.Distance(PlayerObject.transform.position, Camera)/30).y) + transform.position;
                    else transform.position = new Vector3((-Option1.normalized * Vector2.Distance(PlayerObject.transform.position, Camera)/30).x, (-Option1.normalized * Vector2.Distance(PlayerObject.transform.position, Camera)/30).y) + transform.position;
                }
            }
            
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xBoundS, xBoundL), Mathf.Clamp(transform.position.y, yBoundS, yBoundL), -10);
	}
}
