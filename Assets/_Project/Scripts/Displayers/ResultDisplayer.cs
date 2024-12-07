using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float animationDuration = 0.5f;
    private VerticalLayoutGroup layoutGroup;
    private int currentScore = 0;

    public Button RollButton;

    void Start()
    {
        layoutGroup = scoreText.GetComponentInParent<VerticalLayoutGroup>();
    }

    public void UpdateScoreText(int newScore)
    {
        if (scoreText == null)
        {
            return;
        }

        StartCoroutine(AnimateScore(currentScore, newScore));
        currentScore = newScore;
    }

    private IEnumerator AnimateScore(int oldScore, int newScore)
    {
        layoutGroup.enabled = false;

        GameObject oldTextObject = Instantiate(scoreText.gameObject, scoreText.transform.parent);
        TMP_Text oldText = oldTextObject.GetComponent<TMP_Text>();
        oldText.text = oldScore.ToString();
        scoreText.text = newScore.ToString();

        RectTransform oldTextTransform = oldText.GetComponent<RectTransform>();
        RectTransform newTextTransform = scoreText.GetComponent<RectTransform>();

        // Move old text up
        Vector3 oldTextStartPos = oldTextTransform.localPosition;
        Vector3 oldTextEndPos = oldTextStartPos + Vector3.up * 50;

        // Move new text down
        Vector3 newTextStartPos = oldTextStartPos + Vector3.down * 50;
        newTextTransform.localPosition = newTextStartPos;

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;

            oldTextTransform.localPosition = Vector3.Lerp(oldTextStartPos, oldTextEndPos, t);
            oldText.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), t);

            newTextTransform.localPosition = Vector3.Lerp(newTextStartPos, oldTextStartPos, t);
            scoreText.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final positions and cleanup
        oldTextTransform.localPosition = oldTextEndPos;
        Destroy(oldTextObject);
        newTextTransform.localPosition = oldTextStartPos;

        layoutGroup.enabled = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
    }
}