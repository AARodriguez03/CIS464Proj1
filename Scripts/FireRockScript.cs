using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRockScript : MonoBehaviour             //script for controlling the explosion of the fire_rock combo 
{
    private Animator anim;                              //get animator
    private BoxCollider2D animCollider;                 //get box collider
    private Rigidbody2D objBody;                        //get the body of the rock 

    void Start()                                         //grab components
    {
        anim = GetComponent<Animator>();
        animCollider = GetComponent<BoxCollider2D>();
        objBody = GetComponent<Rigidbody2D>();
        objBody.constraints = RigidbodyConstraints2D.FreezeRotation; //freeze the rotation of object so sprite explodes the same way while giving illusion of it moving 
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))                     // set off the explosion 
        {
           anim.SetTrigger("explode");
        }
    }

    public void Remove()                                 //this is for the animator field. Will remove the bomb after it explodes 
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)      //whenever the blast hits something 
    {
         
        GameObject hitByBlast = collision.gameObject;        //grab it 

        if (hitByBlast.CompareTag("Player") || hitByBlast.CompareTag("Ground"))  // only destroy object is it ISNT player or ground 
        {
            return;
        }
        
        Destroy(hitByBlast);        //destroy the object 
        

        
    }
}