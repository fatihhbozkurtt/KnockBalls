using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallCounterManager : MonoSingleton<BallCounterManager>
{
    [Header("Cofiguration")]
    [SerializeField] int maxBallCount;

    [Header("References")]
    [SerializeField] TextMeshProUGUI text;

    [Header("Debug")]
    [SerializeField] int remainingBallCount;
    [SerializeField] int fallenBlocks;


    private void Start()
    {
        remainingBallCount = maxBallCount;
        text.text = remainingBallCount.ToString();
        ShooterManager.instance.ShootingPerformedSuccessfullyEvent += OnShootingPerformed;
    }

    private void OnShootingPerformed()
    {
        remainingBallCount--;
        text.text = remainingBallCount.ToString();

        if (remainingBallCount <= 0) 
            StartCoroutine(EndGameCheckRoutine());
    }

    public int GetRemainingBallCount()
    {
        return remainingBallCount;
    }

    public bool HasEnoughBall()
    {
        bool hasBall = (remainingBallCount > 0);
        return hasBall;
    }

    IEnumerator EndGameCheckRoutine()
    {
        yield return new WaitForSeconds(2f); //TODO: Check if the all blocks are cleared off the platform.

        if (!GroundBrickCatcher.instance.DidAllBricksCatch())
            GameManager.instance.EndGame(success: false);
    }
}
