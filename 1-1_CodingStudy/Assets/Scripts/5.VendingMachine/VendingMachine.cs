using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour
{
    [Header("����� ���� ��ư")]
    public Button[] drinkSelectButtons;

    [Header("�⺻ ����")]
    [Tooltip("�ʱ� ���� �ݾ�")] public int initialBalance = 5000;

    [Header("���� ���� �ý���")]
    [Tooltip("���� ���� �ݾ�")] public int currentBalance;
    public int[] coinValue = {100, 500, 1000};
    public int coinInputCount;

    [Header("���� �� �Ž�����")]
    DrinkSO selectedDrink;
    public bool canAfford = false;
    public int changeAmount;
}
