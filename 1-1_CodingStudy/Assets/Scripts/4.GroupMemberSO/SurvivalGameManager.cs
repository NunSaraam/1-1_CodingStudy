using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ItemSO;

public class SurvivalGameManager : MonoBehaviour
{
    [Header("�׷� ������ ���ø�")]
    public GroupMemberSO[] groupMembers;

    [Header("������ ���ø�")]
    public ItemSO foodItem;                                 //���� ������ SO
    public ItemSO fuelItem;                                 //���� ������ SO
    public ItemSO medicineItem;                             //�Ǿ�ǰ ������ SO

    [Header("���� UI")]
    public Text dayText;                                    //��¥ ǥ�� UI
    public Text[] memberStatusTexts;                        //�ɹ� ���� ǥ�� Text
    public Button nextDayButton;                            //���� ��¥�� ����Ǵ� ��ư
    public Text inventoryText;                              //�κ��丮 ǥ��

    [Header("������ ��ư")]
    public Button feedButton;                               //���� �ֱ�
    public Button healtButton;                              //���� �ϱ�
    public Button healButton;                               //ġ�� �ϱ�

    [Header("������ ���� ��� ��ư")]
    public Button[] feedButtons;                               //���� �ֱ�
    public Button[] healButtons;                               //ġ�� �ϱ�

    [Header("���� ����")]
    int currentDay;                                         //���� ��¥
    public int food = 5;                                    //���� ����
    public int fuel = 3;                                    //���� ����
    public int medicine = 4;                                //�Ǿ�ǰ ����

    //��Ÿ�� ������
    private int[] memberHealth;
    private int[] memberHunger;
    private int[] memberBodyTemp;


    void Start()
    {
        currentDay = 1;

        InitializeGroup();
        UpdateUI();

        nextDayButton.onClick.AddListener(NextDay);

        feedButton.onClick.AddListener(UseAllMembersFoodItem);
        healtButton.onClick.AddListener(UseAllMembersFuelItem);
        healtButton.onClick.AddListener(UseAllMembersMedicineItem);


        for (int i = 0; i < groupMembers.Length; i++)
        {
            int index = i;

            feedButtons[i].onClick.AddListener(() => OnClickGiveItem(index, ItemType.Food));
            healButtons[i].onClick.AddListener(() => OnClickGiveItem(index, ItemType.Meidicine));
        }
    }

    void InitializeGroup()
    {
        int memberCount = groupMembers.Length;                      //�׷� �ɹ��� ���� ��ŭ �ο� �� �Ҵ�
        memberHealth = new int[memberCount];                        //�׷� �ɹ� ���� ��ŭ �迭 �Ҵ�
        memberHunger = new int[memberCount];
        memberBodyTemp = new int[memberCount];

        for (int i = 0; i < memberCount; i++)
        {
            if (groupMembers[i] != null)                            //�׷� �ɹ����� ������ ���ڸ� �迭�� �ִ´�.
            {
                memberHealth[i] = groupMembers[i].maxHealth;
                memberHunger[i] = groupMembers[i].maxHunger;
                memberBodyTemp[i] = groupMembers[i].normalBodyTemp;
            }
        }
    }

