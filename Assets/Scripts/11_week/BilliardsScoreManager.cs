using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BilliardsScoreManager : MonoBehaviour
{
    public TextMeshProUGUI p1Text;
    public TextMeshProUGUI p2Text;
    public TextMeshProUGUI winner;

    int p1Score;
    int p2Score;

    public GameObject endCanvas;

    private void Start()
    {
        endCanvas.SetActive(false);
    }

    public void AddScore(int player, int amount)
    {
        Debug.Log($"AddScore 호출됨 : player {player}/ amount {amount}");

        if (player == 1)
        {
            p1Score =
                Mathf.Max(0, p1Score + amount);

            Debug.Log($"1p 점수 : {p1Score}");
        }
        else
        {
            p2Score =
                Mathf.Max(0, p2Score + amount);

            Debug.Log($"2p 점수 : {p2Score}");
        }

        UpdateUI();

        if (p1Score >= 5 || p2Score >= 5)
        {
            endCanvas.SetActive(true);

            winner.text = $"승자 : {(p1Score >= 5 ? "1p" : "2p")}";
        }
    }

    void UpdateUI()
    {
        p1Text.text =
            $"1P : {p1Score}점";

        p2Text.text =
            $"2P : {p2Score}점";
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }
}
