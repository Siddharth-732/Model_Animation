using UnityEngine;

public class PlayerMech : MonoBehaviour
{
       //rigid body declared
    private Rigidbody rb;

    //serialize done
    [SerializeField] private float PlayerSpeed_walk;
    [SerializeField] private float PlayerSpeed_run;
    [SerializeField] private float PlayerSpeed_JumpForce;

    //ground collider
    private bool GroundCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundCollider = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundCollider = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        rb.MovePosition(rb.position + movement * PlayerSpeed_walk * Time.fixedDeltaTime);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && GroundCollider)
        {
            rb.AddForce(Vector3.up * PlayerSpeed_JumpForce, ForceMode.Impulse);
            GroundCollider = false;
        }
    }
}
