using UnityEngine;
using PlayerData;
using System.Collections.Generic;
using MonsterData;

public class PlayerController : MonoBehaviour
{
    private Monster monster;
    private Stats stats;
    private Transform target;
    private float attackTimer = 0f;
    private bool autoSkillEnabled = true;
    private Skill[] skills = new Skill[2];
    private const float defenceConstant = 1000f; // 방어 상수
    private float attackRange = 1.0f; // 공격 범위
    public float distanceToTarget;
    void Start()
    {
        stats = Stats.LoadStats(GameManager.currentSlot);
        // 스킬 초기화
        
    }
    void Update()
    {
        FindClosestTarget();
        HandleMovement();
        HandleAttack();
    }    
    void FindClosestTarget()
    {
        // 가까운 적 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");
        float closestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                target = enemy.transform;
            }
        }
    }
    void HandleMovement()
    {
        // 적에게 이동
        if (target == null) return;
        Vector2 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * stats.moveSpeed * Time.deltaTime);
    }
    void HandleAttack()
    {
        // 스킬 또는 일반공격
        attackTimer -= Time.deltaTime;        
        // 공격 범위 확인
        if(target == null) return;
        distanceToTarget = Vector2.Distance(transform.position, target.position);        
        if (distanceToTarget > attackRange) return;
        if (attackTimer <= 0f)
        {
            BasicAttack();
            //if (autoSkillEnabled)
            //{
            //    UseSkill();
            //}
            //else
            //{
            //    BasicAttack();
            //}
            attackTimer = 1f / stats.attackSpeed;
        }
    }
    float CalculateDamage(float atk, MonsterStats enemy)
    {
        if (enemy == null) return atk;
        // 방어율 계산
        float defRate = enemy.monsterDef / (enemy.monsterDef + defenceConstant);
        defRate *= (1f - (stats.defencePenetration / 100f));
        // 치명타 판정
        // 100%일 경우 반드시 치명타
        bool isCritical = (stats.critChance >= 100f) || (Random.value < (stats.critChance / 100f));
        float critMultiplier = isCritical ? (1f + (stats.critDmg / 100f)) : 1f;
        // 최종 데미지
        return atk * (1f - defRate) * critMultiplier;
    }
    void BasicAttack()
    {
        // 기본 공격 로직
        if (target == null) return;
        Monster monsterComponent = target.GetComponent<Monster>();
        if(monsterComponent == null) return;
        // 몬스터 체력 불러오기
        MonsterStats enemyStats = monsterComponent.stats;
        // 데미지 계산
        float damage = CalculateDamage(stats.attackPower, enemyStats);
        // 실제 데미지 적용
        monsterComponent.TakeDamage(damage);
        Debug.LogFormat($"{target.name}에게 {damage}의 데미지 입힘");        
    }
    public void ToggleAutoSkill()
    {
        autoSkillEnabled = !autoSkillEnabled;
        Debug.Log("Auto Skill: " + (autoSkillEnabled ? "Enabled" : "Disabled"));
    }
    void UseSkill()
    {
        for(int i = 0; i < skills.Length; i++)
        {
            if(Time.time >= skills[i].lastUsedTime + skills[i].cooldown)
            {
                skills[i].Use(transform, target);
                skills[i].lastUsedTime = Time.time;
                break;
            }
        }           
    }
    public void TakeDamage(float damage)
    {
        stats.currentHp -= damage;
        //Debug.Log($"player took {damage} damage. HP left : {stats.currentHp}");
        if(stats.currentHp <= 0)
        {
            //Debug.Log($"player died");
        }
    }
}