using PlayerData;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private Stats stats;

    void Start()
    {
        stats = Stats.LoadStats(GameManager.CurrentSlot);
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
            stats.SaveStats(GameManager.CurrentSlot);
            Debug.Log("Level Up! New Level: " + stats.level);
        }
    }
}