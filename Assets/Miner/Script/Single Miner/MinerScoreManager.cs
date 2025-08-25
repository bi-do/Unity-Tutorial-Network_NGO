using TMPro;
using UnityEngine;

public class MinerScoreManager : MonoBehaviour
{
    public int score;
    [SerializeField] private TextMeshProUGUI text_UI;

    void Start()
    {
        text_UI.text = $"Current Mineral : {score}";
    }

    public void AddScore()
    {
        this.score++;
        text_UI.text = $"Current Mineral : {score}";

    }
}