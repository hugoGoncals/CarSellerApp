using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using CarSellerApp.Adapter;
using CarSellerCore.Model;
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
    public ConstraintLayout DetailLayout { get; private set; }
    public ShapeableImageView PhotoPlaceholder { get; private set; }
    
    
    public TextView MakerLabel { get; private set; }
    public TextView ModelLabel { get; private set; }
    public TextView YearLabel { get; private set; }
    public TextView MileageLabel { get; private set; }
    public TextView AuctionLabel { get; private set; }

    public CarViewHolder(View itemView, ICarAdapterListenner listenner) : base(itemView)
    {
        _listenner = listenner;
        PhotoLayout = itemView.FindViewById<ConstraintLayout>(Resource.Id.photo_layout);
        DetailLayout = itemView.FindViewById<ConstraintLayout>(Resource.Id.detailLayout);
        PhotoPlaceholder = itemView.FindViewById<ShapeableImageView>(Resource.Id.goal_image);
        MakerLabel = itemView.FindViewById<TextView>(Resource.Id.maker_label);
        ModelLabel = itemView.FindViewById<TextView>(Resource.Id.model_label);
        YearLabel = itemView.FindViewById<TextView>(Resource.Id.year_label);
        MileageLabel = itemView.FindViewById<TextView>(Resource.Id.mileage_label);
        AuctionLabel = itemView.FindViewById<TextView>(Resource.Id.auction_start_label);
        
        PhotoLayout.Click += PhotoLayoutOnClick;
        DetailLayout.Click += DetailLayoutOnClick;
    }

    private void DetailLayoutOnClick(object? sender, EventArgs e)
    {
        _listenner.NavigateToDetails(_car.Id);
    }

    private void PhotoLayoutOnClick(object? sender, EventArgs e)
    {
        _listenner.OnPhotoAdded(_car.Id);
    }

    public void Bind(Car car)
    {
        _car = car;
        MakerLabel.Text = car.Make;
        ModelLabel.Text = car.Model;
        YearLabel.Text = car.Year.ToString();
        MileageLabel.Text = car.Mileage.ToString();
        AuctionLabel.Text = car.AuctionDateTime;
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
    }
    
    private void StartCountdown(string auctionDateTime)
    {
        // Parse the auction end time
        auctionDateTime = "2024/11/15 09:00:00";
        DateTime auctionEndTime = DateTime.ParseExact(auctionDateTime, "yyyy/MM/dd HH:mm:ss", null);
        long millisInFuture = (long)(auctionEndTime - DateTime.Now).TotalMilliseconds;

        // Stop any existing countdown
        StopCountdown();

        // Only start countdown if there's time remaining
        if (millisInFuture > 0)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            RunCountdown(millisInFuture, _cancellationTokenSource.Token);
        }
        else
        {
            AuctionLabel.Text = "Auction Ended";
        }
    }

    private async void RunCountdown(long millisInFuture, CancellationToken cancellationToken)
    {
        while (millisInFuture > 0 && !cancellationToken.IsCancellationRequested)
        {
            // Update the remaining time
            TimeSpan timeRemaining = TimeSpan.FromMilliseconds(millisInFuture);
            AuctionLabel.Text = $"{timeRemaining.Hours:D2}:{timeRemaining.Minutes:D2}:{timeRemaining.Seconds:D2}";

            await Task.Delay(1000); // Wait for 1 second

            // Decrement remaining time by 1 second
            millisInFuture -= 1000;
        }

        if (!cancellationToken.IsCancellationRequested)
        {
            AuctionLabel.Text = "Auction Ended";
        }
    }

    public void StopCountdown()
    {
        // Cancel the countdown if it's running
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = null;
    }

}