using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MessageTrigger
{
    [SerializeField] private WarpPoint warpPoint;
    [SerializeField] private Vector2 eastPosition;
    [SerializeField] private Vector2 westPosition;
    [SerializeField] private Vector2 southPosition;
    [SerializeField] private Vector2 northPosition;

    public override void OnClickSpaceBar()
    {
        Transform playerTransform = GameManager.Instance.Player.transform;
        Transform cameraTransform = Camera.main.transform;
        Vector3 pos;
        switch (warpPoint)
        {
            case WarpPoint.East:
                playerTransform.position = eastPosition;
                pos = new Vector3(eastPosition.x, eastPosition.y, cameraTransform.position.z);
                cameraTransform.position = pos;
                break;
            case WarpPoint.West:
                playerTransform.position = westPosition;
                pos = new Vector3(westPosition.x, westPosition.y, cameraTransform.position.z);
                cameraTransform.position = pos;
                break;
            case WarpPoint.South:
                playerTransform.position = southPosition;
                pos = new Vector3(southPosition.x, southPosition.y, cameraTransform.position.z);
                cameraTransform.position = pos;
                break;
            case WarpPoint.North:
                playerTransform.position = northPosition;
                pos = new Vector3(northPosition.x, northPosition.y, cameraTransform.position.z);
                cameraTransform.position = pos;
                break;
        }
    }
}
