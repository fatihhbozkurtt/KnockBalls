using TMPro;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI bestScoreTxt;

    [Header("Configuration")]
    [SerializeField] public static readonly string bestScoreSaveName = "bestScore_";

    [Header("Debug")]
    [SerializeField] int totalCurrentScore;

    private void Start()
    {
        GroundBrickCatcher.instance.TargetBrickCaughtEvent += OnBrickCaught;
        SetText(bestScoreTxt, 0);
    }

    private void OnBrickCaught(TargetBrickController targetBrick)
    {
        int pointValue = targetBrick.pointValue;

        totalCurrentScore += pointValue;
        SetText(bestScoreTxt, totalCurrentScore);
    }

    void SetText(TextMeshProUGUI targetText, int value)
    {
        targetText.text = "BEST: " + value.ToString();
    }
}
