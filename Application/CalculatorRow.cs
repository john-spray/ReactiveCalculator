using System;
using System.Collections.Generic;
using System.Windows;

using DomainAbstractions;
using ProgrammingParadigms;
using Libraries;

namespace Application
{
    // TBD
    // Move the generated wiring code to its own function so that it can be called at runtime
    // Call the wiring code in PostWiringInitialize (will need to change the way PostWiring works for internal objects)
    // Work out how to append our InstanceName to all the intername instance names automatically


    /// <summary>
    /// Implements a row of a calculator
    /// Consists of 5 UI elements arrange horizontally: label, formula, result, units, description 
    /// The user enters a label in the label field which labels the output e.g. "velocity"
    /// The user enters a formula e.g. 2+3 e.g. velocity*time consisting of literal values, the labels (of other rows) and operators, including C# math library operators e.g. Sqrt(velocity)
    /// The input operands is a list of inputs which should have the results of all other rows wired to it in the order of the labels in the labelsCommaSeparated.
    /// </summary>
    class CalculatorRow : IUI, IDataFlowB<string>, IDataFlowB<double?>, ITestCalculatorRow
    {
        // properties
        public string InstanceName { get; set; }




        // Ports
        // (implemented) IUI parent out parent UI instance
        // (implemented) IDataFlowB<string> label (output) the value the user enters into the label field
        // (implemented) IDataFlowB<double?> result (output) the result of the calculation
        List<IDataFlowB<double?>> operands;
        IDataFlowB<string> labelsCommaSeparated; // (input) a comma separated list of all the labels corresponding one-to-one with the operands inputs



        /// <summary>
        /// Implements a row of a calculator
        /// Consists of 5 UI elements arrange horizontally: label, formula, result, units, description 
        /// The user enters a label in the label field which labels the output e.g. "velocity"
        /// The user enters a formula e.g. 2+3 e.g. velocity*time consisting of literal values, the labels (of other rows) and operators, including C# math library operators e.g. Sqrt(velocity)
        /// The input operands is a list of inputs which should have the results of all other rows wired to it in the order of the labels in the labelsCommaSeparated.
        /// </summary>
        public CalculatorRow(ConstructorCallbackDelegate ConstructorCallbackMethod = null)
        {
            // This pattern allow the constructor to get its necessary properties set before the rest of the construction takes place
            ConstructorCallbackMethod?.Invoke(this);

            WireInternals();
        }


        public int[] Ratios { get;  set; }
        public int[] MinWidths { get;  set; }
        public int FontSize { get; set; }



        // port IUI parent
        IUI internalIUI;
        UIElement IUI.GetWPFElement()
        {
            return internalIUI.GetWPFElement();
        }




        // port IDataFlowB<string> label implementation
        private IDataFlowB<string> labeldfc; // This is the label textbox in the internal wiring that implements IDataFlowB. To get its data use labeldfc.Data. To know when there is a new label, attach a handler to labeldcf.DataChanged
        event DataChangedDelegate labelChangedEvent;
        event DataChangedDelegate IDataFlowB<string>.DataChanged { add { labelChangedEvent += value; } remove { labelChangedEvent += value; } }
        // implement the get of our border interface through to the internal textBox for the label
        string IDataFlowB<string>.Data { get => labeldfc.Data; }
        // implement the handler that is registered to the internal label textbox output and then invokes our external output. Note the registering to this handler is done in PostWiringInitialize below.
        private void labelChangedHandler()
        {
            labelChangedEvent?.Invoke();
        }





        // port IDataFlowB<double?> result
        // Our class, the containing implements an IDataFlowB, which is an output. Multiple external objects will be wired to our output - (we have no way of knowing about them). 
        // Implementing an IDataFlowB always involves a get and an event. The get is for the outside world to get our result. The event allows us to tell the outside world there is a new result
        // implement the event in the interface (standard explicit implementation of an Interface event)
        event DataChangedDelegate resultChangedEvent;
        event DataChangedDelegate IDataFlowB<double?>.DataChanged { add { resultChangedEvent += value; } remove { resultChangedEvent += value; } }
        // implement the get
        // The internal wiring implements IDataFlowB, which is an output. 
        private DataFlowConnector<double?> internalResult;  // This is the object in the internal wiring that implements IDataFlowB. To get its data use resultConnector.Data. To know when there is a new result, attach a handler to resultConnector.DataChanged
        double? IDataFlowB<double?>.Data { get => internalResult.Data; } // When the outside world asks for our data, get it directly from the internal wiring, which is a 
        // implement the handler. It is registered to the internal wiring's resultConnector by the WireInternals method below. The handler notifies the outside world there is a new result so the outside world can get it using the above getter.
        private void resultChangedHandler()
        {
            resultChangedEvent?.Invoke();
        }




