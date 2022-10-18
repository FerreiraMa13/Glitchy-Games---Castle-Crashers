using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public int full_health;
    public int current_hp;
    public int Damage;
    public float AttackRate;
    public float MovementSpeed;
    public int RotationSpeed;
    public bool online = false;
    public float movement_buff = 1f;
    GameObject player;
    public bool single_axis_movement = false;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Awake()
    {
        current_hp = full_health;
    }
    void Update()
    {
        if(online)
        {
            float step = MovementSpeed * Time.deltaTime;
            
            if (single_axis_movement)
            { 
                float dif_x = Mathf.Abs(transform.position.x - player.transform.position.x);
                float dif_y = Mathf.Abs(transform.position.y - player.transform.position.y);
                var player_x = new Vector2(player.transform.position.x, transform.position.y);
                var player_y = new Vector2(transform.position.x, player.transform.position.y);
                if (dif_x > dif_y)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player_x, step);
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, player_y, step);
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
            }
            
        }
        //Vector3 targetDirection = player.transform.position - transform.position;
        //targetDirection.y = Vector3.zero.y;
        //targetDirection.x = Vector3.zero.x;
        //Vector3 newDirection = Vector3.RotateTowards(transform.right, targetDirection, RotationSpeed * Time.deltaTime, 0.0f);
        //.rotation = Quaternion.EulerRotation(newDirection);
        //transform.LookAt(player.transform, transform.up);
    }
    public void BuffEnemyPassive()
    {
        MovementSpeed += movement_buff;
        if (AttackRate >= 0.3f)
            AttackRate -= 0.1f;
    }

    public void BuffEnemySpecial()
    {
        if (full_health < 3)
        {
            full_health += 1;
        }
    }
    public bool RestartEnemy()
    {
        if (!online)
        {
            current_hp = full_health;
            online = true;
            return true;
        }
        return false;
    }

}
