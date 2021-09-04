using UnityEngine;
public class CubePickup : MonoBehaviour
{
   [SerializeField] private int healthToAdd = 1;
   private void Update()
   {
      transform.Rotate(transform.up, -90f * Time.deltaTime);
   }
   private void OnTriggerEnter(Collider other)
   {
      var playerProperty = other.GetComponent<PlayerProperties>();
      if(playerProperty == null) return;
      playerProperty.AddHealth(healthToAdd);
      Destroy(gameObject);
   }
}
    