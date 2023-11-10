using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Parameters
    [SerializeField]
    float moveSpeed = 6;
    [SerializeField]
    float jumpPower = 10;

    [SerializeField]
    private Inventory theInventory;

    CharacterController cc;
    Animator animator;

    float ySpeed = 0;

    public int rock;
    public int coin;

    public int maxRock;
    public int maxCoin;

    GameObject nearObject;


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

        state = State.Idle;
    }


    void Awake()
    {
        PlayerPrefs.SetInt("MaxScore", 12500);
    }

    void Update()
    {
        switch (state)
        {
            case State.Idle:
                Move();
                Gravity();
                Jump();
                animator.SetInteger("State", 0);
                Interaction();
                break;
            case State.Move:
                Move();
                Gravity();
                Jump();
                animator.SetInteger("State", 1);
                Interaction();
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
            state = State.Move;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);

            Vector3 velocity = transform.rotation * Vector3.forward * moveSpeed;
            cc.Move(velocity * Time.deltaTime);
        }
        else
        {
            state = State.Idle;
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded)
        {
            ySpeed = jumpPower;
            animator.SetTrigger("Jump");
        }
    }

    float falling = 0;
    void Gravity()
    {
        ySpeed -= 9.81f * Time.deltaTime;
        ySpeed = Mathf.Clamp(ySpeed, -10, jumpPower * 2);

        // 중력의 영향을 받으며 항상 아래로 이동한다.
        cc.Move(transform.up * ySpeed * Time.deltaTime);

        if (!cc.isGrounded)
            falling = Mathf.Lerp(falling, 1, 5 * Time.deltaTime);
        else
            falling = Mathf.Lerp(falling, 0, 5 * Time.deltaTime);
        animator.SetFloat("Falling", falling);
    }

    void Interaction()
    {
        if(Input.GetKeyDown(KeyCode.E) && nearObject != null)
        {
            if (nearObject.tag == "Item")
            {
                Item item = nearObject.GetComponent<Item>();
                switch (item.type)
                {
                    case Item.Type.Rock:
                        //theInventory.AcquireItem(nearObject.GetComponent<Item>());
                        rock += item.value;
                        if (rock > maxRock)
                            rock = maxRock;
                        break;
                    case Item.Type.Coin:
                        coin += item.value;
                        if (coin > maxCoin)
                            rock = maxCoin;
                        break;
                }
                Destroy(nearObject);
            }
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Rock:
                    rock += item.value;
                    if (rock > maxRock)
                        rock = maxRock;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        rock = maxCoin;
                    break;
            }
            Destroy(other.gameObject);
        }
    }
    */

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Item")
            nearObject = other.gameObject;

        //Debug.Log(nearObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Item")
            nearObject = null;
    }


}