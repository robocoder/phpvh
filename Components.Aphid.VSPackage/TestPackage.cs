using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Components.Aphid.VSPackage
{
    using System;
    using System.Runtime.InteropServices;
    using System.ComponentModel.Design;
    using Microsoft.VisualStudio.Package;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.TextManager.Interop;

    namespace TestLanguagePackage
    {
        [Microsoft.VisualStudio.Shell.ProvideService(typeof(AphidLanguageService))]
        [Microsoft.VisualStudio.Shell.ProvideLanguageExtension(typeof(AphidLanguageService), ".alx")]
        [Microsoft.VisualStudio.Shell.ProvideLanguageService(typeof(AphidLanguageService), "Test Language", 0,
            AutoOutlining = true,
            EnableCommenting = true,
            MatchBraces = true,
            ShowMatchingBrace = true)]
        [Guid("2e5ec861-04b2-4a6c-ad40-8e1e0ee0ceb6")] //provide a unique GUID for the package

        public class TestLanguagePackage : Package, IOleComponent
        {
            private uint m_componentID;


            protected override void Initialize()
            {
                base.Initialize();  // required

                // Proffer the service.
                IServiceContainer serviceContainer = this as IServiceContainer;
                AphidLanguageService langService = new AphidLanguageService();
                langService.SetSite(this);
                serviceContainer.AddService(typeof(AphidLanguageService),
                                            langService,
                                            true);

                // Register a timer to call our language service during
                // idle periods.
                IOleComponentManager mgr = GetService(typeof(SOleComponentManager))
                                           as IOleComponentManager;
                if (m_componentID == 0 && mgr != null)
                {
                    OLECRINFO[] crinfo = new OLECRINFO[1];
                    crinfo[0].cbSize = (uint)Marshal.SizeOf(typeof(OLECRINFO));
                    crinfo[0].grfcrf = (uint)_OLECRF.olecrfNeedIdleTime |
                                                  (uint)_OLECRF.olecrfNeedPeriodicIdleTime;
                    crinfo[0].grfcadvf = (uint)_OLECADVF.olecadvfModal |
                                                  (uint)_OLECADVF.olecadvfRedrawOff |
                                                  (uint)_OLECADVF.olecadvfWarningsOff;
                    crinfo[0].uIdleTimeInterval = 1000;
                    int hr = mgr.FRegisterComponent(this, crinfo, out m_componentID);
                }
            }


            protected override void Dispose(bool disposing)
            {
                if (m_componentID != 0)
                {
                    IOleComponentManager mgr = GetService(typeof(SOleComponentManager))
                                               as IOleComponentManager;
                    if (mgr != null)
                    {
                        int hr = mgr.FRevokeComponent(m_componentID);
                    }
                    m_componentID = 0;
                }

                base.Dispose(disposing);
            }


            #region IOleComponent Members

            public int FDoIdle(uint grfidlef)
            {
                bool bPeriodic = (grfidlef & (uint)_OLEIDLEF.oleidlefPeriodic) != 0;
                // Use typeof(TestLanguageService) because we need to
                // reference the GUID for our language service.
                LanguageService service = GetService(typeof(AphidLanguageService))
                                          as LanguageService;
                if (service != null)
                {
                    service.OnIdle(bPeriodic);
                }
                return 0;
            }

            public int FContinueMessageLoop(uint uReason,
                                            IntPtr pvLoopData,
                                            MSG[] pMsgPeeked)
            {
                return 1;
            }

            public int FPreTranslateMessage(MSG[] pMsg)
            {
                return 0;
            }

            public int FQueryTerminate(int fPromptUser)
            {
                return 1;
            }

            public int FReserved1(uint dwReserved,
                                  uint message,
                                  IntPtr wParam,
                                  IntPtr lParam)
            {
                return 1;
            }

            public IntPtr HwndGetWindow(uint dwWhich, uint dwReserved)
            {
                return IntPtr.Zero;
            }

            public void OnActivationChange(IOleComponent pic,
                                           int fSameComponent,
                                           OLECRINFO[] pcrinfo,
                                           int fHostIsActivating,
                                           OLECHOSTINFO[] pchostinfo,
                                           uint dwReserved)
            {
            }

            public void OnAppActivate(int fActive, uint dwOtherThreadID)
            {
            }

            public void OnEnterState(uint uStateID, int fEnter)
            {
            }

            public void OnLoseActivation()
            {
            }

            public void Terminate()
            {
            }

            #endregion
        }
    } 
}
