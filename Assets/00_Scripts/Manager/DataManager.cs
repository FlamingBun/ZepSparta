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
    private int bestScore = 0;
    public int BestScore { get => bestScore; }

    private int bestCombo = 0;
    public int BestCombo { get => bestCombo; }

    private int currentScore = 0;
    public int CurrentScore { get => currentScore; }
    private int currentCombo = 0;
    public int CurrentCombo { get => currentCombo; }

    private const string BestScorekey = "BestScore";
    private const string BestComboKey = "BestCombo";
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
        bestScore = PlayerPrefs.GetInt(BestScorekey, 0);
        bestCombo = PlayerPrefs.GetInt(BestComboKey, 0);
    }

    public void UpdateStackScore(int stackCount, int maxCombo)
    {
        currentScore = stackCount;
        currentCombo = maxCombo;

        if (bestScore < stackCount)
        {
            bestScore = stackCount;
            bestCombo = maxCombo;

            PlayerPrefs.SetInt(BestScorekey, bestScore);
            PlayerPrefs.SetInt(BestComboKey, bestCombo);
        }
        currentAddPoint = stackCount * 100;
        EarnPoint(currentAddPoint);
    }

    public void EarnPoint(int addPoint)
    {
        point += addPoint;
        currentAddPoint = addPoint;
        OnChangePoint?.Invoke(point);
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
