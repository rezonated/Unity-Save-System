using System;
using UnityEngine;
public class GameManager : MonoBehaviour
{
   [SerializeField] private string playerHealthKey = "PlayerHealth", playerPositionKey = "PlayerPosition", jsonFileName = "Player", xmlFileName = "Player", binaryFileName = "Player";
   [SerializeField] private SaveMode saveMode = SaveMode.PlayerPrefs;

   public SaveMode SaveMode
   {
      get => saveMode;
      set
      {
         saveMode = value;
         RefreshSaveButton();
         RefreshModeText();
      }
   }
   private PlayerMovement _playerMovement;
   private PlayerProperties _playerProperties;
   private UIManager _uiManager;
   private void Awake()
   {
      _playerMovement = FindObjectOfType<PlayerMovement>();
      _playerProperties = FindObjectOfType<PlayerProperties>();
      _uiManager = FindObjectOfType<UIManager>();
   }

   private void Start()
   {
      RefreshModeText();
      RefreshSaveButton();
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.F1))
         SaveMode = SaveMode.PlayerPrefs;
      if (Input.GetKeyDown(KeyCode.F2))
         SaveMode = SaveMode.JSON;
      if (Input.GetKeyDown(KeyCode.F3))
         SaveMode = SaveMode.XML;
      if (Input.GetKeyDown(KeyCode.F4))
         SaveMode = SaveMode.DAT;
   }

   public void RefreshModeText()
   {
      _uiManager.CurrentModeText.text = $"Current Mode: {saveMode.ToString()}";
   }
   public void RefreshSaveButton()
   {
      if (saveMode == SaveMode.PlayerPrefs)
         _uiManager.LoadPlayerButton.interactable = true;
      else
      {
         string fileName;
         switch (saveMode)
         {
            case SaveMode.JSON:
               fileName = jsonFileName;
               break;
            case SaveMode.XML:
               fileName = xmlFileName;
               break;
            case SaveMode.DAT:
               fileName = binaryFileName;
               break;
            default:
               throw new ArgumentOutOfRangeException();
         }
         _uiManager.LoadPlayerButton.interactable =
            SaveSystem.CheckIfSavePresent($"{Application.dataPath}/Saves/{saveMode.ToString()}/{fileName}.{saveMode.ToString().ToLower()}");
      }
   }

   public void SavePlayer()
   {
      switch (saveMode)
      {
         case SaveMode.PlayerPrefs:
            SaveSystem.SaveHealthPlayerPrefs(playerHealthKey, _playerProperties.CurrentPlayerHealth);
            SaveSystem.SavePositionPlayerPrefs(playerPositionKey, _playerMovement.transform.position);
            break;
         case SaveMode.JSON:
            SaveSystem.SavePlayerJson(jsonFileName, _playerProperties.CurrentPlayerHealth, _playerMovement.transform.position);
            break;
         case SaveMode.XML:
            SaveSystem.SavePlayerXml(xmlFileName, _playerProperties.CurrentPlayerHealth,
               _playerMovement.transform.position);
            break;
         case SaveMode.DAT:
            SaveSystem.SavePlayerBinary(binaryFileName, _playerProperties.CurrentPlayerHealth,
               _playerMovement.transform.position);
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
   }

   public void LoadPlayer()
   {
      var state = "Player loaded with ";
      switch (saveMode)
      {
         case SaveMode.PlayerPrefs:
            _playerProperties.CurrentPlayerHealth = SaveSystem.LoadHealthPlayerPrefs(playerHealthKey);
            _playerMovement.transform.position = SaveSystem.LoadPositionPlayerPrefs(playerPositionKey);
            state += "Player Prefs ";
            break;
         case SaveMode.JSON:
            _playerProperties.CurrentPlayerHealth = SaveSystem.LoadPlayerHealthJson(jsonFileName);
            _playerMovement.transform.position = SaveSystem.LoadPlayerPosJson(jsonFileName);
            state += "JSON ";
            break;
         case SaveMode.XML:
            _playerProperties.CurrentPlayerHealth = SaveSystem.LoadPlayerHealthXml(xmlFileName);
            _playerMovement.transform.position = SaveSystem.LoadPlayerPosXml(xmlFileName);
            state += "XML ";
            break;
         case SaveMode.DAT:
            _playerProperties.CurrentPlayerHealth = SaveSystem.LoadPlayerHealthBinary(binaryFileName);
            _playerMovement.transform.position = SaveSystem.LoadPlayerPosBinary(binaryFileName);
            state += "Binary Formatter ";
            break;
         default:
            throw new ArgumentOutOfRangeException();
      }
      state += $"at {_playerMovement.transform.position} with {_playerProperties.CurrentPlayerHealth} health.";
      _uiManager.SetState(state);
   }
}

public enum SaveMode
{
   PlayerPrefs, 
   // ReSharper disable once InconsistentNaming
   JSON,
   XML,
   // ReSharper disable once InconsistentNaming
   DAT
}