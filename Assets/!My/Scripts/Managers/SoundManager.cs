using System.Collections;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource _source; // Источник звука

    [Space(10)]
    [Header("Button Sounds")]
    [SerializeField] private AudioClip _overButtonClip; // Звук при наведении на кнопку
    [SerializeField] private AudioClip _clickButtonClip; // Звук при нажатии на кнопку

    [Space(10)]
    [Header("Window Sounds")]
    [SerializeField] private AudioClip _openWindowClip; // Звук при открытии окна
    [SerializeField] private AudioClip _closeWindowClip; // Звук при закрытии окна

    [Space(10)]
    [Header("Other Sounds")]
    [SerializeField] private AudioClip _levelUpJingle; // Звук при повышении уровня

    // Воспроизводит звук при наведении на кнопку.
    public void PlayOverButton() => PlaySound(_overButtonClip);

    // Воспроизводит звук при нажатии на кнопку.
    public void PlayClickButton() => PlaySound(_clickButtonClip);

    // Воспроизводит звук при открытии окна.
    public void PlayOpenWindow() => PlaySound(_openWindowClip);

    // Воспроизводит звук при закрытии окна.
    public void PlayCloseWindow() => PlaySound(_closeWindowClip);

    // Воспроизводит джингл открытия окна повышения уровня
    public void PlayLevelUpJingle() => PlaySound(_levelUpJingle);

    // Воспроизводит звук
    private void PlaySound(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }

    // Устанавливает громкость звука.
    public void SetVolume(float volume)
    {
        _source.volume = Mathf.Clamp01(volume);
    }

    // Включает или выключает звук.
    public void SetMuted(bool isMuted)
    {
        _source.mute = isMuted;
    }

    // Проверяет, выключен ли звук.
    public bool IsMuted() => _source.mute;

    // Возвращает текущую громкость.
    public float GetVolume() => _source.volume;
}