using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question {

	public enum Items { KNIFE, AXE, PEN, PAINTING, BOOKS };
	private string text;
	private Dictionary<int, string> subject_response;
	private List<Question> blocks;

	public Question(string text, List<Question> blocks = null){
		subject_response = new Dictionary<int, string>();
		if (blocks == null) {
			blocks = new List<Question> ();
		}
		this.text = text;
	}

	public void addResponse(int item, string response){
		subject_response.Add (item, response);
	}

	public bool removeResponse(int item){
		if(subject_response.ContainsKey (item)){
			subject_response.Remove (item);
			return true;
		}else{
			return false;
		}
	}

	public string getResponse(int item){
		return subject_response [item];
	}

	public void addBlock(Question toBlock){
		blocks.Add (toBlock);
	}

	public bool doesBlock(Question q){
		return blocks.Contains (q);
	}
}
