using UnityEngine;
public class PlayerProperties : MonoBehaviour
{
    [SerializeField] private int currentPlayerHealth = 5;
    private UIManager _uiManager;

    private void Awake()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }

    public int CurrentPlayerHealth
    {
        get => currentPlayerHealth;
        set
        {
            if(value <= -1) return;
            currentPlayerHealth = value;
            _uiManager.RefreshUI();
        }
    }
    public void DeductHealth(int healthParam = 1)
    {
        CurrentPlayerHealth -= healthParam;
    }

    public void AddHealth(int healthParam)
    {
        CurrentPlayerHealth += healthParam;
    }
}