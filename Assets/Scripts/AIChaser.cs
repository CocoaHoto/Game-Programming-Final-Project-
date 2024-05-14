using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AIChaser : MonoBehaviour
{
    public float threshold = 7.0f;
    public float speed = 2.0f;
    GameObject player;
    Rigidbody2D rb;
    AIHelper.State state = AIHelper.State.IDLE;
    SpriteRenderer spriteRenderer;
    private float time;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == AIHelper.State.IDLE){
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if(distance < threshold){
                state = AIHelper.State.CHASE;
            }
        } else if(state == AIHelper.State.CHASE) {
            var direction = player.transform.position - transform.position;
            direction = direction.normalized;
            if(direction.x < 0) {
                spriteRenderer.flipX = true;
            } else {
                spriteRenderer.flipX = false;
            }
            rb.velocity = direction * speed;
        } 
    }
}
