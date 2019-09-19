using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = 0.36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
    [SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching

    // Variables to handle projectile firing
    // - Where Projectile will spawn in Scene
    public Transform projectileSpawnPoint;
    // - What projectile prefab to spawn in Scene
    public Projectile projectilePrefab;
    // - How fast Projectile will move in Scene
    public float projectileSpeed;

    public BoxCollider2D playerHitbox;
    public CircleCollider2D playerCircleBox;


    // For Ladder
    public float distance;
    public LayerMask whatIsLadder;
    private bool playerisClimbing;
    private float inputVertical;
    private float climbSpeed = 5f;



    Animator anim;

    const float k_GroundedRadius = 0.0003f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    const float k_CeilingRadius = 0.0000003f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Vector3 m_Velocity = Vector3.zero;



    public void start()
        {

        }



    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();
        playerHitbox = GetComponent<BoxCollider2D>();
        playerCircleBox = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        

        if (Input.GetButtonDown("Fire1"))
        {
            fire();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_CrouchDisableCollider.enabled = false;
        }



    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Ladder" && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Entered Ladder");
            playerisClimbing = true;
        }
    }





    private void FixedUpdate()
    {
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                anim.SetBool("touchingGround", true);
                m_Grounded = true;
            }
        }






    RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
        Debug.Log("Raycast is" + hitInfo);

        if(hitInfo.collider != null)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerisClimbing = true;
                Debug.Log("Climbing Time");
               }
            else
            {
                playerisClimbing = false;

            }
        }

        if(playerisClimbing == true)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.position.x, inputVertical * climbSpeed);
            m_Rigidbody2D.gravityScale = 0;
        }
        else
        {
            m_Rigidbody2D.gravityScale = 1;
        }




    }


    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;

            }
        }

        float moveValue = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("moveValue", Mathf.Abs(moveValue));



        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {

            // If crouching
            if (crouch)
            {
                // Reduce the speed by the crouchSpeed multiplier
                //move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                //if (m_CrouchDisableCollider != null)
                //m_CrouchDisableCollider.enabled = false;
            }
            else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

            }

            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            anim.SetBool("touchingGround", false);

            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void fire()
    {

        anim.SetTrigger("isShooting");

        Projectile temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        if (!m_FacingRight)
        {
            temp.speed = -projectileSpeed;
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (m_FacingRight)
        {
            temp.speed = projectileSpeed;
        }

        Debug.Log("Projectile Speed is " + projectileSpeed);
        Debug.Log("Temp Speed is " + temp.speed);

    }



}