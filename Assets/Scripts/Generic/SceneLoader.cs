using SuperMaxim.Messaging;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        Messenger.Default.Subscribe<int>(UnloadGame);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<int>(UnloadGame);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadLobby()
    {
        SceneManager.LoadScene("CupSelection");
    }

    public async Task LoadLobbyAsync()
    {
        SceneManager.LoadSceneAsync("CupSelection", LoadSceneMode.Additive);
    }

    public void UnloadGame(int a)
    {
        SceneManager.UnloadSceneAsync("Game");
        Destroy(FindObjectOfType<EndScreen>().gameObject);
    }
}
