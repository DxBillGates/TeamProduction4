using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour
{
    Rigidbody rb;
    bool onGround;
    bool isJump;
    Vector3 groundNormal;
    float speed = 4;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        onGround = false;
        groundNormal = new Vector3();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        transform.Translate(new Vector3(x, 0, z));

        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {
            float distanceOnGround = hit.distance;
            groundNormal = hit.normal;
            if (distanceOnGround <= 0.5f)
            {
                onGround = true;
            }
            else
            {
                onGround = false;
            }
            Vector3 cross = Vector3.Cross(groundNormal, hit.collider.transform.forward);
            Debug.DrawRay(transform.position, cross);
        }

        Vector3 gravity = -groundNormal.normalized;
        if (!onGround)
        {
            rb.AddForce(gravity);
        }
        else
        {
            isJump = false;
        }

        Quaternion toRotation = Quaternion.FromToRotation(transform.up, groundNormal) * transform.rotation;
        transform.rotation = toRotation;

        Debug.DrawRay(transform.position, transform.up, new Vector4(0, 1, 0, 1));
        Debug.DrawRay(transform.position, transform.right, new Vector4(1, 0, 0, 1));
        Debug.DrawRay(transform.position, transform.forward, new Vector4(0, 0, 1, 1));
    }

    void Jump()
    {
        if (!isJump)
        {
            isJump = true;
            rb.AddForce(transform.up.normalized * 100);
            Debug.Log("jump");
        }
    }
}
