using System.Collections;
using UnityEngine;

public class TestLevelUp : MonoBehaviour
{
    [SerializeField] private LevelUpWindow _levelUpWindow;
    [SerializeField] private RewardModel[] _rewardModels;

    private void Start()
    {
        GData.SetRewardPresets(_rewardModels);
        _levelUpWindow.OnClickGetTwoX += () => Debug.Log("GET 2X");
        _levelUpWindow.OnClickClaim += () => Debug.Log("CLAIM");
        _levelUpWindow.gameObject.SetActive(true);
    }

    public void LevelUp()
    {
        PlayerInfo.LevelUp();
    }
}