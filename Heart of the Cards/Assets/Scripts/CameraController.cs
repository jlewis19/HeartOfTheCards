﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform playerBody;
    float pitch = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = transform.parent.transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!MouseSensitivityManager.active)
        {
            float mouseX = Input.GetAxis("Mouse X") * LevelManager.mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * LevelManager.mouseSensitivity * Time.deltaTime;

            playerBody.Rotate(Vector3.up * mouseX);

            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
    }
}
