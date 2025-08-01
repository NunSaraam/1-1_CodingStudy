using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ItemSO;

public class SurvivalGameManager : MonoBehaviour
{
    [Header("그룹 구성원 템플릿")]
    public GroupMemberSO[] groupMembers;

    [Header("아이템 템플릿")]
    public ItemSO foodItem;                                 //음식 아이템 SO
    public ItemSO fuelItem;                                 //연료 아이템 SO
    public ItemSO medicineItem;                             //의약품 아이템 SO

    [Header("참조 UI")]
    public Text dayText;                                    //날짜 표시 UI
    public Text[] memberStatusTexts;                        //맴버 상태 표시 Text
    public Button nextDayButton;                            //다음 날짜로 변경되는 버튼
    public Text inventoryText;                              //인벤토리 표시

    [Header("아이템 버튼")]
    public Button feedButton;                               //음식 주기
    public Button healtButton;                              //난방 하기
    public Button healButton;                               //치료 하기

    [Header("아이템 개별 사용 버튼")]
    public Button[] feedButtons;                               //음식 주기
    public Button[] healButtons;                               //치료 하기

    [Header("게임 상태")]
    int currentDay;                                         //현재 날짜
    public int food = 5;                                    //음식 개수
    public int fuel = 3;                                    //연료 개수
    public int medicine = 4;                                //의약품 개수

    [Header("특정  맴버 아이템 소모 버튼")]
    public Button[] individualHealthButtons;
    public Button[] individualFoodButon;

    [Header("이벤트 시스템")]
    public EventsSO[] events;                       //이벤트 목록
    public GameObject eventPopup;                   //이벤트 팝업 패널
    public Text eventTitleText;                     //이벤트 제목
    public Text eventDescritionText;                //이벤트 설명
    public Button eventConfirmButton;               //이벤트 닫기(확인) 버튼


    //런타임 데이터
    public int[] memberHealth;
    public int[] memberHunger;
    public int[] memberBodyTemp;


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

        /*
        individualFoodButon[0].onClick.AddListener(GiveFoodToMember0);
        individualFoodButon[1].onClick.AddListener(GiveFoodToMember1);
        individualFoodButon[2].onClick.AddListener(GiveFoodToMember2);
        individualFoodButon[3].onClick.AddListener(GiveFoodToMember3);
        
        
        for (int i = 0; i <individualFoodButon.Length && i <groupMembers.Length; i++)
        {
            int memberIndex = i;
            individualFoodButon[0].onClick.AddListener(() => GiveFoodToMember(memberIndex));
        }
        */

