using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectiles : MonoBehaviour
{
    public Game_Manager manager_script;
    public float speed = 50f;

    private void Awake()
    {
        manager_script = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<Game_Manager>();
    }
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<Enemies>())
            {
                var low_enemy = collision.gameObject.GetComponent<Enemies>();
                low_enemy.current_hp--;
                if (low_enemy.current_hp < 1)
                {
                    manager_script.EndEnemy(collision.gameObject);
                }
            }
            else if (collision.gameObject.GetComponent<CharacterScript>())
            {
                var high_enemy = collision.gameObject.GetComponent<CharacterScript>();
                high_enemy.hearts--;
                if (high_enemy.hearts < 1)
                {
                    manager_script.EndEnemy(collision.gameObject);
                }
            }
            Destroy(gameObject);
        }
        if(collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
