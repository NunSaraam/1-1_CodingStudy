using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimplePlayerPrefs : MonoBehaviour
{
    public InputField nameInput;                        //글씨를 입력 받을 수 있는 ui
    public Text scoreText;                              //스코어 ui text
    public Button saveButton;                           //저장 버튼
    public Button loadButton;                           //로드 버튼

    int currentScore = 0;                               //현재 스코어

    private void Start()
    {
        saveButton.onClick.AddListener(SaveData);                       //세이브 버튼을 클릭 시 SaveData 함수를 실행한다. 
        loadButton.onClick.AddListener(LoadData);                       //로드 버튼을 클릭 시 LoadData 함수를 실행한다.

        LoadData();                                                     //시작 할 때 자동 로드
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentScore += 10;
            scoreText.text = "score " + currentScore;
        }
    }

    void SaveData()                     //데이터 저장 함수
    {
        PlayerPrefs.SetString("PlayerName", nameInput.text);                //플레이어 이름을 UI로 입력 받아서 "PlayerName" 이름 지은 키로 저장
        PlayerPrefs.SetInt("HighScore", currentScore);                      //현재 스코어 저장 "HighScore" 이름 지은 키로 저장
        PlayerPrefs.Save();

        Debug.Log("저장 완료");
    }

    void LoadData()                     //데이터 로드 함수
    {
        string savedName = PlayerPrefs.GetString("PlayerName", "Playername");               //Player 키에서 데이터를 가져온다.
        int saverScore = PlayerPrefs.GetInt("HighScore", 0);                                //HighScore 키에서 데이터를 가져온다.

        nameInput.text = savedName;                                                         //텍스트에 저장된 이름 값을 가져온다.
        currentScore = saverScore;                                                          //스토어 변수에 저장된 데이터를 가져온다.
        scoreText.text = "score " + currentScore;                                           //UI 업데이트를 한다.

        Debug.Log("불러오기 완료");
    }
}
