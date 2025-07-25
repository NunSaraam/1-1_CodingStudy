using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour
{
    [Header("����� ���")]
    [SerializeField] DrinkSO[] drinks;

    [Header("UI")]
    public Button[] drinkSelectButtons;
    public Button returnCoinButton;
    public Text[] drinkInfoText;
    public Text userInfoText;
    public Text inputCoinCountText;

    [Header("�⺻ ����")]
    [Tooltip("�ʱ� ���� �ݾ�")] public int initialBalance = 5000;

    [Header("���� ���� �ý���")]
    [Tooltip("���� ���� �ݾ�")] public int currentBalance;
    public int[] coinValue = {100, 500, 1000};
    public int inputCoinCount;

    [Header("���� �� �Ž�����")]
    DrinkSO selectedDrink;
    public bool canAfford = false;
    public int changeAmount;

    void UpdateUI()
    {

    }
}
