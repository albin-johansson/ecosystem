using UnityEngine;

namespace Ecosystem.UI
{
    public class HandlePostProcessing : MonoBehaviour
    {
        
        private void Start()
        {
            //ppProfile.GetComponent<AmbientOcclusion>().intensity = 0f;
            if (GraphicsSettings._postProcessingActive == 10)
            {
                print("hello");
            }

            Component[] components = gameObject.GetComponents<Component>();
            foreach(var component in components)
            {
                print(component.tag);
                print(component.name);
                print(component.gameObject);
            }
        }
    }
}
