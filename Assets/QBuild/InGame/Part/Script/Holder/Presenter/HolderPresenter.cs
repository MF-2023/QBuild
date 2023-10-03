using System;
using System.Collections.Generic;
using QBuild.Part.HolderView;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace QBuild.Part
{
    public class HolderPresenter : IInitializable
    {
        [Inject]
        public HolderPresenter(NextPartHolder playerController, PartHolderView partHolderView)
        {
            _nextPartHolder = playerController;
            _partHolderView = partHolderView;
        }

        public void Initialize()
        {
            Bind();
        }

        private void Bind()
        {
            _nextPartHolder.OnChangeParts += OnChangeParts;
        }


        private void OnChangeParts(IEnumerable<BlockPartScriptableObject> parts)
        {
            Debug.Log("OnChangeParts");
            var index = 0;
            foreach (var part in parts)
            {
                _partHolderView.SetPartIcon(index, part.PartIcon);
                index++;
            }
        }

        private NextPartHolder _nextPartHolder;
        private PartHolderView _partHolderView;
    }
}