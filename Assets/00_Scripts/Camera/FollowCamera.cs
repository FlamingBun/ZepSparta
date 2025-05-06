using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public float followSpeed = 10f;
    public Vector2 minBound = new Vector2(-29, -30);
    public Vector2 maxBound = new Vector2(35, 32);

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            if (GameManager.Instance.Player != null)
                target = GameManager.Instance.Player.transform;
            else return;
        }

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(target.position.x, minBound.x, maxBound.x);
        pos.y = Mathf.Clamp(target.position.y, minBound.y, maxBound.y);

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * followSpeed);
    }
}
