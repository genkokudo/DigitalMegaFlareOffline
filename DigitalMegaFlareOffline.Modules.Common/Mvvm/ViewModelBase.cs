using Prism.Mvvm;
using Prism.Navigation;

namespace DigitalMegaFlareOffline.Modules.Common.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        protected ViewModelBase()
        {

        }

        public virtual void Destroy()
        {

        }
    }
}
