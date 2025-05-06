using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankButton : MonoBehaviour
{
    [SerializeField] private Button rankButton;

    private void Awake()
    {
        rankButton.onClick.AddListener(OpenRankUI);
    }

    private void OpenRankUI()
    {
        UIManager.Instance.ChangeState(UIState.RankUI);
    }
}
