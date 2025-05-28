using UnityEngine;
using MonsterData;
using System.Linq.Expressions;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class Monster : MonoBehaviour
{
    public MonsterStats stats = new MonsterStats();
    // ü�¹� �� ������ ǥ�ñ�
    private GameObject hpBar;
    private GameObject damageText;
    private GameObject hpBarInstance;
    private Transform uiCanvas;
    private Slider hpSlider;
    private List<GameObject> damageTexts = new();
    // ���߿� �����������̺��� �����ð�
    public static float rewardMultiplier = 1.0f;    
    public void Init(string[] csvData)
    {
        stats.LoadDataFromCSV(csvData);
        stats.monsterCurrentHp = stats.monsterMaxHp;
        // ĵ����
        uiCanvas = GameObject.Find("Canvas").transform;
        if (uiCanvas == null) { Debug.LogWarning("sksksksk"); }
        // ������ �ε�
        if (hpBar == null) hpBar = Resources.Load<GameObject>("Prefab/HP_Bar");        
        if (damageText == null) damageText = Resources.Load<GameObject>("Prefab/FloatingDamageText");        
        // ü�¹� ����
        if (hpBar != null)
        {
            hpBarInstance = Instantiate(hpBar, uiCanvas);
            hpSlider = hpBarInstance.GetComponentInChildren<Slider>();
            hpSlider.maxValue = stats.monsterMaxHp;
            hpSlider.value = stats.monsterCurrentHp;
            // ������ġ ����
            hpBarInstance.transform.position = transform.position + Vector3.up * 1.5f;
        }
    }
    public void TakeDamage(int damage)
    {
        stats.monsterCurrentHp -= damage;
        if (stats.monsterCurrentHp < 0) stats.monsterCurrentHp = 0;
        if (hpSlider != null)
        {
            UpdateHpBar();
        }
        ShowDamageText(damage);
        if(stats.monsterCurrentHp <= 0 )
        {
            Die();
        }
    }
    void UpdateHpBar()
    {
        if (hpBarInstance == null) return;
        Vector3 worldPos = transform.position + Vector3.up * 0.7f;
        hpBarInstance.transform.position = worldPos;
        Slider slider = hpBarInstance.GetComponentInChildren<Slider>();
        if (slider != null)
        {
            slider.value = (float)stats.monsterCurrentHp / stats.monsterMaxHp;
        }
    }
    void ShowDamageText(int damage)
    {
        if (damageText == null || uiCanvas == null) return;
        // ���� �ؽ�Ʈ ���� �̵�
        for(int i = 0; i < damageTexts.Count; i++)
        {
            if (damageTexts[i] != null)
            {
                damageTexts[i].transform.position += Vector3.up * 0.3f;
            }
        }
        // �ؽ�Ʈ ����
        GameObject dmgText = Instantiate(damageText, uiCanvas);        
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
        if (text != null)
        {
            Destroy(text);
            damageTexts.Remove(text);
        }
    }
    private void Die()
    {
        // ü�¹� ����
        Destroy(hpBarInstance);
        // �÷��̾� ���� ����
        var playerStats = PlayerData.Stats.LoadStats(GameManager.currentSlot);
        int finalGold = (int)(stats.monsterDropGold * rewardMultiplier);
        int finalExp = (int)(stats.monsterDropExp * rewardMultiplier);
        playerStats.gold += finalGold;
        playerStats.exp += finalExp;
        Debug.Log($"Player gain {finalGold} gold, {finalExp} exp. {playerStats.gold} gold, {playerStats.exp} exp");
        // ������ ó��
        while(playerStats.exp >= playerStats.expToNextLevel)
        {
            Debug.Log($"{playerStats.expToNextLevel}");
            playerStats.exp -= playerStats.expToNextLevel;
            playerStats.level++;
            playerStats.statPoints += 1;
            Debug.Log($"player levelup. {playerStats.level} level, {playerStats.statPoints} statpoint left");
            // ü�� ȸ��
            playerStats.currentHp = playerStats.maxHp;
        }        
        playerStats.SaveStats(GameManager.currentSlot);
        Destroy(gameObject);
    }
}
