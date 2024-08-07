

using Unity.Netcode.Components;

namespace nazaaaar.networkUtils
{
    public class OwnerNetworkAnimator : NetworkAnimator
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}