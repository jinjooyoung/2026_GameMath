using UnityEngine;

public class DestinationArea : MonoBehaviour
{
    public GameObject canvas;
    public Collider playerCollider;

    private void Awake()
    {
        canvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerCollider.isTrigger = false;
            canvas.SetActive(true);
        }
    }

    public void RastartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
