using UnityEngine;
using System.Collections.Generic;
using PlayerData;
using System.Collections;

public abstract class Skill : MonoBehaviour
{
    public string skillName;
    public float damage;
    public float cooldown;
    public float lastUsedTime;
    // 플레이어 위치, 타겟을 받아 스킬 실행
    public abstract void Use(Transform playerTransform, Transform target);
}
// 돌진 스킬
public class DashSkill : Skill
{
    public float dashDistance = 5f;
    public float dashSpeed = 20f;
    public override void Use(Transform playerTransform, Transform target)
    {
        Debug.Log("Dash");
        playerTransform.GetComponent<MonoBehaviour>().StartCoroutine(DashCoroutine(playerTransform, target));
    }
    private IEnumerator DashCoroutine(Transform playerTransform, Transform target)
    {
        if(target == null) yield break;
        Vector2 startPos = playerTransform.position;
        Vector2 endPos = (Vector2)transform.position;
        Vector2 dir = (endPos - startPos).normalized;
        float distance = 0f;
        while(distance < dashDistance)
        {
            float move = dashSpeed * Time.deltaTime;
            playerTransform.Translate(dir * move);
            distance += move;
            yield return null;
        }
    }
}
// 회전 칼날
public class SpinningBladeSkill : Skill
{
    public float duration = 5f;
    public override void Use(Transform playerTransform, Transform target)
    {
        Debug.Log("SpinningBlade");
        GameObject bladePrefab = Resources.Load<GameObject>("SpinningBlade");
        if(bladePrefab != null)
        {
            GameObject blade = GameObject.Instantiate(bladePrefab, playerTransform.position, Quaternion.identity);
            blade.transform.SetParent(playerTransform.parent);
            GameObject.Destroy(blade, duration);
        }
        else
        {
            Debug.Log("Resources폴더에 SpinningBlade 없음");
        }
    }
}