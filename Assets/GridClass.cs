using UnityEngine;

[System.Serializable]
public class GridClass
{
    public int PosX;
    public int PosY;
    public bool IsCubeActive;
    public int BlockScore;

    public GameObject GridBlockObject;
    public GameObject GridObject;

    public bool Visited = false;
}
