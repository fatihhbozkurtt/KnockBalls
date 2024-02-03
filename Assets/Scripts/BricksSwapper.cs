using UnityEngine;
using System;
public class BricksSwapper : MonoSingleton<BricksSwapper>
{
    public event Action<Transform> NewBrickParentAssignedEvent;

    [Header("Debug")]
    [SerializeField] BrickParentsIdentifier[] bricksParents;
    [SerializeField] int activeParentIndex;
    [SerializeField] int maxBrickParentCount;
    const string lastParentIndexSaveName = "lastParentIndex_";

    public void Starter()
    {
        base.Awake();
        activeParentIndex = PlayerPrefs.GetInt(lastParentIndexSaveName, defaultValue: 0);
        bricksParents = GetComponentsInChildren<BrickParentsIdentifier>(includeInactive: true);
        maxBrickParentCount = bricksParents.Length;

        ActivateLastBrickParent();
    }
    private void Start()
    {
        GroundBrickCatcher.instance.PlatformIsClearedEvent += OnPlatformCleared;
        GameManager.instance.LevelEndedEvent += OnLevelEnded;
    }

    private void OnPlatformCleared()
    {
        activeParentIndex++;
        if (activeParentIndex >= maxBrickParentCount)
        {
            GameManager.instance.EndGame(success: true);
            return;
        }

        PlayerPrefs.SetInt(lastParentIndexSaveName, activeParentIndex);
        ActivateLastBrickParent();
    }

    public Transform GetActiveBrickParent()
    {
        return bricksParents[activeParentIndex].transform;
    }

    void ActivateLastBrickParent()
    {
        if (bricksParents.Length == 0) Debug.LogError("There is no brick parent to activate!!!");

        for (int i = 0; i < bricksParents.Length; i++)
        {
            bricksParents[i].gameObject.SetActive(false);
        }

        Transform newParent = bricksParents[activeParentIndex].transform;
        newParent.gameObject.SetActive(true);
        NewBrickParentAssignedEvent?.Invoke(newParent);
    }

    private void OnLevelEnded()
    {
        PlayerPrefs.SetInt(lastParentIndexSaveName, 0);
    }
}
