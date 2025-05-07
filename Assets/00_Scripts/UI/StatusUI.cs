using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI pointText;

    private void Start()
    {
        speedText.text = $"이동 속도: {GameManager.Instance.PlayerController.GetSpeed()}";
        pointText.text = $"내 포인트: {DataManager.Instance.Point}";
        GameManager.Instance.PlayerController.AddSpeedChangeEvent(ChangeSpeed);
    }

    private void OnEnable()
    {
        DataManager.Instance.AddPointChangeEvent(ChangePoint);
    }

    private void ChangePoint(int point)
    {
        pointText.text = $"내 포인트: {point}";
    }

    private void ChangeSpeed(int speed)
    {
        speedText.text = $"이동 속도: {speed}";
    }
}
