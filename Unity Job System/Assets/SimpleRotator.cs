using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    public float speed = 2f;
    public float radius = 5f;
    private Vector3 center;
    private float angle;

    void Start()
    {
        center = transform.position;
        Vector3 relativePos = transform.position - center;
        angle = Mathf.Atan2(relativePos.z, relativePos.x);
    }

    void Update()
    {
        angle += speed * Time.deltaTime;

        Vector3 newPos = center;
        newPos.x += radius * Mathf.Cos(angle);
        newPos.z += radius * Mathf.Sin(angle);

        transform.position = newPos;
        transform.rotation = Quaternion.LookRotation(
            new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
    }
}