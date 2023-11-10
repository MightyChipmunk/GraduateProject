using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item" , menuName = "New Item/item")]
public class Item : MonoBehaviour
{

    public string itemName; //�������� �̸�
    public Type type; //�������� ����
    public Sprite itemImage; //�������� �̹���
    public int value;
    //public GameObject itemPrefab; //������ ������

    //public string weaponType;

    public enum Type
    {
        Rock,
        Coin
    };

}
