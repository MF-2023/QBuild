using VContainer;
using VContainer.Unity;

namespace QBuild.Part
{
    public class PlacePresenter : IInitializable
    {
        [Inject]
        public PlacePresenter(PartRepository partRepository)
        {
            //partPlacer.OnPlaceEvent += partRepository.AddPart;
        }
        
        
        public void Initialize()
        {
            
        }
    }
}