using UnityEngine;
using System.Collections;

public enum MoveCode{

	LEFT,
	RIGHT,
	UP,
	DOWN
}
public enum MoveState {

	Move,
	Idle
}
public class BallController : MonoBehaviour {
	public GameObject ball;
	private MoveState moveState;
	private MoveCode moveCode;
	void Awake () {
		moveState = MoveState.Idle;





	}
	void Update() {

	
	

	}

	IEnumerator Move(MoveCode moveCode) {
		moveState = MoveState.Move;
		Debug.Log ("Move Start");
		Vector3 dest = Vector3.zero;
		switch (moveCode) {
		case MoveCode.LEFT:
			dest = Vector3.left;
			break;
		case MoveCode.RIGHT:
			dest = Vector3.right;
			break;
		case MoveCode.UP:
			dest = Vector3.forward;
			break;
		case MoveCode.DOWN:
			dest = Vector3.back;
			break;

		}

		float speed = 20f;

		dest *= 2f;


		Vector3 orig = ball.transform.position;
		Vector3  newPos = Vector3.Scale(orig, new Vector3(1,0,1));
		Vector3 target = newPos + dest;



		float dist = (target - newPos).sqrMagnitude;
		float howClose = 0.05f;
		while (Vector3.kEpsilon * Vector3.kEpsilon < dist) {
			yield return null;

			orig = ball.transform.position;
			newPos = Vector3.Scale(orig, new Vector3(1,0,1)); 

			newPos = Vector3.Lerp(newPos , target, Time.deltaTime * speed);

			ball.transform.position = new Vector3 (newPos.x, orig.y, newPos.z);
			dist = (target - newPos).sqrMagnitude;
			Debug.Log (newPos);
			Debug.Log (target);
			Debug.Log (dist);
		}

		Debug.Log ("MOve DONE");
		ball.transform.position = new Vector3(target.x, ball.transform.position.y, target.z);
		moveState = MoveState.Idle;
	}
		
	public void Down(int moveCode) {
		if (moveState == MoveState.Move)
			return;
		
		this.moveCode = (MoveCode)moveCode;

		StartCoroutine (Move (this.moveCode));

	}

	public void Up() {
		moveState = MoveState.Idle;

	}

	public void Ttt(){

	}
	public void ten(){

	}
}
