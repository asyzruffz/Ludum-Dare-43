using System.Collections.Generic;
using UnityEngine;

namespace Ruffz.Utilities {

    public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour, IResetable {

        [SerializeField]
        int startWith = 0;
        [SerializeField]
        T prefab;

        public List<T> poolList = new List<T> ();

        protected virtual void Start () {
            for (int i = 0; i < startWith; i++) {
                CreateAnother ();
            }
        }

        public T GetAvailable () {
            for (int i = 0; i < poolList.Count; i++) {
                if (!poolList[i].gameObject.activeSelf) {
                    poolList[i].Reset ();
                    poolList[i].gameObject.SetActive (true);
                    return poolList[i];
                }
            }

            return CreateAnother (true);
        }

        T CreateAnother (bool activate = false) {
            T newObject = Instantiate (prefab, transform);
            newObject.gameObject.SetActive (activate);
            poolList.Add (newObject);
            return newObject;
        }
    }
}
