using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Drink", menuName = "Vending Machine/Drink")]
public class DrinkSO : ScriptableObject
{
    [Header("기본 정보")]
    public int id;                                  //고유 번호
    public string drinkName = "음료수";             //음료 이름
    public Sprite drinkIcon;                        //음료 이미지
    public int price;                               //음료 가격
    public int stock;                               //남은 재고

    [Header("설명")]
    [TextArea(2, 3)]
    public string description = "음료수 입니다.";

}
