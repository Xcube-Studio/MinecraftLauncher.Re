using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MinecraftLauncher.Core.Wrapper;
using MinecraftLauncher.Core.Standard;
using MinecraftLauncher.Core.Standard.Enums;
using MinecraftLauncher.Core.Standard.Events;
using MinecraftLauncher.Core.Standard.Items;
using MinecraftLauncher.Core.Standard.Service.Local;

namespace MinecraftLauncher.Core
{
    public class MinecraftLauncher : IMinecraftLauncher
    {
        public MinecraftLauncher(LaunchConfiguration launchConfiguration,AuthenticationData authenticationData)
        {
            using (var minecraftDataLoader = new MinecraftDataLoader
                (authenticationData: authenticationData, minecraftLocator: new MinecraftLocator(launchConfiguration.GameFolder), name: launchConfiguration.Version, launchConfiguration: launchConfiguration))
            {
                this.Minecraft = minecraftDataLoader.Load();
            }

            Args = new ArgsGenerator(this.Minecraft).BulidArguments();

            this.MinecraftProcess = new Process
            {
                StartInfo = new ProcessStartInfo()
                {
                    Arguments = Args,
                    FileName = this.Minecraft.LaunchConfiguration.JavaPath,
                    WorkingDirectory = this.Minecraft.LaunchConfiguration.GameFolder,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    ErrorDialog = true
                },
                EnableRaisingEvents = true
            };

            this.MinecraftProcess.OutputDataReceived += MinecraftProcess_OutputDataReceived;
            this.MinecraftProcess.ErrorDataReceived += MinecraftProcess_OutputDataReceived;
            this.MinecraftProcess.Exited += MinecraftProcess_Exited;

            this.state = LaunchState.Initialized;
        }

        protected void MinecraftProcess_Exited(object sender, EventArgs e)
        {
            try
            {
                this.OnProcessExited(new ProcessExitedEventArgs()
                {
                    ExitCode = this.MinecraftProcess.ExitCode
                });
                if (this.MinecraftProcess.ExitCode != 0)
                    this.OnProcessCrashed(new ProcessCrashedEventArgs()
                    {
                        //Reason = 
                    });
            }
            catch {
                this.OnProcessExited(new ProcessExitedEventArgs()
                {
                    ExitCode = 0
                });
            }

            Stop();
        }

        protected void MinecraftProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            OnProcessOutputDateReceived(new ProcessOutputDateReceivedEventArgs(e.Data));
        }

        public MinecraftData Minecraft { get; set; }

        public Process MinecraftProcess { get; private set; }

        public LaunchState State
        {
            get { return this.state; }
        }

        protected LaunchState state = LaunchState.Undefined;

        protected string Args;

        //protected readonly object monitor = new object();

        public event LaunchDelegates.LaunchCancelledHandler LaunchCancelled;

        public event LaunchDelegates.LaunchFaildHandler LaunchFaild;

        public event LaunchDelegates.LaunchStartedHandler LaunchStarted;

        public event LaunchDelegates.LaunchStoppedHandler LaunchStopped;

        public event LaunchDelegates.ProcessCrashedHandler ProcessCrashed;

        public event LaunchDelegates.ProcessExitedHandler ProcessExited;

        public event LaunchDelegates.ProcessOutputDateReceivedHandler ProcessOutputDateReceived;

        public event LaunchDelegates.ProcessStartedHandler ProcessStarted;

        public virtual void Cancel()
        {
            if (!this.MinecraftProcess.HasExited)
                this.MinecraftProcess.CloseMainWindow();

            this.Stop();
        }

