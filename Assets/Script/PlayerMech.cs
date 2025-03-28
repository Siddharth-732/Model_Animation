using UnityEngine;

public class PlayerMech : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private float playerSpeedWalk = 5f;
    [SerializeField] private float playerSpeedRun = 10f;
    [SerializeField] private float playerJumpForce = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private bool groundCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCollider = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCollider = false;
        }
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float speed = isRunning ? playerSpeedRun : playerSpeedWalk;
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        if (movement.magnitude > 0)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && groundCollider)
        {
            rb.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);
            groundCollider = false;
        }
    }
}
