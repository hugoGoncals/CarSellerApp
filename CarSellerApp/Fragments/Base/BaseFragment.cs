using System.ComponentModel;
using Android.Views;
using CarSellerCore.ViewModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using Xamarin.Essentials;

namespace CarSellerApp.Fragments.Base;

public class BaseFragment<TViewModel> : AndroidX.Fragment.App.Fragment where TViewModel : BaseViewModel
    {
        private TViewModel? _viewModel;

        protected TViewModel ViewModel => _viewModel ??= Ioc.Default.GetService<TViewModel>();

        protected MainActivity? Parent => Platform.CurrentActivity as MainActivity;

        public override async void OnResume()
        {
            base.OnResume();

            ViewModel.PropertyChanged += OnPropertyChanged;
            await ViewModel.OnAppearing();
        }

        public override async void OnPause()
        {
            base.OnPause();

            ViewModel.PropertyChanged -= OnPropertyChanged;
        }

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);
            Parent.SetToolbarUI(ViewModel.Title, ViewModel.HasBackNavigation);
            //HasOptionsMenu = false;

            //ViewModel.InitializeViewModel();
            return view;
        }

        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }
    }