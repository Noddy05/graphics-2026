using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Graphics2026.Model.BufferObjects.UniformBufferObject
{
    internal class LightBlock : UBO
    {
        private Vector3 _lightFromDirection = Vector3.UnitY;
        public Vector3 lightFromDirection
        {
            get => _lightFromDirection;
            set
            {
                if (_lightFromDirection != value)
                {
                    _lightFromDirection = value;
                    LightDirectionUpdated();
                }
            }
        }
        private Color4 _lightColor = Color4.White;
        public Color4 lightColor
        {
            get => _lightColor;
            set
            {
                if (_lightColor != value)
                {
                    _lightColor = value;
                    LightColorUpdated();
                }
            }
        }
        private float _sunStrength = 1f;
        public float sunStrength
        {
            get => _sunStrength;
            set
            {
                if (_sunStrength != value)
                {
                    _sunStrength = value;
                    SunStrengthUpdated();
                }
            }
        }
        private Color4 _shadowColor = new Color4(47, 27, 194, 255);
        public Color4 shadowColor
        {
            get => _shadowColor;
            set
            {
                if (_shadowColor != value)
                {
                    _shadowColor = value;
                    ShadowColorUpdated();
                }
            }
        }
        private float _shadowLightStrength = 0.1f;
        public float shadowLightStrength
        {
            get => _shadowLightStrength;
            set
            {
                if (_shadowLightStrength != value)
                {
                    _shadowLightStrength = value;
                    ShadowLightStrengthUpdated();
                }
            }
        }

        public LightBlock(int bindingPoint) : base(bindingPoint) { }
        public LightBlock(int bindingPoint, BufferUsageHint hint) : base(bindingPoint, hint) { }

        private void LightDirectionUpdated() => BindSubData(DataConverter.ToFloatArray(lightFromDirection), 0);
        private void LightColorUpdated() => BindSubData(DataConverter.ToFloatArray(((Vector4)lightColor).Xyz), 4);
        private void SunStrengthUpdated() => BindSubData([ sunStrength ], 7);
        private void ShadowColorUpdated() => BindSubData(DataConverter.ToFloatArray(((Vector4)shadowColor).Xyz), 8);
        private void ShadowLightStrengthUpdated() => BindSubData([ shadowLightStrength ], 11);

        protected override float[] GetData()
            => DataConverter.ToFloatArray(lightFromDirection, 0f,
                ((Vector4)lightColor).Xyz, sunStrength,
                ((Vector4)shadowColor).Xyz, shadowLightStrength);

        protected override int NumFloats() => 12; //1 more for padding
    }
}
