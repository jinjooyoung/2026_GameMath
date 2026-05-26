using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 gravity = new Vector3(0, -9.81f, 0);
    public bool canMove = false;
    float damping = 0.9f;   // °¨¼è °è¼ö
    public BombExplode explode;
    [SerializeField] private int count = 0;

    private void Awake()
    {
        if (explode == null) explode = gameObject.GetComponent<BombExplode>();
    }

    void Update()
    {
        if (!canMove) return;

        velocity += gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!canMove) return;

        if (collision.gameObject.tag == "Enemy")
        {
            explode.Explode();
            return;
        }

        count++;

        if (count >= 3)
        {
            explode.Explode();
            return;
        }

        Vector3 normal = collision.contacts[0].normal.normalized;

        float dot = Vector3.Dot(velocity, normal);
        Vector3 reflect = velocity - 2f * dot * normal;

        velocity = reflect * damping;
    }
}
