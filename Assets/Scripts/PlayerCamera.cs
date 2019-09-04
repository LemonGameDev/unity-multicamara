using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private List<Transform> targets = new List<Transform>();
    [SerializeField] private Vector3 offsetDistance;

    [Header("Params")] [SerializeField] private float followSpeed = 10;
    [SerializeField] private float lookSpeed = 10;


    private Bounds bounds;


    private void LateUpdate()
    {
        if (targets.Count == 0) return;
        LookAtTarget();
        MoveWithTarget();
    }


    void LookAtTarget()
    {
        var lookDirection = GetCenterPoint() - transform.position;
        var step = lookSpeed * Time.deltaTime;
        var newDir = Vector3.RotateTowards(transform.forward, lookDirection, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void MoveWithTarget()
    {
        // var targetPos = GetCenterPoint() + offsetDistance;
        var targetPos = GetCenterPoint() + targets[0].position + offsetDistance;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }


    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1) return targets[0].position;

        bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (var t in targets)
        {
            bounds.Encapsulate(t.position);
        }

        return bounds.center;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetCenterPoint(), 0.5f);
    }
}