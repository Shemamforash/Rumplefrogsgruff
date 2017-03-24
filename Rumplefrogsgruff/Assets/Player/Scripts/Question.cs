using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Question {

	public enum Items { KNIFE, AXE, PEN, PAINTING, BOOKS, DESK, LOGS, CHAIR };
	private string text;
	private Dictionary<Items, string> subject_response;
	//Can't keep a list of Question objects easily, so just look up on Question ids instead
	private List<int> blocks;
	private int id;

	public static Question.Items NameToEnum(string name){
		return (Items)Enum.Parse(typeof(Items), name);
	}

	public Question(int id, string text, Dictionary<Items, string> subject_response, List<int> blocks){
		this.subject_response = subject_response;
		this.id = id;
		this.text = text;
		this.blocks = blocks;
	}

	public void addResponse(Items item, string response){
		subject_response.Add (item, response);
	}

	public bool removeResponse(Items item){
		return subject_response.Remove (item);
	}

	public string getResponse(Items item){
		return subject_response [item];
	}

	public void addBlock(int toBlockId){
		blocks.Add (toBlockId);
	}

	public bool doesBlock(int qid){
		return blocks.Contains (qid);
	}

	public int getId(){
		return id;
	}

	public string getText(){
		return text;
	}
}
