using UnityEngine;
using System.IO;
using System.Collections.Generic;
using MonsterData;

public class MonsterSpawner : MonoBehaviour
{
    [System.Serializable]
    public class MonsterCSVData
    {
        public string prefabName;
        public string[] data;
    }
    private List<MonsterCSVData> monsterList = new List<MonsterCSVData>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadCSV();
        SpawnMonsters();
    }
    void LoadCSV()
    {
        string filePath = Path.Combine(Application.dataPath, "CSV/MonsterTable.csv");        
        string[] lines = File.ReadAllLines(filePath);             
        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
        {
            string[] lineData = lines[i].Split(',');            
            //if (lineData.Length < 11) continue;
            Debug.Log(lineData[1]);           
            string monsterName = lineData[1]; // 이름 = 프리팹 이름            
            monsterList.Add(new MonsterCSVData
            {
                prefabName = monsterName,
                data = lineData
            });
        }
    }
    void SpawnMonsters()
    {
        foreach (var monsterData in monsterList)
        {
            Debug.Log($"스폰시도: {monsterData.prefabName}");
            GameObject prefab = Resources.Load<GameObject>($"Monsters/{monsterData.prefabName}");
            if (prefab == null)
            {
                Debug.LogWarning($"프리팹 {monsterData.prefabName}을(를) 찾을 수 없음");
                continue;
            }
            GameObject monster = Instantiate(prefab, Random.insideUnitCircle * 5f, Quaternion.identity);
            if (!monster.TryGetComponent(out Monster monsterComp))
                monsterComp = monster.AddComponent<Monster>();
            if (!monster.TryGetComponent(out MonsterController controller))
                controller = monster.AddComponent<MonsterController>();
            monsterComp.Init(monsterData.data);
            Debug.Log("Spawn");
        }
    }
}
