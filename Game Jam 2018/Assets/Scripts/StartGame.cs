using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
    public bool isStart;
    public bool isQuit;

    void OnMouseDown()
    {
        if (isStart)
        {
            Debug.Log("Start");
            SceneManager.LoadScene(1);
        }
        if (isQuit)
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
