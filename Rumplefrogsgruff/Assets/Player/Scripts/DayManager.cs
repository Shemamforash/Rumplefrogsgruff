using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayManager : MonoBehaviour
{
    private static bool is_night = true;
    private static int day = 0, night = 0;
    // private static bool is_night = false;
    // private static int day = 3, night = 3;
    private List<Question> intro, day_one, night_one, day_two, night_two, day_three, night_three;
    private static List<List<Question>> question_lists = new List<List<Question>>();
    private static bool loaded = false;

    void Start()
    {
        if (!loaded)
        {
            question_lists.Add(intro = FileReader.Read(0));
            question_lists.Add(day_one = FileReader.Read(1));
            question_lists.Add(night_one = FileReader.Read(2));
            question_lists.Add(day_two = FileReader.Read(3));
            question_lists.Add(night_two = FileReader.Read(4));
            question_lists.Add(day_three = FileReader.Read(5));
            question_lists.Add(night_three = FileReader.Read(6));
            QuestionController.SetQuestions(intro, night);
			// QuestionController.SetQuestions(day_two, 2);
            loaded = true;
        }
    }

    public static int get_day(){
        return day;
    }

    public static void change_day()
    {
        if (!is_night)
        {
            is_night = true;
            ++day;
        }
        else
        {
            is_night = false;
            ++night;
        }
        QuestionController.SetQuestions(question_lists[day + night], day + night);
    }

    public static bool is_it_night()
    {
        return is_night;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
