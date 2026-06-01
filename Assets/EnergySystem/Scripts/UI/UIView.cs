using UnityEngine;

namespace EnergySystem.UI
{
    public abstract class UIView : MonoBehaviour
    {
        public virtual void Initialize() { }
        public virtual void Release() { }
    }

    public abstract class UIView<TVm> : UIView where TVm : class, IUIViewModel
    {
        protected TVm ViewModel { get; private set; }

        public void Initialize(TVm viewModel)
        {
            ViewModel = viewModel;
            OnInitialize();
        }

        protected virtual void OnInitialize() { }
        
        public override void Release()
        {
            ViewModel?.Dispose();
            ViewModel = null;
            base.Release();
        }
    }
}