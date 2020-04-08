namespace ConControls.Controls 
{
    /// <summary>
    /// A <see cref="FrameCharSet"/> for single-lined frames.
    /// </summary>
    public class SingleLinedFrameCharSet : FrameCharSet
    {
        /// <summary>
        /// The upper left corner of a single-lined frame.
        /// </summary>
        public override char UpperLeft => (char)0x250C;
        /// <summary>
        /// The upper right corner of a single-lined frame.
        /// </summary>
        public override char UpperRight => (char)0x2510;
        /// <summary>
        /// The lower left corner of a single-lined frame.
        /// </summary>
        public override char LowerLeft => (char)0x2514;
        /// <summary>
        /// The lower right corner of a single-lined frame.
        /// </summary>
        public override char LowerRight => (char)0x2518;
        /// <summary>
        /// The horizontal line of a single-lined frame.
        /// </summary>
        public override char Horizontal => (char)0x2500;
        /// <summary>
        /// The vertical line of a single-lined frame.
        /// </summary>
        public override char Vertical => (char)0x2502;
    }
}
