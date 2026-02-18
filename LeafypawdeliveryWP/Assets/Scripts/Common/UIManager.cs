using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

using Factories;

namespace Common
{
    public class UIManager
    {
        private readonly UIFactory _uiFactory = null;
        
        // 현재 열려있는 UI들을 관리하는 리스트 (나중에 닫기/찾기 위해 필요)
        private readonly Dictionary<Type, object> _activePresenters = new();
        
        public UIManager(AddressableManager addressableManager)
        {
            _uiFactory = new(addressableManager);
        }
        
        public async UniTask<TPresenter> OpenAsync<TPresenter>() where TPresenter : class
        {
            var type = typeof(TPresenter);

            // 1. 이미 열려있다면 기존 것 반환 (중복 방지 옵션)
            if (_activePresenters.TryGetValue(type, out var activePresenter))
            {
                return activePresenter as TPresenter;
            }

            // 2. 팩토리를 통해 생성
            var presenter = await _uiFactory.CreateAsync<TPresenter>();
            if (presenter != null)
            {
                // 3. 관리 목록에 등록
                _activePresenters.Add(type, presenter);
                
                // (선택 사항) 만약 IPresenter 인터페이스가 있다면 Open() 자동 호출
                // (presenter as IPresenter)?.Open();
            }

            return presenter;
        }
    }
}

