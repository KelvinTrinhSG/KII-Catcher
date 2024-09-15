using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState gameState;
    public int maxLife;
    public float stickSpawnOffset;
    public StickController stickPrefab;
    public BottleController bottlePrefab;
    private StickController m_curStick;
    private Level m_curLevel;
    private int m_score;
    private int m_curLevelIdx;
    private int m_curLife;

    public Level CurLevel { get => m_curLevel; }
    public int CurLevelIdx { get => m_curLevelIdx; }

    //Blockchain
    public GameObject playBtn;

    protected override void Awake()
    {
        MakeSingleton(false);
        ViewPortUtil.GetWorldPos();
    }

    private void Start()
    {
        gameState = GameState.Starting;
        AudioController.Ins.PlayBackgroundMusic();
        playBtn.SetActive(false);
    }

    public void Prepare()
    {
            m_curLevel = LevelManager.Ins.GetLevel(m_curLevelIdx);
            m_curLife = maxLife;
            SpawnStick();
            GUIManager.Ins.UpdateScore(m_score);
            GUIManager.Ins.UpdateProgress((float)m_score / m_curLevel.scoreRequire);
            GUIManager.Ins.lifePanel.DrawLife(m_curLife);
            GUIManager.Ins.ShowGameplay(true);
            PlayGame();
    }

    public void PlayGame()
    {
        gameState = GameState.Playing;
        m_curLevel = LevelManager.Ins.GetLevel(m_curLevelIdx);
        m_score = 0;
        StartCoroutine(SpawnBottleCo());
        GUIManager.Ins.ShowGirlUnlockDialog(false);
        GUIManager.Ins.UpdateProgress((float)m_score / m_curLevel.scoreRequire);
    }

    private void SpawnStick()
    {
        if (!stickPrefab) return;
        float spawnPosY = ViewPortUtil.MinY + stickSpawnOffset;
        m_curStick = Instantiate(stickPrefab, new Vector3(0f, spawnPosY, 0f), Quaternion.identity);
        m_curStick.speed = 15;
    }

    private IEnumerator SpawnBottleCo()
    {
        if (m_curLevel != null && bottlePrefab && m_curLife > 0)
        {
            
            SpriteRenderer bottleRenderer = bottlePrefab.GetComponent<SpriteRenderer>();
            float bottleWidth = 0;
            float bottleHeight = 0;
            float margin = 0.5f;
            if (bottleRenderer != null)
            {
                bottleWidth = bottleRenderer.bounds.size.x;
                bottleHeight = bottleRenderer.bounds.size.y;
            }            
            while (gameState == GameState.Playing)
            {                
                float spawnPosX = Random.Range(
                    ViewPortUtil.MinX + (bottleWidth / 2) + margin,
                    ViewPortUtil.MaxX - (bottleWidth / 2) - margin
                );
                float spawnPosY = ViewPortUtil.MaxY + (bottleHeight / 2) + 5f;
                Vector2 spawnPos = new Vector2(spawnPosX, spawnPosY);
                var bottleClone = Instantiate(bottlePrefab, spawnPos, Quaternion.identity);
                if (bottleClone)
                {
                    bottleClone.speed = m_curLevel.bottleSpeed;
                }
                yield return new WaitForSeconds(m_curLevel.bottleSpawnTime);
            }
        }
        yield return null;
    }

    public void AddScore(int scoreToAdd)
    {
        if (m_curLevel == null || gameState == GameState.Gameover) return;
        int scoreBonus = scoreToAdd * (m_curLevelIdx + 1);
        m_score += scoreBonus;
        if (m_score >= m_curLevel.scoreRequire)
        {
            gameState = GameState.Gameover;

            StopCoroutine(SpawnBottleCo());

            m_curLevelIdx++;
            m_curStick.speed += 2;
            m_curLevelIdx = Mathf.Clamp(m_curLevelIdx, 0, LevelManager.Ins.levels.Length);
            if (m_curLevelIdx >= LevelManager.Ins.levels.Length)
            {
                m_curLevelIdx = 0;
            }

            DestroyAllBottles();

            GUIManager.Ins.ShowGirlUnlockDialogDelay(true, true);
            AudioController.Ins.PlaySound(AudioController.Ins.crowd);
        }
        else
        {
            AudioController.Ins.PlaySound(AudioController.Ins.getScore);
        }
        GUIManager.Ins.UpdateScore(m_score);
        GUIManager.Ins.UpdateProgress((float)m_score / m_curLevel.scoreRequire);
    }

    private void DestroyAllBottles()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Bottle");
        foreach (GameObject go in gos) { Destroy(go); }
    }

    public void Gameover()
    {
        if (gameState == GameState.Gameover) return;
        m_curLife--;
        m_curLife = Mathf.Clamp(m_curLife, 0, maxLife);
        GUIManager.Ins.lifePanel.DrawLife(m_curLife);
        if (m_curLife <= 0)
        {
            DestroyAllBottles();
            gameState = GameState.Gameover;
            GUIManager.Ins.ShowGameoverDialog();
            AudioController.Ins.PlaySound(AudioController.Ins.gameover);
        }
        else
        {
            AudioController.Ins.PlaySound(AudioController.Ins.lostLife);
        }
    }
}
