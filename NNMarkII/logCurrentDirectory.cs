using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logCurrentDirectory : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log(System.Environment.CurrentDirectory.ToString());	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
