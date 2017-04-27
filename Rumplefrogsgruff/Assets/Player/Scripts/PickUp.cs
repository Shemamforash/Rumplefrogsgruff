using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public bool can_pick_up = false;
    private bool is_picked_up = false;
    private GameObject player;
    private float distance_from_player = 15f;
    public LayerMask environment_layer;
    private RaycastHit hit;
    private bool burning = false;
    private float burn_time = 1f;
    private Vector3 original_scale;

    void Start()
    {
        player = GameObject.Find("FirstPersonCharacter");
        original_scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!burning)
        {
            if (Input.GetKeyUp("f") && QuestionController.is_burning_allowed())
            {
                if (is_picked_up)
                {
                    is_picked_up = false;
                    GetComponent<Rigidbody>().isKinematic = false;
                }
                else
                {
                    Vector3 player_look_dir = player.transform.forward;
                    Debug.DrawRay(player.transform.position, player_look_dir * distance_from_player * 2, Color.red, 2f);
                    if (Physics.Raycast(player.transform.position, player_look_dir, out hit, distance_from_player * 4))
                    {
                        if (hit.collider.gameObject == gameObject)
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
                if (Physics.Raycast(player.transform.position, player_look_dir, out hit, held_distance, environment_layer))
                {
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
        else
        {
            burn_time -= Time.deltaTime * 0.33f;
            if (burn_time <= 0)
            {
                int logs_remaining = -1;
                List<GameObject> npcs = new List<GameObject>(GameObject.FindGameObjectsWithTag("NPC"));
                foreach(GameObject npc in npcs){
                    if(npc.name == "Logs"){
                        ++logs_remaining;
                    }
                }
                if(logs_remaining == 0){
                    QuestionController.burn_logs();
                }
                Destroy(gameObject);
            }
            else
            {
                transform.localScale = original_scale * burn_time;
            }
        }
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.name == "Campfire")
        {
            burning = true;
            GetComponent<ParticleSystem>().Play();
        }
    }
}
