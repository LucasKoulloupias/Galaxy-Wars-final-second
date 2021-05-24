using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void NextScene()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("UnitMovement"))
            SceneManager.LoadScene("SampleScene");
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SampleScene"))
            SceneManager.LoadScene("UnitMovement");
    }
}