using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAbilities : MonoBehaviour                      // Script that controls all abilites 
{
    private PlayerMovement playerBody;                            // make a connection to player movements
    private Rigidbody2D playerRigid;                              // for collision/movement
    [SerializeField] private float fire_speed_mult;               // how much faster we want the player to go when on fire 
    [SerializeField] private int fire_duration;                   // and for how long 
    [SerializeField] private float dash_force;                    // how far the dash should bring player 
    [SerializeField] private GameObject Rock;                     //Rock object to be used 
    [SerializeField] private GameObject bridgeRock;               //Bridge Rock 
    [SerializeField] private Transform Object_Spawn_Point;
    [SerializeField] private GameObject Pad;
    [SerializeField] private GameObject Bomb;                     // used to spawn a bomb
    public GameObject DoubleEarthObject; 

    public GameObject FireFirePre;                                //this section is dedicated to prefabs that need to be spawned 
    public GameObject FireEarthPre;
    public GameObject FireWaterPre;
    public GameObject FireWindPre;

    public GameObject WaterWaterPre;
    public GameObject WaterEarthPre;
    public GameObject WaterWindPre;

    public GameObject EarthEarthPre;
    public GameObject EarthWindPre;

    public GameObject WindWindPre;                                //end the dedication

    [SerializeField] public int numShots;                         //how many shots are allowed for water

    public bool water;
    public bool fire;

    public bool fire_fire;

    public bool water_wind;                                      //booleans for water combos 
    public bool water_earth;
    public bool water_water;
    public bool water_fire;

    private float playerPositionx;
    private float playerPositiony;

    private bool canDash;                                         //boolean for when base air card is used 
    private Vector2 dash_dir;                                  //Used for dashing a certain direction
    private bool makingCombination;                               // making a combination 
    public int mixingIndex;
    private GameObject[] mixingArray;
    private string activeAbility;
    private FireProjectile waterScript;

    void Start()
    {
        playerBody = GetComponent<PlayerMovement>();              // this is for getting all values of playerMovement script 
        playerRigid = GetComponent<Rigidbody2D>();                // get the rigid body 
        canDash = true;
        mixingArray = new GameObject[2];                          //two cards added to mixing
        mixingIndex = 0;
        GameObject pivotPoint = GameObject.Find("PivotPoint");
        waterScript = pivotPoint.GetComponent<FireProjectile>();
    }
  
    void Update()
    {
        if (playerBody.facing_left == true)                      // if player is going left or facing left 
        {
            dash_dir = new Vector2(-1f,0f);                            // want to dash in that direction
        }
        else                                                     // going right 
        {
            dash_dir = new Vector2 (1f,0f);                             //revert it 
        }

        switch (activeAbility)                                 //this will only trigger when the activeAbility is a string ( when q is pressed)
        {                                                      // essentially reads the activeAbility tag and makes that ability happen 
                                                           
            case "Fire":
                fire = true;
                activeAbility = "";
                FireCombo();
                break;

            case "Water":
                activeAbility = "";
                water = true;
                break;

            case "Earth":
                activeAbility = "";
                RockAbility(Rock);
                break;

            case "Wind":
                activeAbility = "";
                StartCoroutine(DashAbility());
                break;

            case "FireWater":
                activeAbility = "";
                water_fire = true;
                break;

            case "FireEarth":
                activeAbility = "";
                GameObject newRock = Instantiate(Bomb, Object_Spawn_Point.position, Quaternion.identity); //spawn a bomb 
                break;

            case "FireWind":
                activeAbility = "";
                playerBody.canJumpAgain = true; 
                break;

            case "FireFire":
                activeAbility = "";
                fire_fire = true;
                FireCombo();
                break;

            case "WaterWind":
                activeAbility = "";
                water_wind = true;
                break;

            case "WaterEarth":
                activeAbility = "";
                water_earth = true;
                break;

            case "WaterWater":
                activeAbility = "";
                water_water = true;
                break;

            case "EarthEarth":
                activeAbility = "";
                StartCoroutine(DoubleRock());
                break;

            case "EarthWind":
                activeAbility = "";
                RockAbility(bridgeRock);
                break;

            case "WindWind":
                activeAbility = "";
                SpawnAPad();
                break;

            default:
                break; 

        }


    }

    private IEnumerator DoubleRock()
    {
        float timer = 0f; 

        GameObject Player = GameObject.FindGameObjectWithTag("Player");//get access to player object in its entirety 
        playerRigid.velocity = Vector2.zero;

        BoxCollider2D playerBox = Player.GetComponent<BoxCollider2D>();
        playerBox.enabled = false;               //allow the player to no longer interact with the environment 
        playerBody.canMove = false;              //stop player from moving 
        playerRigid.gravityScale = 0;            //player begins transforming

        SpriteRenderer playerSprite = Player.GetComponent<SpriteRenderer>();//get the sprite of the player 
        yield return new WaitForSeconds(1);                                 //after one second player transforms
        playerSprite.enabled = false;                                       //disable it 
        GameObject Statue = Instantiate(DoubleEarthObject, playerRigid.transform.position, Quaternion.identity); // create a statue game objectg

        while (timer < 5f)                                                                                      //wait for 5 seconds before allowing control (change to be more dynamic later)
        {
            Player.transform.position = Statue.transform.position;                                              //update the position
            timer += Time.deltaTime;                                                                            //increase timer 
            yield return null; 
        }

        Destroy(Statue);                         //destroy the statue 
        playerBox.enabled = true;                //re enable everything for the player 
        playerSprite.enabled = true; 
        playerBody.canMove = true;
        playerRigid.gravityScale = 1; 


    }

    private void SpawnAPad()                                     //spawn the pad 
    {
        GameObject newPad = Instantiate(Pad, Object_Spawn_Point.position, Quaternion.identity);

    }

    public void UseCard(GameObject usedCard)               //use a card
    {
        string cardTag = usedCard.tag;                     //get the tag
        if ((cardTag == "Fire" || cardTag == "FireFire") && (fire == true || fire_fire == true)) //player is already on fire, cant keep stacking
        {
            Debug.Log("cant do that! Already on Fire!");
            return; 
        }
        if ((cardTag == "Water" || cardTag == "WaterWater" || cardTag == "WaterEarth" || cardTag == "WaterWind") && waterScript.allowedShots > 0) //if water is used and shots are still there
        { //water shots are still being used, dont do anything
            Debug.Log("cant do that! Still have Shots!");
            return;
        }

        if (playerBody.canMove == false)      //player is in RockRock state. Dont do anything
        {
            Debug.Log("cant do that! Youre a rock!");
            return; 
        }

        //otherwise 
        CardHand hand = GetComponent<CardHand>();          //get the CardHand component (where cards are held) 
        activeAbility = cardTag;                           //make the name = the card tag
        hand.FixArrayCard(usedCard);                       //remove that card from the index
        
    }

    public void AddToMixArray(GameObject card, int removeFlag) //this will make a combination based on the cards in the array 
    {
        if (removeFlag ==1)                                    // if x is pressed 
        {
            mixingIndex = 0;                                   //reset the data
            mixingArray[0] = null;
            mixingArray[1] = null; 
            return;
        }                                                      //other wise
        mixingArray[mixingIndex] = card;                       //get the card 
        mixingIndex++;                                         //increase index for next card
        if (mixingIndex == 2)                                  //when two cards are in 
        {
            GameObject cardToSpawn = null;                     //declare the object to spawn for later 
            CardHand hand = GetComponent<CardHand>();         // get the cardhand array
           
            string combo = "";                                //combo will equal the cards tag 
            for (int i = 0; i < 2; i++)
            {
                combo += mixingArray[i].GetComponent<Card>().tag;  // get the tag in the array 
                Destroy(mixingArray[i]);                           //destroy it
                
            }
            hand.FixArray(mixingArray);                            //fix the array 

            switch (combo)                                         //instantiate the ability card based on the combo id
            {                                                      //whole bunch of nonsense. Assigns the card as one of the prefabs
                case "FireFire":
                   cardToSpawn = Instantiate(FireFirePre);
                    break;

                case "FireWind":
                    cardToSpawn = Instantiate(FireWindPre);
                    break;

                case "FireEarth":
                    cardToSpawn = Instantiate(FireEarthPre);
                    break;
                case "FireWater":
                    cardToSpawn = Instantiate(FireWaterPre);
                    break;

                case "WaterWind":
                    cardToSpawn = Instantiate(WaterWindPre);
                    break;
                case "WaterEarth":
                    cardToSpawn = Instantiate(WaterEarthPre);
                    break;
                case "WaterWater":
                    cardToSpawn = Instantiate(WaterWaterPre);
                    break;
                case "WaterFire":
                    cardToSpawn = Instantiate(FireWaterPre);
                    break;

                case "EarthWind":
                    cardToSpawn = Instantiate(EarthWindPre);
                    break;
                case "EarthEarth":
                    cardToSpawn = Instantiate(EarthEarthPre);
                    break;
                case "EarthWater":
                    cardToSpawn = Instantiate(WaterEarthPre);
                    break;
                case "EarthFire":
                    cardToSpawn = Instantiate(FireEarthPre);
                    break;

                case "WindFire":
                    cardToSpawn = Instantiate(FireWindPre);
                    break;
                case "WindEarth":
                    cardToSpawn = Instantiate(EarthWindPre);
                    break;
                case "WindWater":
                    cardToSpawn = Instantiate(WaterWindPre);
                    break;
                case "WindWind":
                    cardToSpawn = Instantiate(WindWindPre);
                    break;

            }
            
            Rigidbody2D cardRig = cardToSpawn.GetComponent<Rigidbody2D>();
            cardRig.Sleep();
            BoxCollider2D cardBox = cardToSpawn.GetComponent<BoxCollider2D>();
            cardBox.enabled = false; 

            hand.hand[hand.cardIndex] = cardToSpawn;                                 //make the new index the card Spawned 
            cardToSpawn.GetComponent<Card>().arrayIndex = hand.cardIndex;            //set its index 
            cardToSpawn.GetComponent<Card>().orderInLayer = hand.hand.Length - hand.cardIndex; //set the order in layer
            hand.UpdateHUD();              //update hud
            mixingIndex = 0;               //restart for later 
           

        }
     
    }
  
    private void FireCombo()                      //whenever a fire or fire fire card is used 
    {
        StartCoroutine(DoubleFireCombo());
    }

    private IEnumerator DashAbility()                               //make player dash 
    {

        canDash = false;                                            //use up card
        float time = 0f;                                            //used for while loop

        while (time <= .1f)                                                                      //do this all in 1/10th of a second 
        {
            playerRigid.velocity = dash_dir * dash_force;                                        //move player in direction of dash a set amount 
            time += Time.deltaTime;                                                              //increment time 
                                                                                              
            yield return null;                                                                   //will stop the while loop and end the IEnumerator


        }

    }

    public void RockAbility(GameObject rockToSpawn)                                                                  //Rock abilites
    {
        GameObject newRock = Instantiate(rockToSpawn, Object_Spawn_Point.position, Quaternion.identity); //Create rock at point (wow)

    }
    private IEnumerator DoubleFireCombo()                                                       // fire default and fire + fire combo 
    {
        float currentFireMode = 0.0f; 
        if (fire_fire == true)
        {
            currentFireMode = fire_speed_mult * 2;                                              // double the fire mult speed 
                                                                              //use up card 

        }
        else if (fire == true)
        {
            currentFireMode = fire_speed_mult;
        }
        playerBody.SetSpeed(currentFireMode);                                                   // speed up player for a bit
        yield return new WaitForSeconds(fire_duration);                                         // wait for a couple seconds ability length 
        playerBody.SetSpeed(1 / currentFireMode);
        fire_fire = false;                                                                      // return it back to normal by halfing
        fire = false;

    }

}
