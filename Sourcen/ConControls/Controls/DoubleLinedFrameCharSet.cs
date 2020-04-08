namespace ConControls.Controls 
{
    /// <summary>
    /// A <see cref="FrameCharSet"/> for double-lined frames.
    /// </summary>
    public class DoubleLinedFrameCharSet : FrameCharSet
    {
        /// <summary>
        /// The upper left corner of a double-lined frame.
        /// </summary>
        public override char UpperLeft => (char)0x2554;
        /// <summary>
        /// The upper right corner of a double-lined frame.
        /// </summary>
        public override char UpperRight => (char)0x2557;
        /// <summary>
        /// The lower left corner of a double-lined frame.
        /// </summary>
        public override char LowerLeft => (char)0x255A;
        /// <summary>
        /// The lower right corner of a double-lined frame.
        /// </summary>
        public override char LowerRight => (char)0x255D;
        /// <summary>
        /// The horizontal line of a double-lined frame.
        /// </summary>
        public override char Horizontal => (char)0x2550;
        /// <summary>
        /// The vertical line of a double-lined frame.
        /// </summary>
        public override char Vertical => (char)0x2551;
    }
}
