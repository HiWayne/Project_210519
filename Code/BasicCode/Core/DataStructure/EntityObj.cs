using UnityEngine;

namespace GameBasic.EntityV1
{

    public struct EntityObj
    {
        #region Data
        public EntityGroup entityGroup;

        public int id;
        public bool used;
        public GameObject gameObject;
        #endregion

        public Transform Transform { get { return gameObject.transform; } }
        public int Index { get { return EntityPool.ENTITY_ID_MASK & id; } }
        public int GroupId { get { return id >> EntityPool.BIT_SHIFT;  } }

        public void Use()
        {
            used = true;
            //gameObject.SetActive(true);
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public void Release()
        {
            if(entityGroup != null)
                entityGroup.Release(this);
        }

        public void Set(EntityGroup group, int id, GameObject obj)
        {
            this.entityGroup = group;
            this.id = id;
            this.gameObject = obj;
        }

        public void Set(EntityObj entity)
        {
            entityGroup = entity.entityGroup;
            id = entity.id;
            gameObject = entity.gameObject;
        }

        public bool HasObject()
        {
            return gameObject != null;
        }
    }
}