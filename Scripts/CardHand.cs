using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHand : MonoBehaviour             //simple script to pickup a card((this was made before my 12 hr session :)))))) (i lied this is NOT simple what was I THINKING) 
{
    public int highlightIndex;                    //index of wheel scroll position
    public int cardIndex;                         //what places the card in the array. Will change on its own and not from user input 
    public GameObject[] hand;                     // array of objects
    public RectTransform canvasTransform;         //Rect transform box of the canvas for positioning on the canvas
    private Canvas canvas;                        //reference to the canvas we want to draw on
   

    private void Start()
    {
        hand = new GameObject[10];               //create the hand array
        highlightIndex = 0;                      //instaniate the indexs
        cardIndex = 0;

        GameObject cardCanvas = GameObject.FindGameObjectWithTag("CardCanvas"); //grab references 
        canvasTransform = cardCanvas.GetComponent<RectTransform>(); 
        canvas = GameObject.Find("CardCanvas").GetComponent<Canvas>();
        canvas.overrideSorting = true;                                   //these two allow us to control the positioning of the cards next to each other
        canvas.sortingOrder = 1;

    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");                //get the mouse wheel input 

        if (scroll >= 0.1f) // scrolled up
        {
            highlightIndex++; 
        }
        else if(scroll <= -0.1f)           //scrolled down         
        {
            highlightIndex--;    
        }

        highlightIndex = Mathf.Clamp(highlightIndex, 0, cardIndex - 1);    // you can only highlight based on the amount of cards player has 

        for (int i = 0; i < hand.Length; i++)                             //highlighting logic 
        {
            if (hand[i] != null)                                          // if not a null spot 
            {
                SpriteRenderer sprite = hand[i].GetComponent<SpriteRenderer>(); //get the sprite 
                if (i == highlightIndex || hand[i].GetComponent<Card>().flag == 1) // if the currentIndex is highlighted or the card is being used for a combo 
                {
                    
                    sprite.sortingOrder = hand.Length + 1; //  set order of layer to make it always largest
                    hand[i].GetComponent<RectTransform>().localScale = Vector2.one* 80 * 1.2f; // make it BIG
                   
                }
                else
                {
                    
                    sprite.sortingOrder = hand[i].GetComponent<Card>().orderInLayer; // reset the ordering in the layer to default 
                    hand[i].GetComponent<RectTransform>().localScale = Vector2.one*80; //revert it


                }
            }
        }

        if ( Input.GetKeyUp(KeyCode.X))    // if user presses x
        {
            cancelMix();                   //stop mixing and make array normal
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))        // if user presses control 
        {
            GameObject toBeMixed = hand[highlightIndex].gameObject;  //get the object where the user pressed control
            if ( toBeMixed.tag.Length > 5)                           //way to easily tell if the card is a mixed card
            {
                toBeMixed = null;
                // do nothing

            }
            else
            {
                toBeMixed.GetComponent<Card>().flag = 1;                      //make it stay big
                PlayerAbilities abilites = GetComponent<PlayerAbilities>();   //gain access to abilites script
                abilites.AddToMixArray(toBeMixed,0);                          //add it 

            }

        
        }

        if (Input.GetKeyUp(KeyCode.Q) && hand[highlightIndex].GetComponent<Card>().flag != 1) // if the user presses Q on a card WHILE its not being used for swapping 
        {
            PlayerAbilities abilites = GetComponent<PlayerAbilities>();            //gain access to abilites script 
            abilites.UseCard(hand[highlightIndex]);                                //use the card 

        }

    }

    public void cancelMix()                                   //cancel the mix 
    {
        GameObject currCard;                                  //get the curr card 
        for ( int x = 0; x < cardIndex; x++)                  //reset all flags to 0 when applicable 
        {
            currCard = hand[x].gameObject;
            if(currCard.GetComponent<Card>().flag == 1)
            {
                currCard.GetComponent<Card>().flag = 0;
            }
        }
        PlayerAbilities abilites = GetComponent<PlayerAbilities>(); //gain access to abilites 
        abilites.AddToMixArray(null, 1);                            //empty the array inside of it 

    }

    public void FixArray(GameObject[] objsToRemove)                 //fix array when cards are mixed 
    {
        List<GameObject> tempList = new List<GameObject>(hand);      // make it a linked list because for SOME reason you cant remove a gameObject from an Array UNLESS you make it a linked list and...
                                                                     //... use linked list functions to remove it. Yeah, its DUMB 
        foreach (GameObject obj in objsToRemove)                     //for every object that needs to be removed 
        {
            tempList.Remove(obj);                                    //do it
        }
        
        hand = tempList.ToArray();                                  //make the linkedList into the array 
        cardIndex -= 2;                                             //subtract 2 cards from the array 

        UpdateHUDAfterMix();                                        //special update 
    }

    public void FixArrayCard(GameObject objsToRemove)              //same thing as above but only for one card 
    {
        Destroy(objsToRemove);
        List<GameObject> tempList = new List<GameObject>(hand);
        
        tempList.Remove(objsToRemove);
       
        hand = tempList.ToArray();
        cardIndex -= 1;

        UpdateHUDAfterMix();
    }

    public void UpdateHUDAfterMix()                   //update the hud after the mix 
    {

        float xP = -345f;                             //get the base x and y position that will be on canvas 
        float y = -153f;


        for (int x = 0; x < cardIndex; x++)           //for every single card available 
        {
            
            GameObject cardObject = hand[x];                  //grab it
            Card cardInfo = cardObject.GetComponent<Card>();  //get the info 
            cardInfo.arrayIndex = x;                          //keep information regarding index to remove easily 
            cardInfo.orderInLayer = hand.Length - x;          // make the order in layer based on how close it is to the left (card is in stack with left being the top) 
            cardObject.GetComponent<RectTransform>().localScale = Vector2.one * 80; //transform the sprite to make it BIG 
     

            Vector2 newPosition = new Vector3(xP + x * 85, y);                //calculate the new position on the screen dynamically. 85 is random number that looked nice you can change :)

            
            cardObject.GetComponent<RectTransform>().anchoredPosition = newPosition; // set the position of the card within the canvas

        }
        

    }

    public void UpdateHUD()    //variation of the above script that is used whenever a card is pciked up
    {
        
        float cardWidth = 75f;   //card width, height is to make it appear big 
        float cardHeight = 75f;
        float x = -345f;
        float y = -153f; 

       
        GameObject cardObject = hand[cardIndex];
        cardObject.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth, cardHeight);
        cardObject.GetComponent<RectTransform>().localScale = Vector2.one * 80;

        Vector2 newPosition = new Vector3(x + cardIndex * 85, y);

            
        cardObject.GetComponent<RectTransform>().anchoredPosition = newPosition; //set it within the canvas 

        cardObject.transform.SetParent(canvasTransform, false);
        cardIndex++;  //increase card index, as a card was picked up

    }
}
