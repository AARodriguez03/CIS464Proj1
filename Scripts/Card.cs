using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour             //simple script to pickup a card 
{
    public int orderInLayer; 
    private CardHand holder;
    public int flag; //used to determine if being used as a mixing card
    public int arrayIndex;

    private void OnTriggerEnter2D(Collider2D other) //whenever any collision is detected 
    {
        GameObject hitObject = other.gameObject;     //grab the object 
    
        if (hitObject.tag == "Player")              //if player 
        {
            CardHand holder = hitObject.GetComponent<CardHand>();  // get access to the array that will hold cards 
            if (holder.cardIndex + 1 <= 9)                         //if the player wont have more than they can hold 
            {
                Rigidbody2D cardRigid = gameObject.GetComponent<Rigidbody2D>(); //get the rigid body 
                cardRigid.Sleep();                                              //make it sleep 
                BoxCollider2D cardBox = gameObject.GetComponent<BoxCollider2D>();   //get the box collider
                cardBox.enabled = false;                                            //disable it. This is so that hud doesn't get physically effected 

                holder.hand[holder.cardIndex] = gameObject;        //set the array index to the new card 
                arrayIndex = holder.cardIndex;                     //set information
                orderInLayer = holder.hand.Length - holder.cardIndex; //set order in hierarchy
                holder.UpdateHUD();                                   //update the UI 
            }
            else                                                     //user doesnt have enough space or object wasnt player
            {
                return; 
            }

        }
    }
}
