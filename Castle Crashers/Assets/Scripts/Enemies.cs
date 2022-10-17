using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public int full_health;
    public int current_hp;
    public int Damage;
    public int AttackRate;
    public int MovementSpeed;
    public int RotationSpeed;
    public bool online = false;
    GameObject player;
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
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
        }
        //Vector3 targetDirection = player.transform.position - transform.position;
        //targetDirection.y = Vector3.zero.y;
        //targetDirection.x = Vector3.zero.x;
        //Vector3 newDirection = Vector3.RotateTowards(transform.right, targetDirection, RotationSpeed * Time.deltaTime, 0.0f);
        //.rotation = Quaternion.EulerRotation(newDirection);
        //transform.LookAt(player.transform, transform.up);
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
