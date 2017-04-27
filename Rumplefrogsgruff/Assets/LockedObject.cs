using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedObject : MonoBehaviour {

	private string previousTag = null;

	public void unlock () {
		previousTag = gameObject.tag;
		gameObject.tag = "OPENS";
	}

	public void resetTag(){
		gameObject.tag = previousTag;
	}
}
