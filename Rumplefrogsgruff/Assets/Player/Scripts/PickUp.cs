using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public bool can_pick_up = false;
    private bool is_picked_up = false;
    private GameObject player;
    private float distance_from_player = 30f;
    public LayerMask environment_layer;
    private RaycastHit hit;

    void Start()
    {
        player = GameObject.Find("FirstPersonCharacter");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("f"))
        {
            Debug.Log(is_picked_up);
            if (is_picked_up)
            {
                is_picked_up = false;
                GetComponent<Rigidbody>().isKinematic = false;
            }
            else
            {
                Vector3 player_look_dir = player.transform.forward;
                if (Physics.Raycast(player.transform.position, player_look_dir, out hit, distance_from_player * 2))
                {
                    if (hit.collider.gameObject.tag == "Interactive")
                    {
                        is_picked_up = true;
                        GetComponent<Rigidbody>().isKinematic = true;
                    }
                }
            }
        }
        if (is_picked_up)
        {
            Vector3 player_look_dir = player.transform.forward;
            Vector3 new_pos;
			float held_distance = distance_from_player / 2.5f;
			Debug.DrawRay(player.transform.position, player_look_dir * held_distance, Color.green, 2f);
            if (Physics.Raycast(player.transform.position, player_look_dir, out hit, held_distance, environment_layer))
            {
				Debug.Log(hit.collider.name);
                new_pos = player.transform.position + player.transform.forward * hit.distance;
            }
            else
            {
                new_pos = player.transform.position + player.transform.forward * held_distance;
            }
            new_pos.y -= 3;
            transform.position = new_pos;


        }
    }
}
