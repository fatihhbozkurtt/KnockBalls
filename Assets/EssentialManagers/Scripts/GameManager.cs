using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public static readonly string lastPlayedStageKey = "n_lastPlayedStage";
    public static readonly string randomizeStagesKey = "n_randomizeStages";
    public static readonly string cumulativeStagePlayedKey = "n_cumulativeStages";

    [HideInInspector] public bool isLevelActive = false;
    [HideInInspector] public bool isLevelSuccessful = false;

    public event System.Action LevelStartedEvent;
    public event System.Action LevelEndedEvent; // fired regardless of fail or success
    public event System.Action LevelSuccessEvent; // fired only on success
    public event System.Action LevelFailedEvent; // fired only on fail
    public event System.Action LevelAboutToChangeEvent; // fired just before next level load

    public List<GameObject> LevelPath = new List<GameObject>();
    public GameObject CubePrefab;

    protected override void Awake()
    {
        base.Awake();

        if (!PlayerPrefs.HasKey(cumulativeStagePlayedKey)) PlayerPrefs.SetInt(cumulativeStagePlayedKey, 1);

        Application.targetFrameRate = 999;
        QualitySettings.vSyncCount = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        
    }

    public void Start()
    {
        int LevelNumber = PlayerPrefs.GetInt("LevelNumber", 0);

        Instantiate(LevelPath[LevelNumber], Vector3.zero, Quaternion.identity);

        M_LevelInfo.I.LoadGrid();

        for (int i = 0; i < M_LevelInfo.I.PartAmount; i++)
        {
            GameObject newBricksParent = new GameObject("BrickParent");
            newBricksParent.AddComponent<BrickParentsIdentifier>();

            for (int x = 0; x < M_LevelInfo.I.GridSizeX; x++)
            {
                for (int y = 0; y < M_LevelInfo.I.GridSizeY; y++)
                {
                    if (M_LevelInfo.I.GridPlan[i, x, y].IsCubeActive)
                    {
                        GameObject cloneCubePrefab = Instantiate(CubePrefab, Vector3.zero, CubePrefab.transform.rotation);
                        cloneCubePrefab.transform.SetParent(newBricksParent.transform);
                        cloneCubePrefab.transform.localPosition = new Vector3(x, y, 0);
                    }
                }
            }

            newBricksParent.transform.SetParent(BricksSwapper.instance.transform);
        }

        BricksSwapper.instance.Starter();
    }

    public void StartGame()
    {
        

        isLevelActive = true;
        LevelStartedEvent?.Invoke();
    }

    public void EndGame(bool success)
    {
        isLevelActive = false;
        isLevelSuccessful = success;

        LevelEndedEvent?.Invoke();
        if (success)
        {
            LevelSuccessEvent?.Invoke();
        }
        else
        {
            LevelFailedEvent?.Invoke();

        }
    }

    public void NextStage()
    {
        PlayerPrefs.SetInt(cumulativeStagePlayedKey, PlayerPrefs.GetInt(cumulativeStagePlayedKey, 1) + 1);

        int targetScene;

        if (PlayerPrefs.GetInt(randomizeStagesKey, 0) == 0)
        {
            targetScene = SceneManager.GetActiveScene().buildIndex + 1;
            if (targetScene == SceneManager.sceneCountInBuildSettings)
            {
                targetScene = RandomStage();
                PlayerPrefs.SetInt(randomizeStagesKey, 1);
            }
        }

        else
        {
            targetScene = RandomStage();
        }

        PlayerPrefs.SetInt(lastPlayedStageKey, targetScene);
        LevelAboutToChangeEvent?.Invoke();
        SceneManager.LoadScene(targetScene);
    }

    public void RestartStage()
    {
        LevelAboutToChangeEvent?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private int RandomStage()
    {
        return Random.Range(2, SceneManager.sceneCountInBuildSettings);
    }

    public int GetTotalStagePlayed()
    {
        return PlayerPrefs.GetInt(cumulativeStagePlayedKey, 1);
    }
}
