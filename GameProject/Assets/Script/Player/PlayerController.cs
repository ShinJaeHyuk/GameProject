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
    private const float defenceConstant = 1000f; // ��� ���
    private float attackRange = 1.0f; // ���� ����
    public float distanceToTarget;
    // ü�¹� �� ������ ǥ�ñ�
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
        // ĵ����
        uiCanvas = GameObject.Find("Canvas").transform;
        // ������ �ε�
        if (playerHpBar == null) playerHpBar = Resources.Load<GameObject>("Prefab/HP_Bar");
        if (playerDamageText == null) playerDamageText = Resources.Load<GameObject>("Prefab/FloatingDamageText");
        // ü�¹� ����
        if(playerHpBar != null)
        {
            hpBarInstance = Instantiate(playerHpBar, uiCanvas);
            hpSlider = hpBarInstance.GetComponentInChildren<Slider>();
            hpSlider.maxValue = stats.maxHp;
            hpSlider.value = stats.currentHp;
            hpBarInstance.transform.position = transform.position + Vector3.up * 1.5f;
        }
        // ��ų �ʱ�ȭ
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
        // ����� �� ã��
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
        // ������ �̵�
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
        // ��ų �Ǵ� �Ϲݰ���
        attackTimer -= Time.deltaTime;        
        // ���� ���� Ȯ��
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
        // ����� ���
        float defRate = enemy.monsterDef / (enemy.monsterDef + defenceConstant);
        defRate *= (1f - (stats.defencePenetration / 100f));
        // ġ��Ÿ ����
        // 100%�� ��� �ݵ�� ġ��Ÿ
        bool isCritical = (stats.critChance >= 100f) || (Random.value < (stats.critChance / 100f));
        float critMultiplier = isCritical ? (1f + (stats.critDmg / 100f)) : 1f;
        // ���� ������
        return (int)(atk * (1f - defRate) * critMultiplier);
    }
    void BasicAttack()
    {
        // �⺻ ���� ����
        if (target == null) return;
        Monster monsterComponent = target.GetComponent<Monster>();
        if(monsterComponent == null) return;
        // ���� ü�� �ҷ�����
        MonsterStats enemyStats = monsterComponent.stats;
        // ������ ���
        int damage = CalculateDamage(stats.attackPower, enemyStats);
        // ���� ������ ����
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
            // ����� �������� ���̺� ó������ ���ư��� �ڵ�
        }
    }
    void UpdateHpBar()
    {
        if (hpBarInstance == null) return;
        Vector3 worldPos = transform.position + Vector3.up * 1.5f;
        hpBarInstance.transform.position = worldPos;
        // �����̴� ������Ʈ
        Slider slider = hpBarInstance.GetComponentInChildren<Slider>();
        if(slider != null)
        {
            slider.value = (float)stats.currentHp / stats.maxHp;
        }
    }
    void ShowDamageText(int damage)
    {
        if (playerDamageText == null || uiCanvas == null) return;
        // ���� �ؽ�Ʈ�� ���� �̵�
        for(int i = 0; i < damageTexts.Count; i++)
        {
            if (damageTexts[i] != null)
            {
                damageTexts[i].transform.position += Vector3.up * 0.3f;
            }
        }
        // �ؽ�Ʈ ����
        GameObject dmgText = Instantiate(playerDamageText, uiCanvas);        
        var text = dmgText.GetComponentInChildren<TextMeshProUGUI>();
        if(text != null)
        {
            text.text = damage.ToString();
        }
        // ��ġ ����
        Vector3 worldPos = transform.position + Vector3.up * 1.0f;
        dmgText.transform.position = worldPos;
        // ����Ʈ�� ���ϱ�
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