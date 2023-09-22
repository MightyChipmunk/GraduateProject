using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    GameObject player; 

    [SerializeField]
    float rotSpeed = 100f;
    float mx;
    float my;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position;

        float mh = Input.GetAxis("Mouse X");
        float mv = Input.GetAxis("Mouse Y");

        mx += mh * rotSpeed * Time.deltaTime;
        my += mv * rotSpeed * Time.deltaTime;
        my = Mathf.Clamp(my, -60, 60);

        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
