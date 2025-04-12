using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private int gameScene;
    [SerializeField] private CanvasGroup signFader;
    [SerializeField] private Button startButton;
    [SerializeField] private float timer = 3f;
    [SerializeField] private AudioSource audioSource;
    private void Start()
    {
        signFader.alpha = 0f;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            signFader.alpha += Time.deltaTime;
            audioSource.Play();
            if (signFader.alpha >= 1)
            {
               audioSource.Stop();
                startButton.gameObject.SetActive(true);
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameScene);
    }
}
