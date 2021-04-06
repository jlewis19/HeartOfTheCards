using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float moveSpeed = 10f;
    public float dashSpeed = 50f;
    public float dashCooldown = 3f;
    public float dashTime = 0.15f;
    public AudioClip dashSFX;

    CharacterController controller;
    Vector3 input, moveDirection;
    bool canDash = true;
    public static bool dashing = false;
    float dashCooldownTimer = 0f;

    float y;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
        y = transform.position.y;
    }

    // Update is called once per frame
    void Update() {
        transform.position.Set(transform.position.x, y, transform.position.z);
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (!canDash && !dashing)
        {
            dashCooldownTimer += Time.deltaTime;
            DashCooldown.UpdateDashUI(dashCooldownTimer, dashCooldown);
            if (dashCooldownTimer >= dashCooldown)
            {
                canDash = true;
                dashCooldownTimer = 0f;
            }
        }

        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        if (Input.GetKeyDown(KeyCode.R) && canDash)
        {
            dashing = true;
            canDash = false;
            AudioSource.PlayClipAtPoint(dashSFX, Camera.main.transform.position);
        }

        if (dashing)
        {
            input *= dashSpeed;
            dashCooldownTimer += Time.deltaTime;
            if (dashCooldownTimer >= dashTime)
            {
                dashing = false;
                dashCooldownTimer = 0f;
            }
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
