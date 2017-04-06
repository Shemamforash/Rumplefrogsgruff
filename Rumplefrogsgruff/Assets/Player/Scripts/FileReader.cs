﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public static class FileReader {
	public static List<Question> Read () {
		List<Question> imported_questions = new List<Question>();
		TextAsset questions = (TextAsset)Resources.Load("questions");
		XmlDocument xmldoc = new XmlDocument ();
		xmldoc.LoadXml ( questions.text );
		XmlNodeList dataNodes = xmldoc.SelectNodes("//Question"); 
		foreach(XmlNode node in dataNodes){
			int id = int.Parse(node.SelectSingleNode("//ID").InnerText);
			string text = node.SelectSingleNode("//Text").InnerText;
			XmlNodeList responses = node.SelectNodes("//Response");
			Dictionary<Question.Items, string> response_dictionary = new Dictionary<Question.Items, string>();
			foreach(XmlNode response_node in responses){
				string subject = response_node.SelectSingleNode("//Subject").InnerText;
				Question.Items subject_enum = Question.NameToEnum(subject);
				string response_text = response_node.SelectSingleNode("//Text").InnerText;
				response_dictionary[subject_enum] = response_text;
			}
			XmlNodeList blocked_questions = node.SelectNodes("//BlockedQuestions");
			List<int> blocked_list = new List<int>();
			foreach(XmlNode blocked_question in blocked_questions){
				int blocked_question_id = int.Parse(blocked_question.InnerText);
				blocked_list.Add(blocked_question_id);
			}
			Question new_question = new Question(id, text, response_dictionary, blocked_list);
			imported_questions.Add(new_question);
		}
		return imported_questions;
	}
}