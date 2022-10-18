using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TMP_Text updatable_text;
    private void Awake()
    {
        if(StaticGlobals.final_timer != null)
        {
            updatable_text.text = StaticGlobals.final_timer;
        }
    }
    public void ExitMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
