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
    private GameObject dialogue_background, speaker_text, response_container, button_to_press;
    private Image fade_background;
    private List<GameObject> dialogue_options = new List<GameObject>();
    private List<Question> current_questions;
    private float max_distance_to_interact = 50f, fade_time = 3f, current_fade_amount = 0f;
    private GameObject rumplestiltskin_object;

	private bool stoneActivated = false;
	public GameObject stone;

    private enum State { ASKING, LISTENING, READING, NONE, DRINKING };
    private enum Fade { OUT, IN, NONE, INSTANTIN };
    private State current_state = State.NONE;
    private Fade current_fade = Fade.OUT;

    void Start()
    {
        rumplestiltskin_object = new GameObject();
        rumplestiltskin_object.name = "RSS";
        dialogue_background = GameObject.Find("Dialogue Background");
        response_container = GameObject.Find("Response Container");
        fade_background = GameObject.Find("Background Fade").GetComponent<Image>();
        button_to_press = GameObject.Find("Button to Press");
        for (int i = 1; i <= 5; ++i)
        {
            dialogue_options.Add(GameObject.Find("Option " + i));
        }
        speaker_text = GameObject.Find("Speaker");
        dialogue_background.SetActive(false);


		//TODO: remove
		for (int i = 0; i < dialogue_options.Count; ++i){
			dialogue_options[i].SetActive(false);
		}
    }

    private void GetNewQuestions()
    {
        string speaker_name = interactible_object.name;
        if (speaker_name == "RSS")
        {
            speaker_name = "A Voice in the Darkness";
        }
        speaker_text.GetComponent<Text>().text = speaker_name;
        current_questions = QuestionController.GetQuestions(interactible_object);
        response_container.SetActive(false);
        dialogue_background.SetActive(true);

        if (current_questions.Count == 0)
        {
            current_state = State.LISTENING;
            if (interactible_object.name == "RSS")
            {
                interactible_object = null;
                DayManager.change_day();
                dialogue_background.SetActive(false);
                dialogue_box_open = false;
                current_fade = Fade.OUT;
                current_state = State.NONE;
            }
            else
            {
                nothing_to_say = true;
                EnableResponse(interactible_object.name + " has nothing else to say.");
            }
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
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.forward, Mathf.Infinity, player_mask);

        //If hit(s) detected
        if (hits.Length > 0)
        {
            //Prioritise openable objects
            GameObject openableObject = null;
            GameObject interactableObject = null;
            float contactDistance = float.MaxValue;

            foreach (RaycastHit rh in hits)
            {
                if (rh.collider.gameObject.tag == "OPENS")
                {
                    if (openableObject == null)
                    {
                        openableObject = rh.collider.gameObject;
                    }
                }
                else if (rh.collider.gameObject.tag == "NPC" || rh.collider.gameObject.tag == "NOTE")
                {
                    if (interactableObject == null)
                    {
                        interactableObject = rh.collider.gameObject;
                        contactDistance = rh.distance;
                    }
                    else
                    { //if another is closer...
                        float newDistance = rh.distance;
                        if (newDistance < contactDistance)
                        {
                            interactableObject = rh.collider.gameObject;
                            contactDistance = newDistance;
                        }
                    }
                }
            }

            if (openableObject != null)
            {
                float distance = Vector3.Distance(openableObject.transform.position, transform.position);
                if (distance < max_distance_to_interact)
                {
                    cursorAnimator.SetBool("canIdentifyObject", true);
                    button_to_press.GetComponent<Text>().text = "(f)";
                    if (Input.GetKeyDown("f"))
                    {
                        Animator ani = openableObject.GetComponent<Animator>();
                        ani.SetBool("unlocked", true);
                    }
                }
            }
            else if (interactableObject != null)
            {
                float distance = Vector3.Distance(interactableObject.transform.position, transform.position);
                if (distance < max_distance_to_interact)
                {
                    cursorAnimator.SetBool("canIdentifyObject", true);
                    button_to_press.GetComponent<Text>().text = "(e)";
                    if (Input.GetKeyDown("e"))
                    {
                        interactible_object = interactableObject;
                        if (interactableObject.tag == "NPC")
                        {
                            current_state = State.ASKING;
                        }
                        else if (interactableObject.tag == "NOTE")
                        {
                            current_state = State.READING;
                        }
                    }
                }
            }
            else
            {
                cursorAnimator.SetBool("canIdentifyObject", false);
                button_to_press.GetComponent<Text>().text = "";
                if (Input.GetKeyDown("e"))
                {
                    interactible_object = null;
                    dialogue_box_open = false;
                    dialogue_background.SetActive(false);
                    current_state = State.NONE;
                }
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
        Debug.Log(response);
        GameObject.Find("Reply Text").GetComponent<Text>().text = response;
        current_state = State.LISTENING;
    }

    private void CloseDialogue(bool close)
    {
        if (Input.GetKeyDown("q") || close || (interactible_object != null && interactible_object != rumplestiltskin_object && Vector3.Distance(transform.position, interactible_object.transform.position) > max_distance_to_interact))
        {

            current_questions = null;
            dialogue_box_open = false;
            dialogue_background.SetActive(false);
            current_state = State.NONE;
            nothing_to_say = false;
        }
    }


    private void ReadNote()
    {
        Note note = interactible_object.GetComponent<Note>();
        bool isPotion = note.isPotion;
        bool isStone = note.isStone;

        dialogue_box_open = true;
        response_container.SetActive(true);
        dialogue_background.SetActive(true);

        GameObject replyText = GameObject.Find("Reply Text");
        replyText.GetComponent<Text>().text = note.getText();

        speaker_text.GetComponent<Text>().text = note.getTitle();

        if (note.unlocks != null)
        {
            LockedObject lo = note.unlocks.GetComponent<LockedObject>();
            lo.unlock();
        }

        if (note.shouldDestroy)
        {
            Destroy(interactible_object);
        }

        if (isPotion)
        {
            current_state = State.DRINKING;
        }
        else if (isStone)
        {
			GameObject book = GameObject.Find ("Book");
			book.tag = "NPC";

			GameObject chair = GameObject.Find ("Chair");
			chair.tag = "NPC";

			QuestionController.openStoneQuestions ();
        }

    }

    private void UpdateFade()
    {
        if (current_fade == Fade.INSTANTIN)
        {
            fade_background.color = new Color(0, 0, 0, 1);
        }
        else if (current_fade != Fade.NONE)
        {
            current_fade_amount += Time.deltaTime;
            if (fade_time <= current_fade_amount)
            {
                current_fade = Fade.NONE;
                current_fade_amount = 0;
            }
            if (current_fade == Fade.IN)
            {
                fade_background.color = new Color(0, 0, 0, current_fade_amount);
            }
            else if (current_fade == Fade.OUT)
            {
                fade_background.color = new Color(0, 0, 0, 1 - current_fade_amount);
            }
        }
    }

    void Update()
    {
        if (current_state == State.NONE)
        {
            if (DayManager.is_it_night())
            {
                current_state = State.ASKING;
                current_fade = Fade.INSTANTIN;
                interactible_object = rumplestiltskin_object;
            }
            else
            {
                RayCastInFront();
            }
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
        else if (current_state == State.READING)
        {
            if (!dialogue_box_open)
            {
                ReadNote();
            }
            if (Input.GetKeyDown("1"))
            {
                CloseDialogue(true);
                current_state = State.NONE;
            }
        }
        else if (current_state == State.DRINKING && Input.GetKeyDown("1"))
        {
            response_container.SetActive(false);
            dialogue_background.SetActive(false);
            dialogue_box_open = false;
            DayManager.change_day();
            current_state = State.NONE;
        }
        CloseDialogue(false);
        UpdateFade();

		//Can't see a better way of doing this, feel free to move somewhere else
		if (!stoneActivated) {
			if (DayManager.get_day () == 3) {
				stone.SetActive (true);
				stoneActivated = true;
			}
		}
    }
}