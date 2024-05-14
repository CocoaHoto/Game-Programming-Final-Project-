using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        var playerPOS = player.transform.position;
        var cameraPOS = transform.position;

        cameraPOS.x = playerPOS.x;
        cameraPOS.y = playerPOS.y;
        transform.position = cameraPOS;
    }
}
