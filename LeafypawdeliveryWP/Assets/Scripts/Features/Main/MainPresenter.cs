using UnityEngine;

using Common;

namespace Features.Main
{
    public class MainPresenter : BasePresenter<MainView, MainModel>
    {
        public MainPresenter(MainView view, MainModel model) : base (view, model)
        {

        }

        //public override void Initialize(MainView view, MainModel model)
        //{
        //    base.Initialize(view, model);


        //}
    }
}

