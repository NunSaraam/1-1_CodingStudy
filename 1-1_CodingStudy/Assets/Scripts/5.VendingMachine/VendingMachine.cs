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
    public Text resultText;

    [Header("�⺻ ����")]
    [Tooltip("�ʱ� ���� �ݾ�")] public int initialBalance = 5000;

    [Header("���� ���� �ý���")]
    [Tooltip("���� ���� �ݾ�")] public int currentBalance = 0;
    [Tooltip("������ �ݾ�")] public int inputCoin = 0;
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
        userInfoText.text = $"�ܾ� : {currentBalance}��";

        inputCoinCountText.text = $"������ �ݾ� : {inputCoin}��";

        for (int i = 0; i < drinks.Length; i++)
        {
            if (drinks[i] != null && inputCoin >= drinks[i].drinkPrice)         //���� �ݾ� ����� �ݾ� ��
                drinkSelectButtons[i].interactable = true;
            else
                drinkSelectButtons[i].interactable = false;
        }

        for (int i = 0; i < drinks.Length; i++)
        {
            string name = drinks[i].drinkName;
            int price = drinks[i].drinkPrice;

            drinkInfoText[i].text = $"{name}\n" +
                                    $"{price}��";
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
            resultText.text = $"{drink.drinkName}���� �Ͽ����ϴ�.\n" +
                              $"{drink.drinkPrice}���� ����Ͽ����ϴ�.";
            resultText.color = Color.green;
        }
        else
        {
            resultText.text = $"�ܾ��� �����մϴ�.";
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
                resultText.text = "���Ե� �ݾ��� �����ϴ�.";
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
