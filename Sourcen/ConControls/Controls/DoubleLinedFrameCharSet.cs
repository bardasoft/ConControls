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
        public override char UpperLeft => '╔';
        /// <summary>
        /// The upper right corner of a double-lined frame.
        /// </summary>
        public override char UpperRight => '╗';
        /// <summary>
        /// The lower left corner of a double-lined frame.
        /// </summary>
        public override char LowerLeft => '╚';
        /// <summary>
        /// The lower right corner of a double-lined frame.
        /// </summary>
        public override char LowerRight => '╝';
        /// <summary>
        /// The horizontal line of a double-lined frame.
        /// </summary>
        public override char Horizontal => '═';
        /// <summary>
        /// The vertical line of a double-lined frame.
        /// </summary>
        public override char Vertical => '║';
        /// <summary>
        /// The cross of a double-lined frame.
        /// </summary>
        public override char Cross => '╬';
    }
}
