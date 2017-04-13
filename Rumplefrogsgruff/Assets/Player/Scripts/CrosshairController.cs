using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    public Animator cursorAnimator;
    private bool can_interact_with_object = false;
    private GameObject interactible_object = null;
    public LayerMask player_mask;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, player_mask))
        {
            if (hit.collider.gameObject.tag == "NPC")
            {
                cursorAnimator.SetBool("canIdentifyObject", true);
                interactible_object = hit.collider.gameObject;
            }
            else
            {
                interactible_object = null;
            }
        }
        else
        {
            cursorAnimator.SetBool("canIdentifyObject", false);
            interactible_object = null;
        }
        if (interactible_object != null && Input.GetKeyDown("e"))
        {
            Debug.Log(QuestionController.GetResponse(interactible_object, QuestionController.GetQuestions(interactible_object)[0]));
        }
    }
}
