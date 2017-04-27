using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

	public string title = "SET TITLE HERE";
	[TextArea(3,10)]
	public string text = "SET TEXT HERE";
	public LockedObject unlocks;
	public bool shouldDestroy = false;
	public bool isPotion = false;
	public bool isStone = false;

	public string getTitle(){
		return title;
	}

	public string getText(){
		return text;
	}
}
