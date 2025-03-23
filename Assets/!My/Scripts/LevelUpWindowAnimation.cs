using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using System.Collections.Generic;

public class LevelUpWindowAnimation : MonoBehaviour
{
    [Header("Main Animation Settings")]
    [SerializeField] private Transform _target; // ������� ������ ��� �������� (���� ����)
    [SerializeField] private float _durationOpen = 1f; // ������������ �������� ����
    [SerializeField] private float _durationOpenPunch = 1f; // ������������ ����-��������
    [SerializeField] private int _durationOpenVibrato = 5; // �������� ����-��������
    [SerializeField] private float _durationOpenPunchSize = 0.1f; // ������ ����-��������
    [SerializeField] private float _elasticySize = 1f; // ������������ ����-��������
    [SerializeField] private Ease _easeOpen = Ease.Linear; // ��� �������� ��� ��������

    [Space(10)]
    [Header("Alpha Change Settings")]
    [SerializeField] private Image[] _targetAlphaChange; // ����������� ��� ��������� �����-������ (Fog)
    [SerializeField] private float _durationAlphaChange = 1f; // ������������ ��������� �����-������

    [Space(10)]
    [Header("Stars Animation Settings")]
    [SerializeField] private bool _enableStarsAnimation = true; // ���������/���������� �������� �����
    [SerializeField] private Image _imageStars; // ����������� �����
    [SerializeField] private float _durationAlphaStars = 1f; // ������������ ��������� �����-������ �����
    [SerializeField] private float _endValueAlphaStars = 1f; // �������� �������� �����-������ �����
    [SerializeField] private float _durationStarsScale = 1f; // ������������ ��������� �������� ����� (��� ������ ��������)
    [SerializeField] private float _sizeStarsScale = 1.1f; // ������ ������� ����� (��� ������ ��������)

    [Space(10)]
    [Header("Rays Animation Settings")]
    [SerializeField] private bool _enableRaysAnimation = true; // ���������/���������� �������� �����
    [SerializeField] private Image _imageRays; // ����������� �����
    [SerializeField] private float _durationRaysScale = 1f; // ������������ ��������� ������� �����
    [SerializeField] private float _sizeRaysScale = 1.1f; // ������ ������� �����
    [SerializeField] private float _durationRaysColor = 3.5f; // ������������ ��������� ����� �����
    [SerializeField] private Color _colorRaysChange = Color.gray; // ������� ���� ��������� �����

    [Space(10)]
    [Header("Level Text Settings")]
    [SerializeField] private bool _enableTextAnimation = true; // ���������/���������� �������� ������
    [SerializeField] private TMP_Text _levelTmp; // ����� � ������� ������
    [SerializeField] private float _durationLevelText = 1f; // ������������ �������� ������ � ������� ������
    private string _targetLevelText; // ����� ������ ������, ������� ������ ���� �������

    private const string _prefLevelTxt = "level"; // ������� ������ ������

    private void Awake()
    {
        Close(false); // ��������� ���� ��� ������
    }

    public void Open()
    {
        // ���������� ��� ���������� ������� ����� ������� ��������
        BreakAllVisual();

        _target.DOKill(true);
        _target.gameObject.SetActive(true);

        // ��������� �������� �������� ����
        PlayWindowOpenAnimation(() =>
        {
            // ����� ���������� �������� �������� ���������� ���� � ������
            _imageRays.gameObject.SetActive(true);
            _imageStars.gameObject.SetActive(true);

            // ��������� �������� ������ ������
            if (_enableTextAnimation)
                DoLevelText();

            // ��������� ����-�������� ����
            PlayPunchAnimation(() =>
            {
                // ��������� �������� �����
                SetRaysWhiteColor(() => StartRaysAnimation());

                // ��������� �������� �����
                SetStarsWhiteColor(() => StartStarsAnimation());
            });

        });
        if (_enableTextAnimation == false)
            _levelTmp.text = $"- {_targetLevelText} -";

        // �������� �����-����� ��� ���� _targetAlphaChange
        ChangeAlpha(Color.white);

        // ������������� �������� �������
        PlaySoundEffects();
    }

    public void Close(bool soundAndAnimation = true)
    {
        // ������������ ���� � ������
        _imageRays.gameObject.SetActive(false);
        _imageStars.gameObject.SetActive(false);
        if (soundAndAnimation)
        {
            // �������� �����-����� ��� ���� ����������� �� ����������
            ChangeAlpha(Color.clear);

            // ��������� �������� �������� ����
            _target.DOKill(true);
            _target.transform.DOScale(Vector3.zero, _durationOpen)
                .SetAutoKill()
                .OnComplete(() =>
                {
                    _target.gameObject.SetActive(false);
                // ���������� �����-����� ��� ���� _targetAlphaChange
                BreakAlphaImage();
                });

            // ������������� ��� �������� �����
            KillTweens();

            // ������������� ���� �������� ����
            SoundManager.Instance.PlayCloseWindow();
        }
        else
        {
            _target.gameObject.SetActive(false);
            BreakAlphaImage();
        }
    }

