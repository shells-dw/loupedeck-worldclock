namespace Loupedeck.WorldClockPlugin
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Loupedeck.WorldClockPlugin.Helpers;

    // This class contains the plugin-level logic of the Loupedeck plugin.

    public class WorldClockPlugin : Plugin
    {
        public event EventHandler<EventArgs> Tick;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private async void Timer()
        {
            while (true && !this._cancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(1000);
                Tick?.Invoke(this, new EventArgs());
            }

        }
        // Gets a value indicating whether this is an Universal plugin or an Application plugin.
        public override Boolean UsesApplicationApiOnly => true;

        // Gets a value indicating whether this is an API-only plugin.
        public override Boolean HasNoApplication => true;

        // This method is called when the plugin is loaded during the Loupedeck service start-up.
        public override void Load()
        {
    //        WorldClockPluginInstaller.Install();
            HelperFunctions.ReadTzData();
            this.Timer();
        }

        // This method is called when the plugin is unloaded during the Loupedeck service shutdown.
        public override void Unload() => this._cancellationTokenSource.Cancel();
    }
}
