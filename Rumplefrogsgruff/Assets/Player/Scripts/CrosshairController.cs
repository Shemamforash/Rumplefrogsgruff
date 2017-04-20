using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    /*
    README --- Controls
    e to interact with objects
    q to exit dialogue
    1-5 for dialogue choices
     */

    public Animator cursorAnimator;
    private bool dialogue_box_open = false, nothing_to_say = false;
    private GameObject interactible_object = null;
    public LayerMask player_mask;
    private GameObject dialogue_background, speaker_text, response_container;
    private List<GameObject> dialogue_options = new List<GameObject>();
    private List<Question> current_questions;
    private float max_distance_to_interact = 50f;


    private enum State { ASKING, LISTENING, NONE };
    private State current_state = State.NONE;

    void Start()
    {
        dialogue_background = GameObject.Find("Dialogue Background");
        response_container = GameObject.Find("Response Container");
        for (int i = 1; i <= 5; ++i)
        {
            dialogue_options.Add(GameObject.Find("Option " + i));
        }
        speaker_text = GameObject.Find("Speaker");
        dialogue_background.SetActive(false);
    }

    private void GetNewQuestions()
    {
        speaker_text.GetComponent<Text>().text = interactible_object.name;
        current_questions = QuestionController.GetQuestions(interactible_object);
        response_container.SetActive(false);
        dialogue_background.SetActive(true);

        if (current_questions.Count == 0)
        {
            current_state = State.LISTENING;
            nothing_to_say = true;
            EnableResponse(interactible_object.name + " has nothing else to say.");
        }
        else
        {
            for (int i = 0; i < dialogue_options.Count; ++i)
            {
                if (i < current_questions.Count)
                {
                    dialogue_options[i].SetActive(true);
                    dialogue_options[i].transform.Find("Text").GetComponent<Text>().text = "(" + (i + 1) + ")  " + current_questions[i].getText();
                }
                else
                {
                    dialogue_options[i].SetActive(false);
                }
            }
            dialogue_box_open = true;
        }
    }

    private void RayCastInFront()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, player_mask);
        if (hit.collider != null && hit.collider.gameObject.tag == "NPC")
        {
            cursorAnimator.SetBool("canIdentifyObject", true);
            if (Input.GetKeyDown("e"))
            {
                interactible_object = hit.collider.gameObject;
                current_state = State.ASKING;
            }
        }
        else
        {
            cursorAnimator.SetBool("canIdentifyObject", false);
            if (Input.GetKeyDown("e"))
            {
                interactible_object = null;
                dialogue_box_open = false;
                dialogue_background.SetActive(false);
                current_state = State.NONE;
            }
        }
    }

    private void GetQuestionChoice()
    {
        int input_num = -1;
        if (Input.GetKeyDown("1"))
        {
            input_num = 1;
        }
        else if (Input.GetKeyDown("2"))
        {
            input_num = 2;
        }
        else if (Input.GetKeyDown("3"))
        {
            input_num = 3;
        }
        else if (Input.GetKeyDown("4"))
        {
            input_num = 4;
        }
        else if (Input.GetKeyDown("5"))
        {
            input_num = 5;
        }
        if (input_num > -1 && current_questions.Count >= input_num)
        {
            string response = QuestionController.GetResponse(interactible_object, current_questions[input_num - 1]);
            EnableResponse(response);
        }
    }

    private void EnableResponse(string response)
    {
        foreach (GameObject dialogue_option in dialogue_options)
        {
            dialogue_option.SetActive(false);
        }
        response_container.SetActive(true);
        GameObject.Find("Reply Text").GetComponent<Text>().text = response;
        current_state = State.LISTENING;
    }

    private void CloseDialogue(bool close)
    {
        if (Input.GetKeyDown("q") || close || (interactible_object != null && Vector3.Distance(transform.position, interactible_object.transform.position) > max_distance_to_interact))
        {
            current_questions = null;
            dialogue_box_open = false;
            dialogue_background.SetActive(false);
            current_state = State.NONE;
            nothing_to_say = false;
        }
    }

    void Update()
    {
        if (current_state == State.NONE)
        {
            RayCastInFront();
        }
        else if (current_state == State.ASKING)
        {
            if (!dialogue_box_open)
            {
                GetNewQuestions();
            }
            else
            {
                GetQuestionChoice();
            }
        }
        else if (current_state == State.LISTENING)
        {
            if (Input.GetKeyDown("1"))
            {
                if (nothing_to_say)
                {
                    CloseDialogue(true);
                }
                else
                {
                    current_state = State.ASKING;
                    GetNewQuestions();
                }
            }
        }
        CloseDialogue(false);
    }
}