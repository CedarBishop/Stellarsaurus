using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public string battleSceneName;
    public void PlayButton ()
    {
        SceneManager.LoadScene(battleSceneName);
    }

    public void QuitButton ()
    {
        Application.Quit();
    }
}
