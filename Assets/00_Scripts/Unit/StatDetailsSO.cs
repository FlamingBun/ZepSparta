using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatDetailsSO_", menuName = "ScriptableObjects/Unit/StatDetails")]
public class StatDetailsSO : ScriptableObject
{
    public ItemSO currentLookItem;
    public ItemSO currentRideItem;
    [Header("Stat")]
    public float Health = 10f;

    [Header("Move")]
    public float Speed = 3;
}