        eventPopup.SetActive(false);
        eventConfirmButton.onClick.AddListener(CloseEventPopup);

    }

    public void GiveFoodToMember0() { GiveFoodToMember(0); }
    public void GiveFoodToMember1() { GiveFoodToMember(1); }
    public void GiveFoodToMember2() { GiveFoodToMember(2); }
    public void GiveFoodToMember3() { GiveFoodToMember(3); }
    void InitializeGroup()
    {
        int memberCount = groupMembers.Length;                      //그룹 맴버의 길이 만큼 인원 수 할당
        memberHealth = new int[memberCount];                        //그룹 맴버 길이 만큼 배열 할당
        memberHunger = new int[memberCount];
        memberBodyTemp = new int[memberCount];

        for (int i = 0; i < memberCount; i++)
        {
            if (groupMembers[i] != null)                            //그룹 맴버에서 정의한 숫자를 배열에 넣는다.
            {
                memberHealth[i] = groupMembers[i].maxHealth;
                memberHunger[i] = groupMembers[i].maxHunger;
                memberBodyTemp[i] = groupMembers[i].normalBodyTemp;
            }
        }
    }

    public void UpdateUI()
    {
        dayText.text = $"Day {currentDay}";

        inventoryText.text = $"음식   : {food} 개\n" +
                             $"연료   : {fuel} 개\n" +
                             $"의약품 : {medicine} 개\n";

        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] != null && memberStatusTexts[i] != null)
            {
                GroupMemberSO member = groupMembers[i];

                //상태 메세지 결정
                string status = GetMemberStatus(i);

                memberStatusTexts[i].text =
                    $"{member.memberName}  {status} \n" +
                    $"체력   : {memberHealth[i]} \n" +
                    $"배고픔 : {memberHunger[i]} \n" +
                    $"체온   : {memberBodyTemp[i]} 도 ";
            }

            UpdateTextColor(memberStatusTexts[i], memberHealth[i]);
        }
    }

    void ProcessDailyChange()
    {
        int baseHungerLoss = 15;
        int baseTempLoss = 1;
        
        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] == null) continue;                      //그룹중 1명이 사망해도 계속 진행
            {
                GroupMemberSO member = groupMembers[i];

                //나이에 따른 배고픔 조정
                float hungerMultiplier = member.agegroup == GroupMemberSO.AgeGroup.Child ? 0.8f : 1.0f;

                //상태 감소
                memberHunger[i] -= Mathf.RoundToInt(baseHungerLoss * hungerMultiplier);         //맵버별 배고픔 저항 설정
                memberBodyTemp[i] -= Mathf.RoundToInt(baseTempLoss * member.coldResistance);   //맴버별 추위 저항력

                //건강 체크
                if (memberHunger[i] <= 0) memberHunger[i] -= 15;                    //굶주림
                if (memberBodyTemp[i] <= 32) memberHealth[i] -= 10;                //저체온증 (32도 이하)
                if (memberBodyTemp[i] <= 30) memberHealth[i] -= 20;                //심각한 저체온증

                //최소값 제한
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
        CheckRandomEvent();             //이벤트 체크
        UpdateUI();
        CheackGameOver();
    }

    string GetMemberStatus(int memberIndex)
    {
        //사망 체크
        if (memberHealth[memberIndex] <= 0)
            return "(사망)";

        //가장 위험한 상태부터 체크
        if (memberBodyTemp[memberIndex] <= 30) return "(심각한 저체온증)";
        else if (memberHealth[memberIndex] <= 20) return "(위험)";
        else if (memberHunger[memberIndex] <= 10) return "(굶주림)";
        else if (memberBodyTemp[memberIndex] <= 32) return "(저체온증)";
        else if (memberHealth[memberIndex] <= 50) return "(약함)";
        else if (memberHunger[memberIndex] <= 30) return "(배고픔)";
        else if (memberBodyTemp[memberIndex] <= 35) return "(추위)";
        else return "(건강)";
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
            Debug.Log("게임 오버! 모든 구성원이 혹독한 상황을 이겨내지 못했습니다.");
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

    public void UseAllMembersFoodItem()                                               //음식 아이템 사용
    {
        if (food <= 0 || foodItem == null) return;                          //오류 방지 처리

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
            if (groupMembers[i] != null && memberHealth[i] > 0)                                 //살아있는 가족만
            {
                ApplyItemEffect(i, item);
            }
        }
    }

    void UseItemOnMember(int memberIndex, ItemSO item)              //실제 아이템 사용 매서드
    {
        if (groupMembers[memberIndex] != null && memberHealth[memberIndex] > 0)
        {
            ApplyFMItemEffect(memberIndex, item);
        }
    }

    public void OnClickGiveItem(int memberIndex, ItemType type)                 //버튼 클릭시 차감, UI적용을 위한 switch문 사용
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


    //특정 가족에게만 음식 주기

    public void GiveFoodToMember(int memberIndex)
    {
        if (food <= 0 || foodItem == null) return;
        if (memberHealth[memberIndex] <= 0) return;

        food--;
        ApplyItemEffect(memberIndex, foodItem);
        UpdateUI();
    }

    public void HealToMember(int memberIndex)
    {
        if (medicine <= 0 || medicineItem == null) return;
        if (memberHealth[memberIndex] <= 0) return;

        medicine--;
        ApplyItemEffect(memberIndex, medicineItem);
        UpdateUI();
    }


    void ApplyItemEffect(int memberIndex, ItemSO item)
    {
        GroupMemberSO member = groupMembers[memberIndex];

        //개인 특성 적용해서 아이템 효과 계산
        int actualHealth = Mathf.RoundToInt(item.healthEffect * member.recoveryRate);
        int actualHunger = Mathf.RoundToInt(item.hungerEffect * member.foodEfficiency);
        int actualTemp = item.tempEffect;

        //효과 적용
        memberHealth[memberIndex] += actualHealth;
        memberHunger[memberIndex] += actualHunger;
        memberBodyTemp[memberIndex] += actualTemp;

        //최대치 제한
        memberHealth[memberIndex] = Mathf.Min(memberHealth[memberIndex], member.maxHealth);
        memberHunger[memberIndex] = Mathf.Min(memberHunger[memberIndex], member.maxHunger);
        memberBodyTemp[memberIndex] = Mathf.Min(memberBodyTemp[memberIndex], member.normalBodyTemp);
    }

    void ApplyFMItemEffect(int memberIndex, ItemSO item)
    {
        GroupMemberSO member = groupMembers[memberIndex];

        //개인 특성 적용해서 아이템 효과 계산
        int actualHealth = Mathf.RoundToInt(item.healthEffect * member.recoveryRate);
        int actualHunger = Mathf.RoundToInt(item.hungerEffect * member.foodEfficiency);

        //효과 적용
        memberHealth[memberIndex] += actualHealth;
        memberHunger[memberIndex] += actualHunger;

        //최대치 제한
        memberHealth[memberIndex] = Mathf.Min(memberHealth[memberIndex], member.maxHealth);
        memberHunger[memberIndex] = Mathf.Min(memberHunger[memberIndex], member.maxHunger);
    }

    //이벤트에 따른 변경 수치 함수
    void ApplyEventEffects(EventsSO eventsSO)
    {
        //자원 변화
        food += eventsSO.foodChange;
        fuel += eventsSO.fuelChange;
        medicine += eventsSO.medicineChange;

        //자원 최소값 보정
        food = Mathf.Max(0, food);
        fuel = Mathf.Max(0, fuel);
        medicine = Mathf.Max(0, medicine);

        //모든 살아있는 멤버에게 상태 변화 적용
        for (int i = 0; i < groupMembers.Length; i++)
        {
            if (groupMembers[i] != null && memberHealth[i] >0)
            {
                memberHealth[i] += eventsSO.healthChange;
                memberHunger[i] += eventsSO.hungerChange;
                memberBodyTemp[i] += eventsSO.tempChange;

                //제한 값 적용
                GroupMemberSO member = groupMembers[i];
                memberHealth[i] = Mathf.Clamp(memberHealth[i], 0, member.maxHealth);
                memberHunger[i] = Mathf.Clamp(memberHunger[i], 0, member.maxHunger);
                memberBodyTemp[i] = Mathf.Clamp(memberBodyTemp[i], 0, member.normalBodyTemp);
            }
        }
    }

    void ShowEventPopup(EventsSO eventsSO)
    {
        //팝업 활성화
        eventPopup.SetActive(true);

        //텍스트 설정
        eventTitleText.text = eventsSO.eventTitle;
        eventDescritionText.text = eventsSO.eventDescription;

        //이벤트 효과 적용
        ApplyEventEffects(eventsSO);

        //게임 진행 일시정지
        nextDayButton.interactable = false;

    }

    public void CloseEventPopup()
    {
        eventPopup.SetActive(false);
        nextDayButton.interactable = true;
        UpdateUI();
    }

    void CheckRandomEvent()
    {
        int totalProbability = 0;

        //전체 확률 합 구하기
        for (int i = 0; i < events.Length; i++)
        {
            totalProbability += events[i].probability;
        }

        if (totalProbability == 0)
            return;                         //모든 이벤트 확률이 0이면 이벤트 없음

        int roll = Random.Range(1, totalProbability + 1 + 50);          //전체 확률 더하기에 아무것도 없을 확률50
        int cumualtive = 0;

        for (int i = 0; i < events.Length; i++)
        {
            cumualtive += events[i].probability;
            if (roll <= cumualtive)
            {
                ShowEventPopup(events[i]);
                return;
            }
        }
    }
}
