using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerSpeed;
    bool paused = false;
    Vector3 movement;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!paused)
        {
            movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
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
