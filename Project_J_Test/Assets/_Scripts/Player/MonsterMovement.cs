using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Transform player;
    public int moveDir;
    public float speed = 3f;
    public float cliffRayLen = 2.0f;
    public float noticeDis = 6.0f;
    public float attackDis = 2.0f;
    public float rayXPos = 2.0f;

    private Rigidbody2D rb;
    private bool isNotice;

    private bool isAttack;  

    enum MonsterStates { Patrol, Notice, Attack, Delay }    //Delay�� ���� �̻��

    private static MonsterStates currentState = MonsterStates.Patrol;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //ó�� ������ ����
        moveDir = Random.Range(0, 2);
        if (moveDir == 0) moveDir = -1;
    }

    void Update()
    {
        //���� �˻� (�Դٰ���)
        Vector2 cliffV = new Vector2(transform.position.x + moveDir * rayXPos, rb.position.y);
        Debug.DrawRay(cliffV, Vector3.down * cliffRayLen, new Color(0, 0, 1));
        RaycastHit2D cliffRayHit = Physics2D.Raycast(cliffV, Vector3.down, cliffRayLen, LayerMask.GetMask("Platform"));

        //�ν� �˻�
        var distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackDis && isNotice)
            currentState = MonsterStates.Attack;
        else if(distance <= noticeDis && isNotice)
            currentState = MonsterStates.Notice;
        else
            currentState = MonsterStates.Patrol;

        switch (currentState)
        {
            case MonsterStates.Patrol:
                this.GetComponent<Renderer>().material.color = Color.green;

                //move forward
                speed = 3f;
                transform.Translate(new Vector3(moveDir * speed * Time.deltaTime, 0, 0));

                //switch direction
                if (cliffRayHit.collider == null)
                    moveDir *= -1;
                break;

            case MonsterStates.Notice:
                this.GetComponent<Renderer>().material.color = Color.yellow;

                //calc position with player
                if (this.transform.position.x < player.transform.position.x)
                    moveDir = 1;
                else
                    moveDir = -1;

                //chase speedly
                speed = 5f;
                transform.Translate(new Vector3(moveDir * speed * Time.deltaTime, 0, 0));
                break;

            case MonsterStates.Attack:
                this.GetComponent<Renderer>().material.color = Color.red;

                if (!isAttack)
                    StartCoroutine("attackMotion");
                break;
        }

    }

    IEnumerator attackMotion()  //���� ��� �Լ� (������ ���� �޶��� �� ����)
    {
        isAttack = true;
        this.transform.localScale = new Vector3(transform.localScale.x + 2.0f, transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(0.2f);
        this.transform.localScale = new Vector3(transform.localScale.x - 2.0f, transform.localScale.y, transform.localScale.z);
        yield return new WaitForSeconds(0.5f);
        isAttack = false;
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNotice = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isNotice = false;
        }
    }
}
