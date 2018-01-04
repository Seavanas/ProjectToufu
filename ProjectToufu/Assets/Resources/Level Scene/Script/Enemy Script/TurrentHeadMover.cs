using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentHeadMover : MonoBehaviour {
    public Transform player;
	void Update () {
        Vector3 difference = player.position - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
	}
}
