using System.ComponentModel;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Navigation;
using AndroidX.RecyclerView.Widget;
using CarSellerApp.Adapter;
using CarSellerApp.Fragments.Base;
using CarSellerCore.ViewModel.Dashboard;

namespace CarSellerApp.Fragments;

public class DashboardFragment : BaseFragment<DashboardViewModel>
{
    private CarAdapter _carAdapter;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        // Inflate the layout for this fragment
        View view = inflater.Inflate(Resource.Layout.dashboard, container, false);
        var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

        var layoutManager = new LinearLayoutManager(view.Context);
        _carAdapter = new CarAdapter();
        recyclerView.SetLayoutManager(layoutManager);
        var itemDecor = new DividerItemDecoration(view.Context, layoutManager.Orientation);
        recyclerView.AddItemDecoration(itemDecor);
        recyclerView.SetAdapter(_carAdapter);
        
        return view;
    }

    protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(sender, e);
        switch (e.PropertyName)
        {
            case nameof(ViewModel.CarsList):
                _carAdapter.CarList = ViewModel.CarsList;
                break;
        }
    }
}