        public virtual void DetachAllHandlers()
        {
            this.MinecraftProcess.OutputDataReceived += MinecraftProcess_OutputDataReceived;
            this.MinecraftProcess.ErrorDataReceived += MinecraftProcess_OutputDataReceived;
            this.MinecraftProcess.Exited += MinecraftProcess_Exited;

            if (this.ProcessOutputDateReceived != null)
            {
                foreach (var i in this.ProcessOutputDateReceived.GetInvocationList())
                {
                    this.ProcessOutputDateReceived -= (LaunchDelegates.ProcessOutputDateReceivedHandler)i;
                }
            }

            if (this.ProcessStarted != null)
            {
                foreach (var i in this.ProcessStarted.GetInvocationList())
                {
                    this.ProcessStarted -= (LaunchDelegates.ProcessStartedHandler)i;
                }
            }

            if (this.ProcessCrashed != null)
            {
                foreach (var i in this.ProcessCrashed.GetInvocationList())
                {
                    this.ProcessCrashed -= (LaunchDelegates.ProcessCrashedHandler)i;
                }
            }

            if (this.ProcessExited != null)
            {
                foreach (var i in this.ProcessExited.GetInvocationList())
                {
                    this.ProcessExited -= (LaunchDelegates.ProcessExitedHandler)i;
                }
            }

            if (this.LaunchCancelled != null)
            {
                foreach (var i in this.LaunchCancelled.GetInvocationList())
                {
                    this.LaunchCancelled -= (LaunchDelegates.LaunchCancelledHandler)i;
                }
            }

            if (this.LaunchFaild != null)
            {
                foreach (var i in this.LaunchFaild.GetInvocationList())
                {
                    this.LaunchFaild -= (LaunchDelegates.LaunchFaildHandler)i;
                }
            }

            if (this.LaunchStarted != null)
            {
                foreach (var i in this.LaunchStarted.GetInvocationList())
                {
                    this.LaunchStarted -= (LaunchDelegates.LaunchStartedHandler)i;
                }
            }

            if (this.LaunchStopped != null)
            {
                foreach (var i in this.LaunchStopped.GetInvocationList())
                {
                    this.LaunchStopped -= (LaunchDelegates.LaunchStoppedHandler)i;
                }
            }

        }

        public virtual void Dispose()
        {
            this.DetachAllHandlers();

            if (!this.MinecraftProcess.HasExited)
                this.MinecraftProcess.Kill();

            this.MinecraftProcess.Close();

            this.Minecraft = null;
            this.Args = null;
        }

        public virtual void Start()
        {
            new NativesManager(Minecraft).Extract();

            this.MinecraftProcess.Start(); 
            this.MinecraftProcess.BeginErrorReadLine();
            this.MinecraftProcess.BeginOutputReadLine();

            Task.Run(() =>
            {
                this.MinecraftProcess.WaitForInputIdle();
                this.OnProcessStarted(new ProcessStartedEventArgs());
            });

            OnLaunchStarted(new LaunchEventArgs());
            this.state = LaunchState.Running;
        }

        public virtual void Stop()
        {
            if (!this.MinecraftProcess.HasExited)
                this.MinecraftProcess.CloseMainWindow();

            this.state = LaunchState.Stopped;
            OnLaunchStopped(new LaunchEventArgs(this));
        }

        protected virtual void OnProcessOutputDateReceived(ProcessOutputDateReceivedEventArgs args)
        {
            this.ProcessOutputDateReceived?.Invoke(args);
        }

        protected virtual void OnProcessCrashed(ProcessCrashedEventArgs args)
        {
            this.ProcessCrashed?.Invoke(args);
        }

        protected virtual void OnProcessExited(ProcessExitedEventArgs args)
        {
            this.ProcessExited?.Invoke(args);
        }

        protected virtual void OnProcessStarted(ProcessStartedEventArgs args)
        {
            this.ProcessStarted?.Invoke(args);
        }

        protected virtual void OnLaunchCancelled(LaunchEventArgs args)
        {
            this.LaunchCancelled?.Invoke(args);
        }

        protected virtual void OnLaunchFaild(LaunchFaildEventArgs args)
        {
            this.LaunchFaild?.Invoke(args);
        }

        protected virtual void OnLaunchStarted(LaunchEventArgs args)
        {
            this.LaunchStarted?.Invoke(args);
        }

        protected virtual void OnLaunchStopped(LaunchEventArgs args)
        {
            this.LaunchStopped?.Invoke(args);
        }
    }
}
