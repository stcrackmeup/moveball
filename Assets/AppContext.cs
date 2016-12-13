using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;


public enum TileIntent {

	PLUS,
	MINUS
}
public class AppContext : MonoBehaviour {
	public int column = 5;
	public int row = 5;
	private List<Grid> gridList;
	public GameObject tilePrefab;

	public class GridFactory {
		public List<Grid> grids{ get; set;}

		private static GridFactory instance;

		public static GridFactory GetInstance() {
			if (instance == null) {
				instance = new GridFactory ();
				instance.InitGrid (4, 4);
			}

			return instance;
		}

		private GridFactory() {}

		private void InitGrid(int row, int column) {
			


			for (int i = 0; i < row; i++) {
				for (int j = 0; j < column; j++) {
					
				}

			}

		}


		public void ChangeCloseIntent(Grid criteria) {
			List<Grid> grids = GetCloseGrids (criteria);

			foreach (Grid grid in grids) {
				grid.TurnIntent ();


			}
		}


		public List<Grid> GetCloseGrids(Grid criteria) {

			return null;
		}
	}

	public class Tile : MonoBehaviour {
		AppContext ctx;
		TextMesh txtMesh;
		Grid grid;
		int num;
		void Awake() {
			ctx = GameObject.FindObjectOfType<AppContext> ();
			txtMesh = GetComponentInChildren<TextMesh> ();



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
		TileIntent tileIntent{get;set;}

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



	void Awake() {
		int targetScore = 31;

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



		int forward = idx + column;
		int back = idx - column;
		int right = idx + 1;
		int left = idx - 1;


		List<Grid> closeGrids = new List<Grid> ();


		int[] cr = {left,right,forward,back};
		List<int> excludeCond = new List<int> ();


		return closeGrids;
	}

	private Grid GetSimple(int idx) {
//		if (idx < 0 || idx >= (column * row))
//			return null;
		
		return gridList [idx];
	}






	

}
