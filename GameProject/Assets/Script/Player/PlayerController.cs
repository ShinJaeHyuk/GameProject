using UnityEngine;
using PlayerData;
using System.Collections.Generic;
using MonsterData;
using TMPro;
using UnityEngine.UI;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;
using System.Collections;

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
    // 체력바 및 데미지 표시기
    private GameObject playerHpBar;
    private GameObject playerDamageText;
    private GameObject hpBarInstance;
    private Slider hpSlider;
    private Transform uiCanvas;
    public float stopDistance = 0.5f;
    private List<GameObject> damageTexts = new();
    void Start()
    {
        stats = Stats.LoadStats(GameManager.currentSlot);
        // 캔버스
        uiCanvas = GameObject.Find("Canvas").transform;
        // 프리팹 로드
        if (playerHpBar == null) playerHpBar = Resources.Load<GameObject>("Prefab/HP_Bar");
        if (playerDamageText == null) playerDamageText = Resources.Load<GameObject>("Prefab/FloatingDamageText");
        // 체력바 생성
        if(playerHpBar != null)
        {
            hpBarInstance = Instantiate(playerHpBar, uiCanvas);
            hpSlider = hpBarInstance.GetComponentInChildren<Slider>();
            hpSlider.maxValue = stats.maxHp;
            hpSlider.value = stats.currentHp;
            hpBarInstance.transform.position = transform.position + Vector3.up * 1.5f;
        }
        // 스킬 초기화
    }
    void Update()
    {
        if(hpBarInstance != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
            hpBarInstance .transform.position = screenPos;
        }
        if(target == null || !target.gameObject.activeInHierarchy)
        {
            FindClosestTarget();
        }
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
            Debug.Log($"enemy found : {enemy.name}, position: {enemy.transform.position}, distance : {distance}");
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
        float distance = Vector2.Distance(transform.position, target.position);  
        if(distance > stopDistance)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * stats.moveSpeed * Time.deltaTime);
        }
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
    int CalculateDamage(int atk, MonsterStats enemy)
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
        return (int)(atk * (1f - defRate) * critMultiplier);
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
        int damage = CalculateDamage(stats.attackPower, enemyStats);
        // 실제 데미지 적용
        monsterComponent.TakeDamage(damage);     
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
    public void TakeDamage(int damage)
    {
        stats.currentHp -= damage;
        if(stats.currentHp < 0) stats.currentHp = 0;
        if (hpSlider != null)
        {
            UpdateHpBar();
        }
        ShowDamageText(damage);
        if(stats.currentHp <= 0)
        {
            // 사망시 스테이지 웨이브 처음으로 돌아가는 코드
        }
    }
    void UpdateHpBar()
    {
        if (hpBarInstance == null) return;
        Vector3 worldPos = transform.position + Vector3.up * 1.5f;
        hpBarInstance.transform.position = worldPos;
        // 슬라이더 업데이트
        Slider slider = hpBarInstance.GetComponentInChildren<Slider>();
        if(slider != null)
        {
            slider.value = (float)stats.currentHp / stats.maxHp;
        }
    }
    void ShowDamageText(int damage)
    {
        if (playerDamageText == null || uiCanvas == null) return;
        // 이전 텍스트를 위로 이동
        for(int i = 0; i < damageTexts.Count; i++)
        {
            if (damageTexts[i] != null)
            {
                damageTexts[i].transform.position += Vector3.up * 0.3f;
            }
        }
        // 텍스트 생성
        GameObject dmgText = Instantiate(playerDamageText, uiCanvas);        
        var text = dmgText.GetComponentInChildren<TextMeshProUGUI>();
        if(text != null)
        {
            text.text = damage.ToString();
        }
        // 위치 설정
        Vector3 worldPos = transform.position + Vector3.up * 1.0f;
        dmgText.transform.position = worldPos;
        // 리스트에 더하기
        damageTexts.Add(dmgText);
        StartCoroutine(RemoveDamageTextAfterDelay(dmgText, 0.5f));
    }    
    IEnumerator RemoveDamageTextAfterDelay(GameObject text, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(text != null)
        {
            Destroy(text);
            damageTexts.Remove(text);
        }
    }
}