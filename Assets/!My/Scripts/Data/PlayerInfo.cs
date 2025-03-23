using System.Collections;
using UnityEngine;
using System;

public static class PlayerInfo
{
    public static int Level;

    public static event Action<LevelUpData> OnLevelUp;

    public static void SetLevel (int level)
    {
        Level = level;
    }

    public static void LevelUp ()
    {
        SetLevel(Level + 1);
        OnLevelUp?.Invoke(new LevelUpData(Level));
    }
}