using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Настройки")]
    public float jumpForce = 5f;      
    public float rotationSpeed = 5f;  
    public float tiltAngle = 30f;     

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        float tilt;
        if (rb.velocity.y > 0)
            tilt = tiltAngle; 
        else if (rb.velocity.y < 0)
            tilt = -tiltAngle; 
        else
            tilt = 0;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, tilt),
            Time.deltaTime * rotationSpeed
        );
    }
}