namespace Artemis.Plugins.LayerBrushes.ConnectingDots.ConnectingDots
{
    public class Dot
    {
        public float X;
        public float Y;
        public float Xvel;
        public float Yvel;
        public float ColorPercentage;
        public float GetColorPercentageAndMove(float advance)
        {
            ColorPercentage += advance;
            return (ColorPercentage % 100)/100;

        }
    }
}
