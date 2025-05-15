using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayerData;
using System.IO;
using System;
using TMPro;

public class SaveSlotMenu : MonoBehaviour
{
    // 세이브 슬롯 버튼
    public Button saveSlot1Button;
    public Button saveSlot2Button;
    public Button saveSlot3Button;
    // 종료 버튼
    public Button quitButton;
    // 세이브 데이터 삭제 버튼
    public Button deleteButton;
    // 삭제 확인 팝업
    public GameObject confirmDeletePopup;
    // 삭제 확인 텍스트
    public TextMeshProUGUI confirmDeleteText;
    // 삭제 확인 버튼
    public Button confirmDeleteYesButton;
    public Button confirmDeleteNoButton;
    // 빈 슬롯 알림
    public GameObject emptySlotMessagePopup;
    public TextMeshProUGUI emptySlotMessageText;
    public Button emptySlotMessageOkButton;
    // 기타 변수
    private int selectedSlotToDelete = -1;
    private bool isDeleteMode = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateSlotUI(1, saveSlot1Button);
        UpdateSlotUI(2, saveSlot2Button);
        UpdateSlotUI(3, saveSlot3Button);
        saveSlot1Button.onClick.AddListener(() => OnSlotButtonClicked(1));
        saveSlot2Button.onClick.AddListener(() => OnSlotButtonClicked(2));
        saveSlot3Button.onClick.AddListener(() => OnSlotButtonClicked(3));
        quitButton.onClick.AddListener(QuitGame);
        deleteButton.onClick.AddListener(ToggleDeleteMode);
        confirmDeleteYesButton.onClick.AddListener(ConfirmDelete);
        confirmDeleteNoButton.onClick.AddListener(CancelDelete);
        emptySlotMessageOkButton.onClick.AddListener(CloseEmptySlotMessage);
        // 팝업창 비활성화
        emptySlotMessagePopup.SetActive(false);
        confirmDeletePopup.SetActive(false);
    }
    void UpdateSlotUI(int slotNumber, Button slotButton)
    {        
        string path = Application.persistentDataPath + $"/SaveSlot{slotNumber}.json";
        TextMeshProUGUI buttonText = slotButton.GetComponentInChildren<TextMeshProUGUI>();
        RectTransform buttonRectTransform = slotButton.GetComponent<RectTransform>();
        if(File.Exists(path))
        {
            Stats stats = Stats.LoadStats(slotNumber);
            FileInfo fileInfo = new FileInfo(path);
            string creationDate = fileInfo.CreationTime.ToString("yyyy-MM-dd HH:mm");
            buttonText.text = $"Level : {stats.level}\nDate : {creationDate}";
            // 버튼 크기 조절
            Vector2 textSize = buttonText.GetPreferredValues();
            buttonRectTransform.sizeDelta = new Vector2(textSize.x + 5, textSize.y);
        }
        else
        {
            {
                buttonText.text = $"Empty Slot{slotNumber}";
                // 버튼 크기 기존과 동일
                buttonRectTransform.sizeDelta = new Vector2(160, 30);
            }
        }
    }
    void OnSlotButtonClicked(int slotNumber)
    {
        if (isDeleteMode)
        {
            string path = Application.persistentDataPath + $"/SaveSlot{slotNumber}.json";
            // 빈 슬롯 삭제 방지
            if (!File.Exists(path))
            {
                ShowEmptySlotMessage("Empty Slot");
                return;
            }
            selectedSlotToDelete = slotNumber;
            confirmDeleteText.text = $"Really delete slot {slotNumber}'s data?";
            confirmDeletePopup.SetActive(true);
        }
        else
        {
            if (SaveManager.SaveFileExists(slotNumber))
            {
                SaveManager.LoadGameData(slotNumber);
            }
            else
            {
                SaveManager.CreateNewSaveData(slotNumber);
            }
            GameManager.CurrentSlot = slotNumber;
            SceneManager.LoadScene("PlayScene");
        }
    }
    public void QuitGame()
    {
        // 유니티 에디터일때
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        // 모바일일때
        #if UNITY_ANDROID
        Application.Quit();
        #endif
    }
    private void ToggleDeleteMode()
    {
        isDeleteMode = !isDeleteMode;
        deleteButton.GetComponentInChildren<TextMeshProUGUI>().text = isDeleteMode ? "Delete Cancel" : "Delete";
        if (!isDeleteMode)
        {
            selectedSlotToDelete = -1;
            confirmDeletePopup.SetActive(false);
        }
    }
    private void ConfirmDelete()
    {
        if (selectedSlotToDelete != -1)
        {
            string path = Application.persistentDataPath + $"/SaveSlot{selectedSlotToDelete}.json";
            if (File.Exists(path))
            {
                File.Delete(path);
                UpdateSlotUI(selectedSlotToDelete, GetButtonBySlotNumber(selectedSlotToDelete));
                selectedSlotToDelete = -1;
            }            
        }
        confirmDeletePopup.SetActive(false);
    }
    private void CancelDelete()
    {
        confirmDeletePopup.SetActive(false);
        selectedSlotToDelete = -1;
    }
    private Button GetButtonBySlotNumber(int slotNumber)
    {
        switch (slotNumber)
        {
            case 1: return saveSlot1Button;
            case 2: return saveSlot2Button;
            case 3: return saveSlot3Button;
            default: return null;
        }
    }
    void ShowEmptySlotMessage(string message)
    {
        emptySlotMessageText.text = message;
        emptySlotMessagePopup.SetActive(true);
    }
    void CloseEmptySlotMessage()
    {
        emptySlotMessagePopup.SetActive(false);
    }
}
