using UnityEngine;
using System.Collections;


namespace GameBasic
{
    public struct CountTrigger
    {
        public int maxLife;// = 1;
        public int life;

        /*
        public CountTrigger()
        {

        }
        */

        public CountTrigger(int maxLife, int life) {
            this.life = life;
            this.maxLife = maxLife;
        }

        /// <summary>
        /// Rest life to max life
        /// </summary>
        public void PutOn()
        {
            life = maxLife;
        }

        public void Cancel()
        {
            life = 0;
        }

        public void Kill()
        {
            life = 0;
        }

        public bool IsTriigerOn()
        {
            return life > 0 || IsInfinite();
        }

        /// <summary>
        /// comsume the trigger life by 1
        /// </summary>
        /// <returns></returns>
        public bool Perform()
        {
            if (IsInfinite())
                return true;

            if (life > 0)
            {
                life--;
                return true;
            }
            return false;
        }

        public bool IsInfinite()
        {
            return maxLife == 0;
        }

        public void OneTime(bool isTriggerOn = false)
        {
            maxLife = 1;
            life = 0;
            if (isTriggerOn)
                PutOn();
        }

        public void Infinite()
        {
            maxLife = 0;
            life = 0;
        }

        /// <summary>
        /// Create One Time trigger
        /// </summary>
        /// <param name="isTriggerOn"></param>
        /// <returns></returns>
        public static CountTrigger CreateOneTime(bool isTriggerOn = false)
        {
            CountTrigger result = new CountTrigger();
            result.OneTime(isTriggerOn);
            return result;
        }

        public static CountTrigger CreateInfinite()
        {
            CountTrigger result = new CountTrigger();
            result.OneTime();
            return result;
        }
    }
}