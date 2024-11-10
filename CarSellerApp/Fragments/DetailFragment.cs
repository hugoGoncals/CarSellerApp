using System.ComponentModel;
using Android.Views;
using Bumptech.Glide;
using CarSellerApp.Fragments.Base;
using CarSellerCore.ViewModel.Details;

namespace CarSellerApp.Fragments;

public class DetailFragment : BaseFragment<DetailViewModel>
{
    private LinearLayout _infoList;
    private TextView _makerLabel;
    private TextView _modelLabel;
    private ImageView _carImage;

    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        ViewModel.CarId = Arguments?.GetInt("CarId");

        View view = inflater.Inflate(Resource.Layout.detail_page, container, false);
        
        _infoList = view.FindViewById<LinearLayout>(Resource.Id.detailsLayout);
        _makerLabel = view.FindViewById<TextView>(Resource.Id.makerLabel);
        _modelLabel = view.FindViewById<TextView>(Resource.Id.modelLabel);
        _carImage = view.FindViewById<ImageView>(Resource.Id.carImage);
        
        
        _infoList.RemoveAllViews();
        return view;
    }

    protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(sender, e);
        switch (e.PropertyName)
        {
            case nameof(ViewModel.CarDetailList):
                _makerLabel.Text = ViewModel.Car.Make;
                _modelLabel.Text = ViewModel.Car.Model;
                Glide.With(Context).Load(ViewModel.Car.SelectedPhoto).Into(_carImage);
                DisplayCarDetails(ViewModel.CarDetailList);
                break;
        }
    }
    
    private void DisplayCarDetails(List<(string section, List<(string title, string description)>)> carDetailList)
    {
        for (var index = 0; index < carDetailList.Count; index++)
        {
            var section = carDetailList[index];
            var sectionTitleTextView = new TextView(_infoList.Context)
            {
                Text = section.section,
            };

            var layoutParams = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.MatchParent,
                ViewGroup.LayoutParams.WrapContent);

            int topMargin = index == 0 ? 0 : 30;
            layoutParams.SetMargins(0, topMargin, 0, 20);
            sectionTitleTextView.LayoutParameters = layoutParams;

            sectionTitleTextView.SetTextAppearance(Resource.Style.SectionTitle);

            sectionTitleTextView.Text = section.section;
            _infoList.AddView(sectionTitleTextView);

            foreach (var detail in section.Item2)
            {
                CreateDataView(detail);
            }
        }
    }

    private void CreateDataView((string title, string description) dataItem)
    {
        var dataView = LayoutInflater.From(_infoList.Context)?.Inflate(Resource.Layout.item_detail, _infoList, false);

        var titleView = dataView.FindViewById<TextView>(Resource.Id.transaction_confirmation_item_title);
        var descriptionView = dataView.FindViewById<TextView>(Resource.Id.transaction_confirmation_item_description);

        titleView.Text = dataItem.title;
        descriptionView.Text = dataItem.description;

        _infoList.AddView(dataView);
    }

}