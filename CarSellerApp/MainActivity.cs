using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.Navigation;
using AndroidX.Navigation.Fragment;
using CarSellerApp.Services;
using CommunityToolkit.Mvvm.DependencyInjection;
using Xamarin.Essentials;

namespace CarSellerApp;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : AppCompatActivity
{
    private TextView? _pageTitle;
    private ImageView? _backButton;
    private NavController? _navController;
    private ConstraintLayout? _toolbarLayout;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        DIContainer.Configure();
        Platform.Init(this, savedInstanceState);
        
        var navService = Ioc.Default.GetService<INavService>();
        navService?.Init(this);

        SetContentView(Resource.Layout.activity_main);

        _toolbarLayout = FindViewById<ConstraintLayout>(Resource.Id.toolbarLayout);
        _pageTitle = FindViewById<TextView>(Resource.Id.pageTitle);
        _backButton = FindViewById<ImageView>(Resource.Id.backButton);
        
        var navHostFragment = SupportFragmentManager.FindFragmentById(Resource.Id.my_nav_host_fragment) as NavHostFragment;
        _navController = navHostFragment?.NavController;

        _backButton.Click += BackButtonOnClick;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _backButton.Click += BackButtonOnClick;
    }

    private void BackButtonOnClick(object? sender, EventArgs e) => _navController?.NavigateUp();

    public void SetToolbarUI(string title, bool hasBackButton = true)
    {
        if (string.IsNullOrEmpty(title))
        {
            if (_toolbarLayout != null)
            {
                _toolbarLayout.Visibility = ViewStates.Gone;
            }
            return;
        }
        
        if (_pageTitle is null)
        {
            _toolbarLayout.Visibility = ViewStates.Gone;
            return;
        }
        _toolbarLayout.Visibility = ViewStates.Visible;
        _backButton.Visibility = hasBackButton ? ViewStates.Visible : ViewStates.Gone;
        _pageTitle.Text = title;
    }
}
