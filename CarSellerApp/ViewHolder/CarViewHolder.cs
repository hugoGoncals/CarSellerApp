using System;
using System.Threading.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.ConstraintLayout.Widget;
using AndroidX.RecyclerView.Widget;
using Bumptech.Glide;
using CarSellerCore.Model;
using Google.Android.Material.ImageView;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Essentials;

namespace CarSellerApp.ViewHolder;

public class CarViewHolder : RecyclerView.ViewHolder
{
    private Car _car;
    
    public ConstraintLayout PhotoLayout { get; private set; }
    public ShapeableImageView PhotoPlaceholder { get; private set; }
    
    
    public TextView MakerLabel { get; private set; }
    public TextView ModelLabel { get; private set; }
    public TextView YearLabel { get; private set; }
    public TextView MileageLabel { get; private set; }
    public TextView AuctionLabel { get; private set; }

    public CarViewHolder(View itemView) : base(itemView)
    {
        PhotoLayout = itemView.FindViewById<ConstraintLayout>(Resource.Id.photo_layout);
        PhotoPlaceholder = itemView.FindViewById<ShapeableImageView>(Resource.Id.goal_image);
        MakerLabel = itemView.FindViewById<TextView>(Resource.Id.maker_label);
        ModelLabel = itemView.FindViewById<TextView>(Resource.Id.model_label);
        YearLabel = itemView.FindViewById<TextView>(Resource.Id.year_label);
        MileageLabel = itemView.FindViewById<TextView>(Resource.Id.mileage_label);
        AuctionLabel = itemView.FindViewById<TextView>(Resource.Id.auction_start_label);
        
        PhotoLayout.Click += PhotoLayoutOnClick;
    }

    private void PhotoLayoutOnClick(object? sender, EventArgs e)
    {
        AssociatePhotoAsync();
    }

    public void Bind(Car car)
    {
        _car = car;
        MakerLabel.Text = car.Make;
        ModelLabel.Text = car.Model;
        YearLabel.Text = car.Year.ToString();
        MileageLabel.Text = car.Mileage.ToString();
        AuctionLabel.Text = car.AuctionDateTime;
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
    
    public async Task AssociatePhotoAsync()
    {
        try
        {
            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                CompressionQuality = 50,
                PhotoSize = PhotoSize.MaxWidthHeight,
                SaveMetaData = false,
                MaxWidthHeight = 1500,
            });
            await LoadPhotoAsync(file, _car);
        }
        catch (Exception e)
        {
            if (e.Message.Contains("StoragePermission"))
            {
                return;
            }
        }
    }
    
    private async Task LoadPhotoAsync(MediaFile photo, Car car)
    {
        if (photo == null)
        {
            car.SelectedPhoto = null;
            return;
        }

        var stream = photo.GetStream();
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            car.SelectedPhoto = memoryStream.ToArray(); // Set the photo in the model
            UpdateImageLayout(car);
        }
    }


    private void UpdateImageLayout(Car car)
    {
        if (car.SelectedPhoto != null)
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

}