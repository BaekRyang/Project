using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, 
            player.transform.position.y, transform.position.z);
    }
}
