using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpWindow : MonoBehaviour
{
    [SerializeField] private LevelUpWindowAnimation _windowAnimation;

    [Space(10)]
    [Header("Rewards UI")]
    [SerializeField] private RewardUI[] _rewardsUISimple; // Награды (UI) для обычного выбора
    [SerializeField] private RewardUI[] _rewardsUITwoX; // Награды (UI) для выбора с удвоением

    [Space(10)]
    [Header("Buttons")]
    [SerializeField] private Button _buttonGetTwoX; // Кнопка "GET x2"
    [SerializeField] private Button _buttonClaim; // Кнопка "CLAIM"

    // События для обработки кликов по кнопкам
    public event Action OnClickGetTwoX;
    public event Action OnClickClaim;

    // Флаг, указывающий, открыто ли окно
    public bool IsOpen { get; private set; }

    private void Start()
    {
        // Подписываемся на клики по кнопкам
        _buttonGetTwoX.onClick.AddListener(ClickTwoX);
        _buttonClaim.onClick.AddListener(ClickClaim);

        // Подписываемся на событие повышения уровня
        PlayerInfo.OnLevelUp += Open;
    }

    /// <summary>
    /// Открывает окно повышения уровня.
    /// </summary>
    /// <param name="data">Данные о повышении уровня.</param>
    private void Open(LevelUpData data)
    {
        if (IsOpen)
            return;

        // Устанавливаем случайные награды (только для теста!)
        SetRewardsUIRandom();
        
        _windowAnimation.SetLevelText(data.Level);  // Устанавливаем текст уровня в анимации окна      
        _windowAnimation.Open(); // Запускаем анимацию открытия окна

        IsOpen = true; // Устанавливаем флаг, что окно открыто       
        Debug.Log($"OPEN: {data}");
    }

    public void Close()
    {
        if (IsOpen == false)
            return;

        _windowAnimation.Close(); // Запускаем анимацию закрытия окна
        IsOpen = false;
    }

    // Обрабатывает клик по кнопке "GET x2"
    private void ClickTwoX()
    {
        OnClickGetTwoX?.Invoke();
    }

    // Обрабатывает клик по кнопке "CLAIM".
    private void ClickClaim()
    {
        OnClickClaim?.Invoke();
    }

    /// <summary>
    /// Устанавливает награды для обычного выбора.
    /// </summary>
    /// <param name="rewardPresets">Данные наград.</param>
    private void SetRewardsUISimple(params RewardPreset[] rewardPresets)
    {
        SetRewardsUI(_rewardsUISimple, rewardPresets);
    }

    /// <summary>
    /// Устанавливает награды для выбора с удвоением.
    /// </summary>
    /// <param name="rewardPresets">Данные наград.</param>
    private void SetRewardsUITwoX(params RewardPreset[] rewardPresets)
    {
        SetRewardsUI(_rewardsUITwoX, rewardPresets);
    }

    /// <summary>
    /// Устанавливает награды для указанного массива RewardUI.
    /// </summary>
    /// <param name="rewardUIs">Массив RewardUI.</param>
    /// <param name="rewardPresets">Данные наград.</param>
    private void SetRewardsUI(RewardUI[] rewardUIs, params RewardPreset[] rewardPresets)
    {
        // Проходим по всем элементам RewardUI и RewardData
        for (int i = 0; i < rewardUIs.Length && i < rewardPresets.Length; i++)
        {
            // Инициализируем RewardUI данными награды
            rewardUIs[i].Init(rewardPresets[i]);

            // Открываем RewardUI
            rewardUIs[i].Open();
        }
    }

    /// <summary>
    /// Устанавливает случайные награды (только для теста!).
    /// </summary>
    private void SetRewardsUIRandom()
    {
        // Устанавливаем случайные награды для обычного выбора
        for (int i = 0; i < _rewardsUISimple.Length; i++)
        {
            _rewardsUISimple[i].Init();
            _rewardsUISimple[i].Open();
        }

        // Устанавливаем случайные награды для выбора с удвоением
        for (int i = 0; i < _rewardsUITwoX.Length; i++)
        {
            _rewardsUITwoX[i].Init();
            _rewardsUITwoX[i].Open();
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от события повышения уровня при уничтожении объекта
        PlayerInfo.OnLevelUp -= Open;
    }
}
