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
    public Text resultText;

    [Header("기본 정보")]
    [Tooltip("초기 소지 금액")] public int initialBalance = 5000;

    [Header("동전 투입 시스템")]
    [Tooltip("현재 소지 금액")] public int currentBalance = 0;
    [Tooltip("투입한 금액")] public int inputCoin = 0;
    public Button[] coinButtons;


    private void Start()
    {
        currentBalance = initialBalance;
        resultText.text = "";
        UpdateUI();
        Buttons();
    }

    void UpdateUI()
    {
        userInfoText.text = $"잔액 : {currentBalance}원";

        inputCoinCountText.text = $"투입한 금액 : {inputCoin}원";

        for (int i = 0; i < drinks.Length; i++)
        {
            if (drinks[i] != null && inputCoin >= drinks[i].drinkPrice)         //투입 금액 음료수 금액 비교
                drinkSelectButtons[i].interactable = true;
            else
                drinkSelectButtons[i].interactable = false;
        }

        for (int i = 0; i < drinks.Length; i++)
        {
            string name = drinks[i].drinkName;
            int price = drinks[i].drinkPrice;

            drinkInfoText[i].text = $"{name}\n" +
                                    $"{price}원";
        }
    }

    void SelectDrink(int drinkIndex)
    {
        UseCoin(drinks[drinkIndex]);
        UpdateUI();
    }

    void UseCoin(DrinkSO drink)
    {
        if (drink != null && inputCoin >= drink.drinkPrice)
        {
            inputCoin -= drink.drinkPrice;
            resultText.text = $"{drink.drinkName}구매 하였습니다.\n" +
                              $"{drink.drinkPrice}원을 사용하였습니다.";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = $"잔액이 부족합니다.";
            resultText.color = Color.red;
        }

        Invoke("ClearResultText", 3f);
    }

    void InputCoinButton(int index)
    {
        int coin = 0;

        switch (index)
        {
            case 0:
                coin = 100; 
                break;
            case 1: 
                coin = 500; 
                break;
            case 2:
                coin = 1000;
                break;
        }

        if (currentBalance >= coin)
        {
            inputCoin += coin;
            currentBalance -= coin;
            UpdateUI();
        }
    }

    void ReturnCoin()
    {
        if (returnCoinButton)
        {
            if (inputCoin > 0)
            {
                currentBalance += inputCoin;
                inputCoin = 0;
                UpdateUI() ;
            }
            else if(inputCoin <= 0)
            {
                resultText.text = "투입된 금액이 없습니다.";
                resultText.color = Color.red;
            }
        }

        Invoke("ClearResultText", 3f);
    }

    void Buttons()
    {
        for (int i = 0; i < drinkSelectButtons.Length; i++)
        {
            int index = i;
            drinkSelectButtons[index].onClick.AddListener(() => SelectDrink(index));
        }

        for (int i = 0; i < coinButtons.Length; i++)
        {
            int index = i;
            coinButtons[index].onClick.AddListener(() => InputCoinButton(index));
        }

        returnCoinButton.onClick.AddListener(ReturnCoin);

    }

    void ClearResultText()
    {
        resultText.text = "";
    }
}
