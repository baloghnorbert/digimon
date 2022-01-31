﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Backend.Core.Helper
{
    public static class ImageCompressor
    {
        public static byte[] ScaleImage(Image image, int maxWidth, int maxHeight, bool padImage)
        {
            try
            {
                int newWidth;
                int newHeight;
                byte[] returnArray;

                //check if the image needs rotating (eg phone held vertical when taking a picture for example)
                foreach (PropertyItem prop in image.PropertyItems)
                {
                    if (prop.Id == 0x0112)
                    {
                        int rotateValue = image.GetPropertyItem(prop.Id).Value[0];
                        RotateFlipType flipType = GetRotateFlipType(rotateValue);
                        image.RotateFlip(flipType);
                        break;
                    }
                }

                //apply padding if needed
                if (padImage == true)
                {
                    image = ApplyPaddingToImage(image);
                }

                //check if the with or height of the image exceeds the maximum specified, if so calculate the new dimensions
                if (image.Width > maxWidth || image.Height > maxHeight)
                {
                    double ratioX = (double)maxWidth / image.Width;
                    double ratioY = (double)maxHeight / image.Height;
                    double ratio = Math.Min(ratioX, ratioY);

                    newWidth = (int)(image.Width * ratio);
                    newHeight = (int)(image.Height * ratio);
                }
                else
                {
                    newWidth = image.Width;
                    newHeight = image.Height;
                }

                //start with a new image
                Bitmap newImage = new Bitmap(newWidth, newHeight);

                //set the new resolution, 72 is usually good enough for displaying images on monitors
                newImage.SetResolution(180, 180);
                //or use the original resolution
                //newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                //resize the image
                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                }
                image = newImage;

                //save the image to a memorystream to apply the compression level, higher compression = better quality = bigger images
                using (MemoryStream ms = new MemoryStream())
                {
                    EncoderParameters encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);
                    image.Save(ms, GetEncoderInfo("image/jpeg"), encoderParameters);

                    //save the stream as byte array
                    returnArray = ms.ToArray();
                }

                //cleanup
                image.Dispose();
                newImage.Dispose();

                return returnArray;
            }
            catch (Exception ex)
            {
                return ex == null ? throw new Exception(ex.Message) : null;
            }
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType.ToLower() == mimeType.ToLower())
                {
                    return encoders[j];
                }
            }
            return null;
        }

        private static Image ApplyPaddingToImage(Image image)
        {
            //get the maximum size of the image dimensions
            int maxSize = Math.Max(image.Height, image.Width);
            Size squareSize = new Size(maxSize, maxSize);

            //create a new square image
            Bitmap squareImage = new Bitmap(squareSize.Width, squareSize.Height);

            using (Graphics graphics = Graphics.FromImage(squareImage))
            {
                //fill the new square with a color
                graphics.FillRectangle(Brushes.Red, 0, 0, squareSize.Width, squareSize.Height);

                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                //put the original image on top of the new square
                graphics.DrawImage(image, (squareSize.Width / 2) - (image.Width / 2), (squareSize.Height / 2) - (image.Height / 2), image.Width, image.Height);
            }

            return squareImage;
        }

        private static RotateFlipType GetRotateFlipType(int rotateValue)
        {
            RotateFlipType flipType = RotateFlipType.RotateNoneFlipNone;

            switch (rotateValue)
            {
                case 1:
                    flipType = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 2:
                    flipType = RotateFlipType.RotateNoneFlipX;
                    break;
                case 3:
                    flipType = RotateFlipType.Rotate180FlipNone;
                    break;
                case 4:
                    flipType = RotateFlipType.Rotate180FlipX;
                    break;
                case 5:
                    flipType = RotateFlipType.Rotate90FlipX;
                    break;
                case 6:
                    flipType = RotateFlipType.Rotate90FlipNone;
                    break;
                case 7:
                    flipType = RotateFlipType.Rotate270FlipX;
                    break;
                case 8:
                    flipType = RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    flipType = RotateFlipType.RotateNoneFlipNone;
                    break;
            }

            return flipType;
        }
    }
}
