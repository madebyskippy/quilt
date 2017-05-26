using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arcpoly : MonoBehaviour {

	Vector2 p;
	bool ishit;


	// Use this for initialization
	void Start () {
		ishit = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setP(Vector2 pos){
		p = pos;
	}

	public Vector2 getP(){
		return p;
	}

	public void hit(){
		ishit = true;
	}

	public bool gethit(){
		return ishit;
	}
}
