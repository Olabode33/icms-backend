using System.Threading.Tasks;
using ICMSDemo.Views;
using Xamarin.Forms;

namespace ICMSDemo.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
