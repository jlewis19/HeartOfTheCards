using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 10f;
    public float dashSpeed = 200f;
    public float dashCooldown = 3f;

    CharacterController controller;
    Vector3 input, moveDirection;
    bool canDash = true;
    float dashCooldownTimer = 0f;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (!canDash)
        {
            dashCooldownTimer += Time.deltaTime;
            if (dashCooldownTimer >= dashCooldown)
            {
                canDash = true;
                dashCooldownTimer = 0f;
            }
        }

        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        if (Input.GetKeyDown(KeyCode.R) && canDash)
        {
            input *= dashSpeed;
            canDash = false;
        }
        else
        {
            input *= moveSpeed;
        }

        input.y = moveDirection.y;
        moveDirection = Vector3.Lerp(moveDirection, input, Time.deltaTime);

        controller.Move(input * Time.deltaTime);

        /*
        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        input *= moveSpeed;
        input.y = moveDirection.y;
        moveDirection = Vector3.Lerp(moveDirection, input, Time.deltaTime);

        controller.Move(input * Time.deltaTime);
        */
    }
}
