using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Libraries;
using ProgrammingParadigms;
using DomainAbstractions;

namespace Application
{
    public class Application
    {
        private MainWindow mainWindow = new MainWindow("AUT Workshop 2020");

        private Application Initialize()
        {
            Wiring.PostWiringInitialize();
            return this;
        }

        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            app.Initialize().mainWindow.Run();
        }

        private Application()
        {
            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR Calculator.xmind
            // END AUTO-GENERATED INSTANTIATIONS FOR Calculator.xmind

            // BEGIN AUTO-GENERATED WIRING FOR Calculator.xmind
            // END AUTO-GENERATED WIRING FOR Calculator.xmind

            // BEGIN manual instantiations
            var dataFlowConnector1 = new DataFlowConnector<double>();
            var dataFlowConnector2 = new DataFlowConnector<double>();
            var text1 = new Text("-----");
            // END manual instantiations

            // BEGIN manual wiring
            mainWindow.WireTo(new Text("Hello world.") { FontSize = 200 });
            // END manual wiring
        }
    }
}
