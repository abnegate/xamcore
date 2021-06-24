using System;
using System.Diagnostics;

namespace XamCore.Services
{
    public class AppCloserService : IAppCloserService
    {
        public void CloseApp()
        {
            try {
                Process.GetCurrentProcess().CloseMainWindow();
            } catch (PlatformNotSupportedException e) {
                Debug.Write(e);
            } catch (InvalidOperationException e) {
                Debug.Write(e);
            }
        }
    }
}
