using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotButton : MonoBehaviour
{
    [SerializeField] private Button slotButton;


    [SerializeField] private Color normalColor;
    [SerializeField] private Color hasColor;

    [SerializeField] private Image background;
    [SerializeField] private Image image;
    [SerializeField] private Image selectedFrame;
    [SerializeField] private TextMeshProUGUI itemName;

    [HideInInspector] public int itemId;

    private ItemSO itemSO;

    LookUI lookUI;

    public void SetItemInfo(ItemSO _itemSO)
    {
        itemSO = _itemSO;
        itemId = itemSO.itemId;

        image.sprite = itemSO.sprite;
        itemName.text = itemSO.name;
    }

    public void Initialize(LookUI _lookUI)
    {
        lookUI = _lookUI;
        slotButton.onClick.AddListener(OnClickItemSlot);

    }

    private void OnClickItemSlot()
    {
        lookUI.SelectSlot(itemId);
    }

    public void SetFrameActive(bool isSelected)
    {
        selectedFrame.gameObject.SetActive(isSelected);
    }

    public void SetBackgroundColor(bool hasItem)
    {
        if (hasItem)
            background.color = hasColor;
        else
            background.color = normalColor;
    }
}
