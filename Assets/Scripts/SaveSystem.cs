using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;
using UnityEngine;
public class SaveSystem : MonoBehaviour
{
   private static PlayerPropertyTemplate _playerPropertyTemplate = new PlayerPropertyTemplate();
   private static PlayerPropertyTemplateBinary _playerPropertyTemplateBinary = new PlayerPropertyTemplateBinary();

   public static void SaveHealthPlayerPrefs(string keyParam, int currentPlayerHealthParam)
   {
      var state = $"Player's Health: {currentPlayerHealthParam} saved using PlayerPrefs.";
      Debug.Log(state);
      UIManager.Instance.SetState(state);
      PlayerPrefs.SetInt(keyParam, currentPlayerHealthParam);
   }

   public static int LoadHealthPlayerPrefs(string keyParam)
   {
      return PlayerPrefs.GetInt(keyParam);
   }
   public static void SavePositionPlayerPrefs(string keyParam, Vector3 currentPlayerPosParam)
   {
      PlayerPrefs.SetFloat(keyParam + "-PosX", currentPlayerPosParam.x);
      PlayerPrefs.SetFloat(keyParam + "-PosY", currentPlayerPosParam.y);
      PlayerPrefs.SetFloat(keyParam + "-PosZ", currentPlayerPosParam.z);
      var state =
         $"Player's Position: {currentPlayerPosParam.x}, {currentPlayerPosParam.y}, {currentPlayerPosParam.z} saved using PlayerPrefs."; 
      Debug.Log(state);
      UIManager.Instance.SetState(state);
   }

   public static Vector3 LoadPositionPlayerPrefs(string keyParam)
   {
      return new Vector3
      {
         x = PlayerPrefs.GetFloat(keyParam + "-PosX"),
         y = PlayerPrefs.GetFloat(keyParam + "-PosY"),
         z = PlayerPrefs.GetFloat(keyParam + "-PosZ"),
      };
   }

   public static void SavePlayerJson(string jsonFileNameParam, int currentPlayerHealthParam, Vector3 currentPlayerPosParam)
   {
      _playerPropertyTemplate.playerHealth = currentPlayerHealthParam;
      _playerPropertyTemplate.playerPosition = currentPlayerPosParam;
      var json = JsonConvert.SerializeObject(_playerPropertyTemplate, Formatting.Indented);
      var jsonFilePath = $"{Application.dataPath}/Saves/JSON/{jsonFileNameParam}.json";
      if(File.Exists(jsonFilePath)) Debug.Log($"{jsonFileNameParam}.json already exists. Overwriting...");
      File.WriteAllText(jsonFilePath, json);
      var state = $"Player's State is saved using JSON at {jsonFilePath}.";
      Debug.Log(state);
      UIManager.Instance.SetState(state);
   }

   private static PlayerPropertyTemplate DeserializePlayerPropertyJson(string jsonFilePathParam)
   {
      if (File.Exists(jsonFilePathParam))
      {
         var jsonContent = File.ReadAllText(jsonFilePathParam);
         _playerPropertyTemplate = JsonConvert.DeserializeObject<PlayerPropertyTemplate>(jsonContent);
         return _playerPropertyTemplate;
      }

      var state = "JSON File Not Found!";
      Debug.Log(state);
      UIManager.Instance.SetState(state);
      return null;
   }
   public static int LoadPlayerHealthJson(string jsonFileNameParam)
   {
      var jsonFilePath = $"{Application.dataPath}/Saves/JSON/{jsonFileNameParam}.json";
      var loadedPlayerProperty = DeserializePlayerPropertyJson(jsonFilePath);
      if (loadedPlayerProperty != null)
         return loadedPlayerProperty.playerHealth;
      return -1;
   }

   public static Vector3 LoadPlayerPosJson(string jsonFileNameParam)
   {
      var jsonFilePath = $"{Application.dataPath}/Saves/JSON/{jsonFileNameParam}.json";
      var loadedPlayerProperty = DeserializePlayerPropertyJson(jsonFilePath);
      return loadedPlayerProperty?.playerPosition ?? Vector3.zero;
   }

   public static void SavePlayerXml(string xmlFileNameParam, int currentPlayerHealthParam,
      Vector3 currentPlayerPosParam)
   {
      _playerPropertyTemplate.playerHealth = currentPlayerHealthParam;
      _playerPropertyTemplate.playerPosition = currentPlayerPosParam;
      var xmlSerializer = new XmlSerializer(typeof(PlayerPropertyTemplate));
      var xmlFilePath = $"{Application.dataPath}/Saves/XML/{xmlFileNameParam}.xml";
      if(File.Exists(xmlFilePath)) Debug.Log($"{xmlFileNameParam}.xml already exists. Overwriting...");
      var writer = new StreamWriter(xmlFilePath);
      xmlSerializer.Serialize(writer, _playerPropertyTemplate);
      writer.Close();
      var state = $"Player's State is saved using XML at {xmlFilePath}.";
      Debug.Log(state);
   }

