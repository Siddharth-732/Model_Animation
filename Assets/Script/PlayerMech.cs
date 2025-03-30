using UnityEngine;

public class PlayerMech : MonoBehaviour
{
    private Rigidbody rb;
    //DECLARED ALL THE PRIVATE VARIABLES
    [SerializeField] private float playerSpeedWalk;
    [SerializeField] private float playerSpeedRun = 15f;
    [SerializeField] private float playerJumpForce = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    //Bool for the Runnign animation
    private bool isRunning;
    private Animator animator;
    
    //For the groundcollider tag
    private bool groundCollider;
    
    //To fetch the rigidbody component and initiate the animation as soon as the scene starts
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    
    // ground collider
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            animator.SetInteger("nowjump", 0);
            groundCollider = true;
        }
    }
    // ground collider
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCollider = false;
        }
    }
    
    //deals the movement aswell as the rotation of the player
    //also the conditions for the animation
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");   //horizontal input
        float vertical = Input.GetAxis("Vertical");         //vertical input
        bool isRunning = Input.GetKey(KeyCode.LeftShift);   //bool to check if running

        float speed = isRunning ? playerSpeedRun : playerSpeedWalk;    //adjust what speed the player moves
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized; 

        //rotation + animation
        if (movement.magnitude > 0)
        {
            animator.SetBool("WalkStart", true);
            if (speed == playerSpeedRun)
            {
                animator.SetBool("WalkStart", false);
                animator.SetBool("stayIdle", false);
                animator.SetInteger("Running", 10);
            }
            else
            {
                animator.SetInteger("Running", 0);
            }
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            animator.SetInteger("Running", 0);
            animator.SetBool("WalkStart", false);
            animator.SetBool("stayIdle", true);
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && groundCollider)
        {
            rb.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);
            animator.SetInteger("nowjump", 5);
            groundCollider = false;
        }
    }
}
