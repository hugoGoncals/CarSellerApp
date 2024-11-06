using Android.Views;
using AndroidX.Navigation;
using CarSellerApp.Fragments.Base;
using CarSellerCore.ViewModel.Dashboard;

namespace CarSellerApp.Fragments;

public class DashboardFragment : BaseFragment<DashboardViewModel>
{
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        // Inflate the layout for this fragment
        View view = inflater.Inflate(Resource.Layout.dashboard, container, false);

        // Get the button and set up navigation
        Button button = view.FindViewById<Button>(Resource.Id.navigate_button);
        button.Text = ViewModel.Title;
        button.Click += (sender, e) => {
            //Navigation.FindNavController(view).Navigate(Resource.Id.action_firstFragment_to_secondFragment);
        };

        return view;
    }
}