        // port List<IDataFlowB<double?>> operands;
        private Formula internalFormula;





        // port IDataFlowB<string> labelsCommaSeparated
        // Wire our outer boundary, to the inner wiring. 
        // direction of dataflow is from the outside world into the internal wiring.
        // But internal wiring is a private field of IDataFlowB which must be wired to something that implements IdataFlowB
        // Although data is into the internal wiring, the WireTo is the opposite direction, as is always the case with IDataFlowB.
        // Two posibilities
        // Method 1 : use a DataFlowConnector object (or a DataFlowBNull). Our boundary interface then has an event handler that gives the data to the connector
        // Method 2 : An object that implements IDataFlowB<string> will be wired to us on the outside. We already have a reference to it in labelsCommaSeparated after that wiring is done. So wire the inner object directly to the object outside. The outside object is not wired to us when we are being constructed, so need to do it after construction and external wiring is done.
        // private IDataFlow<string> labelsCommaSeparatedInternalConnector = new DataFlowConnector<string>() { InstanceName = @"{this.InstanceName}_labelsCommaSeparatedInternalConnector" };  // method 1
        private IDataFlowB<string> labelsCommaSeparatedInternal; // method 2 // This is the object in interal wiring that has a field of type idataFlowB that must be wired to somethiing that implements IDataFlowB on the outside
        /*
        private void labelsCommaSeparatedChangedHandler()  // method 1
        {
            labelsCommaSeparatedInternalConnector.Data = labelsCommaSeparated.Data;
        }
        */






