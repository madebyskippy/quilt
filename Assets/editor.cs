using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editor : MonoBehaviour {

	/*
	 *	System.IO.File.AppendAllText ("Assets/Resources/worddata.txt", " "+wordCountV+"\n");
	 *	System.IO.File.AppendAllText ("Assets/Resources/worddatawords.txt", "\n");
	 */

	[SerializeField] quilt_manager manager;
	[SerializeField] GameObject l;

	// 0,0 is the top left
	Vector2 current;

	//these are from the manager and should be the same i'm just too lazy so it's hardcoded
	int r = 20;
	int c = 20;

	GameObject[] circs;
	Vector2[] grid;

	int[] q = new int[0];
	List<Vector2> t = new List<Vector2>();

	string time;

	// Use this for initialization
	void Start () {
		current = new Vector2 (0f, 0f);
		circs = manager.getcircs ();
		grid = manager.getgrid ();
		time = System.DateTime.Now.ToString("yyyyMMddHHmmss");
	}
	
	// Update is called once per frame
	void Update () {
		if (circs.Length > 0) {
			for (int i = 0; i < circs.Length; i++) {
				circs [i].GetComponent<SpriteRenderer> ().color = Color.gray;
				if (i==(int)(current.y * r + current.x)) {
					circs [i].GetComponent<SpriteRenderer> ().color = Color.red;
				}
			}
		}

//		if (Input.GetKeyDown (KeyCode.F)) {
//			circs = GameObject.FindGameObjectsWithTag ("grid");
//		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			current.x = Mathf.Clamp (current.x + 1, current.x + 1, r-1);
		}if (Input.GetKeyDown (KeyCode.RightArrow)) {
			current.y = Mathf.Clamp (current.y + 1, current.y + 1, c-1);
		}if (Input.GetKeyDown (KeyCode.UpArrow)) {
			current.x = Mathf.Clamp (current.x - 1, 0, current.x - 1);
		}if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			current.y = Mathf.Clamp (current.y - 1, 0, current.y - 1);
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
		}if (Input.GetKeyDown (KeyCode.Z)) {
			System.IO.File.AppendAllText ("Assets/Resources/pattern"+time+".txt", current.x+","+current.y+",");
			t.Add (new Vector2(current.x,current.y));
		}if (Input.GetKeyDown (KeyCode.X)) {
			System.IO.File.AppendAllText ("Assets/Resources/pattern"+time+".txt", "\n");

			q = new int[t.Count];

			GameObject line = Instantiate (l, Vector3.zero, Quaternion.identity);
			Vector3[] points = new Vector3[q.Length+1];
			for (int k = 0; k < points.Length-1; k++) {
				q [k] = (int)t [k].x + (int)t [k].y*20;
				points [k] = grid[q [k]];
			}
			points [points.Length - 1] = points [0];
			line.GetComponent<LineRenderer> ().SetPositions (points);
			line.GetComponent<LineRenderer> ().startColor = Color.red;
			line.GetComponent<LineRenderer> ().endColor = Color.red;
			t.Clear ();
//			manager.drawquads ();
		}

		//z to enter a point
		//x to finish a quad
	}
}
