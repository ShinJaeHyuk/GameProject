using System;
using UnityEngine;

namespace MonsterData
{
    [Serializable]
    public class MonsterStats
    {
        public int monsterID;
        public string monsterName;
        public string monsterType;
        public float monsterCurrentHp;
        public float monsterMaxHp;
        public float monsterAtk;
        public float monsterDef;
        public float monsterCritChance;
        public float monsterCritDmg;
        public float monsterDefencePenetration;
        // 드랍 보상
        public int monsterDropGold;
        public int monsterDropExp;

        public void LoadDataFromCSV(string[] data)
        {
            monsterID = int.Parse(data[0]);
            monsterName = data[1];
            monsterType = data[2];
            monsterMaxHp = float.Parse(data[3]);
            monsterAtk = float.Parse(data[4]);
            monsterDef = float.Parse(data[5]);
            monsterCritChance = float.Parse(data[6]);
            monsterCritDmg = float.Parse(data[7]);
            monsterDefencePenetration = float.Parse(data[8]);
            monsterDropGold = int.Parse(data[9]);
            monsterDropExp = int.Parse(data[10]);
        }
    }
}
