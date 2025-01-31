using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbLadder : MonoBehaviour
{
    GameObject subLadder;
    GameObject player;
    Vector3 goalPos;

    public bool isOpen;
    bool isInteract;

    void Start()
    {
        subLadder = this.transform.GetChild(0).gameObject;
        player = GameObject.FindWithTag("Player");
        goalPos = new Vector3(
            subLadder.transform.position.x, subLadder.transform.position.y - 5.5f, subLadder.transform.position.z);
    }

    void Update()
    {
        if (isOpen)
        {
            subLadder.transform.position = Vector3.MoveTowards(
                subLadder.transform.position, goalPos, 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.F) && isInteract)
        {
            if (!isOpen)
                isOpen = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isInteract = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isInteract = false;
    }
}
