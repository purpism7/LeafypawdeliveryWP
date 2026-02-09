using UnityEngine;

using Common;

public interface IPresenter
{
    
}

public abstract class BasePresenter<TView, TModel> where TView : Entity where TModel : class
{
    protected TView _view = null;
    protected TModel _model = null;

    public BasePresenter(TView view, TModel model)
    {
        _view = view;
        _model = model;
    }

    //public virtual void Initialize(TView view, TModel model)
    //{
    //    _view = view;
    //    _model = model;
    //}
}
