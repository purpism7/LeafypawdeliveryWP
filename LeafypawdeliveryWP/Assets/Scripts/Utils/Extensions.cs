using UnityEngine;

namespace Util
{
    public static class Extensions
    {
        public static void SetActive(this Component component, bool active)
        {
            if (component == null)
                return;
            
            component.gameObject.SetActive(active);
        }
    }
}

