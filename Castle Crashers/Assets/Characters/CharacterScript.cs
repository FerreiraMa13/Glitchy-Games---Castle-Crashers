using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    GameObject character;
    Game_Manager manager_script;
    [SerializeField] private Transform targetTransform;
    private Transform selfTransform;
    //public Transform selfSpawn; 
    private float speedMultiplier = 1;
    public float moveSpeed = 1;
    private BoxCollider2D attackTrigger;
    private Vector2 attackRange = new Vector2(0, 0);
    private Vector2 characterSize = new Vector2(1, 1);
    public GameObject target;
    public float start_cd = 1f;
    public float follow_cd = 0.25f;
    public int full_hearts = 2;
    public bool online = false;
    [SerializeField] private bool inCombat = true;
    [SerializeField] private bool playerInAttack = false;
    [SerializeField] private int hearts;
    [SerializeField] private int damage = 1;
    
    public enum CharacterDirection
    {
        LEFT,
        UP,
        RIGHT,
        DOWN
    }

    private CharacterDirection direction; 
    
    // Start is called before the first frame update
    void Awake()
    {
        attackTrigger = GameObject.Find("Attack Range").GetComponent<BoxCollider2D>();
        character = gameObject;
        attackTrigger.size = new Vector2(attackRange.x, attackRange.y);
        //transform.position = new Vector2(selfSpawn.position.x, selfSpawn.position.y);
        selfTransform = transform;
        manager_script = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<Game_Manager>();
        target = GameObject.FindGameObjectWithTag("Player");
        hearts = full_hearts;
    }
    private void Start()
    {
        InvokeRepeating("RotationManage", start_cd, follow_cd);
    }
    private void FixedUpdate()
    {
        //Debug.Log("in udpate");
        if (online)
        {
            switch (direction)
            {
                case CharacterDirection.DOWN:
                    transform.position = new Vector2(transform.position.x, transform.position.y - (moveSpeed * Time.deltaTime * speedMultiplier));
                    break;
                case CharacterDirection.LEFT:
                    transform.position = new Vector2(transform.position.x - (moveSpeed * Time.deltaTime * speedMultiplier), transform.position.y);
                    break;
                case CharacterDirection.UP:
                    transform.position = new Vector2(transform.position.x, transform.position.y + (moveSpeed * Time.deltaTime * speedMultiplier));
                    break;
                case CharacterDirection.RIGHT:
                    transform.position = new Vector2(transform.position.x + (moveSpeed * Time.deltaTime * speedMultiplier), transform.position.y);
                    break;
            }
        }
    }
    public void RotationManage()
    {
        if(online)
        {
            changeDirection(target);
        }
    }
    public void changeDirection(GameObject targetObject)
    {
        float xDif = transform.position.x - targetObject.transform.position.x;
        float yDif = transform.position.y - targetObject.transform.position.y;
        float xDifNormal = Mathf.Abs(xDif);
        float yDifNormal = Mathf.Abs(yDif);

        if (yDifNormal > xDifNormal)
        {
            if (yDif > 0)
            {
                //transform.Rotate(0, 0, 270);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 90);
                direction = CharacterDirection.DOWN;
            }
            else
            {
                //transform.Rotate(0, 0, 90);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 270);
                direction = CharacterDirection.UP;

            }
        }    
        else
        {
            if (xDif > 0)
            {
                //transform.Rotate(0, 0, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                direction = CharacterDirection.LEFT;

            }
            else
            {
                //transform.Rotate(0, 0, 180);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                direction = CharacterDirection.RIGHT;

            }
        }
    }
    private void OnTriggerEnter2D(Collider2D targetCollider)
    {
        if(online)
        {
            if (targetCollider.gameObject.tag == "Player")
            {
                Debug.Log("in Combat");
                target = targetCollider.gameObject;
                inCombat = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(online)
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("NOT Combat");
                /*inCombat = false;*/
            }
        }
    }
    public bool RestartEnemy()
    {
        if (!online)
        {
            hearts = full_hearts;
            online = true;
            return true;
        }
        return false;
    }
}
