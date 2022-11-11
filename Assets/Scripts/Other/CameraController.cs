using System;
using System.Collections;
using System.Collections.Generic;
using Creature.Player;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float cameraSpeed = 10f;
    [SerializeField][Range(0f, 360f)] private float rotationSpeed;

    private Vector3 startPosOffset;
    private Vector3 posOffset;

    private Vector3 startRot;
    private float rotOffset = 0f;

    private void Start()
    {
        posOffset = startPosOffset = transform.position - target.position;
        startRot = transform.rotation.eulerAngles;
    }

    void Update() {
        if (Input.GetKeyDown("r"))
        {
            posOffset = startPosOffset;
            rotOffset = 0f;
        }

        float rotDir = (Input.GetKey("q") ? -1 : 0) + (Input.GetKey("e") ? 1 : 0);
        rotOffset += rotDir * rotationSpeed * Time.deltaTime;
        Vector2 newOffset2D = MyMath.RotateVector2(new Vector2(startPosOffset.x, startPosOffset.z), rotOffset * Mathf.Deg2Rad);
        posOffset = new Vector3(newOffset2D.x, posOffset.y, newOffset2D.y);

        transform.position = Vector3.Lerp(transform.position, target.position + posOffset, cameraSpeed * Time.deltaTime);

        float lerpedRot = Mathf.LerpAngle(transform.rotation.eulerAngles.y, startRot.y - rotOffset, cameraSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(startRot.x, lerpedRot, startRot.z);
    }
}
