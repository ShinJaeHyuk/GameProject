using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(GameStart);
    }
    void GameStart()
    {
        SceneManager.LoadScene("SaveSlotScene");
        Debug.Log("press");
    }
}
