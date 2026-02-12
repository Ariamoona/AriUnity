using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x < -12f)
            Destroy(gameObject);
    }
}