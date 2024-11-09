using Android.Views;
using CarSellerApp.Fragments.Base;
using CarSellerCore.ViewModel.Details;

namespace CarSellerApp.Fragments;

public class DetailFragment : BaseFragment<DetailViewModel>
{
    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        ViewModel.CarId = Arguments?.GetInt("CarId");

        View view = inflater.Inflate(Resource.Layout.detail_page, container, false);
        return view;
    }
}