        private void WireInternals()
        {

            // Note WireInternals has been designed to be tolerant of when it is called relative to the when the external wiring is done. It can be called from the constructor, or even at runtime.
            if (internalWiringDone) throw new Exception("Please don't call WireInternals more than once");

            // BEGIN AUTO-GENERATED INSTANTIATIONS FOR CalculatorRow.xmind
            DataFlowBNull<string> id_b9e566abb4cc42d1a7d3927615231c50 = new DataFlowBNull<string>() { InstanceName = "Default" };
            DataFlowConnector<double?> dfc1 = new DataFlowConnector<double?>() { InstanceName = "dfc1" };
            DataFlowConnector<string> id_2ce385f7abc549b98a72fc2c4dd709fd = new DataFlowConnector<string>() { InstanceName = "Default" };
            DataFlowConnector<string> id_65f22d8aa160470e8da02d0fce01edca = new DataFlowConnector<string>() { InstanceName = "Default" };
            ForceAssociativity id_02f042986f3242648a019c5fbf7c8752 = new ForceAssociativity("^",rightAssociative:true ) { InstanceName = "Default" };
            Formula formula = new Formula() { InstanceName = "formula" };
            FormulaRender formulaRender = new FormulaRender() { InstanceName = "formulaRender" };
            Horizontal id_6fe26e8021c64d8dad4e5b6016f7b659 = new Horizontal() { InstanceName = "Default", Ratios = Ratios, MinWidths = MinWidths };
            NumberFormatting id_bab796380f6d4c4eb93428662ce78dc2 = new NumberFormatting() { InstanceName = "Default" };
            NumberToString<double?> id_4c9cb86bce4544fe90c628e9eaecbcec = new NumberToString<double?>() { InstanceName = "Default" };
            RegexReplace id_6008429a36ce435da09c8f7c5534800c = new RegexReplace("Sqrt","\u221A" ) { InstanceName = "Default" };
            RegexReplace id_8537240f2a654a788fbc6103c2e3a45f = new RegexReplace(@"\s","" ) { InstanceName = "Default" };
            SelectionBox<FormatModes> format = new SelectionBox<FormatModes>() { InstanceName = "format", Margins = new Thickness(5,5,5,0) };
            StringFormat<string,string> sf1 = new StringFormat<string,string>("({1})=>{0}" ) { InstanceName = "sf1" };
            StringToNumber<int> id_ccfb4bf2e9df48e3a9c4d7f0f34d2d3a = new StringToNumber<int>() { InstanceName = "Default" };
            Text resultText = new Text() { InstanceName = "resultText", FontSize=FontSize };
            TextBox descriptionText = new TextBox() { InstanceName = "descriptionText", FontSize=15, Multiline = true };
            TextBox digitsText = new TextBox() { InstanceName = "digitsText", FontSize=15, Margins = new Thickness(5,0,5,5), Text = "3" };
            TextBox formulaText = new TextBox() { InstanceName = "formulaText", FontSize=FontSize };
            TextBox labelText = new TextBox() { InstanceName = "labelText", FontSize=FontSize };
            TextBox unitsText = new TextBox() { InstanceName = "unitsText", FontSize=FontSize };
            TransformOperator id_3d142790cd894fffbe31c6a9936a40f9 = new TransformOperator("^","Pow",rightAssociative:true ) { InstanceName = "Default" };
            TransformOperator id_ba99ef2eb1eb4ab7adfeab8d1b9bfb2b = new TransformOperator("!","Fact",unary:true ) { InstanceName = "Default" };
            Vertical id_6448e518651246a3af0d4f7d49c13077 = new Vertical() { InstanceName = "Default" };
            // END AUTO-GENERATED INSTANTIATIONS FOR CalculatorRow.xmind


            dfc1.InstanceName = $"{InstanceName}_{dfc1.InstanceName}";
            dfc1.InstanceName = $"{InstanceName}_{dfc1.InstanceName}";
            id_b9e566abb4cc42d1a7d3927615231c50.InstanceName = $"{InstanceName}_{id_b9e566abb4cc42d1a7d3927615231c50.InstanceName}";
            id_65f22d8aa160470e8da02d0fce01edca.InstanceName = $"{InstanceName}_{id_65f22d8aa160470e8da02d0fce01edca.InstanceName}";
            formula.InstanceName = $"{InstanceName}_{formula.InstanceName}";
            id_6fe26e8021c64d8dad4e5b6016f7b659.InstanceName = $"{InstanceName}_{id_6fe26e8021c64d8dad4e5b6016f7b659.InstanceName}";
            id_4c9cb86bce4544fe90c628e9eaecbcec.InstanceName = $"{InstanceName}_{id_4c9cb86bce4544fe90c628e9eaecbcec.InstanceName}";
            sf1.InstanceName = $"{InstanceName}_{sf1.InstanceName}";
            resultText.InstanceName = $"{InstanceName}_{resultText.InstanceName}";
            descriptionText.InstanceName = $"{InstanceName}_{descriptionText.InstanceName}";
            formulaText.InstanceName = $"{InstanceName}_{formulaText.InstanceName}";
            labelText.InstanceName = $"{InstanceName}_{labelText.InstanceName}";
            unitsText.InstanceName = $"{InstanceName}_{unitsText.InstanceName}";

            // BEGIN AUTO-GENERATED WIRING FOR CalculatorRow.xmind
            id_6fe26e8021c64d8dad4e5b6016f7b659.WireTo(labelText, "children"); // (Horizontal (id_6fe26e8021c64d8dad4e5b6016f7b659).children) -- [List<IUI>] --> (TextBox (labelText).child)
            id_6fe26e8021c64d8dad4e5b6016f7b659.WireTo(formulaText, "children"); // (Horizontal (id_6fe26e8021c64d8dad4e5b6016f7b659).children) -- [List<IUI>] --> (TextBox (formulaText).child)
            id_6fe26e8021c64d8dad4e5b6016f7b659.WireTo(formulaRender, "children"); // (Horizontal (id_6fe26e8021c64d8dad4e5b6016f7b659).children) -- [List<IUI>] --> (FormulaRender (formulaRender).child)
            id_6fe26e8021c64d8dad4e5b6016f7b659.WireTo(resultText, "children"); // (Horizontal (id_6fe26e8021c64d8dad4e5b6016f7b659).children) -- [List<IUI>] --> (Text (resultText).child)
            id_6fe26e8021c64d8dad4e5b6016f7b659.WireTo(unitsText, "children"); // (Horizontal (id_6fe26e8021c64d8dad4e5b6016f7b659).children) -- [List<IUI>] --> (TextBox (unitsText).child)
            id_6fe26e8021c64d8dad4e5b6016f7b659.WireTo(descriptionText, "children"); // (Horizontal (id_6fe26e8021c64d8dad4e5b6016f7b659).children) -- [List<IUI>] --> (TextBox (descriptionText).child)
            id_6fe26e8021c64d8dad4e5b6016f7b659.WireTo(id_6448e518651246a3af0d4f7d49c13077, "children"); // (Horizontal (id_6fe26e8021c64d8dad4e5b6016f7b659).children) -- [List<IUI>] --> (Vertical (id_6448e518651246a3af0d4f7d49c13077).Child)
            labelText.WireTo(id_65f22d8aa160470e8da02d0fce01edca, "textOutput"); // (TextBox (labelText).textOutput) -- [IDataFlow<string>] --> (DataFlowConnector<string> (id_65f22d8aa160470e8da02d0fce01edca).input)
            formulaText.WireTo(id_2ce385f7abc549b98a72fc2c4dd709fd, "textOutput"); // (TextBox (formulaText).textOutput) -- [IDataFlow<string>] --> (DataFlowConnector<string> (id_2ce385f7abc549b98a72fc2c4dd709fd).input)
            id_2ce385f7abc549b98a72fc2c4dd709fd.WireTo(id_3d142790cd894fffbe31c6a9936a40f9, "outputs"); // (DataFlowConnector<string> (id_2ce385f7abc549b98a72fc2c4dd709fd).outputs) -- [IDataFlow<string>] --> (TransformOperator (id_3d142790cd894fffbe31c6a9936a40f9).input)
            id_2ce385f7abc549b98a72fc2c4dd709fd.WireTo(id_6008429a36ce435da09c8f7c5534800c, "outputs"); // (DataFlowConnector<string> (id_2ce385f7abc549b98a72fc2c4dd709fd).outputs) -- [IDataFlow<string>] --> (RegexReplace (id_6008429a36ce435da09c8f7c5534800c).input)
            id_3d142790cd894fffbe31c6a9936a40f9.WireTo(id_ba99ef2eb1eb4ab7adfeab8d1b9bfb2b, "output"); // (TransformOperator (id_3d142790cd894fffbe31c6a9936a40f9).output) -- [IDataFlow<string>] --> (TransformOperator (id_ba99ef2eb1eb4ab7adfeab8d1b9bfb2b).input)
            id_ba99ef2eb1eb4ab7adfeab8d1b9bfb2b.WireTo(sf1, "output"); // (TransformOperator (id_ba99ef2eb1eb4ab7adfeab8d1b9bfb2b).output) -- [IDataFlow<string>] --> (StringFormat<string,string> (sf1).input0)
            sf1.WireTo(id_b9e566abb4cc42d1a7d3927615231c50, "inputs"); // (StringFormat<string,string> (sf1).inputs) -- [IDataFlowB<string>] --> (DataFlowBNull<string> (id_b9e566abb4cc42d1a7d3927615231c50).output)
            sf1.WireTo(formula, "output"); // (StringFormat<string,string> (sf1).output) -- [IDataFlow<string>] --> (Formula (formula).formula)
            formula.WireTo(dfc1, "result"); // (Formula (formula).result) -- [IDataFlow<double?>] --> (DataFlowConnector<double?> (dfc1).input)
            dfc1.WireTo(id_4c9cb86bce4544fe90c628e9eaecbcec, "outputs"); // (DataFlowConnector<double?> (dfc1).outputs) -- [IDataFlow<T>] --> (NumberToString<double?> (id_4c9cb86bce4544fe90c628e9eaecbcec).input)
            id_4c9cb86bce4544fe90c628e9eaecbcec.WireTo(id_bab796380f6d4c4eb93428662ce78dc2, "output"); // (NumberToString<double?> (id_4c9cb86bce4544fe90c628e9eaecbcec).output) -- [IDataFlow<string>] --> (NumberFormatting (id_bab796380f6d4c4eb93428662ce78dc2).input)
            id_6008429a36ce435da09c8f7c5534800c.WireTo(id_8537240f2a654a788fbc6103c2e3a45f, "output"); // (RegexReplace (id_6008429a36ce435da09c8f7c5534800c).output) -- [IDataFlow<string>] --> (RegexReplace (id_8537240f2a654a788fbc6103c2e3a45f).input)
            id_8537240f2a654a788fbc6103c2e3a45f.WireTo(id_02f042986f3242648a019c5fbf7c8752, "output"); // (RegexReplace (id_8537240f2a654a788fbc6103c2e3a45f).output) -- [IDataFlow<string>] --> (ForceAssociativity (id_02f042986f3242648a019c5fbf7c8752).input)
            id_02f042986f3242648a019c5fbf7c8752.WireTo(formulaRender, "output"); // (ForceAssociativity (id_02f042986f3242648a019c5fbf7c8752).output) -- [IDataFlow<string>] --> (FormulaRender (formulaRender).input)
            id_bab796380f6d4c4eb93428662ce78dc2.WireTo(resultText, "output"); // (NumberFormatting (id_bab796380f6d4c4eb93428662ce78dc2).output) -- [IDataFlow<string>] --> (Text (resultText).textInput)
            id_6448e518651246a3af0d4f7d49c13077.WireTo(format, "children"); // (Vertical (id_6448e518651246a3af0d4f7d49c13077).children) -- [List<IUI>] --> (SelectionBox<FormatModes> (format).child)
            id_6448e518651246a3af0d4f7d49c13077.WireTo(digitsText, "children"); // (Vertical (id_6448e518651246a3af0d4f7d49c13077).children) -- [List<IUI>] --> (TextBox (digitsText).child)
            format.WireTo(id_bab796380f6d4c4eb93428662ce78dc2, "output"); // (SelectionBox<FormatModes> (format).output) -- [IDataFlow<FormatModes>] --> (NumberFormatting (id_bab796380f6d4c4eb93428662ce78dc2).FormatModes>)
            digitsText.WireTo(id_ccfb4bf2e9df48e3a9c4d7f0f34d2d3a, "textOutput"); // (TextBox (digitsText).textOutput) -- [IDataFlow<string>] --> (StringToNumber<int> (id_ccfb4bf2e9df48e3a9c4d7f0f34d2d3a).input)
            id_ccfb4bf2e9df48e3a9c4d7f0f34d2d3a.WireTo(id_bab796380f6d4c4eb93428662ce78dc2, "output"); // (StringToNumber<int> (id_ccfb4bf2e9df48e3a9c4d7f0f34d2d3a).output) -- [IDataFlow<T>] --> (NumberFormatting (id_bab796380f6d4c4eb93428662ce78dc2).digits)
            // END AUTO-GENERATED WIRING FOR CalculatorRow.xmind



            internalIUI = id_6fe26e8021c64d8dad4e5b6016f7b659;

            // id_b9e566abb4cc42d1a7d3927615231c50.WireTo(labelsCommaSeparatedInternalConnector);  // method 1
            labelsCommaSeparatedInternal = id_b9e566abb4cc42d1a7d3927615231c50; // method 2
            // The CalculatorRow label is a border port that is internally wired to the labels DataFlowConnector IDataFlowB allowing it to fan out
            labeldfc = id_65f22d8aa160470e8da02d0fce01edca;  // Dataflow connector on the label

            internalFormula = formula;
            internalResult = dfc1;




            labeldfc.DataChanged += labelChangedHandler;

            // The output result from the formula in the internal wiring goes into a connector so it can fan out to many places.
            // We use the IdataflowB on this connector. IDataFlowB is implemented by the connector and exposes to us a getter so we can get the result. It also exposes an event which we can register to to know when there is a result
            // We register to that event now so we can relay the result to out outside border:
            internalResult.DataChanged += resultChangedHandler;

            // labelsCommaSeparated.DataChanged += labelsCommaSeparatedChangedHandler; // method 1



            internalWiringDone = true;  
            operandsPostWiringInitialize();   // Wire up any operands we already have wired externally. Note that operands can also be wired externally at run-time
            labelsCommaSeparatedPostWiringInitialize();  // Tolerate external wiring being done before or after internal wiring to remove temporal coupling




            internalLabel = labelText;  
            internalFormulaText = formulaText;
            internalResultText = resultText;
            internalUnit = unitsText;
            internalDescription = descriptionText;
            internalFormatDigits = digitsText;
            internalFormatMode = format;
        }



