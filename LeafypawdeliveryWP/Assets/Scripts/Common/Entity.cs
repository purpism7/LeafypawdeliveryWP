
using UnityEngine;

using Util;

namespace Common
{
    public interface IEntity
    {
        
    }
    
    public class Entity : MonoBehaviour
    {
        [SerializeField] private Transform rootTr = null;

        public virtual void Initialize()
        {
            
        }
        
        public bool IsActive
        {
            get
            {
                return rootTr != null && rootTr.gameObject.activeSelf;
            }
        }

        // 함수 통합 및 확장 메서드 활용
        public void SetActive(bool active)
        {
            if (rootTr == null) 
                return;

            // 아까 만드신 확장 메서드를 '멤버 함수'처럼 호출합니다.
            rootTr.SetActive(active);
        }

        public void Activate() => SetActive(true);
        public void Deactivate() => SetActive(false);
    }
}
