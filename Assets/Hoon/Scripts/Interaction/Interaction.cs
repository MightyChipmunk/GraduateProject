using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    protected Collider col;
    protected bool canInter;
    protected GameObject interUI;

    protected virtual void Start()
    {
        col = GetComponent<Collider>();
    }

    protected virtual void Update()
    {
        if (Input.GetMouseButtonDown(0) && canInter)
        {
            Interact();
        }
    }

    protected virtual void Interact()
    {

    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canInter = true;
            interUI.SetActive(true);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canInter = false;
            interUI.SetActive(false);
        }
    }
}
