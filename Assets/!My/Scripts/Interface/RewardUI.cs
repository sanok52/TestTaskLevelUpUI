using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image _icon; // ������ �������
    [SerializeField] private TextMeshProUGUI _tmpTitle; // ����� � ��������� � ����������� �������
    [SerializeField] private Color _countColor = new Color(0.996f, 0.780f, 0.322f); // ���� ��� ����������� Count (#FEC752)
    [SerializeField] private bool _isNativeSize = true; // ���� ��� ��������������� ��������� ������� ������ (���������� SetNativeSize)

    // ������������� UI � ���������� ������� �������.
    public void Init()
    {
        Init(RewardsModel.CreateRandomRewardPreset());
    }

    // ������������� UI � ���������� ������� �������.
    public void Init(RewardPreset rewardPresets)
    {
        RewardModel model = RewardsModel.RewardModels[rewardPresets.Id];

        // ��������� ������ � � �������
        _icon.sprite = model.sprite;
        if (_isNativeSize)
            _icon.SetNativeSize();

        // �������������� ������ � ��������� � ����������� �������
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
