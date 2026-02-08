using UnityEngine;

using Common;

public class EntityFactory
{
    private readonly AddressableManager _addressableManager = null;
    
    public EntityFactory(AddressableManager addressableManager)
    {
        _addressableManager = addressableManager;
    }
    
    public void Create<TView, TPresenter, TModel>()
        where TView : Entity where TPresenter : BasePresenter<TView, TModel>, new() where TModel : class
    {
        var presenter = new TPresenter();
        presenter.CreatePresenter();
    }
}
