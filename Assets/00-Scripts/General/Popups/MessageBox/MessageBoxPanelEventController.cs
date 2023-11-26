
namespace BallsToCup.General.Popups
{
    public class MessageBoxPanelEventController : BaseEventController
    {
        public readonly SimpleEvent  onFirstButtonClick=new();
        public readonly SimpleEvent  onSecondButtonClick=new();
    }
}
