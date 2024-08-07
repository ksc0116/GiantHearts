using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene("JYH");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene("KDW");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene("JJS");
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene("KSC");
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
