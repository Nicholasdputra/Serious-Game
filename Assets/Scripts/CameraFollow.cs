using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] GameObject milo;
    public Vector2 topLeftLimit;
    public Vector2 bottomRightLimit;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(milo.transform.position.x, topLeftLimit.x, bottomRightLimit.x),
            Mathf.Clamp(milo.transform.position.y, bottomRightLimit.y, topLeftLimit.y),
            transform.position.z);
    }
}
