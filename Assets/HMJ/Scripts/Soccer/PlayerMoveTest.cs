using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMoveTest : MonoBehaviour
{
    Rigidbody2D rigidbody;
    float speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = UnityEngine.Input.GetAxisRaw("Horizontal");
        float vertical = UnityEngine.Input.GetAxisRaw("Vertical");

        float moveX = horizontal * speed;
        float moveY = vertical * speed;
        rigidbody.velocity = new Vector2(moveX, moveY);
    }
}
