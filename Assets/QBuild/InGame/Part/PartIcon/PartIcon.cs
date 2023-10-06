// @PartIcon.cs
// @brief
// @author ICE
// @date 2023/10/04
// 
// @details

using System;
using UnityEngine;

namespace QBuild.Part
{
    [Serializable]
    public class PartIcon
    {
        public Sprite Sprite => _sprite;
        [SerializeField] private Sprite _sprite;
    }
}