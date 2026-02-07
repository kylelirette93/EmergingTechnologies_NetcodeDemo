using UnityEngine;
using Unity.Netcode.Components;
namespace Unity.Multiplayer.Samples.Utilities.ClientAuthority
{
    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        // Override base class method to specify that it's not server authoritative. This means the client is responsible
        // for sending its updates to server.
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
