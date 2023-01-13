using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Threading.Tasks;

namespace WalMan
{
    internal class ImageToolbox
    {
        const int TaskbarHeight = 50;

        public static async Task<bool> IsLight(Stream stream)
        {
            Image<Rgb24> image = await Image.LoadAsync<Rgb24>(stream);
            int light = 0;

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = image.Height - TaskbarHeight; j < image.Height; j++)
                {
                    Rgb24 pixel = image[i, j];

                    if (pixel.R * 299 + pixel.G * 587 + pixel.B * 114 < 186000)
                        light++;
                }
            }

            if (light * 2 > image.Width * TaskbarHeight)
                return true;

            return false;
        }
    }
}
