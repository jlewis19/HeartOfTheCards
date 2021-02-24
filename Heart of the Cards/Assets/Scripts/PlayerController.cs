using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 10f;

    CharacterController controller;
    Vector3 input, moveDirection;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        input *= moveSpeed;
        input.y = moveDirection.y;
        moveDirection = Vector3.Lerp(moveDirection, input, Time.deltaTime);

        controller.Move(input * Time.deltaTime); ;
    }
}
