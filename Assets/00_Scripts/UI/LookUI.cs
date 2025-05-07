using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookUI : BaseUI
{
    [SerializeField] private Button exitButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private Transform content;
    [SerializeField] GameObject slotPrefab;

    private List<ItemSO> items;

    private List<SlotButton> slotList;

    private int selectedItemId;

    public override void Init()
    {
        exitButton.onClick.AddListener(CloseUIs);

        slotList = new List<SlotButton>();

        items = ItemManager.Instance.Items;
        MakeSlot();
    }

    protected override UIState GetUIState()
    {
        return UIState.LookUI;
    }

    private void OnEnable()
    {
        SelectSlot(0);
    }

    private void MakeSlot()
    {
        foreach (var item in items)
        {
            if (item.itemType == ItemType.Look)
            {
                GameObject slot = Instantiate(slotPrefab, content);
                SlotButton slotButton = slot.GetComponent<SlotButton>();
                slotButton.SetItemInfo(item);
                slotButton.Initialize(this);
                slotList.Add(slotButton);
            }
        }
    }

    public void SelectSlot(int _itemId)
    {
        selectedItemId = _itemId;

        if (ItemManager.Instance.HaveItem(_itemId))
        {
            equipButton.gameObject.SetActive(true);
            purchaseButton.gameObject.SetActive(false);

            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(OnClickEquipButton);
        }
        else
        {
            equipButton.gameObject.SetActive(false);
            purchaseButton.gameObject.SetActive(true);

            purchaseButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.AddListener(OnClickPurchaseButton);
        }

        SetSelectedFrame();
    }

    private void SetSelectedFrame()
    {
        foreach (var slot in slotList)
        {
            slot.SetBackgroundColor(ItemManager.Instance.HaveItem(slot.itemId));

            if (slot.itemId == selectedItemId)
                slot.SetFrameActive(true);
            else
                slot.SetFrameActive(false);
        }
    }

    private void OnClickEquipButton()
    {
        ItemSO itemSO = ItemManager.Instance.GetItem(selectedItemId);
        Debug.Log("selected Item Id : " + selectedItemId);
        Debug.Log("selected Item name : " + itemSO.name);
        GameManager.Instance.PlayerController.EquipItem(itemSO);
    }

    private void OnClickPurchaseButton()
    {
        ItemManager.Instance.PurchaseItem(selectedItemId);
        SelectSlot(selectedItemId);
    }
}
