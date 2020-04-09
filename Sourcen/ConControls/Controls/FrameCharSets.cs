/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

namespace ConControls.Controls 
{
    /// <summary>
    /// Provides <see cref="FrameCharSet"/> instances for given <see cref="BorderStyle"/> values.
    /// </summary>
    public class FrameCharSets
    {
        static readonly FrameCharSet singleLined = new SingleLinedFrameCharSet();
        static readonly FrameCharSet doubleLined = new DoubleLinedFrameCharSet();
        static readonly FrameCharSet boldLined = new BoldinedFrameCharSet();

        /// <summary>
        /// Gets the <see cref="FrameCharSet"/> to use to draw a border in the given <paramref name="style"/>.
        /// </summary>
        /// <param name="style">The <see cref="BorderStyle"/> to get the <see cref="FrameCharSet"/> for.</param>
        /// <returns>A <see cref="FrameCharSet"/> that can provide the characters needed to draw the given <see cref="BorderStyle"/>.</returns>
        public virtual FrameCharSet this[BorderStyle style] => style switch
        {
            BorderStyle.DoubleLined => doubleLined,
            BorderStyle.Bold => boldLined,
            _ => singleLined
        };
    }
}
