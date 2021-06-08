using Serilog;
using System;
using System.Diagnostics;
using System.Linq;

namespace SRTPluginProviderRE7
{
    public class ReaderRE7 : IDisposable
    {
        private ILogger logger;
        private int? processId;
        private GameMemoryRE7Scanner gameMemoryScanner;
        private Stopwatch stopwatch;

        public ReaderRE7(ILogger logger)
        {
            this.logger = logger;
        }
        public bool GameRunning
        {
            get
            {
                if (gameMemoryScanner != null && !gameMemoryScanner.ProcessRunning)
                {
                    processId = GetProcessId();
                    if (processId != null)
                    {
                        gameMemoryScanner.Initialize((int)processId); // Re-initialize and attempt to continue.
                    }
                }

                return gameMemoryScanner != null && gameMemoryScanner.ProcessRunning;
            }
        }

        public void Init()
        {
            processId = GetProcessId();
            gameMemoryScanner = new GameMemoryRE7Scanner(processId, logger);
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public object PullData()
        {
            if (stopwatch == null || gameMemoryScanner == null)
                return null;

            try
            {
                if (!GameRunning)
                {
                    return null;
                }
                if (!gameMemoryScanner.ProcessRunning)
                {
                    //hostDelegates.Exit();
                    processId = GetProcessId();
                    if (processId != null)
                    {
                        gameMemoryScanner.Initialize(processId.Value); // re-initialize and attempt to continue
                    }
                    else if (!gameMemoryScanner.ProcessRunning)
                    {
                        stopwatch.Restart();
                        return null;
                    }
                }

                if (stopwatch.ElapsedMilliseconds >= 2000L)
                {
                    gameMemoryScanner.UpdatePointers();
                    stopwatch.Restart();
                }
                return gameMemoryScanner.Refresh();
            }
            catch (Exception ex)
            {
                // Log ostDelegates.OutputMessage("[{0}] {1} {2}", ex.GetType().Name, ex.Message, ex.StackTrace);
                return null;
                exCount++;
                if (exCount > 10)
                {
                    exCount = 0;
                    Dispose();
                    Init();
                    logger.Verbose("Memory reader estarted because pointers were created while the game was not ready.");
                }
            }
        }

        int exCount;
        private int? GetProcessId() => Process.GetProcessesByName("re7")?.FirstOrDefault()?.Id;

        public void Dispose()
        {
            stopwatch?.Stop();
            stopwatch = null;
            gameMemoryScanner?.Dispose();
            gameMemoryScanner = null;
        }
    }
}
