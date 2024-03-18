using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour    // removes the projectile as well as something special :) 
{
    [SerializeField] private GameObject objectToSpawn; // for spawning an object on impact 
    private BoxCollider2D projectileBox;           //get the box collider
    private PlayerAbilities abilityCheck;          //get a reference to ability check to activate an ability 
    private GameObject player;                     // player reference 
    private Rigidbody2D playerRigid;               //players rigidbody 
    private FireProjectile bulletType;
    private GameObject pivotPoint; 
    private bool canTeleport;                      //set to false for the moment 
    private bool canSpawn;                         //for spawning a rock 
    private bool didImpact;                        //did the projectile hit something?  

    void Start()
    {
        
        projectileBox = GetComponent<BoxCollider2D>();         // get reference to box collider
        player = GameObject.Find("Player");                    //player
        pivotPoint = GameObject.Find("PivotPoint");
        playerRigid = player.GetComponent<Rigidbody2D>();      // players rigid
        abilityCheck = player.GetComponent<PlayerAbilities>(); //ability script attached to player ( important for accessing information)
        bulletType = pivotPoint.GetComponent<FireProjectile>();

        if (bulletType.shotType == 1)                   // check to see if we can teleport when bullet spawns 
        {
            canTeleport = true;                         // set to true 

        }
        else if(bulletType.shotType == 2)               //check to see if we spawn a rock 
        {
            canSpawn = true; 

        }

        
    }

    void Update()
    {
        if(didImpact)                                        // kill the script when a collision happens. Bullet logic is applied elsewhere so no need to here 
        {
            return; 
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)     //in built function of Unity that determines when something a trigger hits something ( in this case the projectle) 
    {
        didImpact = true;                                   //set to true to kill the script so it doesn't linger 
        projectileBox.enabled = false;                      //remove the box collider of projectile 

        if (canTeleport == true )                            // if true for either 
        {
            WaterWindCombo(collision.ClosestPoint(transform.position)); // get where the collision happened and call water_wind combo
        }
        else if (canSpawn == true)
        {
            WaterRockCombo(collision.ClosestPoint(transform.position)); //get where colision happend and call water_earth combo 

        }
        Destroy(gameObject);                                            // destory it because it was instantiated 
    }

    private void WaterWindCombo(Vector3 collisionLocation)              //Function dealing with teleporting player
    {
      
        playerRigid.position = collisionLocation;                       //teleport player to the spot 

    }

    private void WaterRockCombo(Vector3 collisionLocation)              // function dealing with spawning a rock at bullet collision point 
    {
        Instantiate(objectToSpawn, collisionLocation, Quaternion.identity); // spawn it 
        

    }
}
