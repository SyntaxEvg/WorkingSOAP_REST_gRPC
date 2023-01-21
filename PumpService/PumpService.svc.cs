using System;
using System.ServiceModel;

namespace PumpService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PumpService : IPumpService
    {
        private readonly IScriptService _scriptService;
        private readonly IStatisticsService _statisticsService;
        private readonly ISettingsService _settingsService;


        public PumpService()
        {
            _statisticsService = new StatisticsService();
            _settingsService = new SettingsService();
            _scriptService = new ScriptService(Callback, _statisticsService, _settingsService);
        }


        public void RunScript()
        {
            _scriptService.Run(10);
        }

        public void UpdateAndCompileScript(string fileName)
        {
            _settingsService.FileName = fileName;
            _scriptService.Compile();
        }

        IPumpServiceCallback Callback => OperationContext.Current != null
                ? OperationContext.Current.GetCallbackChannel<IPumpServiceCallback>()
                : null;
    }
}