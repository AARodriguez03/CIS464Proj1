using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FireProjectile : MonoBehaviour                     // script that controls the "water" ability

{
    [SerializeField] private Transform launchPoint;             //Set launch point where projectile will be fired from     
    [SerializeField] private GameObject Projectile;             //Projectile that will be used 
    [SerializeField] private float projectileSpeed;             // How fast projectile goes 
    [SerializeField] private float attackCoolDown;              // frequency of attacks 
    [SerializeField] private float playerLaunchSpeed;           // launch speed of the player aka how fast they will translate to new spot 
    private float timer;                                        // timer used for attack cooldown 
    public int allowedShots;                             // total num of shots allowed 
    private Camera worldCam;                                    //world cam needed for angled shooting
    private Vector3 mouseLoc;                                   //Where mouse is currently located on screen 
    private GameObject player;                                  // get player
    private PlayerAbilities abilites;                           // access to player abilites script
    public int shotType;                                        // 1 for wind. 2 for earth. 3 for fire.
    private Rigidbody2D rigid;                                  //used for moving player when fire_water combo 
   
    void Start()
    {
        worldCam = GameObject.FindObjectOfType<Camera>().GetComponent<Camera>(); // find the Camera 
        player = GameObject.Find("Player");                                      //player
        abilites = player.GetComponent<PlayerAbilities>();                       //gain access to play abilites script 
        rigid = player.GetComponent<Rigidbody2D>();                              //access to rigid body 
        allowedShots = 0; 
    }

    void Update()
    {
        if (abilites.water_water == true)
        {
            shotType = 0;
            allowedShots = abilites.numShots * 2;                                  // double whatever value is our total shot count 
            abilites.water_water = false;
        }
        else if (abilites.water == true)                                           //base water 
        {
            shotType = 0;
            allowedShots = abilites.numShots;                                     //base water shots
            abilites.water = false;
        }
        else if (abilites.water_wind == true)                                     //water wind 
        {
            allowedShots = abilites.numShots;
            shotType = 1;
            abilites.water_wind = false;
        }
        else if (abilites.water_earth == true)                                    //water earth 
        {
            allowedShots = 1;
            shotType = 2;
            abilites.water_earth = false;
        }
        else if (abilites.water_fire == true)                                     //water fire 
        {
            allowedShots = abilites.numShots;
            shotType = 3;
            abilites.water_fire = false;
        }

        if (PauseMenuScript.isPaused)
        {
                //do nothing
        }
        else
        {
            mouseLoc = worldCam.ScreenToWorldPoint(Input.mousePosition);                // get the mouse location 
            Vector3 rotation = mouseLoc - transform.position;                           // find out where 
            float rotation2 = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;      // get the rotation angle 
            transform.rotation = Quaternion.Euler(0, 0, rotation2);                     // angle it towards camera

            

            if (Input.GetMouseButton(1))                                            //when holding down the right mouse button
            {
                if (Input.GetMouseButton(0) && allowedShots != 0 && TimeBetweenShots() && shotType != 3)        // when pressing leftmouse and can be fired for certain num of shots as well as cooldown reached
                {
                    Shoot();                                                            // fire projectile

                }
                else if (Input.GetMouseButton(0) && allowedShots != 0 && TimeBetweenShots())    // fire_water combo is different as it activate moves player instead of teleport                                                               //dont need to check. Will 100% be the fire water combo that allows them to shoot at all if nothing else does 
                {

                    StartCoroutine(FireWaterShot());                                            //start coroutine to move player in real time 

                }

            }

            timer += Time.deltaTime;                                                // timer increases 

        }
     
            
       
    }

    private void Shoot()                                                        // create and fire bullet
    {
        timer = 0; // reset timer for cooldown  
        GameObject newProjectile = Instantiate(Projectile, launchPoint.position, launchPoint.rotation); // create a new game object of current projectile param
        Vector3 direction = mouseLoc - transform.position; // get the direction we want to fire in 
        newProjectile.transform.GetComponent<Rigidbody2D>().velocity = new Vector3(direction.x, direction.y).normalized * projectileSpeed; // fire!
        allowedShots--;                // deincrement number of shots 
    }

    private IEnumerator FireWaterShot()                                                     //ability handler for the fire water combo 
    {
        timer = 0;                                                                          //set timer to 0 as attack was used 
        rigid.gravityScale = 0;                                                             //make the play unaffected by gravity to allow smooth movement
        allowedShots--;                                                                     // deincrement the shots
        Vector3 direction = (Vector3)mouseLoc - transform.position;                         //get the direction/angle from mouse to user 
        direction.Normalize();                                                              // make the magnitude 1, as it will launch player far if not here                                                    
        float distance = 10f;                                                               //how far the player is going to recoil 

        Vector3 destination = transform.position - direction * distance;                    //get the destination by subtracting direction from position and multiply it by the amount of space we want covered 

        while (Vector2.Distance(rigid.transform.position, destination) > 0.1f)              // make sure player stops when reaching the point 
       {
            Vector3 moveDirection = (destination - transform.position).normalized;          //Normalize the vector to allow short bursts

            rigid.velocity = moveDirection * playerLaunchSpeed;                             // launch the player                

            yield return null;                                                              // return null for IEnumerator
       }

        rigid.gravityScale = 1;                                                            // return gravity to normal
    }

    private bool TimeBetweenShots()                                                  // returns true when timer > attackCoolDown, meaning it can be fired
    {
        return timer > attackCoolDown;
    }

}
