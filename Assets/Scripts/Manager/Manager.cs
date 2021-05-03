using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance = null;

    public Transform cameraRod;

    private static int _hp;
    private static int _gold;
    private static Vector3 playerPosition;

    private static bool isLevelComplete;

    [SerializeField]
    private TMPro.TextMeshProUGUI hpField;

    [SerializeField]
    private TMPro.TextMeshProUGUI goldField;

    [SerializeField]
    private Michsky.UI.ModernUIPack.ModalWindowManager gameOverWindow;
    private static Michsky.UI.ModernUIPack.ModalWindowManager gameOverWindow_Static;

    [SerializeField]
    private TMPro.TextMeshProUGUI statusField;
    private static TMPro.TextMeshProUGUI statusField_Static;

    [SerializeField]
    private TMPro.TextMeshProUGUI countdownField;
    private static TMPro.TextMeshProUGUI countdownField_Static;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        //DontDestroyOnLoad(gameObject);

        InitializeManager();
    }

    private void FixedUpdate()
    {
        hpField.text = $"HP: {_hp}";
        goldField.text = $"GOLD: {_gold}";

        cameraRod.position = Vector3.Lerp(cameraRod.position, playerPosition, Time.deltaTime * 2f);
    }

    private void InitializeManager()
    {
        _hp = 50;
        _gold = 0;

        isLevelComplete = false;

        gameOverWindow_Static = gameOverWindow;
        statusField_Static = statusField;
        countdownField_Static = countdownField;
    }

    public static void SetHP(int hp)
    {
        _hp = hp;

        if (_hp <= 0)
        {
            _hp = 0;
            statusField_Static.text = "";
            gameOverWindow_Static.OpenWindow();
        }
    }

    public static void IncreaseGold(int amount)
    {
        _gold += amount;
    }

    public static void SetPlayerPosition(Vector3 position)
    {
        playerPosition = position;
    }

    public static void SetStatusText(string text)
    {
        statusField_Static.text = text;
    }

    public static void SetCountdownText(string text)
    {
        countdownField_Static.text = text;
    }

    public static void LevelCompleteEvent()
    {
        if (!isLevelComplete)
        {
            statusField_Static.text = "";
            gameOverWindow_Static.OpenWindow();
        }
    }
}
