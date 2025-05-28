using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float moveSpeed = 2.0f;
    private int damage;
    private Transform player;
    private float maxDistance = 10.0f;
    private float traveledDistance = 0.0f;
    private float hitDistance = 0.5f; // 플레이어에게 도달했다고 판단할 거리
    public void Init(Transform target, int damage)
    {
        this.player = target;
        this.damage = damage;
        if (player != null)
        {
            direction = ((Vector2)(player.position - transform.position)).normalized;
        }
        else
        {
            Debug.LogWarning("No player Found");
            direction = Vector2.right;
        }
        transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, Vector3.forward);
    }    
    void Update()
    {
        float moveDistance = moveSpeed * Time.deltaTime;
        transform.Translate(direction * moveDistance, Space.World);
        traveledDistance += moveDistance;
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= hitDistance)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(damage);
                }
                Destroy(gameObject);
                return;
            }
        }
        if(traveledDistance >= maxDistance)
        {
            Destroy(gameObject);
        }
    }
}
