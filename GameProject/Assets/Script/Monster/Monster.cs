using UnityEngine;
using MonsterData;

public class Monster : MonoBehaviour
{
    public MonsterStats stats = new MonsterStats();
    public void Init(string[] csvData)
    {
        stats.LoadDataFromCSV(csvData);
    }
}