        private bool internalWiringDone = false;
        private int nOperandsWired = 0;




        private void operandsPostWiringInitialize()
        {
            // Wire up any operands we already have wired externally. Note that operands can also be wired externally at run-time
            // We get called both by WireInternal above when it is finished and by the WireTo method every time our operands are externally wired
            // we need to wait until our internal wiring is done. Our operands may get wired externally before or after our internal wiring is done, sometimes both
            // The bool internalWiringDone and the counter nOperandsWired provide the logic for us to handle any order and so not expose any temporal coupling to the outside world
            if (internalWiringDone)
            {
                while (operands!=null && nOperandsWired < operands.Count)
                {
                    IDataFlowB<double?> operand = operands[nOperandsWired];
                    internalFormula.WireTo(operand); // method2 operands is a list of IDataFlowB<doubles> which have by now been wired to multiple external implementors of IDataFlowB<double?>. Wire the internal formulas operands to the same places.
                    nOperandsWired++;
                }
            }
        }



        private void labelsCommaSeparatedPostWiringInitialize()
        {
            // we are called both when internal wiring is complete and when labelsCommaSeparated is wired externally 
            // wait until both internal and external wiring is done - removes temporal coupling being exposed on the outside
            if (internalWiringDone && labelsCommaSeparated!=null)
            {
                labelsCommaSeparatedInternal.WireTo(labelsCommaSeparated);  // method 2  labelsCommaSeparated has already been wired to an external implmentor of IDataFlowB, wire the interal wiring to that same external place. When the event goes off, only the internal wiring one will react and get the data.
            }
        }


