using System;
using System.Threading.Tasks;
using Android.Animation;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using CarSellerApp.Adapter;
using CarSellerCore.Model;
using Google.Android.Material.Card;
using Google.Android.Material.ImageView;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;

namespace CarSellerApp.ViewHolder;

public class CarViewHolder : RecyclerView.ViewHolder
{
    private readonly ICarAdapterListenner _listenner;
    private Car _car;
    
    private CancellationTokenSource _cancellationTokenSource;
    
    public ConstraintLayout PhotoLayout { get; private set; }
    
    public MaterialCardView RootLayout { get; private set; }
    public ConstraintLayout DetailLayout { get; private set; }
    public ShapeableImageView PhotoPlaceholder { get; private set; }
    
    public ImageView FavoriteImage { get; private set; }
    
    public TextView MakerLabel { get; private set; }
    public TextView ModelLabel { get; private set; }
    public TextView YearLabel { get; private set; }
    public TextView MileageLabel { get; private set; }
    public TextView AuctionLabel { get; private set; }

    public CarViewHolder(View itemView, ICarAdapterListenner listenner) : base(itemView)
    {
        _listenner = listenner;
        RootLayout = itemView.FindViewById<MaterialCardView>(Resource.Id.rootLayout);
        PhotoLayout = itemView.FindViewById<ConstraintLayout>(Resource.Id.photo_layout);
        FavoriteImage = itemView.FindViewById<ImageView>(Resource.Id.favoriteImage);
        DetailLayout = itemView.FindViewById<ConstraintLayout>(Resource.Id.detailLayout);
        PhotoPlaceholder = itemView.FindViewById<ShapeableImageView>(Resource.Id.goal_image);
        MakerLabel = itemView.FindViewById<TextView>(Resource.Id.maker_label);
        ModelLabel = itemView.FindViewById<TextView>(Resource.Id.model_label);
        YearLabel = itemView.FindViewById<TextView>(Resource.Id.year_label);
        MileageLabel = itemView.FindViewById<TextView>(Resource.Id.mileage_label);
        AuctionLabel = itemView.FindViewById<TextView>(Resource.Id.auction_start_label);
        
        PhotoLayout.Click += PhotoLayoutOnClick;
        DetailLayout.Click += DetailLayoutOnClick;
        FavoriteImage.Click += FavoriteClick;
    }

    public void Bind(Car car)
    {
        _car = car;
        MakerLabel.Text = car.Make;
        ModelLabel.Text = car.Model;
        YearLabel.Text = car.Year.ToString();
        MileageLabel.Text = car.Mileage.ToString();
        AuctionLabel.Text = car.AuctionDateTime;
        DetailLayout.Clickable = true;
        RootLayout.Alpha = 1f;
        StartCountdown(car.AuctionDateTime);
        if (car.SelectedPhoto != null && car.SelectedPhoto.Length > 0)
        {
            Glide.With(PhotoPlaceholder.Context).Load(car.SelectedPhoto).Into(PhotoPlaceholder);
            PhotoPlaceholder.Visibility = ViewStates.Visible;
        }
        else
        {
            Glide.With(PhotoPlaceholder.Context).Clear(PhotoPlaceholder);
            PhotoPlaceholder.Visibility = ViewStates.Gone;
        }

        int imageResId = _car.Favourite ? Resource.Drawable.ic_favorite_on : Resource.Drawable.ic_favorite_off;
        FavoriteImage.SetImageResource(imageResId);
    }
    
    private void StartCountdown(string auctionDateTime)
    {
        //auctionDateTime = "2024/11/09 15:53:00";
        DateTime auctionEndTime = DateTime.ParseExact(auctionDateTime, "yyyy/MM/dd HH:mm:ss", null);
        long millisInFuture = (long)(auctionEndTime - DateTime.Now).TotalMilliseconds;

        StopCountdown();

        if (millisInFuture > 0)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            RunCountdown(millisInFuture, _cancellationTokenSource.Token);
        }
        else
        {
            AuctionLabel.Text = "Auction Ended";
            DetailLayout.Clickable = false;
            RootLayout.Alpha = 0.4f;
        }
    }

    private async void RunCountdown(long millisInFuture, CancellationToken cancellationToken)
    {
        while (millisInFuture > 0 && !cancellationToken.IsCancellationRequested)
        {
            TimeSpan timeRemaining = TimeSpan.FromMilliseconds(millisInFuture);
            AuctionLabel.Text = $"{timeRemaining.Hours:D2}:{timeRemaining.Minutes:D2}:{timeRemaining.Seconds:D2}";

            await Task.Delay(1000);

            millisInFuture -= 1000;
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            AuctionLabel.Text = "Auction Ended";
            DetailLayout.Clickable = false;
            RootLayout.Alpha = 0.4f;
        }
    }

    public void StopCountdown()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
    }
    
    private void FavoriteClick(object? sender, EventArgs e)
    {
        // Scale down animation to 0.7 over 100ms
        var scaleDownX = ObjectAnimator.OfFloat(FavoriteImage, "scaleX", 0.7f);
        var scaleDownY = ObjectAnimator.OfFloat(FavoriteImage, "scaleY", 0.7f);
        scaleDownX.SetDuration(100);
        scaleDownY.SetDuration(100);

        // Scale up animation back to 1 over 100ms
        var scaleUpX = ObjectAnimator.OfFloat(FavoriteImage, "scaleX", 1f);
        var scaleUpY = ObjectAnimator.OfFloat(FavoriteImage, "scaleY", 1f);
        scaleUpX.SetDuration(100);
        scaleUpY.SetDuration(100);
        
        // Set up AnimatorSet to play animations in sequence
        var animatorSet = new AnimatorSet();
        animatorSet.Play(scaleDownX).With(scaleDownY);
        animatorSet.Play(scaleUpX).With(scaleUpY).After(scaleDownX);

        // Start the animation
        animatorSet.Start();
        _listenner.OnFavoriteClick(_car.Id, !_car.Favourite);
    }

    private void DetailLayoutOnClick(object? sender, EventArgs e) => _listenner.NavigateToDetails(_car.Id);

    private void PhotoLayoutOnClick(object? sender, EventArgs e) => _listenner.OnPhotoAdded(_car.Id);
}