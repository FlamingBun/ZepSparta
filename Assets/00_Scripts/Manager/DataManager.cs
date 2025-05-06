using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    private static DataManager instance;


    #region Status
    private float speed = 3;

    #endregion Status

    #region Point
    private int point = 0;
    public int Point { get => point; }
    private int currentAddPoint = 0;
    public int CurrentAddPoint { get => currentAddPoint; }

    private const string PointKey = "Point";

    private Action<int> OnChangePoint;
    #endregion Point

    #region Stack Score
    private int stackBestScore = 0;
    public int StackBestScore { get => stackBestScore; }

    private int stackBestCombo = 0;
    public int StackBestCombo { get => stackBestCombo; }

    private int stackCurrentScore = 0;
    public int StackCurrentScore { get => stackCurrentScore; }
    private int stackCurrentCombo = 0;
    public int StackCurrentCombo { get => stackCurrentCombo; }

    private const string StackBestScorekey = "StackBestScore";
    private const string StackBestComboKey = "StackBestCombo";
    #endregion Stack Score


    protected override void Initialize()
    {
        GetPoint();
        GetStackData();
    }

    private void GetPoint()
    {
        point = PlayerPrefs.GetInt(PointKey, 0);
    }

    private void GetStackData()
    {
        // Stack
        stackBestScore = PlayerPrefs.GetInt(StackBestScorekey, 0);
        stackBestCombo = PlayerPrefs.GetInt(StackBestComboKey, 0);
    }

    public void UpdateStackScore(int stackCount, int maxCombo)
    {
        stackCurrentScore = stackCount;
        stackCurrentCombo = maxCombo;

        if (stackBestScore < stackCount)
        {
            stackBestScore = stackCount;
            stackBestCombo = maxCombo;

            PlayerPrefs.SetInt(StackBestScorekey, stackBestScore);
            PlayerPrefs.SetInt(StackBestComboKey, stackBestCombo);
        }
        currentAddPoint = stackCount * 100;
        EarnPoint(currentAddPoint);
    }

    public void EarnPoint(int addPoint)
    {
        point += addPoint;
        currentAddPoint = addPoint;
        OnChangePoint?.Invoke(point);
        PlayerPrefs.SetInt(PointKey, point);
    }

    public bool SpendPoint(int minusPoint)
    {
        if (point >= minusPoint)
        {
            point -= minusPoint;
            OnChangePoint?.Invoke(point);
            PlayerPrefs.SetInt(PointKey, point);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddPointChangeEvent(Action<int> action)
    {
        OnChangePoint += action;
    }

    public void RemovePointChangeEvent(Action<int> action)
    {
        OnChangePoint -= action;
    }
}
