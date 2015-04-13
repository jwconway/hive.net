namespace Hive.Plugin.Plugin
{
	public interface IPluginAppStart
	{
		void StartPluginApp();
		void StopPluginApp();
		void PausePluginApp();
		void DeletePluginApp();
		void UnPausePluginApp();
	}
}