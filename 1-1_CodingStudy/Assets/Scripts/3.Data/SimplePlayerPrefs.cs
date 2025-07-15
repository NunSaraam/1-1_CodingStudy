using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimplePlayerPrefs : MonoBehaviour
{
    public InputField nameInput;                        //�۾��� �Է� ���� �� �ִ� ui
    public Text scoreText;                              //���ھ� ui text
    public Button saveButton;                           //���� ��ư
    public Button loadButton;                           //�ε� ��ư

    int currentScore = 0;                               //���� ���ھ�

    private void Start()
    {
        saveButton.onClick.AddListener(SaveData);                       //���̺� ��ư�� Ŭ�� �� SaveData �Լ��� �����Ѵ�. 
        loadButton.onClick.AddListener(LoadData);                       //�ε� ��ư�� Ŭ�� �� LoadData �Լ��� �����Ѵ�.

        LoadData();                                                     //���� �� �� �ڵ� �ε�
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentScore += 10;
            scoreText.text = "score " + currentScore;
        }
    }

    void SaveData()                     //������ ���� �Լ�
    {
        PlayerPrefs.SetString("PlayerName", nameInput.text);                //�÷��̾� �̸��� UI�� �Է� �޾Ƽ� "PlayerName" �̸� ���� Ű�� ����
        PlayerPrefs.SetInt("HighScore", currentScore);                      //���� ���ھ� ���� "HighScore" �̸� ���� Ű�� ����
        PlayerPrefs.Save();

        Debug.Log("���� �Ϸ�");
    }

    void LoadData()                     //������ �ε� �Լ�
    {
        string savedName = PlayerPrefs.GetString("PlayerName", "Playername");               //Player Ű���� �����͸� �����´�.
        int saverScore = PlayerPrefs.GetInt("HighScore", 0);                                //HighScore Ű���� �����͸� �����´�.

        nameInput.text = savedName;                                                         //�ؽ�Ʈ�� ����� �̸� ���� �����´�.
        currentScore = saverScore;                                                          //����� ������ ����� �����͸� �����´�.
        scoreText.text = "score " + currentScore;                                           //UI ������Ʈ�� �Ѵ�.

        Debug.Log("�ҷ����� �Ϸ�");
    }
}
