using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballController : MonoBehaviour
{
    // Public variables that can be set in the Unity Editor
    public float MoveSpeed = 15;
    public Transform Ball;
    public Transform PosDribble;
    public Transform PosOverHead;
    public Transform Arms;
    public Transform Target;
    // Private variables
    private bool IsBallInHands = true;
    private bool IsBallFlying = false;
    private float T = 0;

    void Update()
    {
        // Walking controls
        Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        transform.position += direction * MoveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + direction);

        // If the ball is in our hands
        if (IsBallInHands)
        {
            // Hold the ball over our head
            if (Input.GetKey(KeyCode.Space))
            {
                Ball.position = PosOverHead.position;
                Arms.localEulerAngles = Vector3.right * 180;

                // Look towards the target
                transform.LookAt(Target.parent.position);
            }
            // Dribbling the ball
            else
            {
                Ball.position = PosDribble.position + Vector3.up * Mathf.Abs(Mathf.Sin(Time.time * 5));
                Arms.localEulerAngles = Vector3.right * 0;
            }

            // Throw the ball
            if (Input.GetKeyUp(KeyCode.Space))
            {
                IsBallInHands = false;
                IsBallFlying = true;
                T = 0;
            }
        }

        // If the ball is in the air
        if (IsBallFlying)
        {
            T += Time.deltaTime;
            float duration = 0.66f;
            float t01 = T / duration;

            // Move the ball to the target
            Vector3 A = PosOverHead.position;
            Vector3 B = Target.position;
            Vector3 pos = Vector3.Lerp(A, B, t01);

            // Move the ball in an arc
            Vector3 arc = Vector3.up * 5 * Mathf.Sin(t01 * 3.14f);

            Ball.position = pos + arc;

            // When the ball reaches the target
            if (t01 >= 1)
            {
                IsBallFlying = false;
                Ball.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsBallInHands && !IsBallFlying)
        {
            IsBallInHands = true;
            Ball.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

}