        // This must be called after a new row is created and completely wired
        public CalculatorRow Initialize()
        {
            labelChangedHandler();  // Send out our blank label. This allows the external wiring to react to our presence at run-time (example case we are a newly added row, we wont have the commaSeparatedValue at our input until any label is changed to push it through the application wiring
            return this;  // support fluent pattern
        }









        //  Testing interface implementation -------------------------------------------------------------------------------------------------------------------------------------


        // Testing interface
        // This interface faces upward, which means it is intended to be used by our client who also used our public interface to construct us
        // This interface allows the client to insert test data into its wiring

        TextBox internalLabel;
        TextBox internalFormulaText;
        Text internalResultText;
        TextBox internalUnit;
        TextBox internalDescription;
        TextBox internalFormatDigits;
        SelectionBox<FormatModes> internalFormatMode;


        void ITestCalculatorRow.EnterFormula(string formula)
        {
            ((TextBox)internalFormulaText).Text = formula;
        }

        void ITestCalculatorRow.EnterLabel(string label)
        {
            ((TextBox)internalLabel).Text = label;
        }

        string ITestCalculatorRow.ReadResult()
        {
            return ((IDataFlow<string>)internalResultText).Data;
        }

        void ITestCalculatorRow.EnterUnit(string unit)
        {
            ((TextBox)internalUnit).Text = unit;
        }

