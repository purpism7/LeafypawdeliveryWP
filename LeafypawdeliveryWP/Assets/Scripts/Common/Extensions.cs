using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common
{
    public static class Extensions 
    {
        public static void SetActive(this Component component, bool active)
        {
            if (component == null)
                return;

            SetActiveAsync(component, active).Forget();
        }

        private static async UniTask SetActiveAsync(this Component component, bool active)
        {
            await UniTask.Yield();
            
            component.gameObject.SetActive(active);
        }
        
        public static List<T> AddList<T, V>(this V[] arrays) where T : class
        {
            if (arrays == null)
                return null;
            
            var list = new List<T>();
            list.Clear();
            
            foreach (V t in arrays)
            {
                if(t == null)
                    continue;
                
                list.Add(t as T);
            }

            return list;
        }

        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            if (list == null)
                return true;

            if (list.Count <= 0)
                return true;

            return false;
        }
    
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            if (array == null)
                return true;
        
            if (array.Length <= 0)
                return true;

            return false;
        }
    
        public static bool IsNullOrEmpty<T>(this HashSet<T> hashSet)
        {
            if (hashSet == null)
                return true;

            if (hashSet.Count <= 0)
                return true;

            return false;
        }
    
        public static bool IsNullOrEmpty<T, V>(this Dictionary<T, V> dic)
        {
            if (dic == null)
                return true;

            if (dic.Count <= 0)
                return true;

            return false;
        }
    }
}

