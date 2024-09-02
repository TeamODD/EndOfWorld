using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchManager : MonoBehaviour
{
    public void SceneTransmition()
    {
        SceneManager.LoadScene("TestScene1", LoadSceneMode.Single);
    }
}
