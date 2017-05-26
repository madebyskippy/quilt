using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class arcs : MonoBehaviour {

	//color all squares without touching any black dots 

	[SerializeField] Vector2 startp;

	[SerializeField] GameObject l;
	[SerializeField] GameObject circ;
	[SerializeField] GameObject poly;
	[SerializeField] Material m;

	[SerializeField] GameObject mouse;
	[SerializeField] Text text;

	bool[,] ishit;
	GameObject[,] colliders;
	GameObject[,] circles;
	Vector2[,] grid;
	Vector2[,] stable;
	Vector2[,] changes;
	int r = 20;
	int c = 20;

	float increment = 0.01f; //change this to speed up the warping

	LineRenderer[] lr_r;
	LineRenderer[] lr_c;
	Vector3[][] rows;
	Vector3[][] cols;

	int count;

	bool gameover;
	bool win;
	bool start;

	float score;

	float downtime = 5*60; //will reset after 5 minutes
	float downtimer;

	// Use this for initialization
	void Start () {
		downtimer = 0;
		score = 0;
		count = 0;
		text.text = "use the mouse to touch each space\n\n0";
		gameover = false;
		win = false;
		start = true;
		grid = new Vector2[r, c];
		stable = new Vector2[r, c];
		changes = new Vector2[r, c];
		circles = new GameObject[r, c];
		colliders = new GameObject[r-1, c-1];
		ishit = new bool[r - 1, c - 1];
		lr_r = new LineRenderer[r];
		lr_c = new LineRenderer[c];
		rows = new Vector3[r][];
		cols = new Vector3[c][];
		for (int i = 0; i < r; i++) {
			GameObject t = Instantiate(l);
			rows [i] = new Vector3[c];
			lr_c [i] = t.GetComponent<LineRenderer> ();

//			GameObject t1 = Instantiate(l);
//			cols [i] = new Vector3[r];
//			lr_r [i] = t1.GetComponent<LineRenderer> ();

			for (int j = 0; j < c; j++) {
				grid [i, j] = new Vector2 (startp.x + i*0.5f, startp.y - j*0.5f);
				stable[i,j] = new Vector2 (startp.x + i*0.5f, startp.y - j*0.5f);
				changes [i, j] = Vector2.zero;
				circles[i,j] = Instantiate (circ, grid [i, j], Quaternion.identity);
				if (i > 0 && j < c - 1) {
					colliders [i - 1, j] = Instantiate (poly, grid [i, j], Quaternion.identity);
					colliders [i - 1, j].GetComponent<arcpoly> ().setP (grid [i, j]);
					ishit[i-1,j] = false;
				}

				lr_c [i].SetPosition (j,new Vector3(grid[i,j].x,grid[i,j].y,0f));
				rows [i][j] = grid[i,j];

//				lr_r [i].SetPosition (j,new Vector3(grid[j,i].x,grid[j,i].y,0f));
//				cols [i][j] = grid [j,i];
			}
		}

		for (int j = 0; j < c; j++) {
			GameObject t = Instantiate(l);
			cols [j] = new Vector3[r];
			lr_r [j] = t.GetComponent<LineRenderer> ();
			for (int i = 0; i < r; i++) {
				lr_r [j].SetPosition (i,new Vector3(grid[i,j].x,grid[i,j].y,0f));
				cols [j][i] = grid [i, j];
			}
		}

		adjustpoly ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Space)) {
			SceneManager.LoadScene ("test");
		}if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		if (Input.GetMouseButtonDown (0)) {
			if (start) {
				start = false;
			}
			if (gameover || win) {
				SceneManager.LoadScene ("test");
			}
		}



		Vector3 temp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		temp.z = 0;
		if (temp.x != mouse.transform.position.x && temp.y != mouse.transform.position.y) {
			//controls were touched
//			Debug.Log(temp.x +" = "+mouse.transform.position.x+", "+temp.y+" = "+mouse.transform.position.y);
			downtimer = 0;
		}
		if (!start) {
			mouse.transform.position = temp;
		}


		if (!gameover && !win && !start) {

			warp ();

			score += Time.deltaTime;
			text.text = "use the mouse to touch each space\n\n" + ((int)score).ToString();

		}
		if (start) {
			text.text = "use the mouse to touch each space\n\nclick to start";
		}
		if (gameover) {
			text.text = "don't touch the intersections.\n\nclick to try again";
		}
		if (win) {
			text.text = "amazing. do it again\n\n" + ((int)score).ToString ();
		}
		if (gameover || win || start) {
			jiggle();
		}

		showlines ();
		placecirc ();
		adjustpoly ();

		downtimer += Time.deltaTime;
		if (downtimer > downtime) {
			SceneManager.LoadScene ("test");
		}
	}

	void jiggle(){
		float threshold = 0.05f;
		for (int j = 0; j < c; j++) {
			for (int i = 0; i < r; i++) {
				float x, y;
				if (grid [i, j].x - stable [i, j].x > threshold) {
					x = Random.Range (-1f, 0) * increment;
				} else if (stable [i, j].x - grid [i, j].x > threshold) {
					x = Random.Range (0, 1f) * increment;
				} else {
					x = Random.Range (-1f, 1f) * increment;
				}
				if (grid [i, j].y - stable [i, j].y > threshold) {
					y = Random.Range (-1f, 0) * increment;
				} else if (stable [i, j].y - grid [i, j].y > threshold) {
					y = Random.Range (0, 1f) * increment;
				} else {
					y = Random.Range (-1f, 1f) * increment;
				}
				changes [i, j] = new Vector2 (x, y);
				edit (i, j, grid [i, j] + changes[i,j]);

			}
		}
	}
	void warp(){
		for (int j = 0; j < c; j++) {
			for (int i = 0; i < r; i++) {
//				changes [i, j] = Vector2.one * Random.Range (-1f, 1f) * increment;
				changes[i,j] = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f)) * increment;
				edit (i, j, grid [i, j] + changes[i,j]);

			}
		}
	}

	void edit(int x, int y, Vector2 p){
		grid [x, y] = p;
		rows [x] [y] = grid [x,y];
		cols [y] [x] = grid [x,y];
	}

	void showlines(){
		for (int i = 0; i < r; i++) {
			lr_c [i].SetPositions (rows [i]);
			lr_r [i].SetPositions (cols [i]);
		}
	}

	void placecirc(){
		for (int j = 0; j < c; j++) {
			for (int i = 0; i < r; i++) {
				circles[i,j].transform.position=grid[i,j];
			}
		}
	}

	void adjustpoly(){
		for (int i = 1; i < r; i++) {
			for (int j = 0; j < c-1; j++) {
				GameObject o = colliders [i - 1, j];
				Vector2[] v2;
				Vector3[] v3;
				v2 = o.GetComponent<PolygonCollider2D> ().points;

				v2 [0] = o.transform.InverseTransformPoint(grid [i,	j]);//-Vector2.one;
				v2 [1] = o.transform.InverseTransformPoint(grid [i,		j+1]	);//-Vector2.one;
				v2 [2] = o.transform.InverseTransformPoint(grid [i-1,		j+1]);//-Vector2.one;
				v2 [3] = o.transform.InverseTransformPoint(grid [i-1,	j]	);//-Vector2.one;

				o.GetComponent<PolygonCollider2D> ().points = v2;
				v3 = o.GetComponent<MeshFilter> ().mesh.vertices;
				v3 [0] = o.transform.InverseTransformPoint((Vector3)grid [i-1,	j+1]+new Vector3(0,0,1f));//-Vector3.one;
				v3 [1] = o.transform.InverseTransformPoint((Vector3)grid [i, 	j]+new Vector3(0,0,1f));//-Vector3.one;
				v3 [2] = o.transform.InverseTransformPoint((Vector3)grid [i,	j+1]+new Vector3(0,0,1f));//-Vector3.one;
				v3 [3] = o.transform.InverseTransformPoint((Vector3)grid [i-1,	j]+new Vector3(0,0,1f));//-Vector3.one;
				o.GetComponent<MeshFilter> ().mesh.vertices = v3;
				o.GetComponent<MeshFilter> ().mesh.triangles = new int[] {0,1,2,1,0,3};
			}
		}
	}

	public void hit(bool corner, Collider2D col = null){
		if (!gameover && !win){
			if (corner) {
				Debug.Log ("hit");
				col.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
				col.gameObject.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
				col.gameObject.transform.GetChild (0).gameObject.SetActive (true);
				if (!gameover) {
					Camera.main.backgroundColor = new Color (0.68f,0.68f,0.68f);
					gameover = true;
					whitelines ();
				}
			} else {
				if (!col.GetComponent<arcpoly> ().gethit ()) {
//					Debug.Log ("hit");
					count++;
					col.GetComponent<arcpoly> ().hit ();
					col.GetComponent<MeshRenderer> ().material = m;
					if (count >= ((r - 1) * (c - 1)) && !win) {
						win = true;
						whitelines ();
						screenshot ();
					}
				}
			}
		}
	}

	void whitelines(){
		for (int i = 0; i < r; i++) {
			lr_r [i].startColor = Color.white;
			lr_r [i].endColor = Color.white;
			lr_c [i].startColor = Color.white;
			lr_c [i].endColor = Color.white;
		}
	}

	void screenshot(){
//		string temp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
//		Debug.Log (temp);
//		Application.CaptureScreenshot("../Screenshot"+temp+".png");
	}
}
