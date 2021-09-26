using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    void Start()
    {
        SceneManager.LoadScene("Singletons", LoadSceneMode.Additive);
    }
}
