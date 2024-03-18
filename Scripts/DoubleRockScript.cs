using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleRockScript : MonoBehaviour                       //to handle the logic of crashing through floors 
{
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        GameObject collidedObj = collision.gameObject;

        if (collidedObj.tag == "Breakable")                   
        {
            Destroy(collidedObj);
        }
    }

}
