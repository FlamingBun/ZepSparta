using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RideButton : MonoBehaviour
{
    [SerializeField] private Button rideButton;
    private void Awake()
    {
        rideButton.onClick.AddListener(OpenRideUI);
    }

    private void OpenRideUI()
    {
        UIManager.Instance.ChangeState(UIState.RideUI);
    }
}
