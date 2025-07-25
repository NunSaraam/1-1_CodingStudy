using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour
{
    [Header("음료수 목록")]
    [SerializeField] DrinkSO[] drinks;

    [Header("UI")]
    public Button[] drinkSelectButtons;
    public Button returnCoinButton;
    public Text[] drinkInfoText;
    public Text userInfoText;
    public Text inputCoinCountText;

    [Header("기본 정보")]
    [Tooltip("초기 소지 금액")] public int initialBalance = 5000;

    [Header("동전 투입 시스템")]
    [Tooltip("현재 소지 금액")] public int currentBalance;
    public int[] coinValue = {100, 500, 1000};
    public int inputCoinCount;

    [Header("구매 및 거스름돈")]
    DrinkSO selectedDrink;
    public bool canAfford = false;
    public int changeAmount;

    void UpdateUI()
    {

    }
}
