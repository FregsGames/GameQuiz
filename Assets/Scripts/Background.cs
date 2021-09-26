using UnityEngine;

public class Background : MonoBehaviour
{
    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }
}
