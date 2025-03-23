using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class LevelUpWindowAnimation : MonoBehaviour
{
    [Header("Main Animation Settings")]
    [SerializeField] private Transform _target; // Целевой объект для анимации (Само окно)
    [SerializeField] private float _durationOpen = 1f; // Длительность открытия окна
    [SerializeField] private float _durationOpenPunch = 1f; // Длительность панч-анимации
    [SerializeField] private int _durationOpenVibrato = 5; // Вибрация панч-анимации
    [SerializeField] private float _durationOpenPunchSize = 0.1f; // Размер панч-анимации
    [SerializeField] private float _elasticySize = 1f; // Эластичность панч-анимации
    [SerializeField] private Ease _easeOpen = Ease.Linear; // Тип анимации для открытия

    [Space(10)]
    [Header("Alpha Change Settings")]
    [SerializeField] private Image[] _targetAlphaChange; // Изображения для изменения альфа-канала (Fog)
    [SerializeField] private float _durationAlphaChange = 1f; // Длительность изменения альфа-канала

    [Space(10)]
    [Header("Stars Animation Settings")]
    [SerializeField] private bool _enableStarsAnimation = true; // Включение/выключение анимации звезд
    [SerializeField] private Image _imageStars; // Изображение звезд
    [SerializeField] private float _durationAlphaStars = 1f; // Длительность изменения альфа-канала звезд
    [SerializeField] private float _endValueAlphaStars = 1f; // Конечное значение альфа-канала звезд
    [SerializeField] private float _durationStarsScale = 1f; // Длительность изменения масштаба звезд (Для илюзии движения)
    [SerializeField] private float _sizeStarsScale = 1.1f; // Размер размера звезд (Для илюзии движения)

    [Space(10)]
    [Header("Rays Animation Settings")]
    [SerializeField] private bool _enableRaysAnimation = true; // Включение/выключение анимации лучей
    [SerializeField] private Image _imageRays; // Изображение лучей
    [SerializeField] private float _durationRaysScale = 1f; // Длительность изменения размера лучей
    [SerializeField] private float _sizeRaysScale = 1.1f; // Размер размера лучей
    [SerializeField] private float _durationRaysColor = 3.5f; // Длительность изменения цвета лучей
    [SerializeField] private Color _colorRaysChange = Color.gray; // Целевой цвет изменения лучей

    [Space(10)]
    [Header("Level Text Settings")]
    [SerializeField] private bool _enableTextAnimation = true; // Включение/выключение анимации текста
    [SerializeField] private TMP_Text _levelTmp; // Текст с уровнем игрока
    [SerializeField] private float _durationLevelText = 1f; // Длительность анимации текста с уровнем игрока
    private string _targetLevelText; // Текст уровня игрока, который должен быть написан

    private const string _prefLevelTxt = "level"; // Префикс текста уровня

    private void Awake()
    {
        Close(false); // Закрываем окно при старте
    }

    public void Open()
    {
        // Сбрасываем все визуальные эффекты перед началом анимации
        BreakAllVisual();

        _target.DOKill(true);
        _target.gameObject.SetActive(true);

        // Запускаем анимацию открытия окна
        PlayWindowOpenAnimation(() =>
        {
            // После завершения анимации открытия активируем лучи и звезды
            _imageRays.gameObject.SetActive(true);
            _imageStars.gameObject.SetActive(true);

            // Запускаем анимацию текста уровня
            if (_enableTextAnimation)
                DoLevelText();

            // Запускаем панч-анимацию окна
            PlayPunchAnimation(() =>
            {
                // Запускаем анимацию лучей
                SetRaysWhiteColor(() => StartRaysAnimation());

                // Запускаем анимацию звезд
                SetStarsWhiteColor(() => StartStarsAnimation());
            });

        });
        if (_enableTextAnimation == false)
            _levelTmp.text = $"- {_targetLevelText} -";

        // Изменяем альфа-канал для всех _targetAlphaChange
        ChangeAlpha(Color.white);

        // Воспроизводим звуковые эффекты
        PlaySoundEffects();
    }

    public void Close(bool soundAndAnimation = true)
    {
        // Деактивируем лучи и звезды
        _imageRays.gameObject.SetActive(false);
        _imageStars.gameObject.SetActive(false);
        if (soundAndAnimation)
        {
            // Изменяем альфа-канал для всех изображений на прозрачный
            ChangeAlpha(Color.clear);

            // Запускаем анимацию закрытия окна
            _target.DOKill(true);
            _target.transform.DOScale(Vector3.zero, _durationOpen)
                .SetAutoKill()
                .OnComplete(() =>
                {
                    _target.gameObject.SetActive(false);
                // Сбрасываем альфа-канал для всех _targetAlphaChange
                BreakAlphaImage();
                });

            // Останавливаем все активные твины
            KillTweens();

            // Воспроизводим звук закрытия окна
            SoundManager.Instance.PlayCloseWindow();
        }
        else
        {
            _target.gameObject.SetActive(false);
            BreakAlphaImage();
        }
    }

    // Воспроизводит звуковые эффекты при открытии окна.
    private void PlaySoundEffects()
    {
        // Воспроизводим звук открытия окна
        SoundManager.Instance.PlayOpenWindow();

        // Воспроизводим звук повышения уровня
        SoundManager.Instance.PlayLevelUpJingle();
    }

    // Воспроизводит анимацию масштабирования окна
    private void PlayWindowOpenAnimation(Action onComplete)
    {
        _target.transform.DOScale(Vector3.one, _durationOpen)
            .SetAutoKill()
            .SetEase(_easeOpen)            
            .OnComplete(() => onComplete?.Invoke()); // Вызываем колбэк после завершения
    }

    // Воспроизводит панч-анимацию окна.
    private void PlayPunchAnimation(Action onComplete)
    {
        _target.transform.DOPunchScale(Vector3.one * _durationOpenPunchSize, _durationOpenPunch, _durationOpenVibrato, _elasticySize)
            .SetEase(_easeOpen)
            .SetAutoKill()
            .OnComplete(() => onComplete?.Invoke()); // Вызываем колбэк после завершения
    }

    // Воспроизводит анимацию изменения цвета лучей до белого
    private void SetRaysWhiteColor(Action onComplete)
    {
        _imageRays.DOColor(Color.white, 2.5f)
            .SetAutoKill()
            .OnComplete(() => onComplete?.Invoke()); // Вызываем колбэк после завершения
    }

    // Воспроизводит анимацию изменения цвета звезд до белого
    private void SetStarsWhiteColor(Action onComplete)
    {
        // Анимация изменения цвета звезд 
        _imageStars.DOColor(Color.white, 1.5f)
            .SetAutoKill()
            .OnComplete(() => onComplete?.Invoke()); // Вызываем колбэк после завершения
    }

    // Запускает анимацию звезд.
    private void StartStarsAnimation()
    {
        if (_enableStarsAnimation == false) 
            return;

        // Анимация изменения цвета звезд (пульсация)
       _imageStars.DOColor(_imageStars.color - (Color.black * (1f - _endValueAlphaStars)), _durationAlphaChange)
            .SetLoops(-1, LoopType.Yoyo);

        // Анимация изменения размера звезд (иллюзия движения)
        _imageStars.transform.DOScale(new Vector3(1f, _sizeStarsScale, 1f), _durationStarsScale)
            .SetLoops(-1, LoopType.Yoyo);
    }

    // Запускает анимацию лучей.
    private void StartRaysAnimation()
    {
        if (_enableRaysAnimation == false) 
            return;

        // Анимация изменения масштаба лучей (иллюзия движения)
       _imageRays.transform.DOScale(new Vector3(_sizeRaysScale, 1f, 1f), _durationRaysScale)
            .SetLoops(-1, LoopType.Yoyo);

        // Анимация изменения цвета лучей (пульсация)
       _imageRays.DOColor(_colorRaysChange, _durationRaysColor)
            .SetLoops(-1, LoopType.Yoyo);
    }

    // Изменяет альфа-канал для всех _targetAlphaChange
    private void ChangeAlpha(Color target)
    {
        foreach (var image in _targetAlphaChange)
        {
            image.gameObject.SetActive(true);
            image.DOColor(target, _durationAlphaChange);
        }
    }

    // Сбрасывает все визуальные эффекты.
    private void BreakAllVisual()
    {
        // Сбрасываем масштаб
        _target.transform.localScale = Vector3.zero;

        // Сбрасываем альфа-канал для всех _targetAlphaChange
        BreakAlphaImage();

        // Сбрасываем цвет и масштаб лучей
        _imageRays.color = Color.clear;
        _imageRays.transform.localScale = Vector3.one;

        // Сбрасываем цвет и масштаб звезд
        _imageStars.color = Color.clear;
        _imageStars.transform.localScale = Vector3.one;

        // Очищаем текст уровня
        _levelTmp.text = "";
    }

    // Сбрасывает альфа-канал для всех _targetAlphaChange
    private void BreakAlphaImage()
    {
        foreach (var image in _targetAlphaChange)
        {
            image.color = Color.clear;
            image.gameObject.SetActive(false);
        }
    }

    // Останавливает все активные твины, которые не могут остановиться сами (напр. зацикленные)
    private void KillTweens()
    {
        _imageRays.DOKill(false);
        _imageStars.DOKill(false);
        _levelTmp.DOKill(true);
    }

    // Устанавливает текст уровня игрока, добавляя префикс
    public void SetLevelText(int level)
    {
        SetLevelText($"{_prefLevelTxt} {level}");
    }

    // Устанавливает текст уровня игрока конкретно
    public void SetLevelText(string txt)
    {
        // Сохраняем целевой текст уровня
        _targetLevelText = txt;
    }

    // Анимирует текст уровня игрока
    private void DoLevelText()
    {
        DOTween.To(() => "",
            (s) => { _levelTmp.text = $"- {s} -"; },
            _targetLevelText,
            _durationLevelText);
    }
}