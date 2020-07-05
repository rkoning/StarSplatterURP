// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// using AI.Factions;

// public class PlayerLoader : MonoBehaviour
// {

//     public static PlayerLoader instance;

//     public GameObject playerCameraPrefab;

//     public Vector3 spawnPosition;
//     public Vector3 spawnEulerRotation;

//     public MajorFaction faction;

//     private GameObject playerCamera;

//     private Player player;

//     void Awake() {
//         instance = this;
//         this.player = GetComponent<Player>();
//         if (faction == null) {
//             // TODO: Fix the way this is loaded
//             // faction = FindObjectOfType<FactionManager>().GetPlayerFaction();
//         }
//         SpawnPlayerInitial();
//     }

//     // TODO: wacky shit ahead / unsure how the player will enter the level in a real game
//     public void SpawnPlayerInitial() {

//         // *
//         // * Inventory
//         // *
//         PlayerInventory inventory = GetComponent<PlayerInventory>();
//         player.inventory = inventory;
//         inventory.player = player;

//         List<Item> toEquip = inventory.Init(); // items need to be initialized before they are equipped
        
//         // *
//         // * Create the ship and equip it
//         // *
//         var shipGameObject = inventory.EquipShip(spawnPosition, Quaternion.Euler(spawnEulerRotation), faction);

//         for (int i = 0; i < toEquip.Count; i++) {
//             Item itemToEquip = toEquip[i];
//             inventory.EquipItem(itemToEquip);
//         }

//         // *
//         // * Camera
//         // *
//         playerCamera = GameObject.Instantiate(playerCameraPrefab, shipGameObject.transform.position, shipGameObject.transform.rotation, null);
//         UIController.instance.GetComponent<Canvas>().worldCamera = playerCamera.GetComponentInChildren<Camera>();
//         UIController.instance.GetComponent<Canvas>().planeDistance = 1;
//         player.cameraController.MainCamera = playerCamera.GetComponentInChildren<Camera>();
//         player.cameraController.CameraTransform = playerCamera.transform;
//         player.cam = playerCamera.GetComponent<PlayerCamera>();
//         player.cam.Target = shipGameObject.transform; // Camera needs to be reset to new player fighter
//     }

//     public void SpawnPlayer() {
//         // *
//         // * Create the ship and equip it
//         // *
//         var shipGameObject = player.inventory.EquipShip(spawnPosition, Quaternion.Euler(spawnEulerRotation), faction);
//     }

//     public void SetSpawnPoint(Vector3 spawnPoint, Quaternion spawnRotation) {
//         spawnPosition = spawnPoint;
//         spawnEulerRotation = spawnRotation.eulerAngles;
//     }

//     public void RespawnAfter(float duration) {
//         StartCoroutine(RespawnWait(duration));
//     }

//     private IEnumerator RespawnWait(float duration) {
//         yield return new WaitForSeconds(duration);
//         SpawnPlayer();
//     }
// }
