using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    GameObject character;
    [SerializeField] private Transform targetTransform;
    private Transform selfTransform;
    //public Transform selfSpawn; 
    private float speedMultiplier = 1;
    public float moveSpeed = 1;
    private BoxCollider2D attackTrigger;
    private Vector2 attackRange = new Vector2(0, 0);
    private Vector2 characterSize = new Vector2(1, 1);
    public GameObject target;
    [SerializeField] private bool inCombat = false;
    [SerializeField] private bool playerInAttack = false;
    [SerializeField] private int hearts = 1;
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //Debug.Log("in udpate");
        if (inCombat)
        {
            changeDirection(target);
            switch (direction)
            {
                case CharacterDirection.DOWN:
                    transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed);
                    break;
                case CharacterDirection.LEFT:
                    transform.position = new Vector2(transform.position.x - moveSpeed, transform.position.y);
                    break;
                case CharacterDirection.UP:
                    transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed);
                    break;
                case CharacterDirection.RIGHT:
                    transform.position = new Vector2(transform.position.x + moveSpeed, transform.position.y);
                    break;
            }
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
                Debug.Log("up");
            }
        }    
        else
        {
            if (xDif > 0)
            {
                //transform.Rotate(0, 0, 0);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 0);
                direction = CharacterDirection.LEFT;
                Debug.Log("left");
            }
            else
            {
                //transform.Rotate(0, 0, 180);
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 180);
                direction = CharacterDirection.RIGHT;
                Debug.Log("right");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D targetCollider)
    {
        Debug.Log("in Combat");
        target = targetCollider.gameObject;
        inCombat = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("NOT Combat");
        inCombat = false;
    }
}