using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour {
	private List<Question> questions;
	public Animator cursorAnimator;
	int layerMask;

	void Start(){
		//Objects - will probably replace later
		questions = FileReader.Read();
		layerMask = 1 << 9;
	}
		
	void Update () {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity, layerMask)) {
			cursorAnimator.SetBool("canIdentifyObject", true);
			Debug.Log ("Can see object");
		} else {
			cursorAnimator.SetBool("canIdentifyObject", false);
		}
	}
}
