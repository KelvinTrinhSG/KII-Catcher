using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : Singleton<GUIManager>
{
    public GameObject home;
    public GameObject gameplay;
    public Text scoreTxt;
    public Image progressFilled;
    public GameObject heartVfxPrefab;
    public LifePanel lifePanel;
    public Dialog girlUnlockDialog;
    public Dialog gameoverDialog;
    private GameObject m_heartVfx;

    protected override void Awake()
    {
        MakeSingleton(false);
    }

    public void ShowGameplay(bool isShow)
    {
        if (gameplay)
        {
            gameplay.SetActive(isShow);
        }
        if (home)
        {
            home.SetActive(!isShow);
        }
    }

    public void UpdateScore(int score)
    {
        if (scoreTxt)
        {
            scoreTxt.text = "Score: " + score.ToString("0000");
        }
    }

    public void UpdateProgress(float rate)
    {
        if (progressFilled)
        {
            progressFilled.fillAmount = rate;
            progressFilled.transform.parent.gameObject.SetActive(true);
        }
    }
    private void ShowHeartVfx()
    {
        if (!heartVfxPrefab) return;
        Vector2 spawnPos = new Vector2(0f, ViewPortUtil.MinY);
        m_heartVfx = Instantiate(heartVfxPrefab, spawnPos, Quaternion.identity);
    }
    public void ShowGirlUnlockDialog(bool isShow, bool showEffect = false)
    {
        if (!girlUnlockDialog) return;
        girlUnlockDialog.Show(isShow);
        if (isShow && showEffect)
        {
            ShowHeartVfx();
        }
        else
        {
            Destroy(m_heartVfx);
        }
    }
    public void ShowGirlUnlockDialogDelay(bool isShow, bool showEffect = false)
    {
        StartCoroutine(ShowGirlUnlockDialogCo(isShow, showEffect));
    }
    private IEnumerator ShowGirlUnlockDialogCo(bool isShow, bool showEffect = false)
    {
        yield return new WaitForSeconds(1.5f);
        ShowGirlUnlockDialog(isShow, showEffect);
    }
    private IEnumerator ShowGameoverDialogCo()
    {
        yield return new WaitForSeconds(1.5f);
        if (gameoverDialog)
        {
            gameoverDialog.Show(true);
        }
    }

    public void ShowGameoverDialog()
    {
        StartCoroutine(ShowGameoverDialogCo());
    }

}
