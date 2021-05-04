using UnityEngine;
using System.Collections;

namespace GameBasic.EntityV1
{
    public class EntityGroup
    {
        public EntityPool pool;
        public int typeId;
        public int index;
        public GameObject prefab;

        EntityObj[] entities;
        int available;
        //int[] objMask;

        // prefab data
        Vector3 position;
        Quaternion rotation;
        Vector3 localScale;

        public void Init(EntityPool pool, int index, GameObject prefab, int size)
        {
            this.pool = pool;
            this.index = index;
            this.typeId = index << EntityPool.BIT_SHIFT;

            this.prefab = prefab;
            SetInitTransform(prefab.transform);

            // clear old
            if (entities != null)
                Clear();

            // create object
            size = size < 1 ? 1 : size;
            entities = new EntityObj[size];
            Instantiate(entities);
        }

        /*
        public void Init()
        {
            SetInitTransform(prefab.transform);

            // clear old
            if (entities != null)
                Clear();

            // create object
            entities = new EntityObj[initialSize];
            Instantiate(entities);
        }
        */

        public void SetInitTransform(Transform transform)
        {
            this.position = transform.position;
            this.rotation = transform.rotation;
            this.localScale = transform.localScale;
        }

        void ResetTransform(GameObject obj)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.localScale = localScale;
        }

        public void EnusreCapacity(int size)
        {
            if (entities.Length < size)
            {
                EntityObj[] newDatas = new EntityObj[size];
                int oldLength = 0;

                // copy old to new
                if (entities != null)
                {
                    oldLength = entities.Length;
                    for (int i = 0; i < oldLength; i++)
                    {
                        newDatas[i] = entities[i];
                    }
                }

                Instantiate(newDatas, oldLength);
                entities = newDatas;
            }
        }

        void Instantiate(EntityObj[] arrays, int start = 0, int count = -1)
        {
            int length = count == -1 ? arrays.Length : count;
            available += length - start;
            for (int i = start; i < length; i++)
            {
                int entityId = typeId | i;
                arrays[i].Set(this, entityId, Object.Instantiate(prefab, pool.root));
                arrays[i].SetActive(false);
            }
        }

        private void Use(int index, Transform parent, bool active)
        {
            entities[index].used = true;
            if (parent != null)
            {
                entities[index].Transform.SetParent(parent);
                if (active)
                    entities[index].gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Get an available object, increase pool size(doubling) if no available object.
        /// </summary>
        /// <returns></returns>
        public EntityObj Get(Transform parent = null, bool active = true)
        {
            EntityObj result = new EntityObj();
            if (parent == null)
                parent = pool.root;

            // dynamic increase size by double
            if (available == 0)
            {
                int index = entities.Length;
                EnusreCapacity(entities.Length * 2);

                Use(index, parent, active);
                result = entities[index];
            }
            else
            {
                for (int i = 0; i < entities.Length; i++)
                {
                    if (!entities[i].used)
                    {
                        Use(i, parent, active);
                        result = entities[i];
                        break;
                    }
                }
            }

           //TODO: struct
           available--;

            return result;
        }

        public EntityObj[] Get(int count, Transform parent = null, bool active = true)
        {
            EntityObj[] result = new EntityObj[count];

            // no enough entity
            if (available < count)
            {
                int oldLength = entities.Length;
                int newLength = oldLength + (count - available);
                EnusreCapacity(newLength);
            }

            int index = 0;
            for (int i = 0; i < entities.Length; i++)
            {
                if (!entities[i].used)
                {
                    Use(i, parent, active);

                    result[index++] = entities[i];

                    if (index >= count)
                        break;
                }
            }

            available -= count;
            return result;
        }

        public void Release(int entityId)
        {
            int entityIndex = entityId & EntityPool.ENTITY_ID_MASK;
            if (entities[entityIndex].used)
            {
                available++;
                entities[entityIndex].used = false;
                entities[entityIndex].gameObject.transform.SetParent(pool.root);
                entities[entityIndex].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Return object to pool, set object active to false
        /// </summary>
        /// <param name="entity"></param>
        public void Release(EntityObj entity)
        {
            Release(entity.id);
        }

        public void Release(EntityObj[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                if(entities[i].used)
                    Release(entities[i].id);
            }
        }

        public void Clear()
        {
            if (entities != null)
            {
                for (int i = 0; i < entities.Length; i++)
                    Object.Destroy(entities[i].gameObject);

                entities = null;
                available = 0;
            }
        }
    }

}
