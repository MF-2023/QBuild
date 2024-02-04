using UnityEngine;

namespace QBuild.SetupScript
{
    public class ShutdownSystem : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Quit");
                Application.Quit();
            }
        }
    }
}
