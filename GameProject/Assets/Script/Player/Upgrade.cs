
using PlayerData;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    private Stats stats;

    void Start()
    {
        stats = Stats.LoadStats(GameManager.CurrentSlot);
    }
    public void UpgradeStat(string statName)
    {
        int cost = 100;  // 임시 비용
        if (stats.gold < cost) return;
        stats.gold -= cost;
        switch (statName)
        {
            case "AttackPower":
                stats.attackPower += 2f;
                stats.attackPowerUpgradeLevel++;
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
            case "MoveSpeed":
                stats.moveSpeed += 0.2f;
                break;
            case "CritChance":
                if (stats.attackPowerUpgradeLevel >= 50) stats.critChance += 0.02f;
                break;
            case "CritDamage":
                if (stats.attackPowerUpgradeLevel >= 50) stats.critDmg += 0.1f;
                break;
            case "DefencePenetration":
                if (stats.attackPowerUpgradeLevel >= 50) stats.defencePenetration += 1f;
                break;
            default:
                break;
        }
        stats.SaveStats(GameManager.CurrentSlot);
        Debug.Log($"{statName} upgraded. Remaining Gold: {stats.gold}");
    }
}