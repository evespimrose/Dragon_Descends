using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : SingletonManager<UIManager>
{
    public Canvas canvas;

    public TMP_Text Timetext;
    private float Timer;
    public TMP_Text currLeveltext;
    public TMP_Text nextLeveltext;

    public Image expImage;

    public GameObject pausePanel;

    public Button pauseButton;
    public Button resumeButton;
    public Button quitButton;

    private bool isPaused = false;

    private void Start()
    {
        pauseButton.GetComponent<Button>().onClick.AddListener(() => { GamePauseResume(); });
        resumeButton.GetComponent<Button>().onClick.AddListener(() => { GamePauseResume(); });
        quitButton.GetComponent<Button>().onClick.AddListener(() => { GameQuit(); });

    }

    private void Update()
    {
        TikClock();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePauseResume();
        }
    }

    private void TikClock()
    {
        Timer = GameManager.Instance.timeSinceStart;

        int minute = (int)(Timer / 60);
        int second = (int)(Timer % 60);

        Timetext.text = minute + " : " + second;

    }

    private void GamePauseResume()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1.0f;
    }

    private void GameQuit()
    {

    }
}
