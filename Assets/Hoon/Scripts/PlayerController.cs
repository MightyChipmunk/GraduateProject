using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Parameters
    [SerializeField]
    float moveSpeed = 6;
    [SerializeField]
    float rotSpeed = 30;

    CharacterController cc;
    Animator animator;

    // Vector Parameters for Move, Rotate
    Vector3 dir;

    // State Parameters for Animation
    enum State
    {
        Idle,
        Move,
    }
    State state;

    void Start()
    {
        cc = GetComponentInChildren<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        dir = Vector3.forward;

        state = State.Idle;
    }

    void Update()
    {
        Move();
        switch (state)
        {
            case State.Idle:
                animator.SetInteger("State", 0);
                break;
            case State.Move:
                animator.SetInteger("State", 1);
                break;
        }
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = Vector3.zero;
        if (Camera.main != null)
        {
            dir = h * Camera.main.transform.right + v * Camera.main.transform.forward;
        }
        dir.y = 0;
        dir.Normalize();

        if (dir.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

            Vector3 velocity = transform.rotation * Vector3.forward * moveSpeed;
            cc.Move(velocity * Time.deltaTime);
        }

        // 중력의 영향을 받으며 항상 아래로 이동한다.
        cc.Move(-transform.up * 9.81f * Time.deltaTime);
    }
}