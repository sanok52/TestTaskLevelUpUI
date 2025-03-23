using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _icon; // Иконка награды
    [SerializeField] private TextMeshProUGUI _tmpTitle; // Текст с названием и количеством награды
    [SerializeField] private Color _countColor = new Color(0.996f, 0.780f, 0.322f); // Цвет для отображения Count (#FEC752)
    [SerializeField] private bool _isNativeSize = true; // Флаг для автоматического изменения размера иконки (применение SetNativeSize)

    // Инициализация UI с случайными данными награды.
    public void Init()
    {
        Init(RewardsModel.CreateRandomRewardPreset());
    }

    // Инициализация UI с указанными данными награды.
    public void Init(RewardPreset rewardPresets)
    {
        RewardModel model = RewardsModel.RewardModels[rewardPresets.Id];

        // Установка иконки и её размера
        _icon.sprite = model.sprite;
        if (_isNativeSize)
            _icon.SetNativeSize();

        // Форматирование текста с названием и количеством награды
        _tmpTitle.text = $"{model.Title} <color=#{ColorUtility.ToHtmlStringRGBA(_countColor)}>{(model.IsIncremental ? "x" : "+")}{rewardPresets.Count}</color>";
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
