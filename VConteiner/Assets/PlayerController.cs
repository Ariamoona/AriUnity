using UnityEngine;
using VContainer;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    private Rigidbody2D _rb;

    private GameManager _gameManager;
    private ScoreManager _scoreManager;

    [Inject]
    public void Construct(GameManager gameManager, ScoreManager scoreManager)
    {
        _gameManager = gameManager;
        _scoreManager = scoreManager;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    public void Jump()
    {
        _rb.velocity = Vector2.up * jumpForce;
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
        _rb.velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            _gameManager.GameOver();
        }
    }
}