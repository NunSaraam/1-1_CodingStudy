using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class JSONSaveExample : MonoBehaviour
{
    [Header("UI")]
    public InputField nameInput;                            //�̸� �Է� UI
    public Text levelText;                                  //���� �ؽ�Ʈ
    public Text goldText;                                   //�� �ؽ�Ʈ 
    public Text playTimeText;                               //�÷��� �ð� �ؽ�Ʈ
    public Button saveButton;                               //���̺� ��ư
    public Button loadButton;                               //�ε� ��ư


    PlayerData playerData;                                  //�÷��̾� ������ Ŭ���� ����
    string saveFilePath;                                    //������ Ȯ�ο�

    void Start()
    {
        //���� ���� ��� ����
        saveFilePath = Path.Combine(Application.persistentDataPath, "playerData.json");

        //������ �ʱ�
        playerData = new PlayerData();                      //new Ű����� ������ �ϴ°��̴�.
        playerData.playerName = "���ο� �÷��̾�";
        playerData.level = 1;
        playerData.gold = 100;
        playerData.playtime = 0f;
        playerData.position = Vector3.zero;

        //�ڵ� �ε�
        LoadFromJSON();
        UpdataUI();

        Debug.Log(saveFilePath);            //���� ��� ǥ��
    }

    void Update()
    {
        playerData.playtime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.L))
        {
            playerData.level++;
            playerData.gold += 50;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            playerData.gold += 10;
        }

        UpdataUI();
    }

    void UpdataUI()
    {
        nameInput.text = playerData.playerName;
        levelText.text = "Lv : " + playerData.level;
        goldText.text = "Gold : " + playerData.gold;
        playTimeText.text = "PlayTime : " + playerData.playtime;
    }

    void SaveToJSON()
    {
        playerData.playerName = nameInput.text;             //UI ���� �����Ϳ� ����

        string jsonData = JsonUtility.ToJson(playerData, true);         //JSON ���� ��ȯ

        File.WriteAllText (saveFilePath, jsonData);                 //���Ͽ� ����

        Debug.Log("���� �Ϸ�");
    }

    void LoadFromJSON()
    {
        if (File.Exists (saveFilePath))                 //������ �����ϴ��� Ȯ��
        {
            string jsonData = File.ReadAllText(saveFilePath);           //JSON ���� �б�

            playerData = JsonUtility.FromJson<PlayerData>(jsonData);        //JSON ��ü�� ��ȯ

            Debug.Log("�ҷ����� �Ϸ�");
        }
        else
        {
            Debug.Log("���� ������ �����ϴ�.");
        }
        
        UpdataUI();
    }
}
