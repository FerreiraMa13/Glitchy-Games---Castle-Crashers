using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public enum PICKUPS
    {
            SHIELD = 0,
            HP = 1,
            ITEM = 2
    }

    [SerializeField] private float timer = 1;
    private float respawn_timer = 5.0f;
    public GameObject shield_prefab;
    public GameObject hp_prefab;
    public GameObject item_prefab;
    private GameObject prefab;
    public float spawn_rate = 100;
    public Vector2 offline_pos = new Vector2(900, 900);
    private List<Enemies> low_enemies = new List<Enemies>();
    private List<CharacterScript> high_enemies = new List<CharacterScript>();
    [SerializeField] private int round = 0;

    private void Awake()
    {
        var found_enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in found_enemies)
        {
            if(enemy.GetComponent<Enemies>() != null)
            {
                low_enemies.Add(enemy.GetComponent<Enemies>());
            }
            else if(enemy.GetComponent<CharacterScript>() != null)
            {
                high_enemies.Add(enemy.GetComponent<CharacterScript>());
            }
        }
    }
    private void Update()
    {
        if (respawn_timer <= 0)
        {
            timer += Time.deltaTime;
            if ((int)timer % 20 == 0)
            {
                timer++;
                /*
                 * buff enemies
                 * respawn enemies
                 */
                respawn_timer = 5.0f;
                round++;
            }
        }
        else
        {
            respawn_timer -= Time.deltaTime;
        }
    }
    public void CalculatePickUp(Vector2 position, float player_hp)
    {

        prefab = null;
        float roll = Random.Range(1, 100);
        Debug.Log(roll);
        if (roll <= spawn_rate)
        {
            int loot_id = Random.Range(0, 3);
            switch (loot_id) 
            {
                case 0:
                    prefab = shield_prefab;
                    break;
                case 1:
                    prefab = hp_prefab;
                    break;
                case 2:
                    prefab = item_prefab;
                    break;
            }
            Debug.Log(loot_id);
            Instantiate(prefab, position, Quaternion.Euler(0, 0, 0));
        }
        
    }
    public float getTimer()
    {
        return timer;
    }
    public bool spawnLowEnemy(Vector2 spawnpoint)
    {
        foreach(var enemy in low_enemies)
        {
            if(!enemy.online)
            {
                enemy.transform.position = spawnpoint;
                enemy.RestartEnemy();
                return true;
            }
        }
        return false;
    }
    public bool spawnHighEnemy(Vector2 spawnpoint)
    {
        foreach (var enemy in high_enemies)
        {
            if (!enemy.online)
            {
                enemy.transform.position = spawnpoint;
                enemy.RestartEnemy();
                return true;
            }
        }
        return false;
    }
    public void EndEnemy(GameObject enemy)
    {
        if(enemy.tag == "Enemy")
        {
            if(enemy.GetComponent<Enemies>() != null)
            {
                var enemy_script = enemy.GetComponent<Enemies>();
                if(low_enemies.Contains(enemy_script))
                {
                    enemy_script.online = false;
                    enemy.transform.position = offline_pos;
                }
            }
            if (enemy.GetComponent<CharacterScript>() != null)
            {
                var enemy_script = enemy.GetComponent<CharacterScript>();
                if (high_enemies.Contains(enemy_script))
                {
                    enemy_script.online = false;
                    enemy.transform.position = offline_pos;
                }
            }
        }
    }
}
