﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class quilt_manager : MonoBehaviour {

	[SerializeField] Vector2 startp;

	[SerializeField] GameObject l;
	[SerializeField] GameObject circ;
	[SerializeField] GameObject poly;
	[SerializeField] Material m;

	[SerializeField] GameObject mouse;
	[SerializeField] Text text;

	//for the grid
	Vector2[] grid;
	Vector2[] stable;
	Vector2[] changes;
	int r = 20;
	int c = 20;

	//for the level
	List<int[]> quads; //keeps list of groups of index that make up each poly
	List<GameObject> lines;
	//for gameplay, specific to level
	List<GameObject> spaces;
	List<GameObject> intersections;

	float increment = 0.01f; //change this to speed up the warping

	int count;

	bool gameover;
	bool win;
	bool start;

	float score;

	// Use this for initialization
	void Start () {
		score = 0;
		count = 0;
		text.text = "use the mouse to touch each space\n\n0";
		gameover = false;
		win = false;
		start = true;

		grid 	= new Vector2[r*c];
		stable 	= new Vector2[r*c];
		changes = new Vector2[r*c];

		quads = new List<int[]> ();
		lines = new List<GameObject> ();
		intersections = new List<GameObject> ();
		spaces = new List<GameObject> ();

		for (int i = 0; i < r; i++) {

			for (int j = 0; j < c; j++) {
				grid 	[i*r+j] = new Vector2 (startp.x + i*0.5f, startp.y - j*0.5f);
				stable	[i*r+j] = new Vector2 (startp.x + i*0.5f, startp.y - j*0.5f);
				changes [i*r+j] = Vector2.zero;
				intersections.Add(Instantiate (circ, grid [i*r+j], Quaternion.identity));
				if (i > 0 && j < c - 1) {
					quads.Add(new int[]{(i-1)*r+j,(i-1)*r+j+1,(i)*r+j+1,(i)*r+j});
					GameObject t = Instantiate (l, grid [i * r + j], Quaternion.identity);
					GameObject p = Instantiate (poly, grid [i * r + j], Quaternion.identity);
					Vector3[] points = new Vector3[quads [quads.Count - 1].Length+1];
					for (int k = 0; k < points.Length-1; k++) {
						points [k] = grid[quads [quads.Count - 1] [k]];
					}
					points [points.Length - 1] = points [0];
					t.GetComponent<LineRenderer> ().SetPositions (points);
					lines.Add (t);
					spaces.Add (p);
				}
			}
		}
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
	}

	void jiggle(){
		float threshold = 0.05f;
		for (int j = 0; j < c; j++) {
			for (int i = 0; i < r; i++) {
				float x, y;
				if (grid [i*r+j].x - stable [i*r+j].x > threshold) {
					x = Random.Range (-1f, 0) * increment;
				} else if (stable [i*r+j].x - grid [i*r+j].x > threshold) {
					x = Random.Range (0, 1f) * increment;
				} else {
					x = Random.Range (-1f, 1f) * increment;
				}
				if (grid [i*r+j].y - stable [i*r+j].y > threshold) {
					y = Random.Range (-1f, 0) * increment;
				} else if (stable [i*r+j].y - grid [i*r+j].y > threshold) {
					y = Random.Range (0, 1f) * increment;
				} else {
					y = Random.Range (-1f, 1f) * increment;
				}
				changes [i*r+j] = new Vector2 (x, y);
				edit (i*r+j,grid [i*r+j] + changes[i*r+j]);

			}
		}
	}

	void warp(){
		for (int i = 0; i < changes.Length; i++) {
			changes[i] = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f)) * increment;
			edit (i,grid[i]+changes[i]);
		}
	}

	void edit(int i, Vector2 p){
		grid[i] = p;
	}

	void showlines(){
		for (int i = 0; i < lines.Count; i++) {
			lines[i].GetComponent<LineRenderer> ().SetPositions (new Vector3[]{
				grid[quads[i][0]],
				grid[quads[i][1]],
				grid[quads[i][2]],
				grid[quads[i][3]],
				grid[quads[i][0]]
			});
		}
	}

	void placecirc(){
		for (int i = 0; i < intersections.Count; i++) {
			intersections [i].transform.position = grid [i];
		}
	}

	void adjustpoly(){
		for (int i = 0; i < quads.Count; i++) {
			//make a poly for each
			GameObject o = spaces[i];
			Vector2[] v2;
			Vector3[] v3;
			int numtris = (quads[i].Length-2)*3;
			int current_tri = 0;
			int[] tris = new int[numtris] ;//{0,2,1,0,3,2};//{2,1,0,0,3,2};
			tris [0] = 0;
			tris [1] = 2;
			tris [2] = 1;

			v2 = o.GetComponent<PolygonCollider2D> ().points;
			v3 = o.GetComponent<MeshFilter> ().mesh.vertices;
			for (int j = 0; j < quads [i].Length; j++) {
				current_tri = j-2;
				v2 [j] = o.transform.InverseTransformPoint (grid [quads [i] [j]]);
				v3 [j] = o.transform.InverseTransformPoint (grid [quads [i] [j]]);
				if (current_tri > 0) {
					tris [3 * current_tri] = 0;
					tris [3 * current_tri+1] = j;
					tris [3 * current_tri+2] = j-1;
				}
			}

			o.GetComponent<PolygonCollider2D> ().points = v2;
			o.GetComponent<MeshFilter> ().mesh.vertices = v3;
			o.GetComponent<MeshFilter> ().mesh.triangles = tris;
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
		for (int i = 0; i < lines.Count; i++) {
			lines [i].GetComponent<LineRenderer>().startColor = Color.white;
			lines [i].GetComponent<LineRenderer>().endColor = Color.white;
		}
	}

	void screenshot(){
		//		string temp = System.DateTime.Now.ToString("yyyyMMddHHmmss");
		//		Debug.Log (temp);
		//		Application.CaptureScreenshot("../Screenshot"+temp+".png");
	}
}
