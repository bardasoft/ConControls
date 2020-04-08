namespace ConControls.Controls 
{
    /// <summary>
    /// A <see cref="FrameCharSet"/> for bold-lined frames.
    /// </summary>
    public class BoldinedFrameCharSet : FrameCharSet
    {
        /// <summary>
        /// The upper left corner of a bold-lined frame.
        /// </summary>
        public override char UpperLeft => (char)0x250F;
        /// <summary>
        /// The upper right corner of a bold-lined frame.
        /// </summary>
        public override char UpperRight => (char)0x2513;
        /// <summary>
        /// The lower left corner of a bold-lined frame.
        /// </summary>
        public override char LowerLeft => (char)0x2517;
        /// <summary>
        /// The lower right corner of a bold-lined frame.
        /// </summary>
        public override char LowerRight => (char)0x251B;
        /// <summary>
        /// The horizontal line of a bold-lined frame.
        /// </summary>
        public override char Horizontal => (char)0x2501;
        /// <summary>
        /// The vertical line of a bold-lined frame.
        /// </summary>
        public override char Vertical => (char)0x2503;
    }
}
