using MJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour
{
    enum BallMove
    { 
        BallUp,
        BallDown,
        BallMove_End
    }


    private GameObject player;
    float bounceY = 0.0f;
    float bounceSpeed = 2.0f;
    bool bBallBounce = false;

    BallMove ballMove = BallMove.BallUp;

    public SoccerObject soccerObject;
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    private void Update()
    {

        
    }
    private void LateUpdate()
    {
        if(bBallBounce)
            bouncePlayerObject();
    }

    private IEnumerator bounce(float yValue)
    {
        while (bBallBounce)
        {
            switch (ballMove)
            {
                case BallMove.BallDown:
                    bounceY -= Time.deltaTime;
                    break;
                case BallMove.BallUp:
                    bounceY += Time.deltaTime;
                    break;
            }
            ballMoveCheck(yValue);
            yield return null;
        }
    }

    public void ballMoveCheck(float yValue)
    {
        switch (ballMove)
        {
            case BallMove.BallDown:
                if (bounceY < 0.0f)
                    ballMove = BallMove.BallUp;
                break;
            case BallMove.BallUp:
                if (bounceY > yValue)
                    ballMove = BallMove.BallDown;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.name == "Player")
        //{
        //    bBallBounce = true;
        //    StartCoroutine(bounce(0.5f));
        //}
    }

    public void StartBounce()
    {
        bBallBounce = true;
        StartCoroutine(bounce(0.5f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //    playerCollision();
    }

    public void bouncePlayerObject()
    {
        Vector3 transformPosition = player.transform.position;
        transform.position = new Vector3(transformPosition.x, transformPosition.y + bounceY + 0.25f, 0.0f);

        Debug.Log("bounceY: " + bounceY);
    }

    public bool GetBounceBall()
    {
        return bBallBounce;
    }

    public Vector2 GetPlayerDirection()
    {
        return player.GetComponentInChildren<Rigidbody2D>().velocity.normalized;
    }

    public void SetBallBounce(bool bBounce)
    {
        bBallBounce = bBounce;
    }

    public bool CheckAroundPlayer(float dis)
    {
        return Vector3.Distance(player.transform.position, transform.position) < dis;
    }

}
