// @MinoSpawnStop.cs
// @brief
// @author ICE
// @date 2023/09/06
// 
// @details

using System;
using UnityEngine;

namespace QBuild.Mino
{
    public class MinoSpawnStop : MonoBehaviour
    {
        public event Action<bool> OnStop;
        
        public void Stop()
        {
            OnStop?.Invoke(true);
        }

        public void Resume()
        {
            OnStop?.Invoke(false);
        }
    }
}