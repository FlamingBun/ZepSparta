using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI pointText;

    private void Awake()
    {
        pointText.text = $"내 포인트: {DataManager.Instance.Point}";
    }

    private void OnEnable()
    {
        DataManager.Instance.AddPointChangeEvent(ChangePoint);
    }

    private void ChangePoint(int point)
    {
        pointText.text = $"내 포인트: {point}";
    }
}
