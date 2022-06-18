using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float playerSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * playerSpeed;
        float vertical   = Input.GetAxis("Vertical")   * Time.deltaTime * playerSpeed;

        transform.position += new Vector3(horizontal, 0, vertical);
    }

    public void UpdateMovemnentSpeed(float _playerSpeed)
    {
        playerSpeed = _playerSpeed;
    }
}
