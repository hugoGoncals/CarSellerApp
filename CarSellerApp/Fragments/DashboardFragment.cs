using System.ComponentModel;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using CAPMobile.Droid.ItemDecoration;
using CarSellerApp.Adapter;
using CarSellerApp.Fragments.Base;
using CarSellerCore.ViewModel.Dashboard;

namespace CarSellerApp.Fragments;

public class DashboardFragment : BaseFragment<DashboardViewModel>, ICarAdapterListenner
{
    private CarAdapter _carAdapter;
    private ImageView _sortByImage;
    private ImageView _previousPage;
    private ImageView _nextPage;
    private TextView _pageIndicator;
    private RecyclerView _recyclerView;
    private Spinner _spinnerPage;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        // Inflate the layout for this fragment
        View view = inflater.Inflate(Resource.Layout.dashboard, container, false);
        _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
        _sortByImage = view.FindViewById<ImageView>(Resource.Id.sortBy);
        _previousPage = view.FindViewById<ImageView>(Resource.Id.leftOption);
        _nextPage = view.FindViewById<ImageView>(Resource.Id.rightOption);
        _pageIndicator = view.FindViewById<TextView>(Resource.Id.pageIndex);
        _spinnerPage = view.FindViewById<Spinner>(Resource.Id.page_limits);
        
        _sortByImage.Click += SortByImageOnClick;
        _previousPage.Click += PreviousPageOnClick;
        _nextPage.Click += NextPageOnClick;

        var layoutManager = new LinearLayoutManager(view.Context);
        _carAdapter = new CarAdapter(this);
        _recyclerView.SetLayoutManager(layoutManager);
        _recyclerView.AddItemDecoration(new CommonItemSpaceDecoration(Context, 0, 0, 20, 0));
        _recyclerView.SetAdapter(_carAdapter);

        _pageIndicator.Text = ViewModel.CurrentPage.ToString();
        
        var adapter = new ArrayAdapter<int>(view.Context, Android.Resource.Layout.SimpleSpinnerItem, ViewModel.PagesLimitPerPage);
        adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
        _spinnerPage.Adapter = adapter;
        
        int imageResId = ViewModel.Filters ? Resource.Drawable.ic_filters_on : Resource.Drawable.ic_filters_off;
        _sortByImage.SetImageResource(imageResId);

        // Handle selection events
        _spinnerPage.ItemSelected += SpinnerPageOnItemSelected;
        
        return view;
    }

    private void SpinnerPageOnItemSelected(object? sender, AdapterView.ItemSelectedEventArgs e)
    {
        ViewModel.OnPageLimitChanged(e.Position);
    }

    private void NextPageOnClick(object? sender, EventArgs e)
    {
        _recyclerView.ScrollToPosition(0); 
        ViewModel.NextPage();
    }

    private void PreviousPageOnClick(object? sender, EventArgs e)
    {
        _recyclerView.ScrollToPosition(0);
        ViewModel.PreviousPage();
    }

    private void SortByImageOnClick(object? sender, EventArgs e)
    {
        ViewModel.NavigateToFilters();
    }

    protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(sender, e);
        switch (e.PropertyName)
        {
            case nameof(ViewModel.DisplayList):
                _carAdapter.CarList = ViewModel.DisplayList;
                break;
            case nameof(ViewModel.CurrentPage):
                _pageIndicator.Text = ViewModel.CurrentPage.ToString();
                break;
        }
    }

    public void OnPhotoAdded(int id) => ViewModel.OnAssociateImage(id);

    public void NavigateToDetails(int id) => ViewModel.NavigateToCarDetails(id);
    public void OnFavoriteClick(int carId, bool isFavorite) => ViewModel.OnFavorite(carId, isFavorite);
}