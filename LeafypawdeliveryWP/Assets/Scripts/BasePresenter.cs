using UnityEngine;

public interface IPresenter
{
    
}

public abstract class BasePresenter<TView, TModel> where TView : class where TModel : class
{
    protected TView _view = null;
    protected TModel _model = null;

    public abstract void CreatePresenter();
}
