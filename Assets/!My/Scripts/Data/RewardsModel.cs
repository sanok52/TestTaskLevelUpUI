using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public static class RewardsModel
{
    // Словарь наград
    public static Dictionary<string, RewardModel> RewardModels { get; private set; } = new Dictionary<string, RewardModel>();

    // Установка пресетов наград
    public static void SetRewardPresets(params RewardModel[] rewardModels)
    {
        RewardModels.Clear();
        foreach (var preset in rewardModels)
        {
            RewardModels.Add(preset.Id, preset);
        }
    }

    // Получение случайного пресета награды
    public static RewardModel GetRandomRewardModel()
    {
        return RewardModels.ElementAt(UnityEngine.Random.Range(0, RewardModels.Count)).Value;
    }

    // Создание случайных данных награды (Только для теста!)
    public static RewardPreset CreateRandomRewardPreset()
    {
        RewardModel model = RewardModels.ElementAt(UnityEngine.Random.Range(0, RewardModels.Count)).Value;
        return new RewardPreset(model.Id,
            model.IsIncremental ? UnityEngine.Random.Range(2, 4) : UnityEngine.Random.Range(1000, 5000));
    }
}