using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRingScript : MonoBehaviour {
    public float speed;
    public float rotationZ;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        rotationZ += speed;
        rotationZ = rotationZ % 360;
	}
}
