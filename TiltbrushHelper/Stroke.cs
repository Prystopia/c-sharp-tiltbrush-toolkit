using System;

namespace TiltbrushToolkit
{
    public class Stroke
    {
        public enum StrokeMaskEnum
        {
            Flags = 1,
            FlagsAndScale = 3
        }
        public enum CPMaskEnum
        {
            TriggerPressureAndTimestamp = 3
        }
        public int BrushIndex { get; set; }
        public float[] BrushColor { get; set; }
        public float BrushSize { get; set; }
        public ControlPoint[] ControlPoints { get; set; }
        public StrokeMaskEnum StrokeMask { get; set; }
        public CPMaskEnum CPMask { get; set; }
        public int Flags { get; set; }
        public float Scale { get; set; }
        public Stroke()
        {
            BrushColor = new float[4];
        }
    }
}
