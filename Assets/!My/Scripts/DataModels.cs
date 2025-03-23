using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// Модель награды, которую можно получать
/// </summary>
[Serializable]
public struct RewardModel
{
    public string Id; // Уникальный идентификатор пресета
    [SerializeField] private string _title; // Имя награды
    public bool IsIncremental; // Флаг, указывающий, является ли награда инкрементальной (x2, вместо +x)
    public Sprite sprite; // Спрайт для иконки

    // Название награды
    public string Title => _title;
}

/// <summary>
/// Пресет награды с ID и количеством получаемого
/// </summary>
[Serializable]
public struct RewardPreset
{
    public string Id; // Уникальный идентификатор награды
    public int Count; // Количество награды (+Count или хCount)

    public RewardPreset(string id, int count)
    {
        Id = id;
        Count = count;
    }
}

/// <summary>
/// Данные о повышении уровня игрока
/// </summary>
[Serializable]
public struct LevelUpData
{
    public int Level; //Текущий уровень игрока

    public LevelUpData(int level)
    {
        Level = level;
    }

    public override string ToString()
    {
        return $"\nLvl: {Level}";
    }
}