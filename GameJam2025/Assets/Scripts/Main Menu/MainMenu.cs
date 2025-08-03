using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject infoScreen;
    public AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(audioSource.gameObject);
    }

    public void StartGame()
    {
        var lpFilters = audioSource.GetComponents<AudioLowPassFilter>();
        foreach (var filter in lpFilters)
        {
            filter.enabled = false;
        }
        var hpfilters = audioSource.GetComponents<AudioHighPassFilter>();
        foreach (var filter in hpfilters)
        {
            filter.enabled = false;
        }
        audioSource.volume = 0.162f;
        SceneManager.LoadScene("Main Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowInfo()
    {
        infoScreen.SetActive(true);
    }

    public void HideInfo()
    {
        infoScreen.SetActive(false);
    }
}