    // ������������� �������� ������� ��� �������� ����.
    private void PlaySoundEffects()
    {
        // ������������� ���� �������� ����
        SoundManager.Instance.PlayOpenWindow();

        // ������������� ���� ��������� ������
        SoundManager.Instance.PlayLevelUpJingle();
    }

    // ������������� �������� ��������������� ����
    private void PlayWindowOpenAnimation(Action onComplete)
    {
        _target.transform.DOScale(Vector3.one, _durationOpen)
            .SetAutoKill()
            .SetEase(_easeOpen)            
            .OnComplete(() => onComplete?.Invoke()); // �������� ������ ����� ����������
    }

    // ������������� ����-�������� ����.
    private void PlayPunchAnimation(Action onComplete)
    {
        _target.transform.DOPunchScale(Vector3.one * _durationOpenPunchSize, _durationOpenPunch, _durationOpenVibrato, _elasticySize)
            .SetEase(_easeOpen)
            .SetAutoKill()
            .OnComplete(() => onComplete?.Invoke()); // �������� ������ ����� ����������
    }

    // ������������� �������� ��������� ����� ����� �� ������
    private void SetRaysWhiteColor(Action onComplete)
    {
        _imageRays.DOColor(Color.white, 2.5f)
            .SetAutoKill()
            .OnComplete(() => onComplete?.Invoke()); // �������� ������ ����� ����������
    }

    // ������������� �������� ��������� ����� ����� �� ������
    private void SetStarsWhiteColor(Action onComplete)
    {
        // �������� ��������� ����� ����� 
        _imageStars.DOColor(Color.white, 1.5f)
            .SetAutoKill()
            .OnComplete(() => onComplete?.Invoke()); // �������� ������ ����� ����������
    }

    // ��������� �������� �����.
    private void StartStarsAnimation()
    {
        if (_enableStarsAnimation == false) 
            return;

        // �������� ��������� ����� ����� (���������)
       _imageStars.DOColor(_imageStars.color - (Color.black * (1f - _endValueAlphaStars)), _durationAlphaChange)
            .SetLoops(-1, LoopType.Yoyo);

        // �������� ��������� ������� ����� (������� ��������)
        _imageStars.transform.DOScale(new Vector3(1f, _sizeStarsScale, 1f), _durationStarsScale)
            .SetLoops(-1, LoopType.Yoyo);
    }

    // ��������� �������� �����.
    private void StartRaysAnimation()
    {
        if (_enableRaysAnimation == false) 
            return;

        // �������� ��������� �������� ����� (������� ��������)
       _imageRays.transform.DOScale(new Vector3(_sizeRaysScale, 1f, 1f), _durationRaysScale)
            .SetLoops(-1, LoopType.Yoyo);

        // �������� ��������� ����� ����� (���������)
       _imageRays.DOColor(_colorRaysChange, _durationRaysColor)
            .SetLoops(-1, LoopType.Yoyo);
    }

    // �������� �����-����� ��� ���� _targetAlphaChange
    private void ChangeAlpha(Color target)
    {
        foreach (var image in _targetAlphaChange)
        {
            image.gameObject.SetActive(true);
            image.DOColor(target, _durationAlphaChange);
        }
    }

    // ���������� ��� ���������� �������.
    private void BreakAllVisual()
    {
        // ���������� �������
        _target.transform.localScale = Vector3.zero;

        // ���������� �����-����� ��� ���� _targetAlphaChange
        BreakAlphaImage();

        // ���������� ���� � ������� �����
        _imageRays.color = Color.clear;
        _imageRays.transform.localScale = Vector3.one;

        // ���������� ���� � ������� �����
        _imageStars.color = Color.clear;
        _imageStars.transform.localScale = Vector3.one;

        // ������� ����� ������
        _levelTmp.text = "";
    }

    // ���������� �����-����� ��� ���� _targetAlphaChange
    private void BreakAlphaImage()
    {
        foreach (var image in _targetAlphaChange)
        {
            image.color = Color.clear;
            image.gameObject.SetActive(false);
        }
    }

    // ������������� ��� �������� �����, ������� �� ����� ������������ ���� (����. �����������)
    private void KillTweens()
    {
        _imageRays.DOKill(false);
        _imageStars.DOKill(false);
        _levelTmp.DOKill(true);
    }

    // ������������� ����� ������ ������, �������� �������
    public void SetLevelText(int level)
    {
        SetLevelText($"{_prefLevelTxt} {level}");
    }

    // ������������� ����� ������ ������ ���������
    public void SetLevelText(string txt)
    {
        // ��������� ������� ����� ������
        _targetLevelText = txt;
    }

    // ��������� ����� ������ ������
    private void DoLevelText()
    {
        DOTween.To(() => "",
            (s) => { _levelTmp.text = $"- {s} -"; },
            _targetLevelText,
            _durationLevelText);
    }
}