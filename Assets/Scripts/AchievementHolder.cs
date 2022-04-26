using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementHolder : MonoBehaviour
{
    public AchievementItem[] achievementItem;

    public static AchievementHolder Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }
}