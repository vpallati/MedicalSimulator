using UnityEngine;
using System.Collections;

public class mouseClickEvents : MonoBehaviour {

    pinchZoom pZScript;
	// Use this for initialization
	void Start () {
        pZScript = GetComponent<pinchZoom>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Input.mousePosition.x.ToString() +  " , " + Input.mousePosition.y.ToString());
            pZScript.clickEvent((int)Input.mousePosition.x, (int)Input.mousePosition.y);
        }
            

    }



}
