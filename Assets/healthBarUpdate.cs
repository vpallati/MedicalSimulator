using UnityEngine;
using System.Collections;

public class healthBarUpdate : MonoBehaviour {


	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}

	public void updateHealth(float healthValue)
	{
		anim.SetFloat ("HealthValue", healthValue);
	}
}
