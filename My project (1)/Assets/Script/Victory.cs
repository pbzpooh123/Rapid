using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public void Mainmenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void Credits()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
