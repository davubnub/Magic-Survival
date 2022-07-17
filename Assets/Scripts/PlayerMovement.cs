using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerSpeed;
    bool paused = false;
    Vector3 movement;
    public Joystick movementJoystick;
    public PlayerScript playerScript;

    private void Start()
    {
        #if UNITY_STANDALONE_WIN
            movementJoystick.gameObject.SetActive(false);
        #endif
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!paused)
        {
            #if UNITY_STANDALONE_WIN
                movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                playerScript.PlayAnimation((movement.magnitude > 0)?PlayerScript.ANIMATIONS.Walk : PlayerScript.ANIMATIONS.Idle);
            #endif
            #if UNITY_IOS || UNITY_ANDROID || UNITY_IPHONE
                movement = new Vector3(movementJoystick.Horizontal, 0, movementJoystick.Vertical);
                playerScript.PlayAnimation((movement.magnitude > 0)?PlayerScript.ANIMATIONS.Walk : PlayerScript.ANIMATIONS.Idle);
            #endif

            transform.position += movement.normalized * Time.deltaTime * playerSpeed;
        }
    }

    public void UpdateMovemnentSpeed(float _playerSpeed)
    {
        playerSpeed = _playerSpeed;
    }

    public void SetPaused(bool _pause)
    {
        paused = _pause;
    }
}
