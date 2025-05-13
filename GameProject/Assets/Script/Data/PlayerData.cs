using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace PlayerData
{
    [Serializable]
    public class Stats
    {
        // ���� �� ��� ����
        public float attackPower = 10.0f; // ���ݷ�
        public float defence = 5.0f; // ����
        public float currentHp; // ���� ü��
        public float maxHp = 100.0f; // ü��
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
        public int expToNextLevel = 100; // �ӽð�(csv���� �Ľ� ����)
        public int gold = 0;
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
}
