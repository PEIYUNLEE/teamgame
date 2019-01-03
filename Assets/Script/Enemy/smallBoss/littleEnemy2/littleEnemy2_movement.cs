﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class littleEnemy2_movement : MonoBehaviour {

    public Animator anim;
   
    Rigidbody2D playerRigidbody;
    public Transform main;//要跟随英雄
    public Transform count;

    bool border_tag = false;//是否碰到限制範圍
    float followDis = 30f;//達到此距離開始跟隨
    float attackDis = 20f;
    public float idle_speed = 10f;//移動速度
    public float follow_speed = 15f;//跟隨速度
    private littleEnemy2_health enemyHealth;
    float timer_enlarge=5;
    public float timer = 0;

    float timer_dead = 0;

    PlayerHealth playerHealth;
    public Enemy_count countAllEnemy1;
    //跟隨函式
    void follow()
    {
        //當於怪物的右側
        if (Vector2.Distance(transform.position, main.position) <= followDis && (main.position.x - transform.position.x > 0))//跟随距离
        {

            Vector2 transformValue = new Vector2(10, 0);
            playerRigidbody.velocity = transformValue;

        }

        //當於怪物的左側

        if (Vector2.Distance(transform.position, main.position) <= followDis && (main.position.x - transform.position.x < 0))//跟随距离
        {

            Vector2 transformValue = new Vector2(-10, 0);
            playerRigidbody.velocity = transformValue;

        }
    }


    //怪物idle狀態
    int i = 0;
    void idle()
    {
        timer += Time.deltaTime;
        Vector2 transformValue = new Vector2(idle_speed, 0);
        i++;
        playerRigidbody.velocity = transformValue;
        if (i == 100)
        {
            /*Vector2 temp = transform.localScale;
            temp.x *= -1;
            transform.localScale = temp;*/
            idle_speed = idle_speed * -1;
            i = 0;
        }
        /*Debug.Log(i);*/

        /*
        Vector2 transformValue = new Vector2(idle_speed, 0);

        playerRigidbody.velocity = transformValue;
        if (border_tag == true)
        {

            idle_speed = idle_speed * -1;

            border_tag = false;
        }


    */

    }


    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        main = GameObject.FindGameObjectWithTag("Player").transform;
        //Transform target = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<littleEnemy2_health>();
        
        anim = GetComponent<Animator>();
        count = GameObject.FindGameObjectWithTag("count").transform;
        countAllEnemy1 = count.GetComponent<Enemy_count>();
        /* Vector2 temp = transform.localScale;
         temp.x *= -1;
         transform.localScale = temp;*/
    }


    float initialx;

    bool stat_dead = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            playerHealth.TakeDamage(1);

        }
    }

    void resetanim()
    {
        Destroy(this.gameObject);
        anim.SetBool("littleEnemy2_hurt", false);
        CancelInvoke("resetanim");
    }
    
    void Update()
    {
        
        if (stat_dead == false && enemyHealth.isDead)
        {
            anim.SetBool("littleEnemy2_dead", true);
            stat_dead = true;
        }

        if (enemyHealth.isDead)
        {
            countAllEnemy1.littleEnemy2_count--;
            timer_dead += Time.deltaTime;
            Vector2 transformValue = new Vector2(0, 0);
            playerRigidbody.velocity = transformValue;
            Invoke("resetanim", 1f);




        }
        else
        {



            if ((Vector2.Distance(transform.position, main.position) <= followDis))
            {
                Vector2 transformValue = new Vector2(0, 0);
                playerRigidbody.velocity = transformValue;
                timer_enlarge += Time.deltaTime;
                if (((timer_enlarge % 5) > 1) && ((timer_enlarge % 5) < 3))
                {
                    this.gameObject.transform.localScale += new Vector3(0.01f, 0.01f, 0f);


                }

                else if (((timer_enlarge % 5) > 3) && ((timer_enlarge % 5) < 5))
                {
                    this.gameObject.transform.localScale += new Vector3(-0.01f, -0.01f, 0f);

                }


            }




            else if ((Vector2.Distance(transform.position, main.position) > attackDis))
            {
                timer_enlarge = 0;
                if (this.gameObject.transform.localScale.x != 1f)
                {
                    this.gameObject.transform.localScale += new Vector3(-0.01f, -0.01f, 0f);

                }
                idle();


            }

        }

    }
}