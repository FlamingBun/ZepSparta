using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackUI : BaseUI
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    public override void Init()
    {
        startButton.onClick.AddListener(StackGameStart);
        exitButton.onClick.AddListener(CloseUIs);
    }

    protected override UIState GetUIState()
    {
        return UIState.StackUI;
    }

    private void StackGameStart()
    {
        UIManager.Instance.ChangeState(UIState.None);
        GameManager.Instance.StackGameStart();
    }


}
