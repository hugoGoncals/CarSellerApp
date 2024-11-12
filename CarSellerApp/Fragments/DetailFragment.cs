using System.ComponentModel;
using Android.Views;
using Bumptech.Glide;
using CarSellerApp.Fragments.Base;
using CarSellerCore.ViewModel.Details;
using Google.Android.Flexbox;
using Google.Android.Material.Chip;

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
        base.OnCreateView(inflater, container, savedInstanceState);
        View view = inflater.Inflate(Resource.Layout.detail_page, container, false);
        
        _infoList = view.FindViewById<LinearLayout>(Resource.Id.detailsLayout);
        _makerLabel = view.FindViewById<TextView>(Resource.Id.makerLabel);
        _modelLabel = view.FindViewById<TextView>(Resource.Id.modelLabel);
        _carImage = view.FindViewById<ImageView>(Resource.Id.carImage);
        
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
        _infoList.RemoveAllViews();
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
            
            if (section.section == "Equipment")
            {
                CreateEquipmentSection(section.Item2);
                continue;
            }

            foreach (var detail in section.Item2)
            {
                CreateDataView(detail);
            }
        }
    }

    private void CreateEquipmentSection(List<(string title, string description)> sectionItem2)
    {
        FlexboxLayout chipsLayout = new FlexboxLayout(Context);
        var chipsParams = new LinearLayout.LayoutParams(
            ViewGroup.LayoutParams.MatchParent,
            ViewGroup.LayoutParams.WrapContent);
        chipsLayout.LayoutParameters = chipsParams;
        chipsLayout.FlexWrap = 1;
        chipsLayout.JustifyContent = 0;
        
        foreach (var option in sectionItem2)
        {
            var chip = new Chip(Context)
            {
                Checkable = false
            };

            var layoutParams = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);
            chip.LayoutParameters = layoutParams;
            
            chip.Text = option.description;
            chipsLayout.AddView(chip);
        }
        
        _infoList.AddView(chipsLayout);
    }

    private void CreateDataView((string title, string description) dataItem)
    {
        var dataView = LayoutInflater.From(_infoList.Context)?.Inflate(Resource.Layout.item_detail, _infoList, false);

        var titleView = dataView.FindViewById<TextView>(Resource.Id.item_title);
        var descriptionView = dataView.FindViewById<TextView>(Resource.Id.item_description);

        titleView.Text = dataItem.title;
        descriptionView.Text = dataItem.description;

        _infoList.AddView(dataView);
    }

}