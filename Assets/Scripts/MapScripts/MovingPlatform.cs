using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed;
    private float waitTime;
    public float totalTime;
    public Transform[] movePos;
    private Transform playerTransform;
    //i是1则右，是0则变成左
    private int i;
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform.parent;
        i = 1;
        waitTime = totalTime;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, movePos[i].position, moveSpeed * Time.deltaTime);
        //如果两点的距离小于等于0.1
        if (Vector2.Distance(transform.position, movePos[i].position) <= 0.1f)
        {
            //且等待时间小于0
            if (waitTime < 0)
            {
                if (i == 1)
                {
                    i = 0;
                }
                else
                {
                    i = 1;
                }
                waitTime = totalTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            //将movingPlateform作为player的父对象
            other.gameObject.transform.parent = gameObject.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.CapsuleCollider2D")
        {
            //将movingPlateform作为player的父对象
            other.gameObject.transform.parent = playerTransform;
        }
    }
}