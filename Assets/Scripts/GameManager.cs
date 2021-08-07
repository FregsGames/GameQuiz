using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene("Singletons", LoadSceneMode.Additive);
    }
}
