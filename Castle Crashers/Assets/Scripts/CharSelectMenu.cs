using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectMenu : MonoBehaviour
{
    private Scene gameplay;
    public Color player_red;
    public Color player_green;
    public Color player_yellow;
    public Color player_blue;

    private void Awake()
    {
        gameplay = SceneManager.GetSceneByName("Gameplay");
    }
    public void PlayGameCharacter()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void CharacterRed()
    {
        /*StaticGlobals.player_color_id = StaticGlobals.PLAYER_COLOR.RED;*/
        PlayGameCharacter();
    }
    public void CharacterYellow()
    {
        /*StaticGlobals.player_color_id = StaticGlobals.PLAYER_COLOR.YELLOW;*/
        PlayGameCharacter();
    }
    public void CharacterGreen()
    {
        /*StaticGlobals.player_color_id = StaticGlobals.PLAYER_COLOR.GREEN;*/
        PlayGameCharacter();
    }
    public void CharacterBlue()
    {
        /*StaticGlobals.player_color_id = StaticGlobals.PLAYER_COLOR.BLUE;*/
        PlayGameCharacter();
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
