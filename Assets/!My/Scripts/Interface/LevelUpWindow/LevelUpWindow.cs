using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpWindow : MonoBehaviour
{
    [SerializeField] private LevelUpWindowAnimation _windowAnimation;

    [Space(10)]
    [Header("Rewards UI")]
    [SerializeField] private RewardUI[] _rewardsUISimple; // ������� (UI) ��� �������� ������
    [SerializeField] private RewardUI[] _rewardsUITwoX; // ������� (UI) ��� ������ � ���������

    [Space(10)]
    [Header("Buttons")]
    [SerializeField] private Button _buttonGetTwoX; // ������ "GET x2"
    [SerializeField] private Button _buttonClaim; // ������ "CLAIM"

    // ������� ��� ��������� ������ �� �������
    public event Action OnClickGetTwoX;
    public event Action OnClickClaim;

    // ����, �����������, ������� �� ����
    public bool IsOpen { get; private set; }

    private void Start()
    {
        // ������������� �� ����� �� �������
        _buttonGetTwoX.onClick.AddListener(ClickTwoX);
        _buttonClaim.onClick.AddListener(ClickClaim);

        // ������������� �� ������� ��������� ������
        PlayerInfo.OnLevelUp += Open;
    }

    /// <summary>
    /// ��������� ���� ��������� ������.
    /// </summary>
    /// <param name="data">������ � ��������� ������.</param>
    private void Open(LevelUpData data)
    {
        if (IsOpen)
            return;

        // ������������� ��������� ������� (������ ��� �����!)
        SetRewardsUIRandom();
        
        _windowAnimation.SetLevelText(data.Level);  // ������������� ����� ������ � �������� ����      
        _windowAnimation.Open(); // ��������� �������� �������� ����

        IsOpen = true; // ������������� ����, ��� ���� �������       
        Debug.Log($"OPEN: {data}");
    }

    public void Close()
    {
        if (IsOpen == false)
            return;

        _windowAnimation.Close(); // ��������� �������� �������� ����
        IsOpen = false;
    }

    // ������������ ���� �� ������ "GET x2"
    private void ClickTwoX()
    {
        OnClickGetTwoX?.Invoke();
    }

    // ������������ ���� �� ������ "CLAIM".
    private void ClickClaim()
    {
        OnClickClaim?.Invoke();
    }

    /// <summary>
    /// ������������� ������� ��� �������� ������.
    /// </summary>
    /// <param name="rewardPresets">������ ������.</param>
    private void SetRewardsUISimple(params RewardPreset[] rewardPresets)
    {
        SetRewardsUI(_rewardsUISimple, rewardPresets);
    }

    /// <summary>
    /// ������������� ������� ��� ������ � ���������.
    /// </summary>
    /// <param name="rewardPresets">������ ������.</param>
    private void SetRewardsUITwoX(params RewardPreset[] rewardPresets)
    {
        SetRewardsUI(_rewardsUITwoX, rewardPresets);
    }

    /// <summary>
    /// ������������� ������� ��� ���������� ������� RewardUI.
    /// </summary>
    /// <param name="rewardUIs">������ RewardUI.</param>
    /// <param name="rewardPresets">������ ������.</param>
    private void SetRewardsUI(RewardUI[] rewardUIs, params RewardPreset[] rewardPresets)
    {
        // �������� �� ���� ��������� RewardUI � RewardData
        for (int i = 0; i < rewardUIs.Length && i < rewardPresets.Length; i++)
        {
            // �������������� RewardUI ������� �������
            rewardUIs[i].Init(rewardPresets[i]);

            // ��������� RewardUI
            rewardUIs[i].Open();
        }
    }

    /// <summary>
    /// ������������� ��������� ������� (������ ��� �����!).
    /// </summary>
    private void SetRewardsUIRandom()
    {
        // ������������� ��������� ������� ��� �������� ������
        for (int i = 0; i < _rewardsUISimple.Length; i++)
        {
            _rewardsUISimple[i].Init();
            _rewardsUISimple[i].Open();
        }

        // ������������� ��������� ������� ��� ������ � ���������
        for (int i = 0; i < _rewardsUITwoX.Length; i++)
        {
            _rewardsUITwoX[i].Init();
            _rewardsUITwoX[i].Open();
        }
    }

    private void OnDestroy()
    {
        // ������������ �� ������� ��������� ������ ��� ����������� �������
        PlayerInfo.OnLevelUp -= Open;
    }
}
