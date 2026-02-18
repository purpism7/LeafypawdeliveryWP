using UnityEngine;

using Common;

namespace Common
{
    public abstract class BasePresenter<TView, TModel> where TView : Entity where TModel : class
    {
        protected TView _view = null;
        protected TModel _model = null;

        protected BasePresenter(TView view, TModel model)
        {
            _view = view;
            _model = model;
        }
        
        // public virtual Open

        //public virtual void Initialize(TView view, TModel model)
        //{
        //    _view = view;
        //    _model = model;
        //}
    }
}

