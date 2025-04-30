using System.Drawing;

namespace BSoundMute.Controls.Themes
{
    public interface ITheme
    {
        bool IsDisplayed { get; }

        Color BackColor { get; }

        Size ControlBoxSize { get; }

        Point ButtonOffset { get; }

        Size FrameBorder { get; }

        Size SystemButtonSize { get; }
    }
}