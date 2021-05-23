using UnityEngine;
using System.Collections;

namespace GameBasic
{
    public interface IProgress
    {
        int Total { get; }
        int Complete { get; }
    }
}