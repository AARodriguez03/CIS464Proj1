using UnityEngine;

public class PlayerMovement : MonoBehaviour         //Script for player movement and animations 
{
    [SerializeField] private float speed;           // player speed 
    [SerializeField] private float jumpPower;       // player jump power 
    [SerializeField] private LayerMask groundLayer; // layer that indicates ground 
    public GameObject arm;                          // arm object that will move with player. Needed for water ability script "FireProjectile" 
    private Rigidbody2D body;                       // body of the player 
    private Animator anim;                          // animation aspect of the player 
    private BoxCollider2D boxCollider;              // box collider of the player 
    [SerializeField] private int numJumps;          // this lets player jump 3 times in a row and stays the same value every update call
    public float horizontalInput;                   // Left and right movement of the player 
    public bool canJumpAgain;                       // Tells game when player can jump 3 times in a row or not 
    public float X_Pos;                             // x position of player for other scripts
    public float Y_Pos;                             // y position of player for other scripts 
    public bool facing_left = false;                // determines whether player is facing left for dash ability 
    public bool facing_right = false;               // determines whether player is facing right for dash ability 
    private int constantJump;                       //will use the numJumps from the field to use in field 
    public bool canMove; 
    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        canMove = true;
        facing_right = true;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;// stop player from flipping around
        
        
    }

    private void Update()
    {
        X_Pos = body.position.x;                                               // get the player positions for other scripts
        Y_Pos = body.position.y;                                               //

        arm.transform.position = new Vector2(body.position.x, body.position.y);// move an object with player in our case an arm 
        horizontalInput = Input.GetAxis("Horizontal");                         // player input 


        if (horizontalInput > 0.01f)                                           //Flip player sprite when moving left/right
        {
            transform.localScale = Vector2.one;                                // Switches player back to normal facing position
            facing_right = true;                                               // set appropriate booleans for dash abilty 
            facing_left = false;


        }
        else if (horizontalInput < -0.01f)                                     // this is for when moving left 
        {
            transform.localScale = new Vector2(-1, 1);                      //switch players x position around 
            facing_left = true;                                                // set boolean parameters for dash 
            facing_right = false;

        }
        if (canMove == true)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y); //this is what moves the player 

        }
        
        if (canJumpAgain == true)                                              //reset numJumps and make canJumpAgain false
        {
            constantJump = numJumps;
            canJumpAgain = false; 
        }

        if (Input.GetKeyDown(KeyCode.Space) && canMove == true)                               //call player jump logic 
        {
            Jump();                                                        //seperate method because don't want to always be jumping 

        }
     
    }

    private void Jump()                                                          //Jump method. Player can only jump once when on ground 
    {
        if ( constantJump != 0)                                                  //or when they have fire_wind active 
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);            // make player jump
            constantJump--;                                                     //deincrement so that they have to activate fire_wind again              

        }
        if ((isGrounded()))
        {
           
           body.velocity = new Vector2(body.velocity.x, jumpPower);
    
        }
       
    }

    public void SetSpeed(float value)                                            // used in player ability to effect player speed (multiplier) when on fire 
    {
        this.speed *= value;

    }



    private bool isGrounded()                                                   // check to see if grounded 
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);

        return raycastHit.collider != null;
    }

}
