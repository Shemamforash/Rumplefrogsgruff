using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Animator cursorAnimator;
    private bool can_interact_with_object = false;
    private GameObject interactible_object = null;
    public LayerMask player_mask;
    private GameObject dialogue_background;
    private List<GameObject> dialogue_options = new List<GameObject>();
    private GameObject speaker_text;
    private bool dialogue_box_open = false, responded = false;
    private List<Question> current_questions;
    private GameObject response_container;

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
        responded = false;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, player_mask))
        {
            if (hit.collider.gameObject.tag == "NPC")
            {
                cursorAnimator.SetBool("canIdentifyObject", true);
                if (Input.GetKeyDown("e"))
                {
                    interactible_object = hit.collider.gameObject;
                }
            }
            else
            {
                cursorAnimator.SetBool("canIdentifyObject", false);
                if (Input.GetKeyDown("e"))
                {
                    interactible_object = null;
                    dialogue_box_open = false;
                    responded = false;
                    dialogue_background.SetActive(false);
                }
            }
        }
        else
        {
            cursorAnimator.SetBool("canIdentifyObject", false);
            if (Input.GetKeyDown("e"))
            {
                interactible_object = null;
                dialogue_box_open = false;
                responded = false;
                dialogue_background.SetActive(false);
            }
        }
        if ((!dialogue_box_open && interactible_object != null) || (responded && Input.GetKeyDown("1")))
        {
            GetNewQuestions();
        }
        else if (dialogue_box_open)
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
            else if (Input.GetKeyDown("q"))
            {
                current_questions = null;
                dialogue_box_open = false;
                dialogue_background.SetActive(false);
            }
            if (input_num > -1 && current_questions.Count >= input_num)
            {
                string response = QuestionController.GetResponse(interactible_object, current_questions[input_num - 1]);
                foreach (GameObject dialogue_option in dialogue_options)
                {
                    dialogue_option.SetActive(false);
                }
                response_container.SetActive(true);
                GameObject.Find("Reply Text").GetComponent<Text>().text = response;
                responded = true;
            }
        }
    }
}
