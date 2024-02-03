using UnityEngine;
using System;

public class GroundBrickCatcher : MonoSingleton<GroundBrickCatcher>
{
    public event Action<TargetBrickController> TargetBrickCaughtEvent;
    public event Action PlatformIsClearedEvent;

    [Header("References")]
    [SerializeField] Transform bricksParent;

    [Header("Debug")]
    [SerializeField] int totalBricksCount;
    [SerializeField] int caughtBricksCount;

    private void Start()
    {
        BricksSwapper.instance.NewBrickParentAssignedEvent += OnNewBrickParentAssigned;
        bricksParent = BricksSwapper.instance.GetActiveBrickParent();
        totalBricksCount = bricksParent.childCount;
    }

    private void OnNewBrickParentAssigned(Transform newBrick)
    {
        bricksParent = newBrick;
        totalBricksCount = bricksParent.childCount;
        caughtBricksCount = 0;
    }

    public void IncrementCaughtBricksCount(TargetBrickController targetBrick)
    {
        caughtBricksCount++;
        TargetBrickCaughtEvent?.Invoke(targetBrick);
        if (DidAllBricksCatch())
        {
            PlatformIsClearedEvent?.Invoke();
        }
    }

    public bool DidAllBricksCatch()
    {
        bool status = caughtBricksCount >= totalBricksCount;
        return status;
    }
}
