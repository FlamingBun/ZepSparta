using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookButton : MonoBehaviour
{
    [SerializeField] private Button lookButton;
    private void Awake()
    {
        lookButton.onClick.AddListener(OpenLookUI);
    }

    private void OpenLookUI()
    {
        UIManager.Instance.ChangeState(UIState.LookUI);
    }
}
