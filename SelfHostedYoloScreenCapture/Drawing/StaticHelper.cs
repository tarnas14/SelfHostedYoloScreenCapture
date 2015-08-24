namespace SelfHostedYoloScreenCapture.Drawing
{
    using System;
    using System.Drawing;

    class StaticHelper
    {
        public static Rectangle GetRectangle(Point startLocation, Point endLocation)
        {
            var startX = Math.Min(startLocation.X, endLocation.X);
            var startY = Math.Min(startLocation.Y, endLocation.Y);
            var endX = Math.Max(startLocation.X, endLocation.X);
            var endY = Math.Max(startLocation.Y, endLocation.Y);

            return new Rectangle(new Point(startX, startY), new Size(endX - startX, endY - startY));
        }

        public static Rectangle Contain(Rectangle one, Rectangle two)
        {
            var left = one.Left < two.Left ? one.Left : two.Left;
            var top = one.Top < two.Top ? one.Top : two.Top;
            var right = one.Right > two.Right ? one.Right : two.Right;
            var bottom = one.Bottom > two.Bottom ? one.Bottom : two.Bottom;

            return new Rectangle(new Point(left, top), new Size(right - left, bottom - top));
        }

        public static Rectangle Inflate(Rectangle rectangle, int margin)
        {
            return new Rectangle(new Point(rectangle.Left - margin, rectangle.Top - margin), new Size(rectangle.Width + 2*margin, rectangle.Height + 2*margin));            
        }
    }
}
