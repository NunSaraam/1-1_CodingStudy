using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("���� ����")]
    [SerializeField] private float attackRange = 5f;            //���� ������ 5
    [SerializeField] private int damage = 30;                   //�������� 30

    [Header("�ݺ��� ���� �ɼ�")]
    [SerializeField] private int loopType = 0;                  //0 = foreach, 1 = for, 2 = while, 3 = do while

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");                              //���� �̵� ����
        float v = Input.GetAxis("Vertical");                                //���� �̵� ����
        transform.Translate(new Vector3(h, 0, v) * 5f * Time.deltaTime);    //�ش� ������Ʈ ������ ����
    }

    void OnDrawGizmos()                                                     //Gizmos�� ���ؼ� ���ݹ����� ������ ��ü�� ǥ��
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AreaAttack();
        }
    }

    void AreaAttack()
    {
        //���� �� �� ã��
        List<Enemy> enemies = new List<Enemy>();                //�� ����Ʈ�� ����
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);      //���� ���� �ȿ� �ִ� �ݶ��̴����� �����´�.

        foreach (Collider col in colliders)                                                 //foreach ������ colliders �迭�� �ִ� ��� ������Ʈ�� �����ؼ�
        {
            Enemy enemy = col.GetComponent<Enemy>();                                        //Enemy ������Ʈ�� �޾ƿ���
            if (enemy != null) enemies.Add(enemy);                                          //Enemy ������Ʈ�� ���� ��� list �迭�� �߰��Ѵ�.
        }

        switch (loopType)
        {
            case 0: //foreach 
                foreach (Enemy enemy in enemies)                            //enemies ����Ʈ�� �ִ� ��� ������Ʈ�� �����ؼ�
                {
                    enemy.TakeDamage(damage);                               //�������� �ش�.
                }
                break;
            case 1: //for                                                   //For ������ ��ȯ
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].TakeDamage(damage);
                }
                break;
            case 2: //while                                                 //While ������ ��ȯ
                int j = 0;
                while (j < enemies.Count)
                {
                    enemies[j].TakeDamage(damage);
                    j++;
                }
                break;
            case 3: //do while                                              //DoWhile ������ ��ȯ
                if (enemies.Count > 0)
                {
                    int k = 0;
                    do
                    {
                        enemies[k].TakeDamage(damage);
                        k++;
                    }
                    while (k < enemies.Count);
                }
                break;
        }
    }
}
