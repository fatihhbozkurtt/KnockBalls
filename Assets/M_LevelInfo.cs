using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class M_LevelInfo : MonoBehaviour
{
    public static M_LevelInfo I;
    public GameObject GridBlockParent;
    //-----------------------
    [HideInInspector]
    public List<List<GridClass>> GridInfos;
    public GridClass[,,] GridPlan;
    public int GridSizeX;
    public int GridSizeY;
    public int PartAmount;
    //-----------------------
    [Space(15)]
    [Header("Dont Touch This Area")]
    public string LevelCode = "";
    //-----------------------
    //[HideInInspector]
    public int CurrentPartInt;
    private void Awake()
    {
        I = this;
    }
    public void LoadGrid()
    {
        GridPlan = new GridClass[PartAmount, GridSizeX, GridSizeY];
        for (int j = 0; j < PartAmount; j++)
        {
            for (int y = 0; y < GridSizeY; y++)
            {
                for (int x = 0; x < GridSizeX; x++)
                {
                    for (int i = 0; i < GridInfos[j].Count; i++)
                    {
                        if (GridInfos[j][i].PosX == x && GridInfos[j][i].PosY == y)
                        {
                            GridPlan[j, x, y] = NewSlot(
                            GridInfos[j][i].PosX,
                            GridInfos[j][i].PosY,
                            GridInfos[j][i].IsCubeActive,
                            GridInfos[j][i].BlockScore);
                        }
                    }
                }
            }
        }
    }
    public GridClass NewSlot(int x, int y, bool _IsCubeActive, int _BlockScore)
    {
        GridClass newSlot = new GridClass();
        newSlot.PosX = x;
        newSlot.PosY = y;
        newSlot.IsCubeActive = _IsCubeActive;
        newSlot.BlockScore = _BlockScore;
        return newSlot;
    }
}
//----------------------
#if UNITY_EDITOR
[CustomEditor(typeof(M_LevelInfo))]
public class LevelInfoEditor : Editor
{
    public M_LevelInfo myGridData;
    private bool GridUploaded = false;
    public override void OnInspectorGUI()
    {
        if (myGridData == null)
        {
            myGridData = (M_LevelInfo)serializedObject.targetObject;
        }

        if (!GridUploaded)
        {
            DrawDefaultInspector();
        }

        if (!Application.isPlaying)
        {
            if (myGridData != null)
            {
                if (myGridData.GridSizeX != 0 && myGridData.GridSizeY != 0)
                {
                    if (!GridUploaded)
                    {
                        GUILayout.Space(15);
                        if (GUILayout.Button("Show Grid"))
                        {
                            GridUploaded = true;

                            if (myGridData.CurrentPartInt < 1)
                            {
                                myGridData.CurrentPartInt = 1;
                            }
                            if (myGridData.CurrentPartInt > myGridData.PartAmount)
                            {
                                myGridData.CurrentPartInt = myGridData.PartAmount;
                            }

                            if (myGridData.GridInfos == null)
                            {
                                InstanceGrid();
                            }
                            else
                            {
                                UpdateGrid();
                            }
                        }
                    }
                    if (GridUploaded)
                    {
                        GUILayout.Space(15);
                        DrawGrid();

                        GUILayout.Space(15);
                        GUI.color = Color.green;
                        if (GUILayout.Button("Save Grid"))
                        {
                            SaveGrid();
                            EditorUtility.SetDirty(myGridData);
                        }
                    }
                }

            }
        }
    }
    public void UpdateGrid()
    {
        myGridData.GridPlan = new GridClass[myGridData.PartAmount, myGridData.GridSizeX, myGridData.GridSizeY];
        for (int j = 0; j < myGridData.PartAmount; j++)
        {
            for (int y = 0; y < myGridData.GridSizeY; y++)
            {
                for (int x = 0; x < myGridData.GridSizeX; x++)
                {
                    for (int i = 0; i < myGridData.GridInfos[j].Count; i++)
                    {
                        if (myGridData.GridInfos[j][i].PosX == x && myGridData.GridInfos[j][i].PosY == y)
                        {
                            myGridData.GridPlan[j, x, y] = myGridData.NewSlot(
                            myGridData.GridInfos[j][i].PosX,
                            myGridData.GridInfos[j][i].PosY,
                            myGridData.GridInfos[j][i].IsCubeActive,
                            myGridData.GridInfos[j][i].BlockScore);
                        }
                    }
                }
            }
        }
    }
    public void InstanceGrid()
    {
        if (myGridData.GridPlan == null)
        {
            myGridData.GridPlan = new GridClass[myGridData.PartAmount, myGridData.GridSizeX, myGridData.GridSizeY];
            for (int j = 0; j < myGridData.PartAmount; j++)
            {
                for (int y = 0; y < myGridData.GridSizeY; y++)
                {
                    for (int x = 0; x < myGridData.GridSizeX; x++)
                    {
                        myGridData.GridPlan[j, x, y] = myGridData.NewSlot(x, y, false, 0);
                    }
                }
            }
        }
    }
    public void DrawGrid()
    {
        if (myGridData == null)
        {
            return;
        }
        else if (myGridData.GridPlan == null)
        {
            return;
        }
        else if (myGridData.GridPlan[0, 0, 0] == null)
        {
            return;
        }

        if (myGridData.CurrentPartInt < 1)
        {
            myGridData.CurrentPartInt = 1;
        }
        if (myGridData.CurrentPartInt > myGridData.PartAmount)
        {
            myGridData.CurrentPartInt = myGridData.PartAmount;
        }

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Part Int");
        myGridData.CurrentPartInt = EditorGUILayout.IntField(myGridData.CurrentPartInt, GUILayout.Width(20));
        EditorGUILayout.EndHorizontal();

        if (myGridData.CurrentPartInt < 1)
        {
            myGridData.CurrentPartInt = 1;
        }
        if (myGridData.CurrentPartInt > myGridData.PartAmount)
        {
            myGridData.CurrentPartInt = myGridData.PartAmount;
        }

        Color outlineColor = Color.black;
        int a = 50;
        int spacing = 10;

        for (int y = 0; y < myGridData.GridSizeY; y++)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            for (int x = 0; x < myGridData.GridSizeX; x++)
            {
                GridClass slot = myGridData.GridPlan[myGridData.CurrentPartInt - 1, x, y];
                if (slot == null)
                {
                    myGridData.GridPlan[myGridData.CurrentPartInt - 1, x, y] = myGridData.NewSlot(x, y, false, 0);
                    slot = myGridData.GridPlan[myGridData.CurrentPartInt - 1, x, y];
                }

                Rect rect = EditorGUILayout.BeginVertical(GUILayout.Width(a), GUILayout.Height(a));
                Color ActiveBoxColor = Color.grey;

                if (slot.IsCubeActive)
                {
                    ActiveBoxColor = Color.green;
                }
                EditorGUI.DrawRect(new Rect(rect.x - 2, rect.y - 2, rect.width + 4, rect.height + 4), ActiveBoxColor);
                GUILayout.Space(3);

                //-----
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Is Active");
                GUILayout.Space(14);
                slot.IsCubeActive = EditorGUILayout.Toggle(slot.IsCubeActive);
                EditorGUILayout.EndHorizontal();

                //-----
                EditorGUILayout.BeginHorizontal();
                if (!slot.IsCubeActive)
                {
                    GUILayout.Label("");
                    slot.BlockScore = 0;
                }
                else
                {
                    GUILayout.Label("Score");
                    slot.BlockScore = EditorGUILayout.IntField(slot.BlockScore, GUILayout.Width(20));
                }
                EditorGUILayout.EndHorizontal();

                //-----
                EditorGUILayout.EndVertical();
                if (x < myGridData.GridSizeX - 1)
                {
                    GUILayout.Space(spacing);
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            if (y < myGridData.GridSizeY - 1)
            {
                GUILayout.Space(spacing);
            }
        }
    }
    public void SaveGrid()
    {
        myGridData.GridInfos = new List<List<GridClass>>();
        for (int j = 0; j < myGridData.PartAmount; j++)
        {
            List<GridClass> NewGC = new List<GridClass>();
            for (int y = 0; y < myGridData.GridSizeY; y++)
            {
                for (int x = 0; x < myGridData.GridSizeX; x++)
                {
                    NewGC.Add(myGridData.NewSlot(x, y, myGridData.GridPlan[j, x, y].IsCubeActive, myGridData.GridPlan[j, x, y].BlockScore));
                }
            }
            myGridData.GridInfos.Add(NewGC);
        }
        GridUploaded = false;

        myGridData.LevelCode = "";
        string glyphs = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ0123456789";
        int charAmount = Random.Range(15, 20); //set those to the minimum and maximum length of your string
        for (int i = 0; i < charAmount; i++)
        {
            myGridData.LevelCode += glyphs[Random.Range(0, glyphs.Length)];
        }
    }
}
#endif
