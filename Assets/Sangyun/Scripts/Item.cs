using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Item" , menuName = "New Item/item")]
public class Item : Interaction
{
    public string itemName; //아이템의 이름
    public Type type; //아이템의 유형
    public Sprite itemImage; //아이템의 이미지
    public int value;

    public enum Type
    {
        Rock,
        Coin
    }

    void Awake()
    {
        interUI = GameObject.Find("Item Interaction");
    }

    protected override void Start()
    {
        interUI.SetActive(false);
    }

    protected override void Interact()
    {
        switch (type)
        {
            case Item.Type.Rock:
                NetworkManager.Instance.playerTeam.stones += value;
                break;
            case Item.Type.Coin:
                NetworkManager.Instance.playerTeam.gold += value;
                break;
        }
        APIManager.Instance.UpdateUserInfo();
        interUI.SetActive(false);
        Destroy(gameObject);
    }
}
