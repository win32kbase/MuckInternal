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
        private ObjectCache<Camera> Camera = new ObjectCache<Camera>(single: true);
        private ObjectCache<PlayerMovement> PlayerMovement = new ObjectCache<PlayerMovement>(single: true);

        public void Start()
        {
            Statuses.Init(this);
            Camera.Init(this);
            PlayerMovement.Init(this);
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

            CheatSettings.WindowRect = GUI.Window(0, CheatSettings.WindowRect, RenderWindow, "Muck Internal: [@IAmWolfie | github.com/win32kbase]");
        }
        public void RenderWindow(int windowID)
        {
            ItemManager ItemManager = ItemManager.Instance;
            PowerupInventory PowerupInventory = PowerupInventory.Instance;

            GUI.DragWindow(new Rect(0, 0, 0x1000, 30));

            CheatSettings.GodMode = GUI.Toggle(CheatSettings.GodModePosition, CheatSettings.GodMode, " God mode");
            CheatSettings.InfiniteStamina = GUI.Toggle(CheatSettings.InfiniteStaminaPosition, CheatSettings.InfiniteStamina, " Infinite stamina");
            CheatSettings.InfiniteFood = GUI.Toggle(CheatSettings.InfiniteFoodPosition, CheatSettings.InfiniteFood, " Infinite food");

            GUI.tooltip = "Item spawner - scroll for more";
            CheatSettings.ScrollPosition = GUI.BeginScrollView(CheatSettings.ItemSpawnerScrollViewPosition, CheatSettings.ScrollPosition, CheatSettings.ItemSpawnerScrollViewBounds, false, true);

            int x = 0;
            int y = 0;

            for (int i = 0; i < ItemManager.allScriptableItems.Count(); i++)
            {
                InventoryItem item = ItemManager.allScriptableItems[i];

                if (GUI.Button(new Rect(x, y, 50, 50), new GUIContent(item.sprite.texture, "Spawn " + item.name)))
                    ClientSend.DropItem(item.id, CheatSettings.ItemSpawnerAmount);

                if (x == 360)   //Old method puts the 7th item offscreen. i % 7 for the 7th item would be 6 % 7 because i starts at 0.
                {
                    x = 0; y += 60;
                }
                else
                    x += 60;
            }

            x = 0;  // Starts power ups at new line 
            y += 60;

            for (int i = 0; i < ItemManager.allPowerups.Count(); i++)
            {
                Powerup powerup = ItemManager.allPowerups[i];

                if (GUI.Button(new Rect(x, y, 50, 50), new GUIContent(powerup.sprite.texture, "Spawn " + powerup.name)))
                    for (int j = 0; j < CheatSettings.ItemSpawnerAmount; j++)
                        PowerupInventory.AddPowerup(powerup.name, powerup.id, ItemManager.GetNextId());

                if (x == 360)   //see above.
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
                    Tree.Hit(9999, 9999, 1, Vector3.zero, 1);
            if (GUI.Button(CheatSettings.BreakAllRocksPosition, "Break all rocks"))
                foreach (HitableRock Rock in GameObject.FindObjectsOfType<HitableRock>())
                    Rock.Hit(9999, 9999, 1, Vector3.zero, 1);
            if (GUI.Button(CheatSettings.BreakAllResourcesPosition, "Break all resources"))
                foreach (HitableResource Resource in GameObject.FindObjectsOfType<HitableResource>())
                    Resource.Hit(9999, 9999, 1, Vector3.zero, 1);
            if (GUI.Button(CheatSettings.BreakAllUserChestsPosition, "Break user chests"))
                foreach (HitableChest Chest in GameObject.FindObjectsOfType<HitableChest>())
                    Chest.Hit(9999, 9999, 1, Vector3.zero, 1);
            if (GUI.Button(CheatSettings.BreakEverythingPosition, "Break/kill everything"))
                foreach (Hitable Entity in GameObject.FindObjectsOfType<Hitable>())
                    Entity.Hit(9999, 9999, 1, Vector3.zero);
            if (GUI.Button(CheatSettings.KillAllMobsPosition, "Kill all mobs"))
                foreach (HitableMob Mob in GameObject.FindObjectsOfType<HitableMob>())
                    Mob.Hit(9999, 9999, 1, Vector3.zero);
            if (GUI.Button(CheatSettings.UseAllShrinesPosition, "Use all shrines"))
                foreach (ShrineInteractable Shrine in GameObject.FindObjectsOfType<ShrineInteractable>())
                    Shrine.Interact();
            if (GUI.Button(CheatSettings.UseAllChestsPosition, "Use all chests"))
                foreach (LootContainerInteract Container in GameObject.FindObjectsOfType<LootContainerInteract>())
                    ClientSend.PickupInteract(Container.GetId());


            if (GUI.Button(CheatSettings.UnloadCheatPosition, "Unload cheat"))
            {
                Cursor.visible = CheatSettings.OldCursorVisible;
                Cursor.lockState = CheatSettings.OldCursorLockMode;
                GameObject.Destroy(Loader.MainGameObject);
            }
        }
    }
}
