using System;
using System.Collections;
using System.Collections.Generic;
using Creature.Player;
using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField][Range(0f, 1f)] private float cameraSpeed = 0.5f;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    void Update() {
        Debug.DrawLine(transform.position, target.position + offset);
        transform.position = Vector3.Lerp(
                transform.position, 
                target.position + offset, 
                cameraSpeed * Time.deltaTime);
    }
}
