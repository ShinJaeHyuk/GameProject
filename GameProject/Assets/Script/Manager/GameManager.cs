using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int CurrentSlot = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
