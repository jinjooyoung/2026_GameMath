using UnityEngine;
using UnityEngine.InputSystem;

public class BombAttack : MonoBehaviour
{
    public GameObject bombPrefab;
    public Transform attactPoint;
    public Transform plantPoint;
    public bool isPlanted = false;
    public float force = 3.0f;
    public float explodeDelay = 3.0f;

    public void OnAttack(InputValue value)
    {
        GameObject createdBombOBJ = Instantiate(bombPrefab, attactPoint);
        createdBombOBJ.transform.SetParent(null);
        Bomb bomb = createdBombOBJ.GetComponent<Bomb>();
        bomb.velocity = transform.forward * force;
        bomb.canMove = true;
    }

    public void OnRightClick(InputValue value)
    {
        if (isPlanted) return;

        GameObject createdBombOBJ = Instantiate(bombPrefab, plantPoint);
        isPlanted = true;
        createdBombOBJ.transform.SetParent(null);
        Bomb bomb = createdBombOBJ.GetComponent<Bomb>();
        bomb.canMove = false;

        bomb.explode.ExplodeDelay(explodeDelay);
        Invoke(nameof(TurnToFalse), explodeDelay);
    }

    void TurnToFalse()
    {
        isPlanted = false;
    }
}
