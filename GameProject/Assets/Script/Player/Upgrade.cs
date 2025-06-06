using PlayerData;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    private Stats stats;

    void Start()
    {
        stats = Stats.LoadStats(GameManager.currentSlot);
    }
    public void UpgradeStat(string statName)
    {
        int cost = 100;  // �ӽ� ���
        if (stats.gold < cost) return;
        bool isLockedStat = statName == "CritChance" || statName == "CritDamage" ||
                            statName == "DefencePenetration";
        if (isLockedStat && stats.isGoldUpgradeUnlock)
        {
            Debug.Log($"{statName} is locked.");
            return;
        }
        stats.gold -= cost;
        switch (statName)
        {
            case "AttackPower":
                stats.attackPower += 2;
                stats.attackPowerUpgradeLevel++;
                if(stats.attackPowerUpgradeLevel >= 50)
                {
                    stats.isGoldUpgradeUnlock = true;
                }
                break;
            case "Defense":
                stats.defence += 2;
                break;
            case "MaxHealth":
                stats.maxHp += 20;
                stats.currentHp = stats.maxHp;
                break;
            case "AttackSpeed":
                stats.attackSpeed += 0.1f;
                break;
            case "MoveSpeed":
                stats.moveSpeed += 0.2f;
                break;
            case "CritChance":
                stats.critChance += 0.02f;
                break;
            case "CritDamage":
                stats.critDmg += 0.1f;
                break;
            case "DefencePenetration":
                stats.defencePenetration += 1f;
                break;
            default:
                break;
        }        
        stats.SaveStats(GameManager.currentSlot);
        Debug.Log($"{statName} upgraded. Remaining Gold: {stats.gold}");
    }
}