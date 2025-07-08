using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeTest : MonoBehaviour
{

    public int number = 0;
    public GameObject temp;


    void Start()
    {
        FuntionTest_01();

        int myNumber = 10;

        number = FuntionTest_02(myNumber);

    }

    void FuntionTest_01()                   //따로 파라미터나 리턴 값이 필요 없을때
    {
        number += 1;
    }

    int FuntionTest_02(int num)
    {
        return num + 10;
    }


    void Update()
    {
        
    }
}
