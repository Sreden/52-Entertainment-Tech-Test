using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    public Button rollButton;

    public void UpdateScoreText(int newScore)
    {
        if (scoreText == null)
        {
            return;
        }

        scoreText.text = newScore.ToString();
    }
}
