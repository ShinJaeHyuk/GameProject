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
        // ���� �� ��� ����
        public int attackPower = 50; // ���ݷ�
        public int defence = 5; // ����
        public int currentHp; // ���� ü��
        public int maxHp = 100; // ü��
        public float attackSpeed = 1.0f; // ���ݼӵ�
        public float moveSpeed = 1.0f;
        public float critChance = 0.0f; // ġ��Ÿ Ȯ��
        public float critDmg = 1.0f; // ġ��Ÿ ������
        public float defencePenetration = 0.0f; // ���� ���� ��ġ
        // ��ƿ ����
        public float goldGain = 1.0f; // ��� ȹ�淮 ����
        public float expGain = 1.0f; // ����ġ ȹ�淮 ����        
        // ���� �� ����ġ, ���
        public int level = 1; // �÷��̾� ����
        public int statPoints = 0; // ��������Ʈ
        public int exp = 0; // ����ġ
        public int expToNextLevel = 30; // �ӽð�(csv���� �Ľ� ����)
        public int gold = 0; // ���
        // ���׷��̵� �ر� ����
        public bool isStatUpgradeUnlock = false; // ���� ���׷��̵� �ر� ����
        public bool isGoldUpgradeUnlock = false; // ��� ���׷��̵� �ر� ����
        // ������ ���׷��̵� ��
        public int attackPowerStatPoint = 0; // ��������Ʈ�� ���ݷ¿� ������ ��
        public int attackPowerUpgradeLevel = 0; // ���� ���׷��̵��� ���ݷ� ����
        
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
                return new Stats(); // �⺻ ������ �ʱ�ȭ
            }
        }
        public void SaveStats(int slotNumber)
        {
            string path = Application.persistentDataPath + $"/SaveSlot{slotNumber}.json";
            string json = JsonUtility.ToJson(this, true);
            File.WriteAllText(path, json);
        }
    }
    // ��ų
    [Serializable]
    public class Skill
    {
        public int skillID;
        public string skillName;
        public float damage;
        public float cooldown;
        public float range;
        public bool unlocked;
        public float duration;  // ������ ��ų�� ���� ���� (��: SpinningBlade)
        public float effectRadius;  // ������ ��ų�� ���� ����
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

            // �ӽ� ������ (CSV ���Ϸ� ��ü ����)
            skills[1] = new Skill(1, "Dash Strike", 100f, 5f, 10f, true);
            skills[2] = new Skill(2, "Spinning Blades", 50f, 15f, 0f, true, 5f, 2f);

            /*
            string filePath = Application.dataPath + "/Data/Skills.csv";
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines.Skip(1))  // ��� ����
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
