/*using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private float jumpPower = 10;
    [SerializeField] private LayerMask groundLayer = default;
    [SerializeField] private LayerMask wallLayer = default;
    private Rigidbody2D body;
    //private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private bool canDoubleJump;
    private float jumpDelay;


    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //Set animator parameters
        //anim.SetBool("Run", horizontalInput != 0);
        //anim.SetBool("Grounded", isGrounded());


        //Wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 2;
                //body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 4;



            if (Input.GetKeyDown(KeyCode.Space) && jumpDelay > 0.05f)
            {
                print("Bad");

                Jump();
                jumpDelay = 0;
            }
            /*if (canDoubleJump == true)
                {
                if (Input.GetKeyDown(KeyCode.Space) && jumpDelay >0.5f)
                {
                    print("Jump2");
                    //anim.SetTrigger("jump");
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    canDoubleJump = false;// remove this line if want screw attack esque jump 

                }

                }
            if (!Input.GetKey(KeyCode.Space))
            {
                print("Good!");
                jumpDelay += Time.deltaTime;
            }
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        if (isGrounded())
        {

            //anim.SetTrigger("jump");
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            canDoubleJump = true;

        }

        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }

    }



    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size*0.95f, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        // will only attack when not moving and not on wall and is also grounded
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

}*/