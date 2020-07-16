using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour {
   public PlayerInput input;
   public PlayerFighter ship;
   public new PlayerCamera camera;
   public CameraController cameraController;

   public PlayerInventory inventory;

   public Transform spawnPoint;

   private void Awake() {
      SpawnPlayerInitial();
      camera.target = ship.transform;
      cameraController.Player = this;
      cameraController.MainCamera = camera.camera;
      cameraController.SetCameraBehaviour(camera);
   }

   // TODO: wacky shit ahead / unsure how the player will enter the level in a real game
   public void SpawnPlayerInitial() {

      // *
      // * Inventory
      // *
      inventory = GetComponent<PlayerInventory>();

      List<Item> toEquip = inventory.Init(); // items need to be initialized before they are equipped
      
      // *
      // * Create the ship and equip it
      // *
      ship = inventory.EquipShip(spawnPoint.position, spawnPoint.rotation).GetComponent<PlayerFighter>();

      for (int i = 0; i < toEquip.Count; i++) {
         Item itemToEquip = toEquip[i];
         inventory.EquipItem(itemToEquip);
      }

      // *
      // * Camera
      // *
      // playerCamera = GameObject.Instantiate(playerCameraPrefab, ship.transform.position, ship.transform.rotation, null);
   //   UIController.instance.GetComponent<Canvas>().worldCamera = playerCamera.GetComponentInChildren<Camera>();
   //   UIController.instance.GetComponent<Canvas>().planeDistance = 1;
      cameraController.MainCamera = camera.GetComponentInChildren<Camera>();
      cameraController.CameraTransform = camera.transform;
      camera = camera.GetComponent<PlayerCamera>();
      camera.target = ship.transform; // Camera needs to be reset to new player fighter
   }

   public void SpawnPlayer() {
      // *
      // * Create the ship and equip it
      // *
      var ship = inventory.EquipShip(spawnPoint.position, spawnPoint.rotation);
   }

   public void SetSpawnPoint(Vector3 spawnPosition, Quaternion spawnRotation) {
      spawnPoint.position = spawnPosition;
      spawnPoint.rotation = spawnRotation;
   }

   public void RespawnAfter(float duration) {
      StartCoroutine(RespawnWait(duration));
   }

   private IEnumerator<WaitForSeconds> RespawnWait(float duration) {
      yield return new WaitForSeconds(duration);
      SpawnPlayer();
   }
}