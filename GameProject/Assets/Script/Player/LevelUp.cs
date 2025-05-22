using PlayerData;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private Stats stats;

    void Start()
    {
        stats = Stats.LoadStats(GameManager.currentSlot);
    }
    public void GainExp(int amount)
    {
        stats.exp += amount;
        if (stats.exp >= stats.expToNextLevel)
        {
            stats.exp -= stats.expToNextLevel;
            stats.level++;
            stats.statPoints++;
            stats.expToNextLevel = Mathf.RoundToInt(stats.expToNextLevel * 1.2f);
            stats.SaveStats(GameManager.currentSlot);
            Debug.Log("Level Up! New Level: " + stats.level);
        }
    }
    // 스탯 업그레이드
    public void UpgradeStatWithPoint(string statName)
    {
        if(stats.statPoints <= 0)
        {
            Debug.Log("Not enough stat point");
            return;
        }
        bool isLockedStat = statName == "CritChance" || statName == "CritDamage" ||
                            statName == "DefencePenetration" || statName == "GoldGain" || statName == "ExpGain";
        if(isLockedStat && !stats.isStatUpgradeUnlock)
        {
            Debug.Log($"{statName} is locked.");
            return;
        }
        stats.statPoints--;
        switch (statName)
        {
            case "AttackPower":
                stats.attackPower += 2f;
                stats.attackPowerStatPoint++;
                // 공격력 스탯 포인트가 50 이상이면 잠금 해제
                if (stats.attackPowerStatPoint >= 50)
                {
                    stats.isStatUpgradeUnlock = true;
                }
                break;
            case "Defense":
                stats.defence += 2f;
                break;
            case "MaxHealth":
                stats.maxHp += 20f;
                stats.currentHp = stats.maxHp;
                break;
            case "AttackSpeed":
                stats.attackSpeed += 0.1f;
                break;
            case "CritChance":
                stats.critChance += 1f; // 1% 증가
                break;
            case "CritDamage":
                stats.critDmg += 5f; // 5% 증가
                break;
            case "DefencePenetration":
                stats.defencePenetration += 2f;
                break;
            case "GoldGain":
                stats.goldGain += 5f; // 5% 증가
                break;
            case "ExpGain":
                stats.expGain += 5f; // 5% 증가
                break;
            default:
                Debug.Log("Invalid stat name.");
                return;
        }
        stats.SaveStats(GameManager.currentSlot);
        Debug.Log($"{statName} upgraded using stat point. Remaining Points: {stats.statPoints}");
    }
}