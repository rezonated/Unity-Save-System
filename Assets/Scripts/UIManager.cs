using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private TextMeshProUGUI currentPlayerHealthValueText, currentModeText, currentState;
    [SerializeField] private Image faderImage;
    [SerializeField] private Button loadPlayerButton;
    [SerializeField] private Transform stateLogContentTransform;
    [SerializeField] private GameObject stateLogMessagePrefab;
    public Button LoadPlayerButton
    {
        get => loadPlayerButton;
        set
        {
            loadPlayerButton = value;
            _gameManager.RefreshSaveButton();
        }
    }

    public TextMeshProUGUI CurrentModeText
    {
        get => currentModeText;
        set
        {
            currentPlayerHealthValueText = value;
            _gameManager.RefreshModeText();
        }
    }

    private GameManager _gameManager;
    private PlayerProperties _playerProperties;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        _playerProperties = FindObjectOfType<PlayerProperties>();
        _gameManager = FindObjectOfType<GameManager>();
        RefreshUI();
    }
    public void RefreshUI()
    {
        currentPlayerHealthValueText.SetText(_playerProperties.CurrentPlayerHealth.ToString());
    }
    public void OnSaveButtonClicked()
    {
        _gameManager.SavePlayer();
    }

    public void OnLoadButtonClicked()
    {
        StartCoroutine(LoadPlayerProcedure());
    }

    private IEnumerator LoadPlayerProcedure()
    {
        faderImage.DOFade(1.0f, 1f).OnComplete(()=>_gameManager.LoadPlayer());
        yield return new WaitForSeconds(2f);
        faderImage.DOFade(0f, 1f);
    }

    public void SetState(string stateParam)
    {
        var logMessagePrefab = Instantiate(stateLogMessagePrefab, transform.position, Quaternion.identity,
            stateLogContentTransform);
        logMessagePrefab.GetComponent<TextMeshProUGUI>().SetText(stateParam);
    }
}
