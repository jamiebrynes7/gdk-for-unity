using Improbable.Gdk.Core;

namespace Improbable.Gdk.Subscriptions
{
    public interface ICommandRequestCallbackManager
    {
        void InvokeCallbacks(CommandSystem commandSystem);
    }
}
