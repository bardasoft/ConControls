namespace ConControls.Controls 
{
    /// <summary>
    /// Provides characters for drawing frames.
    /// </summary>
    public abstract class FrameCharSet
    {
        /*
        ┌──┬──┐  ╔══╦══╗ ╒══╤══╕ ╓──╥──╖
        │  │  │  ║  ║  ║ │  │  │ ║  ║  ║
        ├──┼──┤  ╠══╬══╣ ╞══╪══╡ ╟──╫──╢
        │  │  │  ║  ║  ║ │  │  │ ║  ║  ║
        └──┴──┘  ╚══╩══╝ ╘══╧══╛ ╙──╨──╜
         */

        /// <summary>
        /// The upper left corner of a frame.
        /// </summary>
        public abstract char UpperLeft { get; }
        /// <summary>
        /// The upper right corner of a frame.
        /// </summary>
        public abstract char UpperRight { get; }
        /// <summary>
        /// The lower left corner of a frame.
        /// </summary>
        public abstract char LowerLeft { get; }
        /// <summary>
        /// The lower right corner of a frame.
        /// </summary>
        public abstract char LowerRight { get; }
        /// <summary>
        /// The horizontal line of a frame.
        /// </summary>
        public abstract char Horizontal { get; }
        /// <summary>
        /// The vertical line of a frame.
        /// </summary>
        public abstract char Vertical { get; }
        /// <summary>
        /// The cross of a frame.
        /// </summary>
        public abstract char Cross { get; }
    }
}
