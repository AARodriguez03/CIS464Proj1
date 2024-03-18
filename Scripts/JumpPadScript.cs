using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadScript : MonoBehaviour                         //handles air_air combo 
{
    [SerializeField] private float launchPower;                    //launch power of the pad 
    private GameObject other;                                      //other collision object
    [SerializeField] private float lifeTime;                       //how long the object lasts 
    private float lifeTimeTimer;                                   //count down how much longer it has in seconds 
    private Rigidbody2D otherBod;                                  //get the rigid of the object 
    private Rigidbody2D padRig;                                    //rigid of pad 
    private BoxCollider2D padBox;                                  //collision of the pad 

    void Start()
    {
        padRig = GetComponent<Rigidbody2D>();                           //get the rigid of the pad 
        padBox = GetComponent<BoxCollider2D>();
        padRig.constraints = RigidbodyConstraints2D.FreezePositionX;     //this locks the rigid body in place on x axis. Still able to fall 

    }

    void Update()
    {
        if (lifeTimeTimer > lifeTime)                            //when time is up, take necessary steps to kill object 
        {
            padBox.enabled = false;                              //remove collision 
            Destroy(gameObject);                                 //destory object itself

            return;

        }

        lifeTimeTimer += Time.deltaTime;                         //increase timer until destroyed

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 whereHit = collision.ClosestPoint(transform.position); // find out where exactly the player interacted with the pad 
        other = collision.gameObject;                           //get the game object that collided
        if (other.tag == "Bullet" || other.tag == "Ground")    // do nothing when a bullet or ground (for whatever reason) impacts. This allows player to shot themselves onto the pad
        {                                                       //trying to access a destroyed object or something with no rigid is a bad idea and crashes
            return;
        }
        otherBod = other.GetComponent<Rigidbody2D>();           //get the rigid body 
        StartCoroutine(Launch(otherBod, whereHit));              //run the function

    }

    private IEnumerator Launch(Rigidbody2D objectBod, Vector2 collisionPoint)
    {
        float timer = 0f;                                           //timer for how far player should be launched when touched
        Vector2 launchEndpoint = new Vector2(transform.position.x, transform.position.y + 5) - collisionPoint; // dont ask me why, but 5 is just the magic number (maybe to do with velocity of launch?)
        while (timer < .1f)
        {
            objectBod.velocity = launchEndpoint * launchPower;     //launch the player 
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
