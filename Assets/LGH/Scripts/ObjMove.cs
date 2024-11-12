using GH;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ObjMove : MonoBehaviour
{
    public float moveTime = 3;
    public float currentTime = 0;

    public bool moving = false;
    public bool objMove = false;

    public float lerpValue = 0.03f;
    PlayerMove playerMove;
    public float distance = 1;

    private Vector3 objTransform;
    void Start()
    {

    }

    void Update()
    {
        if (moving)
        {
            currentTime += Time.deltaTime;

        }

        if (currentTime > moveTime)
        {
            objMove = true;
            moving = false;
            currentTime = 0;

        }

        if (objMove)
        {
            transform.position = Vector3.Lerp(transform.position, objTransform, lerpValue);

            distance = Vector3.Distance(transform.position, objTransform);

            if (distance < 0.2f)
            {
                transform.position = objTransform;
                objMove = false;
                distance = 1;
            }
        }

    }

    private void OnMoving()
    {
        transform.position = Vector3.Lerp(transform.position, objTransform, lerpValue);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerMove = collision.gameObject.GetComponent<PlayerMove>();
            if (!moving)
            {

                objTransform = transform.position + playerMove.stingDir;
            }
            moving = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // objTransform = transform.position + playerMove.stingDir;
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            moving = false;
            currentTime = 0;

        }
    }
}
