using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverDialog : Dialog
{
    public override void Show(bool isShow)
    {
        base.Show(isShow);
        AudioController.Ins.PlaySound(AudioController.Ins.onichan);
    }

    public void Home_Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
