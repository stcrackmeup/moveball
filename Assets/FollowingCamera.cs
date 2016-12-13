using UnityEngine;
using System.Collections;

namespace Com.MoveBall.Test {


public class FollowingCamera : MonoBehaviour {
	public Transform target;
	public float dist = 4.0f;
	public float height = 15.0f;
	public float dampRotate = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate() {
			Vector3 tr = target.position;
			transform.position = new Vector3 (tr.x, 10, tr.z);
			transform.LookAt (target);
//		float currYAngle = Mathf.LerpAngle(transform.eulerAngles.y,target.eulerAngles.y,dampRotate*Time.deltaTime);
//
//		Quaternion rot = Quaternion.Euler (0,currYAngle,0);
//
//		transform.position = target.position - (rot * Vector3.forward * dist);
//		transform.position = new Vector3(transform.position.x , height, transform.position.z);
//		transform.LookAt(target.position - Vector3.up * target.position.y);

	}
}


}