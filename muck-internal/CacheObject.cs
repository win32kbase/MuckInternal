using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MuckInternal
{
    class ObjectCache<T> where T : UnityEngine.Object
    {
        public float UpdateInterval { get; private set; }
        public T[] Objects { get; private set; }
        public T Object { get; private set; }
        public bool Single = false;

        public ObjectCache(float updateInterval = 5.0f, bool single = false)
        {
            UpdateInterval = updateInterval;
            Single = single;
        }

        public IEnumerator Update()
        {
            while (true)
            {
                if (Single)
                    Object = GameObject.FindObjectOfType<T>();
                else
                    Objects = GameObject.FindObjectsOfType<T>();

                yield return new WaitForSeconds(UpdateInterval);
            }
        }

        public void Init(MonoBehaviour self)
        {
            self.StartCoroutine(this.Update());
        }
    }
}
