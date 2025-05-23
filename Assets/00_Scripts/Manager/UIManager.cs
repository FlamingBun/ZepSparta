using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    private UIState currentState;

    private List<BaseUI> UIs;

    #region Stack UI
    [SerializeField] private StackUI stackUI;
    [HideInInspector] public StackGameUI stackGameUI;
    [SerializeField] private StackGameOverUI stackGameOverUI;
    #endregion StackUI

    [SerializeField] private GameObject leftUI;
    [SerializeField] private RankUI rankUI;
    [SerializeField] private LookUI lookUI;
    [SerializeField] private RideUI rideUI;


    protected override void Initialize()
    {
        stackUI.Init();
        stackGameOverUI.Init();
        rankUI.Init();
        lookUI.Init();
        rideUI.Init();

        UIs = new List<BaseUI>();

        UIs.Add(stackUI);
        UIs.Add(stackGameOverUI);
        UIs.Add(rankUI);
        UIs.Add(lookUI);
        UIs.Add(rideUI);

        ChangeState(UIState.None);
    }

    public void ChangeState(UIState state)
    {
        if (currentState == state)
            return;

        currentState = state;
        foreach (var ui in UIs)
        {
            ui.SetActive(currentState);
        }

        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);

    }

    public void SetLeftUIActive(bool isMainScene)
    {
        leftUI.SetActive(isMainScene);
    }

}
