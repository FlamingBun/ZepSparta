using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
    [SerializeField] private List<ItemSO> items;
    public List<ItemSO> Items { get { return items; } }
    private Dictionary<int, ItemSO> itemDatas;

    [SerializeField] private Dictionary<int, ItemSO> hasItems;
    protected override void Initialize()
    {
        hasItems = new Dictionary<int, ItemSO>();
        itemDatas = new Dictionary<int, ItemSO>();

        foreach (var item in items)
        {
            itemDatas.Add(item.itemId, item);
        }

        hasItems = DataManager.Instance.LoadHasItems();
    }

    public bool HaveItem(int itemId)
    {
        if (hasItems.ContainsKey(itemId))
            return true;
        else
            return false;
    }

    public void PurchaseItem(int itemId)
    {
        if (HaveItem(itemId))
        {
            Debug.Log("이미 구매한 아이템입니다.");
            return;
        }

        ItemSO item = itemDatas[itemId];

        hasItems.Add(item.itemId, item);
        DataManager.Instance.SpendPoint(item.price);
        DataManager.Instance.SaveHasItems(hasItems);
    }

    public ItemSO GetItem(int itemId)
    {
        return itemDatas[itemId];
    }
}
