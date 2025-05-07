using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RideUI : BaseUI
{
    [SerializeField] private Button exitButton;

    public override void Init()
    {
        exitButton.onClick.AddListener(CloseUIs);
    }

    protected override UIState GetUIState()
    {
        return UIState.RideUI;
    }

    private void OnEnable()
    {

    }
}
