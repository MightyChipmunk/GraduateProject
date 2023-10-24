using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasurebox : MonoBehaviour
{
    [SerializeField]
    private int hp;

    [SerializeField]
    private float destroyTime;

    [SerializeField]
    private BoxCollider col;

    [SerializeField]
    private GameObject go_box; //�Ϲ� ���� 


    public void OpenBox()
    {
        hp--;
        if (hp <= 0)
            Destruction();
    }

    private void Destruction()
    {
        col.enabled = false;
        Destroy(go_box, destroyTime);
    }
  
}
