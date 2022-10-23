using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = true;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character

    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
                                        //const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody2D;
    public float gravityScale = 2.0f; // use this instead of changing the rigidbody gravity
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    [SerializeField] private Vector3 m_Velocity = Vector3.zero;
    public float runSpeed = 400f;
    public float airRunSpeed = 300f;

    public float upVelocityLimit = 100f;

    public Color humanColor =  new Color(255, 255, 255, 1);

    // ghost variables
    //public float m_AmountOfTimeInGhostForm = 2.0f; // in seconds
    //public float m_TimeLeftInGhostForm = 0.0f;
    public bool m_isInGhostForm = false;

    public float shiftAmountWhenHitTrigger = 0.5f;
    public enum GhostState
    {
        Leap,
        Speed,
       //Heavy, // can't rewind
    }

    public GhostState currentGhostState = GhostState.Leap;

    // float state
    public float leapGhostRunSpeed = 200f;
    [SerializeField] private float m_LeapGhostJumpForce = 100f;                          // Amount of force added when the player jumps.
    public float leapGhostGravity = 1.0f;
    public Color leapGhostColor = new Color(0, 0, 100, 1);

    // Speed state
    public float speedGhostRunSpeed = 500f;
    [SerializeField] private float m_SpeedGhostJumpForce = 400f;                          // Amount of force added when the player jumps.
    public float speedGhostGravity = 2.0f;
    public Color speedGhostColor = new Color(100, 0, 0, 1);

    // Heavy state
    //public float heavyGhostRunSpeed = 200f;
    //[SerializeField] private float m_HeavyGhostJumpForce = 200f;                          // Amount of force added when the player jumps.
    //public float heavyGhostGravity = 1.0f;


    //private Vector3 rewindPosition; // position where the ghost died

    // jank way to ignore physics objects that are either human or ghost
    private Vector3 lastPosition;
    private Vector3 lastVelocity;
    private float lastAngularVelocity;

    public SpriteRenderer deadBodySpritePrefab;
    private SpriteRenderer deadBodySprite;

    // rendering stuff
    private SpriteRenderer sprite;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }


    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Rigidbody2D.gravityScale = gravityScale;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        deadBodySprite = Instantiate<SpriteRenderer>(deadBodySpritePrefab);
        deadBodySprite.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            //Debug.Log("Collider: " + colliders[i].gameObject.ToString());
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }

        /*
        // going ghost, rewind back to original body
        if (m_isInGhostForm)
        {
            m_TimeLeftInGhostForm -= Time.deltaTime;
            if (m_TimeLeftInGhostForm <= 0)
            {
                m_isInGhostForm = false;
                // Change the 'color' property of the 'Sprite Renderer' back to human
                sprite.color = new Color(255, 255, 255, 1);
                m_Rigidbody2D.gravityScale = gravityScale;
                Debug.Log("Back to Human.");

                // rewind time + sprite
                if (currentGhostState != GhostState.Heavy)
                {
                    GetComponent<Transform>().position = rewindPosition;
                    Debug.Log("Rewind Time.");
                }
                deadBodySprite.gameObject.SetActive(false);

                m_Rigidbody2D.velocity = Vector2.zero;
            }
        }
        */

        // set limiter for up velocity
        m_Rigidbody2D.velocity = new Vector2(Mathf.Clamp(m_Rigidbody2D.velocity.x, -upVelocityLimit, upVelocityLimit), m_Rigidbody2D.velocity.y);
        

        lastPosition = transform.position;
        lastVelocity = m_Rigidbody2D.velocity;
        lastAngularVelocity = m_Rigidbody2D.angularVelocity;
    }

    public void GoGhost()
    {
        // only go ghost if your not already in ghost
        if (!m_isInGhostForm)
        {
            Debug.Log("GOING GHOST");

            //m_TimeLeftInGhostForm = m_AmountOfTimeInGhostForm;
            m_isInGhostForm = true;

            // Change the 'color' property of the 'Sprite Renderer' to Ghost
            //sprite.color = new Color(0, 0, 0, 1);

            // state stuff
            switch (currentGhostState)
            {
                case GhostState.Leap:
                    m_Rigidbody2D.gravityScale = leapGhostGravity;
                    sprite.color = leapGhostColor;
                    break;
                case GhostState.Speed:
                    m_Rigidbody2D.gravityScale = speedGhostGravity;
                    sprite.color = speedGhostColor;
                    break;
                //case GhostState.Heavy:
                    //m_Rigidbody2D.gravityScale = heavyGhostGravity;
                    //break;
            }

            
            // rewind time + sprite
            Vector3 rewindPosition = GetComponent<Transform>().position;
            // collision stuff (shift the player by a bit)
            if (m_FacingRight)
            {
                rewindPosition = new Vector2(rewindPosition.x - shiftAmountWhenHitTrigger, rewindPosition.y);
            }
            else
            {
                rewindPosition = new Vector2(rewindPosition.x + shiftAmountWhenHitTrigger, rewindPosition.y);
            }
            
            deadBodySprite.gameObject.SetActive(true);
            deadBodySprite.GetComponent<Transform>().position = rewindPosition;
            Debug.Log("Spawn Dead Body");
        }
    }

    public void TurnBackIntoHuman()
    {
        m_isInGhostForm = false;
        sprite.color = humanColor;

        //remove dead body cuz your human again
        deadBodySprite.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        deadBodySprite.gameObject.SetActive(false);
    }

    public void RewindBackToDeadBody()
    {
        if (m_isInGhostForm)
        {
            m_isInGhostForm = false;
            // Change the 'color' property of the 'Sprite Renderer' back to human
            sprite.color = humanColor;
            m_Rigidbody2D.gravityScale = gravityScale;
            Debug.Log("Back to Human.");

            // rewind time + sprite
            GetComponent<Transform>().position = deadBodySprite.transform.position;
            Debug.Log("Rewind Time.");

            m_Rigidbody2D.velocity = Vector2.zero;

            TurnBackIntoHuman();
        }
    }

    public void Move(float move, bool jump)
    {

        //Debug.Log("is grounded: " + m_Grounded.ToString() + ", jump: " + jump.ToString());

        if (m_isInGhostForm)
        {
            float ghostRunSpeed = runSpeed;
            float m_GhostJumpForce = m_JumpForce;

            switch (currentGhostState) {
                case GhostState.Leap:
                    ghostRunSpeed = leapGhostRunSpeed;
                    m_GhostJumpForce = m_LeapGhostJumpForce;
                    break;
                case GhostState.Speed:
                    ghostRunSpeed = speedGhostRunSpeed;
                    m_GhostJumpForce = m_SpeedGhostJumpForce;
                    break;
                //case GhostState.Heavy:
                    //ghostRunSpeed = heavyGhostRunSpeed;
                    //m_GhostJumpForce = m_HeavyGhostJumpForce;
                    //break;
            }

            Vector3 targetVelocity = new Vector2(move * ghostRunSpeed, m_Rigidbody2D.velocity.y);

            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                //Debug.Log("Face Right");

                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                //Debug.Log("Face Left");
                Flip();
            }
            // If the player should jump...
            if (jump)
            {
                switch (currentGhostState)
                {
                    case GhostState.Leap:
                        if (m_Grounded)
                        {
                            m_Rigidbody2D.AddForce(new Vector2(0f, m_GhostJumpForce));
                        }
                        break;
                    case GhostState.Speed:
                        if(m_Grounded)
                        {
                            m_Rigidbody2D.AddForce(new Vector2(0f, m_GhostJumpForce));
                        }
                        break;
                    //case GhostState.Heavy:
                        //if (m_Grounded)
                        //{
                        //    m_Rigidbody2D.AddForce(new Vector2(0f, m_GhostJumpForce));
                        //}
                        //break;
                }
            }
        }
        else
        {
            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {

                Vector3 targetVelocity;
                // Move the character by finding the target velocity
                if (m_Grounded)
                {
                    targetVelocity = new Vector2(move * runSpeed, m_Rigidbody2D.velocity.y);
                }
                else
                {
                    targetVelocity = new Vector2(move * airRunSpeed, m_Rigidbody2D.velocity.y);
                }

                // And then smoothing it out and applying it to the character
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    //Debug.Log("Face Right");

                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    //Debug.Log("Face Left");
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }
    }

    public void OnCollision2DEnter(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Human"))
        {
            transform.position = lastPosition;
            m_Rigidbody2D.velocity = lastVelocity;
            m_Rigidbody2D.angularVelocity = lastAngularVelocity;
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
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
}