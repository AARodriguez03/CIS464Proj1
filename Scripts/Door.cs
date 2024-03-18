using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
   [SerializeField]private Transform previousRoom;
   [SerializeField] private Transform nextRoom;
   [SerializeField]private CameraController cam;
   private Animator anim;

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            anim.SetTrigger("Reached");
            anim.SetTrigger("Idle");

            if (collision.transform.position.x < transform.position.x)
            {
                cam.MoveToNewRoom(nextRoom);
            }
            else
            {

                cam.MoveToNewRoom(previousRoom);
            }
        }
            
    }

}
