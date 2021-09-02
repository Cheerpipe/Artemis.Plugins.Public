
namespace Artemis.Plugins.LayerBrushes.DrippingPaint.LayerBrush.DrippingPaint
{
    public class DrippingPoint
    {
        public float XPosition { get; set; }
        public float YPosition { get; set; }
        public float Speed { get; set; } = 0.1f;
        public float Acceleration { get; set; }

        public void Fall(float deltaTime, float speedModifier = 100f)
        {
            Accelerate(deltaTime);
            YPosition = YPosition + (Speed * speedModifier / 100f);
        }

        private void Accelerate(float deltaTime)
        {
            Speed = Speed + (Acceleration * (deltaTime));
        }
    }
}
