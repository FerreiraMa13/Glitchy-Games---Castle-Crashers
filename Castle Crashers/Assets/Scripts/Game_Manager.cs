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

    public GameObject shield_prefab;
    public GameObject hp_prefab;
    public GameObject item_prefab;
    private GameObject prefab;
    public float spawn_rate = 100;

    public void CalculatePickUp(Vector2 position, float player_hp)
    {

        prefab = null;
        float roll = Random.Range(1, 100);
        Debug.Log(roll);
        if (roll <= spawn_rate)
        {
            int loot_id = Random.Range(0, 2);
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
}
