using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public enum PICKUPS
    {
        SHIELD = 0,
        HP = 1,
        ITEM = 2
    }

    /*[SerializeField] private float timer = 1;
    private float game_timer = 0.0f;
    [SerializeField] private TMP_Text timer_text;
    private float respawn_timer = 5.0f;
    public GameObject shield_prefab;
    public GameObject hp_prefab;
    public GameObject item_prefab;
    private GameObject prefab;
    public GameObject graveyard;
    public float spawn_rate = 100;
    public Vector2 offline_pos = new Vector2(900, 900);
    private List<Enemies> low_enemies = new List<Enemies>();
    private List<CharacterScript> high_enemies = new List<CharacterScript>();
    [SerializeField] private int round = 0;
    private int small_enm_spawn = 1;
    private int big_enm_spawn = 0;
    [SerializeField] private GameObject[] spawnpoints = { null, null, null };*/

    [SerializeField] private float timer = 1;
    private float game_timer = 0.0f;
    [SerializeField] private TMP_Text timer_text;
    private float respawn_timer = 5.0f;
    public GameObject shield_prefab;
    public GameObject hp_prefab;
    public GameObject item_prefab;
    private GameObject prefab;
    public GameObject graveyard;
    public float spawn_rate = 100;
    public Vector2 offline_pos = new Vector2(900, 900);

    private List<CharacterScript> low_enemies = new List<CharacterScript>();
    private List<CharacterScript> high_enemies = new List<CharacterScript>();

    [SerializeField] private int round = 0;
    private int small_enm_spawn = 1;
    private int big_enm_spawn = 0;
    [SerializeField] private GameObject[] spawnpoints = { null, null, null };


    private void Awake()
    {
        var found_enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in found_enemies)
        {
            /*if (enemy.GetComponent<Enemies>() != null)
            {
                low_enemies.Add(enemy.GetComponent<Enemies>());
            }*/
            if (enemy.GetComponent<CharacterScript>() != null)
            {
                if (!enemy.GetComponent<CharacterScript>().isStrong)
                {
                    low_enemies.Add(enemy.GetComponent<CharacterScript>());
                }
                else
                {
                    high_enemies.Add(enemy.GetComponent<CharacterScript>());
                }
            }
        }
    }
    private void Update()
    {
        int on_field_small = 0;
        int on_field_big = 0;
        //Debug.Log("low enemies - " + small_enm_spawn);
        //Debug.Log("big enemies - " + big_enm_spawn);
        if (respawn_timer <= 0)
        {
            timer += Time.deltaTime;
            foreach (var enemy in low_enemies)
            {
                
                if (enemy.online)
                {
                    on_field_small++;
                    //Debug.Log(on_field_small);
                }
               
            }
            foreach (var enemy in high_enemies)
            {
                if(enemy.online)
                {
                    on_field_big++;
                }
            }
            if (on_field_small < small_enm_spawn)
            {
                /*switch (on_field_small)
                {
                    case 0:
                        spawnLowEnemy(spawnpoints[0].transform.position);
                        break;

                    case 1:
                        spawnLowEnemy(spawnpoints[1].transform.position);
                        break;

                    case 2:
                        spawnLowEnemy(spawnpoints[2].transform.position);
                        break;

                }*/
                //GraveSpawn(low_enemies.)
                foreach (var enemy in low_enemies)
                {

                    if (!enemy.online)
                    {
                        GraveSpawn(enemy);
                        break;
                        //enemy.RestartEnemy();
                    }

                    //Debug.Log("I am now online - " + enemy.online);
                }
                //spawnLowEnemy(Vector2.zero);
            }

            if (on_field_big < big_enm_spawn)
            {
                /*switch (on_field_big)
                {

                    case 0:
                        spawnHighEnemy(spawnpoints[0].transform.position);
                        break;

                    case 1:
                        spawnHighEnemy(spawnpoints[1].transform.position);
                        break;

                    case 2:
                        spawnHighEnemy(spawnpoints[2].transform.position);
                        break;

                }*/

                foreach (var enemy in high_enemies)
                {
                    if (!enemy.online)
                    {
                        GraveSpawn(enemy);
                        break;
                        //enemy.RestartEnemy();
                    }
                }
            }

            if ((int)timer % 5 == 0)
            {
                timer++;
                respawn_timer = 5.0f;
                round++;
                if (round % 5 == 0)
                {
                    big_enm_spawn++;
                }
                if (small_enm_spawn < 3)
                {
                    small_enm_spawn++;
                }
                if(spawn_rate > 10)
                {
                    spawn_rate -= 4;
                    Debug.Log(spawn_rate);
                }
                foreach (var small_enm in low_enemies)
                {
                    small_enm.BuffEnemyPassive();
                }
                foreach (var big_enm in high_enemies)
                {
                    big_enm.BuffEnemyPassive();
                }
            }
        }
        else
        {
            respawn_timer -= Time.deltaTime;
        }

        game_timer += Time.deltaTime;
        /*int minutes = Mathf.FloorToInt(game_timer / 60.0f);
        int seconds = Mathf.FloorToInt(game_timer - minutes * 60);
        timer_text.text = string.Format("{0:00}:{1:00}", minutes, seconds);*/
        timer_text.text = UpdateTimer();
    }
    public void CalculatePickUp(Vector2 position, float player_hp)
    {

        prefab = null;
        float roll = Random.Range(1, 100);
        Debug.Log(roll);
        if (roll <= spawn_rate)
        {
            int loot_id = Random.Range(0, 4);
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
                case 3:
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
                /*var enemy_script = enemy.GetComponent<Enemies>();
                if(low_enemies.Contains(enemy_script))
                {
                    enemy_script.online = false;
                    enemy.transform.position = offline_pos;
                }*/
                var enemy_script = enemy.GetComponent<CharacterScript>();
                if (low_enemies.Contains(enemy_script))
                {
                    enemy_script.online = false;
                    enemy.transform.position = offline_pos;

                    //GraveSpawn(enemy);
                }
            }
            if (enemy.GetComponent<CharacterScript>() != null)
            {
                var enemy_script = enemy.GetComponent<CharacterScript>();
                if (high_enemies.Contains(enemy_script) || low_enemies.Contains(enemy_script))
                {
                    enemy_script.online = false;
                    //enemy.transform.position = offline_pos;
                    //enemy.transform.position = graveyard.transform.GetChild(GraveSpawn()).position;

                    enemy.transform.position = offline_pos;

                    //GraveSpawn(enemy);
                }
            }
        }
    }
    public void GraveSpawn(CharacterScript character)
    {
        int randomGravePointer = Random.Range(0, 4);
        character.transform.position = graveyard.transform.GetChild(randomGravePointer).position;
        character.RestartEnemy();
    }
    public void PlayerDeath()
    {
        UpdateTimer();
        StaticGlobals.final_timer = timer_text.text;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public string UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(game_timer / 60.0f);
        int seconds = Mathf.FloorToInt(game_timer - minutes * 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
