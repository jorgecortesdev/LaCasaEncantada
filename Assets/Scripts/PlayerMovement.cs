using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;

    Vector3 movement;
    Quaternion rotation = Quaternion.identity;

    Animator anim;
    Rigidbody rigid;

    public AudioSource audio;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement.Set(horizontal, 0f, vertical);
        movement.Normalize();

        bool horizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool verticalInput = !Mathf.Approximately(vertical, 0f);

        bool isWalking = horizontalInput || verticalInput;
        anim.SetBool("IsWalking", isWalking);

        Vector3 desireForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.deltaTime, 0f);
        rotation = Quaternion.LookRotation(desireForward);

        if (isWalking)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
        else
        {
            audio.Stop();
        }
    }

    private void OnAnimatorMove()
    {
        rigid.MovePosition(rigid.position + movement * anim.deltaPosition.magnitude);
        rigid.MoveRotation(rotation);
    }
}
