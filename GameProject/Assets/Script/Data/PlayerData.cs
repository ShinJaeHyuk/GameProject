using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Linq;

namespace PlayerData
{
    [Serializable]
    public class Stats
    {
        // 공격 및 방어 관련
        public int attackPower = 50; // 공격력
        public int defence = 5; // 방어력
        public int currentHp; // 현재 체력
        public int maxHp = 100; // 체력
        public float attackSpeed = 1.0f; // 공격속도
        public float moveSpeed = 1.0f;
        public float critChance = 0.0f; // 치명타 확률
        public float critDmg = 1.0f; // 치명타 데미지
        public float defencePenetration = 0.0f; // 방어력 관통 수치
        // 유틸 관련
        public float goldGain = 1.0f; // 골드 획득량 배율
        public float expGain = 1.0f; // 경험치 획득량 배율        
        // 레벨 및 경험치, 골드
        public int level = 1; // 플레이어 레벨
        public int statPoints = 0; // 스탯포인트
        public int exp = 0; // 경험치
        public int expToNextLevel = 30; // 임시값(csv에서 파싱 예정)
        public int gold = 0; // 골드
        // 업그레이드 해금 여부
        public bool isStatUpgradeUnlock = false; // 스탯 업그레이드 해금 여부
        public bool isGoldUpgradeUnlock = false; // 골드 업그레이드 해금 여부
        // 투자한 업그레이드 양
        public int attackPowerStatPoint = 0; // 스탯포인트로 공격력에 투자한 양
        public int attackPowerUpgradeLevel = 0; // 골드로 업그레이드한 공격력 레벨
        
        public static Stats LoadStats(int slotNumber)
        {
            string path = Application.persistentDataPath + $"/SaveSlot{slotNumber}.json";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonUtility.FromJson<Stats>(json);
            }
            else
            {
                return new Stats(); // 기본 값으로 초기화
            }
        }
        public void SaveStats(int slotNumber)
        {
            string path = Application.persistentDataPath + $"/SaveSlot{slotNumber}.json";
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(path, json);
        }
    }
    // 스킬
    [Serializable]
    public class Skill
    {
        public int skillID;
        public string skillName;
        public float damage;
        public float cooldown;
        public float range;
        public bool unlocked;
        public float duration;  // 지속형 스킬을 위한 변수 (예: SpinningBlade)
        public float effectRadius;  // 범위형 스킬을 위한 변수
        public Skill(int skillID, string skillName, float damage, float cooldown, float range, bool unlocked, float duration = 0f, float effectRadius = 0f)
        {
            this.skillID = skillID;
            this.skillName = skillName;
            this.damage = damage;
            this.cooldown = cooldown;
            this.range = range;
            this.unlocked = unlocked;
            this.duration = duration;
            this.effectRadius = effectRadius;
        }
        public static Dictionary<int, Skill> LoadSkills()
        {
            Dictionary<int, Skill> skills = new Dictionary<int, Skill>();

            // 임시 데이터 (CSV 파일로 대체 예정)
            skills[1] = new Skill(1, "Dash Strike", 100f, 5f, 10f, true);
            skills[2] = new Skill(2, "Spinning Blades", 50f, 15f, 0f, true, 5f, 2f);

            /*
            string filePath = Application.dataPath + "/Data/Skills.csv";
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines.Skip(1))  // 헤더 제외
                {
                    string[] data = line.Split(',');
                    int id = int.Parse(data[0]);
                    string name = data[1];
                    float dmg = float.Parse(data[2]);
                    float cd = float.Parse(data[3]);
                    float rng = float.Parse(data[4]);
                    bool unlock = bool.Parse(data[5]);
                    float dur = float.Parse(data[6]);
                    float radius = float.Parse(data[7]);
                    skills[id] = new Skill(id, name, dmg, cd, rng, unlock, dur, radius);
                }
            }
            */
            return skills;
        }
    }
}
