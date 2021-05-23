using UnityEngine;
using System.Collections;

namespace GameBasic
{
    public interface ITask
    {
        /// <summary>
        /// Reture is running
        /// </summary>
        /// <returns></returns>
        bool OnUpdate();

        //bool IsSubmited();
        //void SetSubmit(bool submit);

        TaskStatus Status { get; set; }
    }
}