using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class editor : MonoBehaviour {

	/*
	 *	System.IO.File.AppendAllText ("Assets/Resources/worddata.txt", " "+wordCountV+"\n");
	 *	System.IO.File.AppendAllText ("Assets/Resources/worddatawords.txt", "\n");
	 */

	[SerializeField] quilt_manager manager;

	// 0,0 is the top left
	Vector2 current;

	//these are from the manager and should be the same i'm just too lazy so it's hardcoded
	int r = 20;
	int c = 20;

	GameObject[] circs;

	// Use this for initialization
	void Start () {
		current = new Vector2 (0f, 0f);
		circs = new GameObject[0];
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

		if (Input.GetKeyDown (KeyCode.F)) {
			circs = GameObject.FindGameObjectsWithTag ("grid");
		}

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
			System.IO.File.AppendAllText ("Assets/Resources/pattern.txt", current.x+","+current.y+",");
		}if (Input.GetKeyDown (KeyCode.X)) {
			System.IO.File.AppendAllText ("Assets/Resources/pattern.txt", "\n");
		}

		//z to enter a point
		//x to finish a quad
	}
}
