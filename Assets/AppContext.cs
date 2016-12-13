using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public enum TileIntent {

	PLUS,
	MINUS
}
public class AppContext : MonoBehaviour {
	public int column = 5;
	public int row = 5;
	private List<Grid> gridList;
	public GameObject tilePrefab;
	public static AppContext instance;
	public Text moveT;
	public Text scoreT;
	public Text targetScoreT;
	public int move { get; set; }
	public int score { get; set; }
	public int targetScore{ get; set; }

	public void DidMove(int num, TileIntent intent) {
		ShowText ("남은 횟수", --move, moveT);
		ChangeScore (num, intent);

		if (ApproachTargetScore ()) {
			// game win
		}

		if (isGameOverSituation ())
			DidGameOver ();		
	}

	public void DidGameOver() {


	}

	public void ShowText(string pre, int val, Text wh) {
		wh.text = pre + " " + val;
	}

	public bool isGameOverSituation() {
		return 0 >= move;
	}

	public bool ApproachTargetScore() {
		if (score == targetScore)
			return true;
		return false;
	}

	public void ChangeScore(int num, TileIntent intent) {
		this.score += intent == TileIntent.PLUS ? num : -num;
		ShowText ("/", score, scoreT);
	}



	public void ShowGameOverScreen() {
		
	}
		
	void Awake() {
		if (instance == null) {
			instance = this;
		}
		else if (instance != this) {
			Destroy (this);

		}

		moveT = GameObject.Find ("Move").GetComponent<Text> ();
		scoreT = GameObject.Find ("Score").GetComponent<Text> ();
		targetScoreT= GameObject.Find ("TargetScore").GetComponent<Text> ();


		move = 10;
		score = 0;
		targetScore = 31;

		ShowText ("남은 횟수", move, moveT);
		ShowText ("/", score, scoreT);
		ShowText ("정답", targetScore, targetScoreT);



		InitGrid (row, column);
	}

	void OnEnable() {

	}
	// Update is called once per frame
	void Update () {
	
	}


	// each tiles num
	// each tile red or blue 
	// 
	private void InitGrid(int row, int column) {
		int min = 1;
		int max = 45;
		int tileSize = 2;
		float tileHeight = 0f;
		gridList = new List<Grid> ();

		for (int j = 0; j < column; j++) {
			for (int i = 0; i < row; i++) {
				int num = Random.Range (min, max + 1);
				TileIntent intent = (TileIntent)Random.Range(0, Enum.GetValues (typeof(TileIntent)).Length);
				Vector3 pos = new Vector3 (i * tileSize, tileHeight, j * tileSize);

				Debug.Log ("Installing Tiles... ");
				Debug.Log (num + ":::" + intent + ":::" + pos);
				Debug.Log ("Finished Instailling Tiles... ");
				GameObject tr = Instantiate (tilePrefab, pos, Quaternion.identity) as GameObject;

				gridList.Add( new Grid (tr.transform, num, intent) );

			}

		}

	}


	public void ChangeCloseIntent(Grid criteria) {
		List<Grid> grids = GetCloseGrids (criteria);
		criteria.TurnIntent ();
		foreach (Grid grid in grids) {
			if (grid != null) {
				grid.TurnIntent ();

			}


		}
	}


	public List<Grid> GetCloseGrids(Grid criteria) {
		Debug.Log (gridList);
		int idx = gridList.IndexOf (criteria);

		int j = idx % column;
		int i = idx / column;

		Debug.Log ("Your touching idx ::: " + idx);

		int forward = CalculatePlusShape (i + 1, j);
		int back = CalculatePlusShape (i - 1, j);
		int right = CalculatePlusShape (i, j + 1);
		int left = CalculatePlusShape (i, j -1);

		int[] lst = { forward, back, right, left };

		List<Grid> closeGrids = new List<Grid> ();

		foreach (int d in lst) {
			if(d!= -1)
				closeGrids.Add(GetSimple (d));
		}


		return closeGrids;
	}

	private int CalculatePlusShape(int i , int j) {
		if (i < 0 || i >= row || j < 0 || j >= column)
			return -1;
		return CalculateIdxFromArray (i, j);
			
	}


	private int CalculateIdxFromArray(int i, int j) {
		return i * row + j;
	}

	private Grid GetSimple(int idx) {
//		if (idx < 0 || idx >= (column * row))
//			return null;
		
		return gridList [idx];
	}




	public class Tile : MonoBehaviour {
		AppContext ctx;
		TextMesh txtMesh;
		Grid grid;

		public Text move;

		int num;
		void Awake() {
			ctx = GameObject.FindObjectOfType<AppContext> ();
			txtMesh = GetComponentInChildren<TextMesh> ();
			move = GameObject.Find ("Move").GetComponent<Text> ();

		}

		public void Init(Grid grid, int num) {
			this.grid = grid;
			this.num = num;
			ChangeNumDisplay (num);
		}

		private void ChangeNumDisplay(int num) {
			txtMesh.text = num + "";
		}

		void OnCollisionEnter(Collision coll) {
			if (coll.gameObject.CompareTag ("ball")) {
				ctx.ChangeCloseIntent (grid);

				AppContext.instance.DidMove (grid.num, grid.tileIntent);

			}
			foreach (ContactPoint contact in coll.contacts) {
				Debug.DrawRay (contact.point, contact.normal, Color.white);
			}

			//			if (coll.relativeVelocity.magnitude > 2)
			//				GetComponent<AudioSource>().Play ();




		}
	}


	[Serializable]
	public class Grid {
		public Transform tr {get;set;}
		public Vector3 pos {get;set;}
		public int num {get;set;}
		public TileIntent tileIntent{get;set;}

		public Grid(Transform tr, int num, TileIntent tileIntent) {
			this.tr = tr;
			tr.gameObject.AddComponent<Tile>();
			tr.GetComponent<Tile>().Init(this, num);
			this.num = num;
			this.pos = tr.position;

			TurnIntent(tileIntent);

		}

		public void TurnIntent() {

			if(tileIntent == TileIntent.PLUS) {
				TurnIntent (TileIntent.MINUS);


			} else {
				TurnIntent (TileIntent.PLUS);

			}

		}

		private void TurnIntent(TileIntent tileIntent ) {
			this.tileIntent = tileIntent;
			switch (tileIntent) {
			case TileIntent.PLUS:
				this.tr.GetComponent<MeshRenderer> ().material.color = Color.red;
				break;
			case TileIntent.MINUS:
				this.tr.GetComponent<MeshRenderer> ().material.color = Color.blue;
				break;
			}
		}





	}


	

}
