using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MuckInternal
{
    public class Loader
    {
        public static void Init()
        {
            MainGameObject = new GameObject();
            MainGameObject.AddComponent<Cheat>();
            GameObject.DontDestroyOnLoad(MainGameObject);
        }

        public static GameObject MainGameObject;
    }
}
