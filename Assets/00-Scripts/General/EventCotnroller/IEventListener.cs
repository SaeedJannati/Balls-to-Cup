using System;

namespace BallsToCup.General
{
    interface IEventListener
    {
         void RegisterToEvents();

          void UnregisterFromEvents();
    }
}