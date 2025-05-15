using UnityEngine;
using System.Collections.Generic;
using PlayerData;
public class Skill : MonoBehaviour 
{
    /*
    private Dictionary<int, Skill> skillDictionary;
    private Dictionary<int, float> skillCooldownTimers = new Dictionary<int, float>();
    private Stats playerStats;
    private Transform playerTransform;

    void Start()
    {
        skillDictionary = Stats.LoadSkills();  // 스킬 데이터 로드
        playerStats = Stats.LoadStats(GameManager.CurrentSlot);
        playerTransform = transform;

        // 모든 스킬의 쿨타임 초기화
        foreach (int skillID in skillDictionary.Keys)
        {
            skillCooldownTimers[skillID] = 0f;
        }
    }
    void Update()
    {
        // 쿨타임 감소 처리
        List<int> skillIDs = new List<int>(skillCooldownTimers.Keys);
        foreach (int skillID in skillIDs)
        {
            if (skillCooldownTimers[skillID] > 0f)
            {
                skillCooldownTimers[skillID] -= Time.deltaTime;
            }
        }
    }
    public void UseSkill(int skillID)
    {
        if (!skillDictionary.ContainsKey(skillID)) return;
        Skill skill = skillDictionary[skillID];

        // 스킬 사용 가능 여부 확인
        if (skillCooldownTimers[skillID] > 0f || !skill.unlocked) return;

        // 스킬 타입에 따라 처리 분기
        switch (skillID)
        {
            case 1:  // Dash Strike
                UseDashStrike(skill);
                break;
            case 2:  // Spinning Blades
                UseSpinningBlades(skill);
                break;
            default:
                Debug.LogWarning($"Skill ID {skillID} is not defined.");
                break;
        }
        // 스킬 쿨타임 초기화
        skillCooldownTimers[skillID] = skill.cooldown;
    }
    private void UseDashStrike(Skill skill)
    {
        // 가장 가까운 적 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(playerTransform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        // 적이 범위 내에 있을 때만 돌진
        if (closestEnemy != null && closestDistance <= skill.range)
        {
            Vector2 direction = (closestEnemy.transform.position - playerTransform.position).normalized;
            playerTransform.position = closestEnemy.transform.position;
            Debug.Log($"Dash Strike used on {closestEnemy.name}, dealt {skill.damage} damage.");
        }
    }
    private void UseSpinningBlades(Skill skill)
    {
        // 블레이드 프리팹 생성 및 회전 시작
        GameObject bladePrefab = Resources.Load<GameObject>("Prefabs/SpinningBlade");
        GameObject blades = Instantiate(bladePrefab, playerTransform.position, Quaternion.identity, playerTransform);
        blades.GetComponent<SpinningBlades>().Initialize(skill, playerStats);
        Debug.Log("Spinning Blades activated.");
    }*/
}