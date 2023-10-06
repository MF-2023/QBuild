using System;
using System.Collections.Generic;
using System.Linq;
using QBuild.Part.HolderView;
using VContainer;

namespace QBuild.Part.Presenter
{
    public class HolderPresenter
    {
        [Inject]
        public HolderPresenter(PartHolderView partHolderView)
        {
            _partHolderView = partHolderView;
        }


        public void Initialize()
        {
        }

        public void Bind(PartPlacer partPlacer)
        {
            _nextPartHolder.AddRange(partPlacer.NextPartHolders);

            for (var i = 0; i < _nextPartHolder.Count; i++)
            {
                _nextPartHolder[i].OnChangeParts += OnChangeParts(i);
                OnChangeParts(i, _nextPartHolder[i].GetParts());
            }

            partPlacer.OnSelectChangedEvent += OnSelectChanged;
            _partHolderView.SetScaleUp(partPlacer.CurrentSelectHolderIndex);
        }

        private Action<IEnumerable<BlockPartScriptableObject>> OnChangeParts(int holdersIndex)
        {
            return (blocks) => OnChangeParts(holdersIndex, blocks);
        }

        private void OnChangeParts(int holdersIndex, IEnumerable<BlockPartScriptableObject> parts)
        {
            _partHolderView.SetPartIcon(holdersIndex, _nextPartHolder[holdersIndex].GetParts().First().PartIcon);
        }

        private void OnSelectChanged(ChangeSelectEvent changeSelectEvent)
        {
            _partHolderView.SetScaleUp(changeSelectEvent.Index);
            _partHolderView.SetScaleDown(changeSelectEvent.PrevIndex);
        }

        private List<NextPartHolder> _nextPartHolder = new();
        private PartHolderView _partHolderView;
    }
}