using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] private float smooth_spd = 10f;
    [SerializeField] private Vector3 offset;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 target_pos = target.position + offset;
        this.transform.position = Vector3.Lerp(target_pos,this.transform.position,smooth_spd * Time.deltaTime);
    }
}