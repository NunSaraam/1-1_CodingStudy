using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health = 100;          //체력 변수 선언

    private void Start()
    {
        GetComponent<Renderer>().material.color = Color.green;          //적을 초록색으로 만든다.
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(DamageEffect());

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator DamageEffect()
    {
        GetComponent<Renderer>().material.color = Color.red;            //적을 빨간색으로 만든다.
        yield return new WaitForSeconds(0.2f);                          //0.2초 후에
        GetComponent<Renderer>().material.color = Color.green;          //적을 초록색으로 만든다.
    }

    IEnumerator Die()
    {
        GetComponent<Renderer>().material.color = Color.red;            //적을 빨간색으로 만든다.
        Vector3 startScale = transform.localScale;
        float timer = 0f;

        while (timer < 0.5f)                        //0.5초 이하일때까지 반복 실행한다.
        {
            timer += Time.deltaTime;            //시간이 늘어난다.
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, timer / 0.5f);        //스케일을 자연스럽게 줄인다.
            yield return null;              //매 프레임 실행
        }

        Destroy(gameObject);                //오브젝트 파괴.
    }
}
