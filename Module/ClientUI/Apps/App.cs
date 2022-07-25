using GTANetworkAPI;
using Nexus.Module.PlayerUI.Windows;

namespace Nexus.Module.PlayerUI.Apps
{
    public abstract class App<T> : Window<T>
    {
        private string Component { get; }

        public App(string name, string component) : base(name)
        {
            Component = component;
        }

        public override void Open(Player player, string json)
        {
            player.TriggerEvent("openApp", Component, Name, json);
        }
    }
}