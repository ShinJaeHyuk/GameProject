using UnityEngine;
using MonsterData;
using System.Linq.Expressions;

public class Monster : MonoBehaviour
{
    public MonsterStats stats = new MonsterStats();
    // 나중에 스테이지테이블에서 가져올거
    public static float rewardMultiplier = 1.0f;
    public void Init(string[] csvData)
    {
        stats.LoadDataFromCSV(csvData);
        stats.monsterCurrentHp = stats.monsterMaxHp;
    }
    public void TakeDamage(float damage)
    {
        stats.monsterCurrentHp -= damage;
        Debug.Log($"{stats.monsterName} took {damage} damage. HP left: {stats.monsterCurrentHp}");
        if(stats.monsterCurrentHp <= 0 )
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log($"{stats.monsterName} die");
        // 플레이어 보상 지급
        var playerStats = PlayerData.Stats.LoadStats(GameManager.currentSlot);
        int finalGold = (int)(stats.monsterDropGold * rewardMultiplier);
        int finalExp = (int)(stats.monsterDropExp * rewardMultiplier);
        playerStats.gold += finalGold;
        playerStats.exp += finalExp;
        Debug.Log($"Player gain {finalGold} gold, {finalExp} exp. {playerStats.gold} gold, {playerStats.exp} exp");
        // 레벨업 처리
        while(playerStats.exp >= playerStats.expToNextLevel)
        {
            Debug.Log($"{playerStats.expToNextLevel}");
            playerStats.exp -= playerStats.expToNextLevel;
            playerStats.level++;
            playerStats.statPoints += 1;
            Debug.Log($"player levelup. {playerStats.level} level, {playerStats.statPoints} statpoint left");
            // 체력 회복
            playerStats.currentHp = playerStats.maxHp;
        }        
        playerStats.SaveStats(GameManager.currentSlot);
        Destroy(gameObject);
    }
}
