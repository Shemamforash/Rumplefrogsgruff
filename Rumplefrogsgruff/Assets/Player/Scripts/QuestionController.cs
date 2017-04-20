using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionController : MonoBehaviour
{
    public GameObject knife, axe, desk, logs, candle, pen;
    private static List<Question> questions;
    private static List<Question> available_questions = new List<Question>();

    // Use this for initialization
    void Start()
    {
        questions = FileReader.Read();
        OpenQuestion(0);
    }

	private static void OpenQuestion(int q_no){
		foreach(Question q in questions){
			if(q.getId() == q_no && !available_questions.Contains(q)){
				available_questions.Add(q);
			}
		}
	}

    public static List<Question> GetQuestions(GameObject g)
    {
        List<Question> question_arr = new List<Question>();
        Question.Item item = GameObjectToItem(g);
        foreach (Question q in available_questions)
        {	
            string response = q.getResponse(item);
            if (response != "")
            {
                question_arr.Add(q);
            }
        }
        return question_arr;
    }

    public static string GetResponse(GameObject g, Question q)
    {
        Question.Item item = GameObjectToItem(g);
		foreach(int i in q.GetOpens(item)){
			OpenQuestion(i);
		}
		return q.getResponse(item);
    }

    private static Question.Item GameObjectToItem(GameObject g)
    {
        switch (g.transform.root.name)
        {
            case "Knife":
                return Question.Item.KNIFE;
            case "Axe":
                return Question.Item.AXE;
            case "Desk":
                return Question.Item.DESK;
            case "Logs":
                return Question.Item.LOGS;
            case "Candle":
                return Question.Item.CANDLE;
            case "Pen":
                return Question.Item.PEN;
            default:
                return Question.Item.NONE;
        }
    }
}