   private static PlayerPropertyTemplate DeserializePlayerPropertyXml(string xmlFilePathParam)
   {
      var xmlSerializer = new XmlSerializer(typeof(PlayerPropertyTemplate));
      if (File.Exists(xmlFilePathParam))
      {
         var fileStream = new FileStream(xmlFilePathParam, FileMode.Open);
         var playerProperty =  (PlayerPropertyTemplate) xmlSerializer.Deserialize(fileStream);
         fileStream.Close();
         return playerProperty;
      }
      const string state = "XML File Not Found!";
      Debug.Log(state);
      UIManager.Instance.SetState(state);
      return null;
   }

   public static int LoadPlayerHealthXml(string xmlFileNameParam)
   {
      var xmlFilePath = $"{Application.dataPath}/Saves/XML/{xmlFileNameParam}.xml";
      var loadedPlayerProperty = DeserializePlayerPropertyXml(xmlFilePath);
      if (loadedPlayerProperty != null)
         return loadedPlayerProperty.playerHealth;
      return -1;
   }

   public static Vector3 LoadPlayerPosXml(string xmlFileNameParam)
   {
      var xmlFilePath = $"{Application.dataPath}/Saves/XML/{xmlFileNameParam}.xml";
      var loadedPlayerProperty = DeserializePlayerPropertyXml(xmlFilePath);
      return loadedPlayerProperty?.playerPosition ?? Vector3.zero;
   }

   public static void SavePlayerBinary(string binaryFileName, int currentPlayerHealthParam,
      Vector3 currentPlayerPosParam)
   {
      _playerPropertyTemplateBinary.playerHealth = currentPlayerHealthParam;
      _playerPropertyTemplateBinary.playerPositionX = currentPlayerPosParam.x;
      _playerPropertyTemplateBinary.playerPositionY = currentPlayerPosParam.y;
      _playerPropertyTemplateBinary.playerPositionZ = currentPlayerPosParam.z;
      var binaryFormatter = new BinaryFormatter();
      var binaryFilePath = $"{Application.dataPath}/Saves/DAT/{binaryFileName}.dat";
      if(File.Exists(binaryFilePath)) Debug.Log($"{binaryFileName}.dat already exists. Overwriting...");
      var fileStream = File.Create(binaryFilePath);
      binaryFormatter.Serialize(fileStream, _playerPropertyTemplateBinary);
      fileStream.Close();
      var state = $"Player's State is saved using Binary Formatter at {binaryFilePath}.";
      Debug.Log(state);
   }

   private static PlayerPropertyTemplateBinary DeserializePlayerPropertyBinary(string binaryFilePathParam)
   {
      var binaryFormatter = new BinaryFormatter();
      if (File.Exists(binaryFilePathParam))
      {
         var fileStream = new FileStream(binaryFilePathParam, FileMode.Open);
         var playerProperty = (PlayerPropertyTemplateBinary) binaryFormatter.Deserialize(fileStream);
         fileStream.Close();
         return playerProperty;
      }
      const string state = "DAT File Not Found!";
      Debug.Log(state);
      UIManager.Instance.SetState(state);
      return null;
   }

   public static int LoadPlayerHealthBinary(string binaryFileNameParam)
   {
      var binaryFilePath = $"{Application.dataPath}/Saves/DAT/{binaryFileNameParam}.dat";
      var loadedPlayerProperty = DeserializePlayerPropertyBinary(binaryFilePath);
      if (loadedPlayerProperty != null)
         return loadedPlayerProperty.playerHealth;
      return -1;
   }

   public static Vector3 LoadPlayerPosBinary(string binaryFileNameParam)
   {
      var binaryFilePath = $"{Application.dataPath}/Saves/DAT/{binaryFileNameParam}.dat";
      var loadedPlayerProperty = DeserializePlayerPropertyBinary(binaryFilePath);
      if (loadedPlayerProperty != null)
      {
         return new Vector3(loadedPlayerProperty.playerPositionX, loadedPlayerProperty.playerPositionY,
            loadedPlayerProperty.playerPositionZ);
      }
      return Vector3.zero;
   }

   public static bool CheckIfSavePresent(string filePathParam)
   {
      return File.Exists(filePathParam);
   }
}

[Serializable]
public class PlayerPropertyTemplate
{
   public int playerHealth;
   public Vector3 playerPosition;
}
[Serializable]
public class PlayerPropertyTemplateBinary // needs separate player property template because Vector3 is not serializable by default, to mitigate this, 3 float variable are declared in order to represent a Vector3
{
   public int playerHealth;
   public float playerPositionX, playerPositionY, playerPositionZ;
}
