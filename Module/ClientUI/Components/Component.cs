using GTANetworkAPI;

namespace Nexus.Module.PlayerUI.Components
{
    public abstract class Component : Script
    {   
        public string Name { get; }

        public Component(string name)
        {
            Name = name;
            ComponentManager.Instance.Register(this);
        }

        public void TriggerEvent(Player player, string eventName, params object[] args)
        {
            var eventArgs = new object[2 + args.Length];
            eventArgs[0] = Name;
            eventArgs[1] = eventName;

            for (var i = 0; i < args.Length; i++)
            {
                eventArgs[i + 2] = args[i];
            }

            player.TriggerEvent("componentServerEvent", eventArgs);
        }
    }
}