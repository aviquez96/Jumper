using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    //Configuration
    //SerializeField allows the variable to be visible in the Unity page's script! Cool
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;

    //State of the player -> useful for gameSession script
    bool isAlive = true; 

    //Cached component references -> allows to modify components present in the unity engine
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;

    //Method that defines global variables
    void Start()
    {   
        //The GetComponent allows to modify parameters of a specific component
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive) {
            Run();
            FlipSprite();
            Jump();
            ClimbLadder();
            Die();
        }

    }

    private void Run()
    {
        //To change a direction, you need to use the input.getAxis method, and specify the direction. Since we want to make this 
        //game cross platform, we use the CrossPlatformInputManager
        //The velocity is a parameter of your rigidBody, that is, a component that describes the physical forces applied to a game 
        //object. This can be accessed by making a myRigidBody variable of type Rigidbody2D, and getting the components
        //You don't need to address any right or left keys because in the input definition, velocity has already taken care of that
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); //value is bw -1 to 1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        //This code is responsible to trigger true or false to the parameters established in the animator controller and change to 
        //the running animation. If the left or right keys are pressed, then expression is true
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);

    }

    private void FlipSprite() 
    {
        //If the player moves horizontally, then reverse the current scaling of the axis
        //Math.Abs gives the absolute val of a property; if > Mathf.Epsilon (that is 0), then playerHasHorizontalSpeed is true
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        //If the player is moving, then return the sign of the velocity in the x component (Mathf.sign returns 1 or -1)
        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void Jump()
    {
        //If you press the jump button, you get a new velocity in the y dir. You don't need to care about going down because the 
        //rigidbody object takes care of the gravity on its own
        //The outermost if is avoiding double jump; if your game object is touching ground, jump, otherwise don't!
        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) == true)
        {
            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                Vector2 jumpVelocity = new Vector2(0f, jumpSpeed);
                myRigidBody.velocity += jumpVelocity;
            }
        }
    }

    private void ClimbLadder() 
    {

        if (myFeet.IsTouchingLayers(LayerMask.GetMask("Climbing")) == true)
        {
            //If vertical (up or down) is pressed, then scale my velocity according to my speed
            float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
            myRigidBody.velocity = climbVelocity; 

            //If climbing, give me the climbing animation
            bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
            myAnimator.SetBool("Climbing", playerHasVerticalSpeed);

            myRigidBody.gravityScale = 0f;

        }

        else
        {
            //if not climbing, gravity is normal and return to previous animation
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
        }

    }

    private void Die() 
    {
        //if touching enemy or hazards layers, then set dying animation, and inform the game session  
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            myAnimator.SetTrigger("Dying");
            isAlive = false;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}

