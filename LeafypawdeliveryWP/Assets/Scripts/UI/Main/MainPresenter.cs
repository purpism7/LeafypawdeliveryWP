using UI.Main;
using UnityEngine;

public interface IMainPresenter : IPresenter
{
    
}

public class MainPresenter : BasePresenter<MainView, MainModel>
{
    public override void CreatePresenter()
    {
        
    }
}
