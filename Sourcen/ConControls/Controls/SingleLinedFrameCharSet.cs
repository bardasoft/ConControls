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
        public override char UpperLeft => '┌';
        /// <summary>
        /// The upper right corner of a single-lined frame.
        /// </summary>
        public override char UpperRight => '┐';
        /// <summary>
        /// The lower left corner of a single-lined frame.
        /// </summary>
        public override char LowerLeft => '└';
        /// <summary>
        /// The lower right corner of a single-lined frame.
        /// </summary>
        public override char LowerRight => '┘';
        /// <summary>
        /// The horizontal line of a single-lined frame.
        /// </summary>
        public override char Horizontal => '─';
        /// <summary>
        /// The vertical line of a single-lined frame.
        /// </summary>
        public override char Vertical => '│';
        /// <summary>
        /// The cross of a single-lined frame.
        /// </summary>
        public override char Cross => '┼';
    }
}
