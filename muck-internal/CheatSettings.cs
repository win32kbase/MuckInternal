using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MuckInternal
{
    public  class CheatSettings
    {
        public static bool MenuToggle { get; set; }
        public static bool OldCursorVisible { get; set; }
        public static CursorLockMode OldCursorLockMode { get; set; }

        public static Vector2 ScrollPosition { get; set; } = Vector2.zero;
        public static Rect WindowRect { get; set; } = new Rect(20, 20, 570, 400);

        public static bool GodMode { get; set; }
        public static readonly Rect GodModePosition = new Rect(10, 20, 120, 25);
        public static bool InfiniteStamina { get; set; }
        public static readonly Rect InfiniteStaminaPosition = new Rect(10, 40, 120, 25);

        public static bool InfiniteFood { get; set; }
        public static readonly Rect InfiniteFoodPosition = new Rect(10, 60, 120, 25);

        public static readonly Rect ItemSpawnerSliderPosition = new Rect(420, 33, 100, 20);
        public static readonly Rect ItemSpawnerDescPosition = new Rect(160, 28, 200, 20);
        public static readonly Rect ItemSpawnerCountPosition = new Rect(530, 28, 100, 25);
        public static readonly Rect ItemSpawnerScrollViewPosition = new Rect(150, 60, 500, 330);
        public static readonly Rect ItemSpawnerScrollViewBounds = new Rect(0, 0, 0, 15000);
        public static int ItemSpawnerAmount { get; set; } = 1;

        public static readonly Rect BreakAllTreesPosition = new Rect(10, 85, 130, 25);
        public static readonly Rect BreakAllRocksPosition = new Rect(10, 85 + 30, 130, 25);
        public static readonly Rect BreakAllResourcesPosition = new Rect(10, 85 + 60, 130, 25);
        public static readonly Rect BreakAllUserChestsPosition = new Rect(10, 85 + 90, 130, 25);
        public static readonly Rect BreakEverythingPosition = new Rect(10, 85 + 120, 130, 25);
        public static readonly Rect KillAllMobsPosition = new Rect(10, 85 + 150, 130, 25);
        public static readonly Rect UseAllShrinesPosition = new Rect(10, 85 + 180, 130, 25);

        public static readonly Rect UnloadCheatPosition = new Rect(10, 365, 130, 25);
    }

}
