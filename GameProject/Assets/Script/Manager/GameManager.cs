using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int currentSlot {  get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public static void SetSlot(int slotNumber)
    {
        currentSlot = slotNumber;
        Debug.Log($"[GameManager] 선택된 슬롯 번호: {currentSlot}");
    }
}
