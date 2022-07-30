namespace WalMan
{
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Advanced;
    using SixLabors.ImageSharp.Formats;
    using SixLabors.ImageSharp.Formats.Bmp;
    using SixLabors.ImageSharp.Processing;
    using System.IO;
    using System.Threading.Tasks;

    internal static class WalCreator
    {
        static Size desktopSize = Screen.PrimaryScreen.Bounds.Size.ToSize();
        static readonly int blurSigma = 16;

        public static async Task<Stream?> Create(string filePath)
        {
            try
            {
                Image originalImage = await Image.LoadAsync(filePath);
                Image resizedImage;

                ResizeOptions resizeOptions = new ResizeOptions
                {
                    Size = desktopSize,
                    Mode = ResizeMode.Max
                };

                if (originalImage.Width <= desktopSize.Width && originalImage.Height <= desktopSize.Height)
                    resizedImage = originalImage;
                else
                    resizedImage = originalImage.Clone(x => x.Resize(resizeOptions));

                if (resizedImage.Size() == desktopSize)
                    return await resizedImage.ToStream();

                resizeOptions.Mode = ResizeMode.Crop;
                Image bluredImage = originalImage.Clone(x => x.Resize(resizeOptions));
                bluredImage.Mutate(x => x.GaussianBlur(blurSigma));
                Size size = (bluredImage.Size() - resizedImage.Size()) / 2;
                bluredImage.Mutate(x => x.DrawImage(resizedImage, (Point)size, 1));
                return await bluredImage.ToStream();
            }
            catch
            {
                return null;
            }
        }

        static async Task<Stream> ToStream(this Image image)
        {
            MemoryStream memoryStream = new MemoryStream();
            IImageEncoder imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(BmpFormat.Instance);
            await image.SaveAsync(memoryStream, imageEncoder);
            memoryStream.Position = 0;
            return memoryStream;
        }

        public static Size ToSize(this System.Drawing.Size size)
        {
            return new Size(size.Width, size.Height);
        }
    }
}
