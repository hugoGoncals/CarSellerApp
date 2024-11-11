using Android.Animation;
using Android.Views;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using CarSellerApp.Adapter;
using CarSellerCore.Model;
using Google.Android.Material.Card;
using Google.Android.Material.ImageView;

namespace CarSellerApp.ViewHolder;

public class CarViewHolder : RecyclerView.ViewHolder
{
    private readonly ICarAdapterListenner _listenner;
    private Car _car;
    
    private CancellationTokenSource _cancellationTokenSource;

    private ConstraintLayout PhotoLayout { get; }

    private MaterialCardView RootLayout { get; }

    private ConstraintLayout DetailLayout { get; }

    private ShapeableImageView PhotoPlaceholder { get; }

    private ImageView FavoriteImage { get; }

    private TextView MakerLabel { get; }

    private TextView ModelLabel { get; }

    private TextView YearValue { get; }

    private TextView MileageValue { get; }

    private TextView FuelValue { get; }

    private TextView BidingValue { get; }

    private TextView AuctionLabel { get; }

    public CarViewHolder(View itemView, ICarAdapterListenner listenner) : base(itemView)
    {
        _listenner = listenner;
        RootLayout = itemView.FindViewById<MaterialCardView>(Resource.Id.rootLayout);
        PhotoLayout = itemView.FindViewById<ConstraintLayout>(Resource.Id.photoLayout);
        FavoriteImage = itemView.FindViewById<ImageView>(Resource.Id.favoriteImage);
        DetailLayout = itemView.FindViewById<ConstraintLayout>(Resource.Id.detailLayout);
        PhotoPlaceholder = itemView.FindViewById<ShapeableImageView>(Resource.Id.carImage);
        MakerLabel = itemView.FindViewById<TextView>(Resource.Id.makerLabel);
        ModelLabel = itemView.FindViewById<TextView>(Resource.Id.modelLabel);
        YearValue = itemView.FindViewById<TextView>(Resource.Id.yearValue);
        MileageValue = itemView.FindViewById<TextView>(Resource.Id.mileageValue);
        FuelValue = itemView.FindViewById<TextView>(Resource.Id.fuelValue);
        BidingValue = itemView.FindViewById<TextView>(Resource.Id.bidingValue);
        AuctionLabel = itemView.FindViewById<TextView>(Resource.Id.auctionStartLabel);
        
        PhotoLayout.Click += PhotoLayoutOnClick;
        DetailLayout.Click += DetailLayoutOnClick;
        FavoriteImage.Click += FavoriteClick;
    }

    public void Bind(Car car)
    {
        _car = car;
        MakerLabel.Text = _car.Make;
        ModelLabel.Text = _car.Model;
        YearValue.Text = _car.Year.ToString();
        MileageValue.Text = $"{_car.Mileage} Kilometers";
        FuelValue.Text = _car.Fuel;
        BidingValue.Text = $"{_car.StartingBid}€"; 
        DetailLayout.Clickable = true;
        RootLayout.Alpha = 1f;
        StartCountdown(_car.AuctionDateTime);
        
        if (_car.SelectedPhoto != null && _car.SelectedPhoto.Length > 0)
        {
            Glide.With(PhotoPlaceholder.Context).Load(_car.SelectedPhoto).Into(PhotoPlaceholder);
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
        var auctionEndTime = DateTime.ParseExact(auctionDateTime, "yyyy/MM/dd HH:mm:ss", null);
        var millisInFuture = (long)(auctionEndTime - DateTime.Now).TotalMilliseconds;

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
            var timeRemaining = TimeSpan.FromMilliseconds(millisInFuture);
            var remainingDays = timeRemaining.Days % 7;
            AuctionLabel.Text = $"{remainingDays} {(remainingDays == 1 ? "Day" : "Days")} {timeRemaining.Hours:D2}:{timeRemaining.Minutes:D2}:{timeRemaining.Seconds:D2}";
            
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

    private void StopCountdown()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
    }
    
    private void FavoriteClick(object? sender, EventArgs e)
    {
        var scaleDownX = ObjectAnimator.OfFloat(FavoriteImage, "scaleX", 0.7f);
        var scaleDownY = ObjectAnimator.OfFloat(FavoriteImage, "scaleY", 0.7f);
        scaleDownX.SetDuration(100);
        scaleDownY.SetDuration(100);

        var scaleUpX = ObjectAnimator.OfFloat(FavoriteImage, "scaleX", 1f);
        var scaleUpY = ObjectAnimator.OfFloat(FavoriteImage, "scaleY", 1f);
        scaleUpX.SetDuration(100);
        scaleUpY.SetDuration(100);
        
        var animatorSet = new AnimatorSet();
        animatorSet.Play(scaleDownX).With(scaleDownY);
        animatorSet.Play(scaleUpX).With(scaleUpY).After(scaleDownX);

        animatorSet.Start();
        _listenner.OnFavoriteClick(_car.Id, !_car.Favourite);
    }

    private void DetailLayoutOnClick(object? sender, EventArgs e) => _listenner.NavigateToDetails(_car.Id);

    private void PhotoLayoutOnClick(object? sender, EventArgs e) => _listenner.OnPhotoAdded(_car.Id);
}