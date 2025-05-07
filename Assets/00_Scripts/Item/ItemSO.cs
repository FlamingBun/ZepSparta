using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDetailSO_", menuName = "ScriptableObjects/Item/ItemDetail")]
public class ItemSO : ScriptableObject
{
    [Header("Detail")]
    public int itemId = 0;
    public string name = "name";
    public Sprite sprite;
    public ItemType itemType;
    public int price = 1000;
    public RuntimeAnimatorController animator;

    [Header("Stat")]
    public float Health = 10f;

    [Header("Move")]
    public float Speed = 3;
}
