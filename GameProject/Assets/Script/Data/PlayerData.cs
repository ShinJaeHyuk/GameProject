using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace PlayerData
{
    [Serializable]
    public class Stats
    {
        // 공격 및 방어 관련
        public float attackPower = 10.0f; // 공격력
        public float defence = 5.0f; // 방어력
        public float currentHp; // 현재 체력
        public float maxHp = 100.0f; // 체력
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
        public int expToNextLevel = 100; // 임시값(csv에서 파싱 예정)
        public int gold = 0;
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
}
