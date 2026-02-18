using UnityEngine;

using Common;
using Cysharp.Threading.Tasks;

namespace Features.Main
{
    public class MainView : Entity
    {
        public override async UniTask InitializeAsync()
        {
            await base.InitializeAsync();
        }
    }
}


