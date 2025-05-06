using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StackGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI currentCombo;
    private void Awake()
    {
        UIManager.Instance.stackGameUI = this;
    }

    public void SetScore(int score, int combo)
    {
        currentScore.text = $"현재 점수: {score}";
        currentCombo.text = $"현재 콤보: {combo}";
    }
}
