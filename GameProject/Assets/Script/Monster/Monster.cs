using UnityEngine;
using MonsterData;

public class Monster : MonoBehaviour
{
    public MonsterStats stats = new MonsterStats();
    public void Init(string[] csvData)
    {
        stats.LoadDataFromCSV(csvData);
        stats.monsterCurrentHp = stats.monsterMaxHp;
    }
    public void TakeDamage(float damage)
    {
        stats.monsterCurrentHp -= damage;
        Debug.Log($"{stats.monsterName} took {damage} damage. HP left: {stats.monsterCurrentHp}");
        if(stats.monsterCurrentHp < 0 )
        {
            Debug.Log($"{stats.monsterName} has died");
            Destroy(gameObject);
        }
    }
}
