using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Drink", menuName = "Vending Machine/Drink")]
public class DrinkSO : ScriptableObject
{
    [Header("�⺻ ����")]
    public int id;                                  //���� ��ȣ
    public string drinkName = "�����";             //���� �̸�
    public Sprite drinkIcon;                        //���� �̹���
    public int price;                               //���� ����
    public int stock;                               //���� ���

    [Header("����")]
    [TextArea(2, 3)]
    public string description = "����� �Դϴ�.";

}
