/*
 * (C) René Vogt
 *
 * Published under MIT license as described in the LICENSE.md file.
 *
 */

namespace ConControls.Controls.Drawing 
{
    /// <summary>
    /// Provides <see cref="FrameCharSet"/> instances for given <see cref="BorderStyle"/> values.
    /// </summary>
    /// <remarks>
    /// You can inherit this class to return your own implementations of <see cref="FrameCharSet"/>
    /// classes to change the characters used to draw control frames.
    /// You can then pass your custom <see cref="FrameCharSets"/> instance to <see cref="IConsoleWindow.FrameCharSets"/>
    /// to have your custom characters used for frames.<br/>
    /// This default implementation uses the followig <see cref="FrameCharSet"/> instances:
    /// <list type="table">
    /// <listheader>
    /// <term><see cref="BorderStyle"/></term>
    /// <description><see cref="FrameCharSet"/></description>
    /// </listheader>
    /// <item>
    /// <term><see cref="BorderStyle.SingleLined"/></term>
    /// <description><see cref="SingleLinedFrameCharSet"/></description>
    /// </item>
    /// <item>
    /// <term><see cref="BorderStyle.DoubleLined"/></term>
    /// <description><see cref="DoubleLinedFrameCharSet"/></description>
    /// </item>
    /// <item>
    /// <term><see cref="BorderStyle.Bold"/></term>
    /// <description><see cref="BoldLinedFrameCharSet"/></description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <seealso cref="FrameCharSet"/>
    /// <seealso cref="SingleLinedFrameCharSet"/>
    /// <seealso cref="DoubleLinedFrameCharSet"/>
    /// <seealso cref="BoldLinedFrameCharSet"/>
    /// <seealso cref="IConsoleWindow.FrameCharSets"/>
    public class FrameCharSets
    {
        static readonly FrameCharSet singleLined = new SingleLinedFrameCharSet();
        static readonly FrameCharSet doubleLined = new DoubleLinedFrameCharSet();
        static readonly FrameCharSet boldLined = new BoldLinedFrameCharSet();

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