    void UpdateUI()
    {
        dayText.text = $"Day {currentDay}";

        inventoryText.text = $"����   : {food} ��\n" +
                             $"����   : {fuel} ��\n" +
                             $"�Ǿ�ǰ : {medicine} ��\n";

        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] != null && memberStatusTexts[i] != null)
            {
                GroupMemberSO member = groupMembers[i];

                //���� �޼��� ����
                string status = GetMemberStatus(i);

                memberStatusTexts[i].text =
                    $"{member.memberName}  {status} \n" +
                    $"ü��   : {memberHealth[i]} \n" +
                    $"����� : {memberHunger[i]} \n" +
                    $"ü��   : {memberBodyTemp[i]} �� ";
            }

            UpdateTextColor(memberStatusTexts[i], memberHealth[i]);
        }
    }

    void ProcessDailyChange()
    {
        int baseHungerLoss = 15;
        int baseTempLoss = 1;
        
        for (int i = 0; i <groupMembers.Length; i++)
        {
            if (groupMembers[i] == null) continue;                      //�׷��� 1���� ����ص� ��� ����
            {
                GroupMemberSO member = groupMembers[i];

                //���̿� ���� ����� ����
                float hungerMultiplier = member.agegroup == GroupMemberSO.AgeGroup.Child ? 0.8f : 1.0f;

                //���� ����
                memberHunger[i] -= Mathf.RoundToInt(baseHungerLoss * hungerMultiplier);         //�ʹ��� ����� ���� ����
                memberBodyTemp[i] -= Mathf.RoundToInt(baseTempLoss * member.coldResistance);   //�ɹ��� ���� ���׷�

                //�ǰ� üũ
                if (memberHunger[i] <= 0) memberHunger[i] -= 15;                    //���ָ�
                if (memberBodyTemp[i] <= 32) memberHealth[i] -= 10;                //��ü���� (32�� ����)
                if (memberBodyTemp[i] <= 30) memberHealth[i] -= 20;                //�ɰ��� ��ü����

                //�ּҰ� ����
                memberHunger[i] = Mathf.Max(0, memberHunger[i]);
                memberBodyTemp[i] = Mathf.Max(25, memberBodyTemp[i]);
                memberHealth[i] = Mathf.Max(0, memberHealth[i]);
            }
        }
    }

    public void NextDay()
    {
        currentDay += 1;
        ProcessDailyChange();
        UpdateUI();
        CheackGameOver();
    }

    string GetMemberStatus(int memberIndex)
    {
        //��� üũ
        if (memberHealth[memberIndex] <= 0)
            return "(���)";

        //���� ������ ���º��� üũ
        if (memberBodyTemp[memberIndex] <= 30) return "(�ɰ��� ��ü����)";
        else if (memberHealth[memberIndex] <= 20) return "(����)";
        else if (memberHunger[memberIndex] <= 10) return "(���ָ�)";
        else if (memberBodyTemp[memberIndex] <= 32) return "(��ü����)";
        else if (memberHealth[memberIndex] <= 50) return "(����)";
        else if (memberHunger[memberIndex] <= 30) return "(�����)";
        else if (memberBodyTemp[memberIndex] <= 35) return "(����)";
        else return "(�ǰ�)";
    }

    void CheackGameOver()
    {
        int aliveCount = 0;

        for (int i= 0; i < memberHealth.Length; i++)
        {
            if (memberHealth[i] > 0) aliveCount++;
        }

        if (aliveCount == 0)
        {
            nextDayButton.interactable = false;
            Debug.Log("���� ����! ��� �������� Ȥ���� ��Ȳ�� �̰ܳ��� ���߽��ϴ�.");
        }
    }

    void UpdateTextColor(Text text, int health)
    {
        if (health <= 0)
            text.color = Color.gray;
        else if (health <= 20)
            text.color = Color.red;
        else if (health <= 50)
            text.color = Color.yellow;
        else
            text.color = Color.white;
    }

    public void UseAllMembersFoodItem()                                               //���� ������ ���
    {
        if (food <= 0 || foodItem == null) return;                          //���� ���� ó��

        food--;
        UseItemOnAllMembers(foodItem);
        UpdateUI();
    }

    public void UseAllMembersFuelItem()
    {
        if (fuel <= 0 || fuelItem == null) return;

        fuel--;
        UseItemOnAllMembers(fuelItem);
        UpdateUI();
    }

    public void UseAllMembersMedicineItem()
    {
        if (medicine <= 0 || medicineItem == null) return;

        medicine--;
        UseItemOnAllMembers(medicineItem);
        UpdateUI();
    }

    void UseItemOnAllMembers(ItemSO item)
    {
        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] != null && memberHealth[i] > 0)                                 //����ִ� ������
            {
                ApplyItemEffect(i, item);
            }
        }
    }

    void UseItemOnMember(int memberIndex, ItemSO item)              //���� ������ ��� �ż���
    {
        if (groupMembers[memberIndex] != null && memberHealth[memberIndex] > 0)
        {
            ApplyFMItemEffect(memberIndex, item);
        }
    }

    public void OnClickGiveItem(int memberIndex, ItemType type)                 //��ư Ŭ���� ����, UI������ ���� switch�� ���
    {
        switch (type)
        {
            case ItemType.Food:
                if (food > 0)
                {
                    UseItemOnMember(memberIndex, foodItem);
                    food--;
                    UpdateUI();
                }
                break;

            case ItemType.Meidicine:
                if (medicine > 0)
                {
                    UseItemOnMember(memberIndex, medicineItem);
                    medicine--;
                    UpdateUI();
                }
                break;
        }
    }

    void ApplyItemEffect(int memberIndex, ItemSO item)
    {
        GroupMemberSO member = groupMembers[memberIndex];

        //���� Ư�� �����ؼ� ������ ȿ�� ���
        int actualHealth = Mathf.RoundToInt(item.healthEffect * member.recoveryRate);
        int actualHunger = Mathf.RoundToInt(item.hungerEffect * member.foodEfficiency);
        int actualTemp = item.tempEffect;

        //ȿ�� ����
        memberHealth[memberIndex] += actualHealth;
        memberHunger[memberIndex] += actualHunger;
        memberBodyTemp[memberIndex] += actualTemp;

        //�ִ�ġ ����
        memberHealth[memberIndex] = Mathf.Min(memberHealth[memberIndex], member.maxHealth);
        memberHunger[memberIndex] = Mathf.Min(memberHunger[memberIndex], member.maxHunger);
        memberBodyTemp[memberIndex] = Mathf.Min(memberBodyTemp[memberIndex], member.normalBodyTemp);
    }

    void ApplyFMItemEffect(int memberIndex, ItemSO item)
    {
        GroupMemberSO member = groupMembers[memberIndex];

        //���� Ư�� �����ؼ� ������ ȿ�� ���
        int actualHealth = Mathf.RoundToInt(item.healthEffect * member.recoveryRate);
        int actualHunger = Mathf.RoundToInt(item.hungerEffect * member.foodEfficiency);

        //ȿ�� ����
        memberHealth[memberIndex] += actualHealth;
        memberHunger[memberIndex] += actualHunger;

        //�ִ�ġ ����
        memberHealth[memberIndex] = Mathf.Min(memberHealth[memberIndex], member.maxHealth);
        memberHunger[memberIndex] = Mathf.Min(memberHunger[memberIndex], member.maxHunger);
    }
}
