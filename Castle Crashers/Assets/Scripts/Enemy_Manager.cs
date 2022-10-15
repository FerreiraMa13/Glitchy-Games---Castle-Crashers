using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    private List<GameObject> enemies = new List<GameObject>();
    private void Awake()
    {
        var found_enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(var enemy in found_enemies)
        {
            enemies.Add(enemy);
        }
    }
    
}
