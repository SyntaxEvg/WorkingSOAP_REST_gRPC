using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace PumpService
{
    public class ScriptService : IScriptService
    {
        private CompilerResults _results = null;
        private readonly IStatisticsService _statisticsService;
        private readonly ISettingsService _settingsService;
        private readonly IPumpServiceCallback _pumpServiceCallback;


        public ScriptService(IPumpServiceCallback pumpServiceCallback,
                             IStatisticsService statisticsService,
                             ISettingsService settingsService)
        {
            _pumpServiceCallback = pumpServiceCallback;
            _statisticsService = statisticsService;
            _settingsService = settingsService;
        }


        public bool Compile()
        {
            var compilerParameters = new CompilerParameters();
            compilerParameters.GenerateInMemory = true;
            compilerParameters.ReferencedAssemblies.AddRange(new string[]
            {
                "System.dll",
                "System.Data.dll",
                "System.Core.dll",
                "Microsoft.CSharp.dll",
                Assembly.GetExecutingAssembly().Location
            });

            try
            {
                _results = new CSharpCodeProvider().CompileAssemblyFromFile(compilerParameters, _settingsService.FileName);
            }
            catch
            {
                return false;
            }

            return _results.Errors != null && _results.Errors.Count == 0;
        }
        public void Run(int count)
        {
            var type = _results.CompiledAssembly.GetType("Sample.SampleScript");
            if (type is null)
            {
                return;
            }

            MethodInfo entryPointMethod = type.GetMethod("Main");
            if (entryPointMethod is null)
            {
                return;
            }

            Task.Run(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    _ = (bool)entryPointMethod.Invoke(Activator.CreateInstance(type), null)
                        ? _statisticsService.SuccessTacts++
                        : _statisticsService.ErrorTacts++;

                    _statisticsService.AllTacts++;
                    _pumpServiceCallback.UpdateStatistics((StatisticsService)_statisticsService);
                }
            });
        }
    }
}