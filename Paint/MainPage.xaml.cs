using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
namespace DniOtwartePOv2
{
    public partial class MainPage : ContentPage
    {
        const string name = "myPaint";
        int counter = 0;

        public MainPage()
        {
            InitializeComponent();
            ElipseSize();
        }

       private void OnColor(object sender, EventArgs e)
        {
            Button pressed = (Button)sender;
            Color pickedColor = pressed.BackgroundColor;
            Canvas.LineColor = pickedColor;
            Elipse.Fill = pickedColor;
            Elipse.Stroke = pickedColor;
        }
       
        private void ElipseSize()
        {
            double width =  LineWidth.Value;
            Elipse.WidthRequest = width;
            double height = LineWidth.Value ;
            Elipse.HeightRequest = height;
        }


        private void OnSlide(object sender, ValueChangedEventArgs e)
        {
            ElipseSize();
            Canvas.LineWidth = (float)LineWidth.Value;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Canvas.Clear();
        }
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            
            var stream = await Canvas.GetImageStream(1024,1024);
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            stream.Position = 0;
            memoryStream.Position = 0;

            var context = Platform.CurrentActivity;
            Android.Content.ContentResolver resolver = context.ContentResolver;
            Android.Content.ContentValues contentValues = new();
            contentValues.Put(Android.Provider.MediaStore.IMediaColumns.DisplayName, $"{name}{counter}.png");
            contentValues.Put(Android.Provider.MediaStore.IMediaColumns.MimeType, "image/png" );
            contentValues.Put(Android.Provider.MediaStore.IMediaColumns.RelativePath, "DCIM/" + "myPaint");
            Android.Net.Uri imageUri = resolver.Insert(Android.Provider.MediaStore.Images.Media.ExternalContentUri, contentValues);
            var os = resolver.OpenOutputStream(imageUri);
            Android.Graphics.BitmapFactory.Options options = new();
            options.InJustDecodeBounds = true;
            var bitmap = Android.Graphics.BitmapFactory.DecodeStream(stream);
            bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png,100,os);
            os.Flush();
            os.Close();

        }
    }

}
