using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public Level[] levels;
    public Level GetLevel(int idx)
    {
        if (levels == null || levels.Length <= 0) return null;
        Level level = levels[idx];
        return level;
    }
}
