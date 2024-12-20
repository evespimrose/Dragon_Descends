using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class UIManager : SingletonManager<UIManager>
{
    public Canvas canvas;

    public TMP_Text Timetext;
    private float Timer;
    public TMP_Text currLeveltext;
    public TMP_Text nextLeveltext;

    public Image expImage;

    public PausePanel pausePanel;
    public LevelUpPanel levelUpPanel;
    public GameOverPanel gameOverPanel;

    public Button pauseButton;

    private bool isPaused = false;
    private bool isGameOver = false;
    private bool isLevelUp = false;

    private List<int> SkillIndexList = new List<int> { 0, 1, 2, 3 };

    public List<Sprite> SkillSpriteList = new(4);

    public List<Image> SelectedSkills;
    private void Start()
    {
        pauseButton.onClick.AddListener(() => { GamePauseResume(); });
        pausePanel.resumeButton.onClick.AddListener(() => { GamePauseResume(); });
        pausePanel.quitButton.onClick.AddListener(() => { GameQuit(); });
        gameOverPanel.reStartButton.onClick.AddListener(() => { OnGameReStart(); });
        gameOverPanel.quitButton.onClick.AddListener(() => { GameQuit(); });
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
        if(CharacterManager.Instance.player.p_isAlive)
        {
            Timer = GameManager.Instance.timeSinceStart;

            int minute = (int)(Timer / 60);
            int second = (int)(Timer % 60);

            Timetext.text = $"{minute:D2} : {second:D2}";
        }
    }

    private void GamePauseResume()
    {
        isPaused = !isPaused;
        pausePanel.gameObject.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1.0f;
    }

    private void PauseResume()
    {
        isLevelUp = !isLevelUp;
        levelUpPanel.gameObject.SetActive(isLevelUp);
        Time.timeScale = isPaused ? 0f : 1.0f;
    }

    public void GameOverPauseResume(bool flag = true)
    {
        isGameOver = !isGameOver;
        if (flag)
        {
            gameOverPanel.messageText.text = "Game Over......";
        }
        else
        {
            gameOverPanel.messageText.text = "Game Clear!!!!!";
        }
        int minute = (int)(Timer / 60);
        int second = (int)(Timer % 60);

        gameOverPanel.TimeText.text = $"�ҿ� �ð� \n {minute:D2} : {second:D2}";
        gameOverPanel.gameObject.SetActive(isGameOver);
        Time.timeScale = isGameOver ? 0f : 1.0f;
    }

    public void GameSkillLevelUpPauseResume()
    {

        isLevelUp = !isLevelUp;
        levelUpPanel.gameObject.SetActive(isLevelUp);

        Tuple<int, int> skillPair = RandomSkillPop();

        SetLevelUpPanel(skillPair.Item1, skillPair.Item2);

        Time.timeScale = isLevelUp ? 0f : 1.0f;
    }

    private Tuple<int, int> RandomSkillPop()
    {
        if (SkillIndexList.Count < 2)
        {
            return new Tuple<int, int>(SkillIndexList[0], -1);
        }

        int index1 = Random.Range(0, SkillIndexList.Count);
        int index2;

        do
        {
            index2 = Random.Range(0, SkillIndexList.Count);
        }
        while (index1 == index2);

        return new Tuple<int, int>(SkillIndexList[index1], SkillIndexList[index2]);
    }

    private void SetLevelUpPanel(int skillIndex1, int skillIndex2)
    {
        InitButtons();
        print($"{skillIndex1}, {skillIndex2}");
        levelUpPanel.skill1Image.sprite = SkillSpriteList[skillIndex1];
        levelUpPanel.skill2Image.sprite = SkillSpriteList[skillIndex2];
        print(SkillSpriteList[skillIndex1].name + ", " + SkillSpriteList[skillIndex2].name);

        switch (skillIndex1)
        {
            case 0:
                levelUpPanel.skill1Button.onClick.AddListener(OnClickBeam);
                levelUpPanel.skill1NameText.text = "���ٱ�";
                levelUpPanel.skill1DescText.text = "������ �� �߻�";
                break;
            case 1:
                levelUpPanel.skill1Button.onClick.AddListener(OnClickBomber);
                levelUpPanel.skill1NameText.text = "���߱�";
                levelUpPanel.skill1DescText.text = "ù ���� �� ����";
                break;
            case 2:
                levelUpPanel.skill1Button.onClick.AddListener(OnClickCrescent);
                levelUpPanel.skill1NameText.text = "�ʽ±�";
                levelUpPanel.skill1DescText.text = "�ʽ´� ��� ����ü �߻�";
                break;
            case 3:
                levelUpPanel.skill1Button.onClick.AddListener(OnClickHoop);
                levelUpPanel.skill1NameText.text = "������";
                levelUpPanel.skill1DescText.text = "ù ���� �� ������� �п�";
                break;
        }

        switch (skillIndex2)
        {
            case 0:
                levelUpPanel.skill2Button.onClick.AddListener(OnClickBeam);
                levelUpPanel.skill2NameText.text = "���ٱ�";
                levelUpPanel.skill2DescText.text = "������ �� �߻�";
                break;
            case 1:
                levelUpPanel.skill2Button.onClick.AddListener(OnClickBomber);
                levelUpPanel.skill2NameText.text = "���߱�";
                levelUpPanel.skill2DescText.text = "ù ���� �� ����";
                break;
            case 2:
                levelUpPanel.skill2Button.onClick.AddListener(OnClickCrescent);
                levelUpPanel.skill2NameText.text = "�ʽ±�";
                levelUpPanel.skill2DescText.text = "�ʽ´� ��� ����ü �߻�";
                break;
            case 3:
                levelUpPanel.skill2Button.onClick.AddListener(OnClickHoop);
                levelUpPanel.skill2NameText.text = "������";
                levelUpPanel.skill2DescText.text = "ù ���� �� ������� �п�";
                break;
        }
    }

    private void InitButtons()
    {
        levelUpPanel.skill1Button.onClick.RemoveAllListeners();
        levelUpPanel.skill2Button.onClick.RemoveAllListeners();
    }

    private void OnClickBeam() 
    {
        Player p = CharacterManager.Instance.player;
        SelectedSkills[p.level - 1].sprite = SkillSpriteList[0];
        SelectedSkills[p.level - 1].color = Color.red;
        Color color = SelectedSkills[p.level - 1].color;
        color.a = 1.0f;
        SelectedSkills[p.level - 1].color = color;

        BodyPart newBody = Instantiate(Resources.Load<BodyPart>("Body"), p.parts[p.parts.Count - 1].transform.position, Quaternion.identity);

        newBody.prevBodyPart = p.parts[p.parts.Count - 1].gameObject;

        p.parts.Insert(p.parts.Count, newBody);
        if (newBody.TryGetComponent<Body>(out Body body))
        {
            Beam skill = newBody.gameObject.AddComponent<Beam>();
            skill.Cannon = body.cannon;
        }
        newBody.SetupJoint();
        p.tail.ChangeChaseBodyPart(newBody.gameObject);

        SkillIndexList.Remove(0);
        InitButtons();
        PauseResume();
    }
    private void OnClickBomber()
    {
        Player p = CharacterManager.Instance.player;
        SelectedSkills[p.level - 1].sprite = SkillSpriteList[1];
        Color color = SelectedSkills[p.level - 1].color;
        color.a = 1.0f;
        SelectedSkills[p.level - 1].color = color;

        BodyPart newBody = 
            Instantiate(Resources.Load<BodyPart>("Body"), 
            p.parts[p.parts.Count - 1].transform.position, Quaternion.identity);

        newBody.prevBodyPart = p.parts[p.parts.Count - 1].gameObject;

        p.parts.Insert(p.parts.Count, newBody);
        if (newBody.TryGetComponent<Body>(out Body body))
        {
            Bomber skill = newBody.gameObject.AddComponent<Bomber>();
            skill.Cannon = body.cannon;
        }
        newBody.SetupJoint();
        p.tail.ChangeChaseBodyPart(newBody.gameObject);

        SkillIndexList.Remove(1);
        InitButtons();
        PauseResume();
    }
    private void OnClickCrescent() 
    {
        //print("�ʽ´�" + isPaused);

        Player p = CharacterManager.Instance.player;
        SelectedSkills[p.level - 1].sprite = SkillSpriteList[2];
        Color color = SelectedSkills[p.level - 1].color;
        color.a = 1.0f;
        SelectedSkills[p.level - 1].color = color;

        BodyPart newBody = Instantiate(Resources.Load<BodyPart>("Body"), p.parts[p.parts.Count - 1].transform.position, Quaternion.identity);

        newBody.prevBodyPart = p.parts[p.parts.Count - 1].gameObject;

        p.parts.Insert(p.parts.Count, newBody);
        if (newBody.TryGetComponent<Body>(out Body body))
        {
            Crescent skill = newBody.gameObject.AddComponent<Crescent>();
            skill.Cannon = body.cannon;
        }
        newBody.SetupJoint();
        p.tail.ChangeChaseBodyPart(newBody.gameObject);

        SkillIndexList.Remove(2);
        InitButtons();
        PauseResume();
    }
    private void OnClickHoop() 
    {
        print("������" + isPaused);

        Player p = CharacterManager.Instance.player;
        SelectedSkills[p.level - 1].sprite = SkillSpriteList[3];
        Color color = SelectedSkills[p.level - 1].color;
        color.a = 1.0f;
        SelectedSkills[p.level - 1].color = color;

        BodyPart newBody = Instantiate(Resources.Load<BodyPart>("Body"), p.parts[p.parts.Count - 1].transform.position, Quaternion.identity);

        newBody.prevBodyPart = p.parts[p.parts.Count - 1].gameObject;

        p.parts.Insert(p.parts.Count, newBody);
        if (newBody.TryGetComponent<Body>(out Body body))
        {
            Hoop skill = newBody.gameObject.AddComponent<Hoop>();
            skill.Cannon = body.cannon;
        }
        newBody.SetupJoint();
        p.tail.ChangeChaseBodyPart(newBody.gameObject);

        SkillIndexList.Remove(3);
        InitButtons();
        PauseResume();
    }

    public void OnGameReStart()
    {
        GameManager.Instance.timeSinceStart = 0f;

        for (int i = CharacterManager.Instance.enemies.Count - 1; i >= 0; --i)
        {
            GameObject enemy = CharacterManager.Instance.enemies[i].gameObject;
            enemy.SetActive(false);
            Destroy(enemy);
        }
        CharacterManager.Instance.enemies.Clear();

        Player player = CharacterManager.Instance.player;
        player.ResetPlayer();

        foreach (var skillImage in SelectedSkills)
        {
            skillImage.sprite = null;
            Color color = Color.white;
            color.a = 0f;
            skillImage.color = color;
        }

        SkillIndexList.Clear();
        for (int i = 0; i < 4; ++i)
            SkillIndexList.Add(i);

        isPaused = false;
        gameOverPanel.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void GameQuit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
