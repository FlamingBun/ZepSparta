using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankUI : BaseUI
{
    [SerializeField] private Button exitButton;

    [SerializeField] private TextMeshProUGUI stackScore;
    [SerializeField] private TextMeshProUGUI stackCombo;
    // [SerializeField] private TextMeshProUGUI runScore;

    public override void Init()
    {
        exitButton.onClick.AddListener(CloseUIs);
    }

    private void OnEnable()
    {
        DataManager dataManager = DataManager.Instance;
        stackScore.text = $"최고 점수: {dataManager.StackBestScore}";
        stackCombo.text = $"최고 콤보: {dataManager.StackBestCombo}";
    }


    protected override UIState GetUIState()
    {
        return UIState.RankUI;
    }
}
