using UnityEngine;
using System.Collections;

public class BallIndependent : MonoBehaviour {

	bool firstCollision = false;
	Vector3 velocity;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnCollisionEnter(Collision coll) {


		if (!firstCollision) {
			velocity = GetComponent<Rigidbody>().velocity;
			firstCollision = true;
		}
		Rigidbody rb = GetComponent<Rigidbody> ();
		rb.velocity = Vector3.Scale(rb.velocity, new Vector3(1,0,1)) + new Vector3 (0, velocity.y, 0);

	}
}
