using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item" , menuName = "New Item/item")]
public class Item : MonoBehaviour
{

    public string itemName; //아이템의 이름
    public Type type; //아이템의 유형
    public Sprite itemImage; //아이템의 이미지
    public int value;
    //public GameObject itemPrefab; //아이템 프리팹

    //public string weaponType;

    public enum Type
    {
        Rock,
        Coin
    };

}