        void ITestCalculatorRow.EnterDescription(string description)
        {
            ((TextBox)internalDescription).Text = description;
        }

        void ITestCalculatorRow.EnterFormatMode(FormatModes mode)
        {
            ((SelectionBox<FormatModes>)internalFormatMode).Value = mode;
        }

        void ITestCalculatorRow.EnterFormatDigits(string description)
        {
            ((TextBox)internalFormatDigits).Text = description;
        }
    }






    // class CalculatorRowFactory -----------------------------------------------------------------------------------------------------------------------------------------------------------
    // allows CalculatorRow to be instantiated by via the IFactoryMethod interface


    /// <summary>
    /// This small class allows the CalculatorRow class to be instantiated at runtime
    /// It implements the IFactoryMethod which allows an instance of this class to be wired to any generic object that uses IFactoryMethod, for example an instance of Multiple.
    /// If CalculatorRows are static in the diagram just instantiate the CalculatorRow class directly instead
    /// </summary>
    class CalculatorRowFactory : IFactoryMethod
    {
        public string InstanceName { get; set; } = "CalculatorRowFactory";

        object IFactoryMethod.FactoryMethod(string InstanceName, ConstructorCallbackDelegate ConstructorCallbackMethod)
        {
            return (object)new CalculatorRow(ConstructorCallbackMethod) { InstanceName = InstanceName  };
        }
    }






    // Testing interface for calculatorRow class -------------------------------------------------------------------------------------------------------------------------------------
    // This interface allows you to enter values into CalculatorRows textBoxes as if the user had done it, and read out the result from the result Text as if the user was reading it
    // i.e. this interface supports acceptance testing of the application that uses the CalculatorRow class

    interface ITestCalculatorRow
    {
        void EnterFormula(string formula);
        void EnterLabel(string label);
        string ReadResult();
        void EnterUnit(string unit);
        void EnterDescription(string description);
        void EnterFormatMode(FormatModes mode);
        void EnterFormatDigits(string description);
    }


}
