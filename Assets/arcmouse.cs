using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arcmouse : MonoBehaviour {

	[SerializeField] arcs manager;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){
		//hit, gameover
		if (col.gameObject.tag == "grid")
			manager.hit (true, col);
		else
			manager.hit (false, col);
	}
}
