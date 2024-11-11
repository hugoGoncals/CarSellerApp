using CarSellerApp.Views;
using CarSellerCore.Services.Abstraction;
using Xamarin.Essentials;

namespace CarSellerApp.Services;

public class DialogService : IDialogService
{
    private LoadingDialog _loadingDialog;

    private Activity CurrentActivity => Platform.CurrentActivity;

    private bool IsActivityAvailable => CurrentActivity != null && !CurrentActivity.IsFinishing;
    
    public void ShowLoading()
    {
        if (!IsActivityAvailable)
        {
            return;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (_loadingDialog != null && _loadingDialog.IsShowing)
            {
                return;
            }

            _loadingDialog ??= new LoadingDialog(CurrentActivity);

            _loadingDialog.Show();
        });
    }
    
    public void HideLoading()
    {
        if (_loadingDialog == null)
        {
            return;
        }

        if (IsActivityAvailable)
        {
            _loadingDialog.Dismiss();
        }

        _loadingDialog = null;
    }
}