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
    // 체력바 및 데미지 표시기
    private GameObject hpBar;
    private GameObject damageText;
    private GameObject hpBarInstance;
    private Transform uiCanvas;
    private Slider hpSlider;
    private List<GameObject> damageTexts = new();
    // 나중에 스테이지테이블에서 가져올거
    public static float rewardMultiplier = 1.0f;    
    public void Init(string[] csvData)
    {
        stats.LoadDataFromCSV(csvData);
        stats.monsterCurrentHp = stats.monsterMaxHp;
        // 캔버스
        uiCanvas = GameObject.Find("Canvas").transform;
        if (uiCanvas == null) { Debug.LogWarning("sksksksk"); }
        // 프리팹 로드
        if (hpBar == null) hpBar = Resources.Load<GameObject>("Prefab/HP_Bar");        
        if (damageText == null) damageText = Resources.Load<GameObject>("Prefab/FloatingDamageText");        
        // 체력바 생성
        if (hpBar != null)
        {
            hpBarInstance = Instantiate(hpBar, uiCanvas);
            hpSlider = hpBarInstance.GetComponentInChildren<Slider>();
            hpSlider.maxValue = stats.monsterMaxHp;
            hpSlider.value = stats.monsterCurrentHp;
            // 최초위치 설정
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
        // 이전 텍스트 위로 이동
        for(int i = 0; i < damageTexts.Count; i++)
        {
            if (damageTexts[i] != null)
            {
                damageTexts[i].transform.position += Vector3.up * 0.3f;
            }
        }
        // 텍스트 생성
        GameObject dmgText = Instantiate(damageText, uiCanvas);        
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
        if (text != null)
        {
            Destroy(text);
            damageTexts.Remove(text);
        }
    }
    private void Die()
    {
        // 체력바 삭제
        Destroy(hpBarInstance);
        // 플레이어 보상 지급
        var playerStats = PlayerData.Stats.LoadStats(GameManager.currentSlot);
        int finalGold = (int)(stats.monsterDropGold * rewardMultiplier);
        int finalExp = (int)(stats.monsterDropExp * rewardMultiplier);
        playerStats.gold += finalGold;
        playerStats.exp += finalExp;
        Debug.Log($"Player gain {finalGold} gold, {finalExp} exp. {playerStats.gold} gold, {playerStats.exp} exp");
        // 레벨업 처리
        while(playerStats.exp >= playerStats.expToNextLevel)
        {
            Debug.Log($"{playerStats.expToNextLevel}");
            playerStats.exp -= playerStats.expToNextLevel;
            playerStats.level++;
            playerStats.statPoints += 1;
            Debug.Log($"player levelup. {playerStats.level} level, {playerStats.statPoints} statpoint left");
            // 체력 회복
            playerStats.currentHp = playerStats.maxHp;
        }        
        playerStats.SaveStats(GameManager.currentSlot);
        Destroy(gameObject);
    }
}
