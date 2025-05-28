using UnityEngine;
using TMPro;

public class FloatingDamage : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeDuration = 1f;
    private TextMeshProUGUI text;
    private Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        color = text.color;
        Destroy(gameObject, fadeDuration);
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
        Color fadeColor = text.color;
        fadeColor.a -= Time.deltaTime / fadeDuration;
        text.color = fadeColor;
    }
}
