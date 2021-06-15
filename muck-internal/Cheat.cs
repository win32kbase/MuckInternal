using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MuckInternal
{
    public class Cheat : MonoBehaviour
    {
        private ObjectCache<PlayerStatus> Statuses = new ObjectCache<PlayerStatus>();
        private ObjectCache<Camera> Camera = new ObjectCache<Camera>(5.0f, true);
        private ObjectCache<PowerupInventory> PowerupInventory = new ObjectCache<PowerupInventory>(5.0f, true);
        private ObjectCache<ItemManager> ItemManager = new ObjectCache<ItemManager>(5.0f, true);

        public void Start()
        {
            Statuses.Init(this);
            Camera.Init(this);
            PowerupInventory.Init(this);
            ItemManager.Init(this);
        }

        public void Update()
        {
            foreach (PlayerStatus Status in Statuses.Objects)
            {
                if (CheatSettings.GodMode)
                    Status.hp = Status.maxHp;
                if (CheatSettings.InfiniteStamina)
                    Status.stamina = Status.maxStamina;
                if (CheatSettings.InfiniteFood)
                    Status.hunger = Status.maxHunger;
            }

            if (Input.GetKeyUp(KeyCode.Insert))
            {
                CheatSettings.MenuToggle = !CheatSettings.MenuToggle;

                // so it doesn't make your cursor disappear in escape menus
                if (CheatSettings.MenuToggle)
                {
                    CheatSettings.OldCursorVisible = Cursor.visible;
                    CheatSettings.OldCursorLockMode = Cursor.lockState;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.visible = CheatSettings.OldCursorVisible;
                    Cursor.lockState = CheatSettings.OldCursorLockMode;
                }
            }
        }

        public void OnGUI()
        {
            if (!CheatSettings.MenuToggle)
                return;

            CheatSettings.WindowRect = GUI.Window(0, CheatSettings.WindowRect, RenderWindow, "Muck cheat [@IAmWolfie | github.com/win32kbase]");
        }
        public void RenderWindow(int windowID)
        {
            GUI.DragWindow(new Rect(0, 0, 0x1000, 30));

            CheatSettings.GodMode = GUI.Toggle(CheatSettings.GodModePosition, CheatSettings.GodMode, " God mode");
            CheatSettings.InfiniteStamina = GUI.Toggle(CheatSettings.InfiniteStaminaPosition, CheatSettings.InfiniteStamina, " Infinite stamina");
            CheatSettings.InfiniteFood = GUI.Toggle(CheatSettings.InfiniteFoodPosition, CheatSettings.InfiniteFood, " Infinite food");

            GUI.tooltip = "Item spawner - scroll for more";
            CheatSettings.ScrollPosition = GUI.BeginScrollView(CheatSettings.ItemSpawnerScrollViewPosition, CheatSettings.ScrollPosition, CheatSettings.ItemSpawnerScrollViewBounds, false, true);

            int x = 0;
            int y = 0;

            for (int i = 0; i < ItemManager.Object.allItems.Count; i++)
            {
                InventoryItem item = ItemManager.Object.allItems[i];

                if (GUI.Button(new Rect(x, y, 50, 50), new GUIContent(item.sprite.texture, "Spawn " + item.name)))
                    ItemManager.Object.DropItemAtPosition(item.id, CheatSettings.ItemSpawnerAmount, Camera.Object.transform.position, ItemManager.Object.GetNextId());

                if (i != 0 && i % 7 == 0)
                { 
                    x = 0; y += 60; 
                }
                else
                    x += 60;
            }

            for (int i = 0; i < ItemManager.Object.allPowerups.Count; i++)
            {
                Powerup powerup = ItemManager.Object.allPowerups[i];

                if (GUI.Button(new Rect(x, y, 50, 50), new GUIContent(powerup.sprite.texture, "Spawn " + powerup.name)))
                    for (int j = 0; j < CheatSettings.ItemSpawnerAmount; j++)
                        PowerupInventory.Object.AddPowerup(powerup.name, powerup.id, ItemManager.Object.GetNextId());

                if (i != 0 && i % 7 == 0)
                {
                    x = 0; y += 60; 
                }
                else
                    x += 60;
            }

            GUI.EndScrollView();

            GUI.Label(CheatSettings.ItemSpawnerDescPosition, GUI.tooltip);
            CheatSettings.ItemSpawnerAmount = (int)GUI.HorizontalSlider(CheatSettings.ItemSpawnerSliderPosition, CheatSettings.ItemSpawnerAmount, 1.0f, 69.0f);
            GUI.Label(CheatSettings.ItemSpawnerCountPosition, CheatSettings.ItemSpawnerAmount.ToString() + "x");

            if (GUI.Button(CheatSettings.BreakAllTreesPosition, "Break all trees"))
                foreach (HitableTree Tree in GameObject.FindObjectsOfType<HitableTree>())
                    Tree.Hit(9999, 9999, 1, Vector3.zero);
            if (GUI.Button(CheatSettings.BreakAllRocksPosition, "Break all rocks"))
                foreach (HitableRock Rock in GameObject.FindObjectsOfType<HitableRock>())
                    Rock.Hit(9999, 9999, 1, Vector3.zero);
            if (GUI.Button(CheatSettings.BreakAllResourcesPosition, "Break all resources"))
                foreach (HitableResource Resource in GameObject.FindObjectsOfType<HitableResource>())
                    Resource.Hit(9999, 9999, 1, Vector3.zero);
            if (GUI.Button(CheatSettings.BreakAllUserChestsPosition, "Break user chests"))
                foreach (HitableChest Chest in GameObject.FindObjectsOfType<HitableChest>())
                    Chest.Hit(9999, 9999, 1, Vector3.zero);
            if (GUI.Button(CheatSettings.BreakEverythingPosition, "Break/kill everything"))
                foreach (Hitable Entity in GameObject.FindObjectsOfType<Hitable>())
                    Entity.Hit(9999, 9999, 1, Vector3.zero);
            if (GUI.Button(CheatSettings.KillAllMobsPosition, "Kill all mobs"))
                foreach (HitableMob Mob in GameObject.FindObjectsOfType<HitableMob>())
                    Mob.Hit(9999, 9999, 1, Vector3.zero);
            if (GUI.Button(CheatSettings.UseAllShrinesPosition, "Use all shrines"))
                foreach (ShrineInteractable Shrine in GameObject.FindObjectsOfType<ShrineInteractable>())
                    Shrine.Interact();

            if (GUI.Button(CheatSettings.UnloadCheatPosition, "Unload cheat"))
                GameObject.Destroy(Loader.MainGameObject);
        }
    }
}
