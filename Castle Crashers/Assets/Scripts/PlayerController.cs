using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //This is the input actions generated by Unity's new input system. I put it in just because it already has the Joystick stuff we would use, so we can make it work with it if we want
    CastleCrashers controls;
    CharacterController controller;


    enum DIRECTION
    {
        NULL = -1,
        RIGHT = 0,
        UP = 1,
        LEFT = 2,
        DOWN = 3
    }
    //Powerup stuffs
    public float max_health = 3;
    public float health = 3;
    public float shield_timer = 0;
    public List<RawImage> hearts;
    [SerializeField] Color Red;
    [SerializeField] Color White;
    [SerializeField] Color Black;
    public projectiles projectileprefab;
    public Transform launch_offset;
    public int projectile_count;
    public float invunerability_timer = 2.5f;
    private float current_invun = 0.0f;
    private bool force_heart_update = false;
    private float last_update_hp;
    public bool immortal = false;

    [System.NonSerialized]
    public Vector2 movement_input;
    public float movement_speed = 2;

    public GameObject closest_enemy;
    public Game_Manager manager_script;
    private SpriteRenderer sprite;
    private AudioSource hit_sound;

    private void Awake()
    {
        if (sprite = transform.GetComponentInChildren<SpriteRenderer>())
        {
            if(StaticGlobals.player_color != null)
            {
                sprite.color = StaticGlobals.player_color;
            }
        }
        controller = GetComponent<CharacterController>();
        controls = new CastleCrashers();
        controls.Player.Move.performed += ctx => movement_input = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => movement_input = Vector2.zero;
        controls.Player.Attack.started += ctx => Attack();
        controls.Player.Debug.performed += ctx => DebugSpawnEnemy();

        //Powerup stuffs
        transform.Find("Shield").gameObject.SetActive(false);

        //Game Manager
        manager_script = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<Game_Manager>();
        health = max_health;

        hit_sound = GetComponent<AudioSource>();
    }
    void Shieldswap()
    {
        Transform Shield = transform.Find("Shield");
        if (Shield.gameObject.activeSelf)
            Shield.gameObject.SetActive(false);
        else
            Shield.gameObject.SetActive(true);
    }
    private void Update()
    {
        if(current_invun > 0)
        {
            current_invun -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        if (last_update_hp != health && health >= 0 || force_heart_update)
        {
            UpdateHealth();
            last_update_hp = health;
            /*heart_updated = true;*/
        }
        Transform Shield = transform.Find("Shield");
        HandleMovement();
        if (shield_timer > 0)
        {
            Shield.gameObject.SetActive(true);
            shield_timer -= Time.deltaTime;
            if (shield_timer < 3 && shield_timer > 0)
            {
                InvokeRepeating("Shieldswap", 0, 0.5f);
            }
        }
        else
        {
            CancelInvoke("Shieldswap");
            transform.Find("Shield").gameObject.SetActive(false);
        }

        if (shield_timer < 0)
        {
            force_heart_update = true;
            shield_timer = 0;
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            if (closest_enemy == null ||
                (transform.position - collision.transform.position).magnitude < 
                (transform.position - closest_enemy.transform.position).magnitude
                )
            {
                closest_enemy = collision.gameObject;
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Health"))
        {
            if(health < max_health)
            {
                health += 1;
            }
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Shield"))
        {
            Destroy(collision.gameObject);
            shield_timer = 10;
            force_heart_update = true;
        }
        if (collision.gameObject.CompareTag("Item"))
        {
            projectile_count += 1;
            Destroy(collision.gameObject);
        }
        
    }
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(current_invun <= 0)
            {
                if (shield_timer > 0)
                {
                    shield_timer = 0;
                    force_heart_update = true;
                }
                else
                {
                    health--;
                    Debug.Log(health);
                    if (health <= 0)
                    {
                        if(!immortal)
                        {
                            manager_script.PlayerDeath();
                        }
                    }
                }

                current_invun = invunerability_timer;
            }
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            closest_enemy = null;
        }
    }
    private void Attack()
    {
        Debug.Log("Attack");
        if(closest_enemy != null)
        {
            hit_sound.Play();
            Vector2 enemy_position = closest_enemy.transform.position;
            manager_script.CalculatePickUp(enemy_position, health);
            if(closest_enemy.GetComponent<Enemies>())
            {
                var low_enemy = closest_enemy.GetComponent<Enemies>();
                low_enemy.current_hp--;
                if(low_enemy.current_hp < 1)
                {
                    manager_script.EndEnemy(closest_enemy);
                }
            }
            else if(closest_enemy.GetComponent<CharacterScript>())
            {
                var high_enemy = closest_enemy.GetComponent<CharacterScript>();
                high_enemy.hearts--;
                if(high_enemy.hearts < 1)
                {
                    manager_script.EndEnemy(closest_enemy);
                }
            }
        }
        if (projectile_count > 0)
        {
            Instantiate(projectileprefab, launch_offset.position, transform.rotation);
            projectile_count -= 1;
        }
    }
    private void HandleMovement()
    {
        Vector3 new_position = gameObject.transform.position;
        if( Mathf.Abs(movement_input.x) >= Mathf.Abs(movement_input.y))
        {
            new_position.x += movement_input.x * movement_speed * Time.deltaTime;
        }
        else
        {
            new_position.y += movement_input.y * movement_speed * Time.deltaTime;
        }
        
        HandleRotation(transform.position, new_position);
        transform.position = new_position;
        
    }
    private void HandleRotation(Vector2 current_pos, Vector2 future_pos)
    {
        DIRECTION mov_direction = DIRECTION.NULL;
        Vector2 dif = future_pos - current_pos;
        Vector2 abs_dif = new Vector2(Mathf.Abs(dif.x), Mathf.Abs(dif.y));
        if (abs_dif.y > abs_dif.x) 
        {
            if(dif.y < 0)
            {
                mov_direction = DIRECTION.DOWN;
            }
            else if(dif.y > 0)
            {
                mov_direction = DIRECTION.UP;
            }
        }
        else if (abs_dif.x > abs_dif.y )
        {
            if(dif.x < 0)
            {
                mov_direction = DIRECTION.LEFT;
            }
            else
            {
                mov_direction = DIRECTION.RIGHT;
            }
        }

        switch(mov_direction)
        {
            case DIRECTION.UP:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case DIRECTION.RIGHT:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case DIRECTION.DOWN:
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
            case DIRECTION.LEFT:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
        }
    }
    public void DebugSpawnEnemy()
    {
        if (!manager_script.spawnLowEnemy(Vector2.zero))
        {
            if (!manager_script.spawnHighEnemy(Vector2.zero))
            {
                Debug.Log("No more enemies");
            }
        }
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    public void EnableInput()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    public void DisableInput()
    {
        controls.Player.Disable();
    }
    public void UpdateHealth()
    {
        foreach(var heart in hearts)
        {
            heart.color = Black;
        }

        for(int i =0; i < health; i++)
        {
            if(shield_timer > 0)
            {
                hearts[i].color = White;
            }
            else
            {
                hearts[i].color = Red;
            }
        }

        if(force_heart_update)
        {
            force_heart_update = false;
        }
/*
        if (health == 3)
        {
            HealthPointOne.color = Red;
            HealthPointTwo.color = Red;
            HealthPointThree.color = Red;
            if (shield_timer > 0)
            {
                HealthPointOne.color = White;
                HealthPointTwo.color = White;
                HealthPointThree.color = White;
            }
        }
        else if (health == 2)
        {
            HealthPointOne.color = Red;
            HealthPointTwo.color = Red;
            HealthPointThree.color = Black;
            if (shield_timer > 0)
            {
                HealthPointOne.color = White;
                HealthPointTwo.color = White;
                HealthPointThree.color = Black;
            }
        }
        else if (health == 1)
        {
            HealthPointOne.color = Red;
            HealthPointTwo.color = Black;
            HealthPointThree.color = Black;
            if (shield_timer > 0)
            {
                HealthPointOne.color = White;
                HealthPointTwo.color = Black;
                HealthPointThree.color = Black;
            }
        }
        else if (health == 0)
        {
            HealthPointOne.color = Black;
            HealthPointTwo.color = Black;
            HealthPointThree.color = Black;
        }*/
    }
}
