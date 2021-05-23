using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameBasic.EntityV1
{
    [System.Serializable]
    public class EntityPool
    {
        public const int BIT_SHIFT = 16;
        public const int ENTITY_ID_MASK = 0x0000ffff;
        public const int GROUP_ID_MASK = ENTITY_ID_MASK << BIT_SHIFT;

        public Transform root;

        public Dictionary<string, EntityGroup> groups;

        public void Init()
        {
            //groups = new List<EntityGroup>();
            groups = new Dictionary<string, EntityGroup>();
        }

        public void Init(GameObject[] prefabs, int groupSize = 0)
        {
            //groups = new List<EntityGroup>(prefabs.Length);

            groups = new Dictionary<string, EntityGroup>(prefabs.Length);
            for (int i = 0; i < prefabs.Length; i++)
                Add(prefabs[i], groupSize);
        }

        public EntityGroup Add(GameObject prefab, int size = 1)
        {
            EntityGroup result = null;
            groups.TryGetValue(prefab.name, out result);

            if(result == null)
            {
                int index = groups.Count;

                result = new EntityGroup();
                result.Init(this, index, prefab, size);
                groups.Add(prefab.name, result);
            }

            return result;
        }

        public EntityGroup GetGroup(string prefab)
        {
            return groups[prefab];
        }

        public EntityObj Get(string prefab, Transform parent = null)
        {
            return groups[prefab].Get(parent);
        }

        public EntityObj[] Get(string prefab, int quantity, Transform parent = null)
        {
            return groups[prefab].Get(quantity, parent);
        }

        /// <summary>
        /// Return object to pool, set object active to false
        /// </summary>
        /// <param name="entities"></param>
        public void Release(EntityObj[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                //Debug.Log("Release " + entities[i].id + ", " + i + "/" + entities.Length);
                entities[i].Release();
            }
        }

        // Factory
        public static EntityPool Create(Transform spawnRoot, GameObject[] prefab, int size)
        {
            EntityPool result = new EntityPool();
            result.root = spawnRoot;
            result.Init(prefab, size);
            return result;
        }
    }
}