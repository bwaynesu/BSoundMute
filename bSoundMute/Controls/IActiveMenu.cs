using System.Windows.Forms;

namespace bSoundMute.Controls
{
  public interface IActiveMenu
  {
    /// <summary>
    /// 	Gets the list of <see cref = "T:ActiveButton"></see> instances
    /// 	associated with the current menu instances.
    /// </summary>
    /// <value>The items.</value>
    IActiveItems Items { get; }

    /// <summary>
    /// 	Gets or sets the tool tip control used for rendering tool tips.
    /// </summary>
    /// <value>The tool tip settings.</value>
    ToolTip ToolTip { get; set; }
  }
}
