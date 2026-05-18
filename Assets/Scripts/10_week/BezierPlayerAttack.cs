using UnityEngine;

public class BezierPlayerAttack : MonoBehaviour
{
    public GameObject spherePrefab;

    public Transform firePoint;

    public Transform enemy;

    public void OnAttack()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj =
                Instantiate(
                    spherePrefab,
                    firePoint.position,
                    Quaternion.identity);

            obj.GetComponent<BezierProjectile>()
                .Init(
                    firePoint.position,
                    enemy.position,
                    2f);
        }
    }
}
