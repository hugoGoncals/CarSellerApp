using System.ComponentModel;
using Android.Views;
using CarSellerApp.Fragments.Base;
using CarSellerCore.ViewModel.Filter;
using Google.Android.Material.Chip;
using Google.Android.Material.Slider;

namespace CarSellerApp.Fragments;

public class FilterFragment : BaseFragment<FilterViewModel>
{
    private ChipGroup _sortLayout;
    private Slider _minValue;
    private TextView _startingBid;
    private Slider _maxValue;
    private Button _confirmOptions;
    private TextView _endBid;
    private AutoCompleteTextView _makerAutoComplete;
    private AutoCompleteTextView _modelAutoComplete;
    private Chip _isFavorite;

    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        base.OnCreateView(inflater, container, savedInstanceState);
        var view = inflater.Inflate(Resource.Layout.filter_layout, container, false);
        _sortLayout = view.FindViewById<ChipGroup>(Resource.Id.sortOptions);
        _minValue = view.FindViewById<Slider>(Resource.Id.priceRange);
        _maxValue = view.FindViewById<Slider>(Resource.Id.maxValue);
        _startingBid = view.FindViewById<TextView>(Resource.Id.startingBid);
        _endBid = view.FindViewById<TextView>(Resource.Id.maxValueLabel);
        _confirmOptions = view.FindViewById<Button>(Resource.Id.confirmOptions);
        _makerAutoComplete = view.FindViewById<AutoCompleteTextView>(Resource.Id.autoComplete);
        _modelAutoComplete = view.FindViewById<AutoCompleteTextView>(Resource.Id.modelAutoComplete);
        _isFavorite = view.FindViewById<Chip>(Resource.Id.isFavorite);

        _minValue.Touch += Slider_Touch;
        _maxValue.Touch += Slider_Touch;
        _confirmOptions.Click += ConfirmOptionsOnClick;
        _isFavorite.CheckedChange += IsFavoriteOnCheckedChange;
        
        _makerAutoComplete.ItemClick += MakerMakerAutoCompleteOnClick;
        _modelAutoComplete.ItemClick += ModelAutoCompleteOnClick;

        _startingBid.Text = ViewModel.StartingBidLabel;
        CreateSortLayout();
        return view;
    }

    private void IsFavoriteOnCheckedChange(object? sender, CompoundButton.CheckedChangeEventArgs e)
    {
        ViewModel.IsFavorite = e.IsChecked;
    }

    private void ModelAutoCompleteOnClick(object? sender, EventArgs e) => ViewModel.SelectedModel = _modelAutoComplete.Text;

    private void MakerMakerAutoCompleteOnClick(object? sender, EventArgs e)
    {
        _modelAutoComplete.Text = string.Empty;
        ViewModel.OnMakerSelected(_makerAutoComplete.Text);
    }

    private void ConfirmOptionsOnClick(object? sender, EventArgs e)
    {
        ViewModel.OnConfigurationsSetted();
    }

    protected override void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(sender, e);
        switch (e.PropertyName)
        {
            case nameof(ViewModel.PriceRange):
                _minValue.ValueFrom = ViewModel.PriceRange.minPrice;
                _minValue.ValueTo = ViewModel.PriceRange.maxPrice;
                _minValue.StepSize = 500;
                _minValue.Value = ViewModel.PriceRange.minPrice;
                _startingBid.Text = ViewModel.StartingBidLabel;
                
                _maxValue.ValueFrom = ViewModel.PriceRange.minPrice;
                _maxValue.ValueTo = ViewModel.PriceRange.maxPrice;
                _maxValue.StepSize = 500;
                _maxValue.Value = ViewModel.PriceRange.maxPrice;
                _endBid.Text = ViewModel.EndBidLabel;
                break;
            
            case nameof(ViewModel.StartingBidValue):
                _startingBid.Text = ViewModel.StartingBidLabel;
                break;
            
            case nameof(ViewModel.EndBidValue):
                _endBid.Text = ViewModel.EndBidLabel;
                break;
            
            case nameof(ViewModel.ModelOptions):
                ArrayAdapter<string> modelAdapter = new ArrayAdapter<string>(Context, Resource.Layout.dropdown_item, ViewModel.ModelOptions);
                _modelAutoComplete.Adapter = modelAdapter;
                break;
            
            case nameof(ViewModel.MakerOptions):
                ArrayAdapter<string> makerAdapter = new ArrayAdapter<string>(Context, Resource.Layout.dropdown_item, ViewModel.MakerOptions);
                _makerAutoComplete.Adapter = makerAdapter;
                break;
        }
    }

    private void CreateSortLayout()
    {
        _sortLayout.RemoveAllViews();
        for (var index = 0; index < ViewModel.SortOptions.Count; index++)
        {
            var option = ViewModel.SortOptions[index];
            var chip = new Chip(Context)
            {
                Checkable = true
            };

            var layoutParams = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);

            int leftMargin = index == 0 ? 0 : 20;
            layoutParams.SetMargins(leftMargin, 0, 0, 20);
            chip.LayoutParameters = layoutParams;

            int currentIndex = index;
            chip.CheckedChange += (sender, args) =>
            {
                ViewModel.OnSortByAdded(currentIndex);
            };
            
            chip.Text = option.Title;
            _sortLayout.AddView(chip);
        }
    }

    private void Slider_Touch(object sender, View.TouchEventArgs e)
    {
        if (e.Event.Action == MotionEventActions.Move || e.Event.Action == MotionEventActions.Up)
        {
            var slider = sender as Slider;
            if (slider != null)
            {
                // Update the specific property based on the slider being touched
                if (slider == _minValue)
                {
                    OnValueChange(slider.Value, true, isMinValue: true);
                }
                else if (slider == _maxValue)
                {
                    OnValueChange(slider.Value, true, isMinValue: false);
                }
            }
        }
        e.Handled = false;
    }
    
    private void OnValueChange(float value, bool fromUser, bool isMinValue)
    {
        if (isMinValue)
        {
            ViewModel.StartingBidValue = (int)value;
        }
        else
        {
            ViewModel.EndBidValue = (int)value;
        }
    }

}