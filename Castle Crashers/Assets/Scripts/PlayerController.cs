using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //This is the input actions generated by Unity's new input system. I put it in just because it already has the Joystick stuff we would use, so we can make it work with it if we want
    CastleCrashers controls;

    //Powerup stuffs
    public float health = 100;
    public float shield_timer = 0;
    public projectiles projectileprefab;
    public Transform launch_offset;
    public int projectile_count;

    [System.NonSerialized]
    public Vector2 movement_input;
    public float movement_speed = 1;

    private void Awake()
    {
        controls = new CastleCrashers();
        controls.Player.Move.performed += ctx => movement_input = ctx.ReadValue<Vector2>();
        //controls.Player.Move.performed += ctx => Debug.Log("Move");
        controls.Player.Move.canceled += ctx => movement_input = Vector2.zero;
        controls.Player.Attack.started += ctx => Attack();

        //Powerup stuffs
        transform.Find("Shield").gameObject.SetActive(false);
    }

    void Shieldswap()
    {
        Transform Shield = transform.Find("Shield");
        if (Shield.gameObject.activeSelf)
            Shield.gameObject.SetActive(false);
        else
            Shield.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        Transform Shield = transform.Find("Shield");
        HandleMovement();
        if (Input.GetKey(KeyCode.D))
        {
            health--;
        }

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
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Health"))
        {
            health += 10;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Shield"))
        {
            Destroy(collision.gameObject);
            shield_timer = 10;
        }

        if (collision.gameObject.CompareTag("Item"))
        {
            projectile_count += 1;
            Destroy(collision.gameObject);
        }
    }


    private void Attack()
    {
        Debug.Log("Attack");
        if (projectile_count > 0)
        {
            Instantiate(projectileprefab, launch_offset.position, transform.rotation);
            projectile_count -= 1;
        }
    }

    private void HandleMovement()
    {
        Vector3 new_position = gameObject.transform.position;
        new_position.x += movement_input.x * movement_speed * Time.deltaTime;
        new_position.y += movement_input.y * movement_speed * Time.deltaTime;
        transform.position = new_position;
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
}
