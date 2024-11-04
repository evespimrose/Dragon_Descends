using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : SingletonManager<UIManager>
{
    public Canvas canvas;

    public TMP_Text Timetext;
    private float Timer;
    public TMP_Text currLeveltext;
    public TMP_Text nextLeveltext;

    public Image expImage;

    public GameObject pausePanel;
    public LevelUpPanel levelUpPanel;

    public Button pauseButton;
    public Button resumeButton;
    public Button quitButton;

    private bool isPaused = false;

    private List<int> SkillIndexList = new List<int> { 0, 1, 2, 3 }; // Assume 4 unique skill indices

    public List<Sprite> SkillSpriteList = new(4);

    private void Start()
    {
        pauseButton.onClick.AddListener(() => { GamePauseResume(); });
        resumeButton.onClick.AddListener(() => { GamePauseResume(); });
        quitButton.onClick.AddListener(() => { GameQuit(); });
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

        Timetext.text = $"{minute:D2} : {second:D2}";
    }

    private void GamePauseResume()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1.0f;
    }

    public void GameSkillLevelUpPauseResume()
    {
        isPaused = !isPaused;
        levelUpPanel.gameObject.SetActive(isPaused);
        Tuple<int, int> skillPair = RandomSkillPop();

        // Call SetLevelUpPanel with the selected skills
        SetLevelUpPanel(skillPair.Item1, skillPair.Item2);

        Time.timeScale = isPaused ? 0f : 1.0f;
    }

    private Tuple<int, int> RandomSkillPop()
    {
        if (SkillIndexList.Count < 2)
        {
            return new Tuple<int, int>(SkillIndexList[0], -1);
        }

        int index1 = UnityEngine.Random.Range(0, SkillIndexList.Count);
        int index2;

        do
        {
            index2 = UnityEngine.Random.Range(0, SkillIndexList.Count);
        }
        while (index1 == index2);

        return new Tuple<int, int>(SkillIndexList[index1], SkillIndexList[index2]);
    }

    private void SetLevelUpPanel(int skillIndex1, int skillIndex2)
    {
        levelUpPanel.skill1Image.sprite = SkillSpriteList[skillIndex1];
        levelUpPanel.skill2Image.sprite = SkillSpriteList[skillIndex2];

        switch (skillIndex1)
        {
            case 0:
                levelUpPanel.skill1Button.onClick.AddListener(OnClickBeam);
                levelUpPanel.skill1NameText.text = "Beam";
                levelUpPanel.skill1DescText.text = "Fires a powerful energy beam.";
                break;
            case 1:
                levelUpPanel.skill1Button.onClick.AddListener(OnClickBomber);
                levelUpPanel.skill1NameText.text = "Bomber";
                levelUpPanel.skill1DescText.text = "Drops explosive bombs around.";
                break;
            case 2:
                levelUpPanel.skill1Button.onClick.AddListener(OnClickCrescent);
                levelUpPanel.skill1NameText.text = "Crescent";
                levelUpPanel.skill1DescText.text = "Unleashes a crescent-shaped slash.";
                break;
            case 3:
                levelUpPanel.skill1Button.onClick.AddListener(OnClickHoop);
                levelUpPanel.skill1NameText.text = "Hoop";
                levelUpPanel.skill1DescText.text = "Throws a hoop that returns.";
                break;
        }

        switch (skillIndex2)
        {
            case 0:
                levelUpPanel.skill2Button.onClick.AddListener(OnClickBeam);
                levelUpPanel.skill2NameText.text = "Beam";
                levelUpPanel.skill2DescText.text = "Fires a powerful energy beam.";
                break;
            case 1:
                levelUpPanel.skill2Button.onClick.AddListener(OnClickBomber);
                levelUpPanel.skill2NameText.text = "Bomber";
                levelUpPanel.skill2DescText.text = "Drops explosive bombs around.";
                break;
            case 2:
                levelUpPanel.skill2Button.onClick.AddListener(OnClickCrescent);
                levelUpPanel.skill2NameText.text = "Crescent";
                levelUpPanel.skill2DescText.text = "Unleashes a crescent-shaped slash.";
                break;
            case 3:
                levelUpPanel.skill2Button.onClick.AddListener(OnClickHoop);
                levelUpPanel.skill2NameText.text = "Hoop";
                levelUpPanel.skill2DescText.text = "Throws a hoop that returns.";
                break;
        }
    }

    private void GameQuit()
    {
        // Implement game quit functionality
    }

    private void OnClickBeam() {
        Player p = CharacterManager.Instance.player;
        BodyPart newBody = Instantiate(Resources.Load<BodyPart>("Body"), p.parts[p.parts.Count - 1].transform.position, Quaternion.identity);

        newBody.prevBodyPart = p.parts[p.parts.Count - 1].gameObject;

        p.parts.Insert(p.parts.Count, newBody);
        if (newBody.TryGetComponent<Body>(out Body body))
        {
            Beam skill = newBody.gameObject.AddComponent<Beam>();
            skill.Cannon = body.cannon;
        }
        p.BindBodyParts();
        p.tail.ChangeChaseBodyPart(newBody.gameObject);
    }
    private void OnClickBomber() {
        Player p = CharacterManager.Instance.player;
        BodyPart newBody = Instantiate(Resources.Load<BodyPart>("Body"), p.parts[p.parts.Count - 1].transform.position, Quaternion.identity);

        newBody.prevBodyPart = p.parts[p.parts.Count - 1].gameObject;

        p.parts.Insert(p.parts.Count, newBody);
        if (newBody.TryGetComponent<Body>(out Body body))
        {
            Bomber skill = newBody.gameObject.AddComponent<Bomber>();
            skill.Cannon = body.cannon;
        }
        p.BindBodyParts();
        p.tail.ChangeChaseBodyPart(newBody.gameObject);
    }
    private void OnClickCrescent() {
        Player p = CharacterManager.Instance.player;
        BodyPart newBody = Instantiate(Resources.Load<BodyPart>("Body"), p.parts[p.parts.Count - 1].transform.position, Quaternion.identity);

        newBody.prevBodyPart = p.parts[p.parts.Count - 1].gameObject;

        p.parts.Insert(p.parts.Count, newBody);
        if (newBody.TryGetComponent<Body>(out Body body))
        {
            Crescent skill = newBody.gameObject.AddComponent<Crescent>();
            skill.Cannon = body.cannon;
        }
        p.BindBodyParts();
        p.tail.ChangeChaseBodyPart(newBody.gameObject);
    }
    private void OnClickHoop() {
        Player p = CharacterManager.Instance.player;
        BodyPart newBody = Instantiate(Resources.Load<BodyPart>("Body"), p.parts[p.parts.Count - 1].transform.position, Quaternion.identity);

        newBody.prevBodyPart = p.parts[p.parts.Count - 1].gameObject;

        p.parts.Insert(p.parts.Count, newBody);
        if (newBody.TryGetComponent<Body>(out Body body))
        {
            Hoop skill = newBody.gameObject.AddComponent<Hoop>();
            skill.Cannon = body.cannon;
        }
        p.BindBodyParts();
        p.tail.ChangeChaseBodyPart(newBody.gameObject);
    }
}
