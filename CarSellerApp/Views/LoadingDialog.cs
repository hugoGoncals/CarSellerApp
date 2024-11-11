using Android.Content;

namespace CarSellerApp.Views;

public class LoadingDialog : Dialog
{
    public LoadingDialog(Context context)
        : base(context)
    {
        SetCancelable(false);
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);
        var prog = new ProgressBar(Context)
        {
            Indeterminate = true,
        };

        SetContentView(prog);
    }
}