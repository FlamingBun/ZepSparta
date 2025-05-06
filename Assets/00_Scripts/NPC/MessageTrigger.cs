using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class MessageTrigger : MonoBehaviour
{
    public LayerMask collisionLayerMask;
    public Vector2 messageOffset;
    public GameObject messageObject;
    public string text;
    public TextMeshProUGUI messageText;

    protected void Awake()
    {
        messageText.text = text;
        messageObject.SetActive(false);
    }

    public abstract void OnClickSpaceBar();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collisionLayerMask.value == (collisionLayerMask.value | (1 << other.gameObject.layer)))
        {
            messageObject.SetActive(true);
            Vector3 pos = transform.position;
            pos.x += messageOffset.x;
            pos.y += messageOffset.y;
            messageObject.transform.position = pos;

            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.SetTriggerOn(this);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (collisionLayerMask.value == (collisionLayerMask.value | (1 << other.gameObject.layer)))
        {
            messageObject.SetActive(true);
            Vector3 pos = transform.position;
            pos.x += messageOffset.x;
            pos.y += messageOffset.y;
            messageObject.transform.position = pos;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (messageObject != null)
        {
            messageObject.SetActive(false);
        }

        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.SetTriggerOff();
        }
    }


}
