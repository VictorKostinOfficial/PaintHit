using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Image BG;
    public Sprite[] sprites;
    public GameObject pauseScreen;
    
    void Start()
    {
        BG.sprite = sprites[Random.Range(0,sprites.Length-1)];
    }

    public void OnPauseClick()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    public void OnUnpauseClick()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }
}
