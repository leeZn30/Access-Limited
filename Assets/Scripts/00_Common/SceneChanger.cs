using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static void goScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
