using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public static event Action<int> onScoreChanged;
    public static event Action<int, int> onPlayerHealthChanged;

    public static void ScoreChanged(int score) { onScoreChanged?.Invoke(score); }

    public static void PlayerHealthChanged(int currentHealth, int maxHealth) { onPlayerHealthChanged?.Invoke(currentHealth, maxHealth); }
}

