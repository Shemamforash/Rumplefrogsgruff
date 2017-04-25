using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionController : MonoBehaviour
{
    public GameObject knife, axe, desk, logs, candle, pen;
    private static List<Question> questions;
    private static List<Question> available_questions = new List<Question>();


    /*Set the questions that will be used for today.
    Receives a list of questions for the current day, and the day number.
    The first question available to the player is always question 0, so is set to open.
    */
    public static void SetQuestions(List<Question> new_questions, int day_no)
    {
        questions = new_questions;
        OpenQuestion(int.Parse(day_no + "0"));
    }

    /*Makes a question "open", when the player queries for available questions, the open questions are returned.
     */
    private static void OpenQuestion(int q_no)
    {
        foreach (Question q in questions)
        {
            if (q.getId() == q_no && !available_questions.Contains(q))
            {
                available_questions.Add(q);
            }
        }
    }

    /*Returns all open questions that can be asked of the target item (g)
    Will not return questions if the player has already asked them to the target item.
     */
    public static List<Question> GetQuestions(GameObject g)
    {
        List<Question> question_arr = new List<Question>();
        Question.Item item = GameObjectToItem(g);
        foreach (Question q in available_questions)
        {
            if (q.ResponseContainsItem(item) != null && !q.HasSeenItem(item))
            {
                question_arr.Add(q);
            }
        }
        return question_arr;
    }

    /*Opens up new questions from the question that was last asked.
    Returns the objects response to the question.
     */
    public static string GetResponse(GameObject g, Question q)
    {
        Question.Item item = GameObjectToItem(g);
        foreach (int i in q.GetOpens(item))
        {
            OpenQuestion(i);
        }
        return q.getResponse(item);
    }

    /*Converts a gameobject name to the corresponding item enum.
     */
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
            case "RSS":
                return Question.Item.RSS;
            default:
                return Question.Item.NONE;
        }
    }
}
