using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Starting,
    Playing,
    Gameover,
}

public enum GameTag
{
    Bottle,
}

[System.Serializable]
public class Level
{
    public int scoreRequire;
    public float bottleSpeed;
    public float bottleSpawnTime;
    public Sprite unlockImg;
}
