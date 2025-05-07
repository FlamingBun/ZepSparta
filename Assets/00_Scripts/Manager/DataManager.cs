using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItmeDataPair
{
    public int key;
    public ItemSO itemSO;
}

[Serializable]
public class ItemDataWrapper
{
    public List<ItmeDataPair> itemList = new();
}

public class DataManager : SingletonMonoBehaviour<DataManager>
{
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


    #region Items
    private const string HasItemKey = "HasItems";
    private const string LookItemKey = "LookItem";
    private const string RideItemKey = "RideItem";
    #endregion Items

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

    public void SaveHasItems(Dictionary<int, ItemSO> hasItems)
    {
        ItemDataWrapper wrapper = new();

        foreach (var pair in hasItems)
        {
            wrapper.itemList.Add(new ItmeDataPair { key = pair.Key, itemSO = pair.Value });
        }

        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(HasItemKey, json);
        PlayerPrefs.Save();
    }

    public Dictionary<int, ItemSO> LoadHasItems()
    {
        Dictionary<int, ItemSO> hasItems = new Dictionary<int, ItemSO>();

        string json = PlayerPrefs.GetString(HasItemKey);

        if (!string.IsNullOrEmpty(json))
        {
            ItemDataWrapper wrapper = JsonUtility.FromJson<ItemDataWrapper>(json);

            foreach (var pair in wrapper.itemList)
            {
                hasItems[pair.key] = pair.itemSO;
            }
        }

        return hasItems;
    }

    public void SavePlayerData(int lookId, int rideId)
    {
        PlayerPrefs.SetInt(LookItemKey, lookId);
        PlayerPrefs.SetInt(RideItemKey, rideId);
    }

    public void LoadPlayerData()
    {
        GameManager.Instance.PlayerController.EquipItem(ItemManager.Instance.GetItem(PlayerPrefs.GetInt(LookItemKey)));
        GameManager.Instance.PlayerController.EquipItem(ItemManager.Instance.GetItem(PlayerPrefs.GetInt(RideItemKey)));
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
