using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBall : MonoBehaviour {

    public GameObject ball;
    public Transform position;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            GameObject inst = Instantiate(ball);
            inst.transform.position = new Vector3(position.position.x, position.position.y +0.5f, position.position.z);
        }
	}
}
