using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StackGameOverUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private TextMeshProUGUI bestCombo;
    [SerializeField] private TextMeshProUGUI currentScore;
    [SerializeField] private TextMeshProUGUI currentCombo;
    [SerializeField] private TextMeshProUGUI getPoint;
    [SerializeField] private Button confirmButton;

    public override void Init()
    {
        confirmButton.onClick.AddListener(CloseUIs);
    }

    public override void SetActive(UIState state)
    {
        if (state == GetUIState())
        {
            SetStackScore();
        }
        base.SetActive(state);
    }

    private void SetStackScore()
    {
        DataManager dataManager = DataManager.Instance;
        bestScore.text = $"최고 점수: {dataManager.BestScore}";
        bestCombo.text = $"최고 콤보: {dataManager.BestCombo}";

        currentScore.text = $"현재 점수: {dataManager.CurrentScore}";
        currentCombo.text = $"현재 콤보: {dataManager.CurrentCombo}";

        getPoint.text = $"획득 포인트: {dataManager.CurrentAddPoint}";
    }

    protected override UIState GetUIState()
    {
        return UIState.StackGameOverUI;
    }
}
