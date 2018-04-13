using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.ComponentModel;

namespace Bonsai.Harp.CF
{
    public enum LoadCellsCommandType : byte
    {
        StartAcquisition,
        StopAcquisition,

        OffsetPort0Channel0,
        OffsetPort0Channel1,
        OffsetPort0Channel2,
        OffsetPort0Channel3,
        OffsetPort1Channel0,
        OffsetPort1Channel1,
        OffsetPort1Channel2,
        OffsetPort1Channel3,

        SetOutput0,
        SetOutput1,
        SetOutput2,
        SetOutput3,
        SetOutput4,
        SetOutput5,
        SetOutput6,
        SetOutput7,
        SetOutput8,

        ClearOutput0,
        ClearOutput1,
        ClearOutput2,
        ClearOutput3,
        ClearOutput4,
        ClearOutput5,
        ClearOutput6,
        ClearOutput7,
        ClearOutput8,

        ToggleOutput0,
        ToggleOutput1,
        ToggleOutput2,
        ToggleOutput3,
        ToggleOutput4,
        ToggleOutput5,
        ToggleOutput6,
        ToggleOutput7,
        ToggleOutput8,

        RegisterSetOutputs,
        RegisterClearOutputs,
        RegisterToggleOutputs,
        RegisterOutputs
    }

    [Description(
        "\n" +
        "StartAcquisition: Any\n" +
        "StopAcquisition: Any\n" +
        "\n" +
        "OffsetPort0Channel0: Integer\n" +
        "OffsetPort0Channel1: Integer\n" +
        "OffsetPort0Channel2: Integer\n" +
        "OffsetPort0Channel3: Integer\n" +
        "OffsetPort1Channel0: Integer\n" +
        "OffsetPort1Channel1: Integer\n" +
        "OffsetPort1Channel2: Integer\n" +
        "OffsetPort1Channel3: Integer\n" +
        "\n" +
        "SetOutput0: Any\n" +
        "SetOutput1: Any\n" +
        "SetOutput2: Any\n" +
        "SetOutput3: Any\n" +
        "SetOutput4: Any\n" +
        "SetOutput5: Any\n" +
        "SetOutput6: Any\n" +
        "SetOutput7: Any\n" +
        "SetOutput8: Any\n" +
        "ClearOutput0: Any\n" +
        "ClearOutput1: Any\n" +
        "ClearOutput2: Any\n" +
        "ClearOutput3: Any\n" +
        "ClearOutput4: Any\n" +
        "ClearOutput5: Any\n" +
        "ClearOutput6: Any\n" +
        "ClearOutput7: Any\n" +
        "ClearOutput8: Any\n" +
        "ToggleOutput0: Any\n" +
        "ToggleOutput1: Any\n" +
        "ToggleOutput2: Any\n" +
        "ToggleOutput3: Any\n" +
        "ToggleOutput4: Any\n" +
        "ToggleOutput5: Any\n" +
        "ToggleOutput6: Any\n" +
        "ToggleOutput7: Any\n" +
        "ToggleOutput8: Any\n" +
        "\n" +
        "RegisterSetOutputs: Bitmask\n" +
        "RegisterClearOutputs: Bitmask\n" +
        "RegisterToggleOutputs: Bitmask\n" +
        "RegisterOutputs: Bitmask\n"
    )]
    public class LoadCellsCommand : SelectBuilder, INamedElement
    {
        public LoadCellsCommand()
        {
            Type = LoadCellsCommandType.StartAcquisition;
        }

        string INamedElement.Name
        {
            get { return typeof(LoadCellsCommand).Name.Replace("Command", string.Empty) + "." + Type.ToString(); }
        }

        public LoadCellsCommandType Type { get; set; }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                case LoadCellsCommandType.StartAcquisition:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessStartAcquisition", new[] { expression.Type }, expression);
                case LoadCellsCommandType.StopAcquisition:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessStopAcquisition", new[] { expression.Type }, expression);


                case LoadCellsCommandType.OffsetPort0Channel0:
                    if (expression.Type != typeof(int)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessOffsetPort0Channel0", null, expression);
                case LoadCellsCommandType.OffsetPort0Channel1:
                    if (expression.Type != typeof(int)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessOffsetPort0Channel1", null, expression);
                case LoadCellsCommandType.OffsetPort0Channel2:
                    if (expression.Type != typeof(int)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessOffsetPort0Channel2", null, expression);
                case LoadCellsCommandType.OffsetPort0Channel3:
                    if (expression.Type != typeof(int)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessOffsetPort0Channel3", null, expression);
                case LoadCellsCommandType.OffsetPort1Channel0:
                    if (expression.Type != typeof(int)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessOffsetPort1Channel0", null, expression);
                case LoadCellsCommandType.OffsetPort1Channel1:
                    if (expression.Type != typeof(int)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessOffsetPort1Channel1", null, expression);
                case LoadCellsCommandType.OffsetPort1Channel2:
                    if (expression.Type != typeof(int)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessOffsetPort1Channel2", null, expression);
                case LoadCellsCommandType.OffsetPort1Channel3:
                    if (expression.Type != typeof(int)) { expression = Expression.Convert(expression, typeof(int)); }
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessOffsetPort1Channel3", null, expression);
                    

                case LoadCellsCommandType.SetOutput0:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessSetOutput0", new[] { expression.Type }, expression);
                case LoadCellsCommandType.SetOutput1:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessSetOutput1", new[] { expression.Type }, expression);
                case LoadCellsCommandType.SetOutput2:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessSetOutput2", new[] { expression.Type }, expression);
                case LoadCellsCommandType.SetOutput3:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessSetOutput3", new[] { expression.Type }, expression);
                case LoadCellsCommandType.SetOutput4:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessSetOutput4", new[] { expression.Type }, expression);
                case LoadCellsCommandType.SetOutput5:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessSetOutput5", new[] { expression.Type }, expression);
                case LoadCellsCommandType.SetOutput6:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessSetOutput6", new[] { expression.Type }, expression);
                case LoadCellsCommandType.SetOutput7:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessSetOutput7", new[] { expression.Type }, expression);
                case LoadCellsCommandType.SetOutput8:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessSetOutput8", new[] { expression.Type }, expression);

                case LoadCellsCommandType.ClearOutput0:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessClearOutput0", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ClearOutput1:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessClearOutput1", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ClearOutput2:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessClearOutput2", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ClearOutput3:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessClearOutput3", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ClearOutput4:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessClearOutput4", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ClearOutput5:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessClearOutput5", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ClearOutput6:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessClearOutput6", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ClearOutput7:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessClearOutput7", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ClearOutput8:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessClearOutput8", new[] { expression.Type }, expression);

                case LoadCellsCommandType.ToggleOutput0:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessToggleOutput0", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ToggleOutput1:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessToggleOutput1", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ToggleOutput2:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessToggleOutput2", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ToggleOutput3:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessToggleOutput3", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ToggleOutput4:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessToggleOutput4", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ToggleOutput5:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessToggleOutput5", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ToggleOutput6:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessToggleOutput6", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ToggleOutput7:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessToggleOutput7", new[] { expression.Type }, expression);
                case LoadCellsCommandType.ToggleOutput8:
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessToggleOutput8", new[] { expression.Type }, expression);

                case LoadCellsCommandType.RegisterSetOutputs:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessRegisterSetOutputs", null, expression);
                case LoadCellsCommandType.RegisterClearOutputs:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessClearDigitalOutputs", null, expression);
                case LoadCellsCommandType.RegisterToggleOutputs:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessRegisterToggleOutputs", null, expression);
                case LoadCellsCommandType.RegisterOutputs:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(LoadCellsCommand), "ProcessRegisterOutputs", null, expression);

                default:
                    break;
            }
            return expression;
        }
        

        static HarpMessage ProcessStartAcquisition<TSource>(TSource input) {
            return new HarpMessage(true, 2, 5, 32, 255, (byte)PayloadType.U8, 1, 0);
        }
        static HarpMessage ProcessStopAcquisition<TSource>(TSource input) {
            return new HarpMessage(true, 2, 5, 32, 255, (byte)PayloadType.U8, 0, 0);
        }


        static HarpMessage ProcessOffsetPort0Channel0(int input) {            
            return new HarpMessage(true, 2, 6, 48, 255, (byte)PayloadType.S16, (byte)(Convert.ToInt16(input) & 255), (byte)((Convert.ToInt16(input) >> 8) & 255), 0);
        }
        static HarpMessage ProcessOffsetPort0Channel1(int input) {
            return new HarpMessage(true, 2, 6, 49, 255, (byte)PayloadType.S16, (byte)(Convert.ToInt16(input) & 255), (byte)((Convert.ToInt16(input) >> 8) & 255), 0);
        }
        static HarpMessage ProcessOffsetPort0Channel2(int input) {
            return new HarpMessage(true, 2, 6, 50, 255, (byte)PayloadType.S16, (byte)(Convert.ToInt16(input) & 255), (byte)((Convert.ToInt16(input) >> 8) & 255), 0);
        }
        static HarpMessage ProcessOffsetPort0Channel3(int input) {
            return new HarpMessage(true, 2, 6, 51, 255, (byte)PayloadType.S16, (byte)(Convert.ToInt16(input) & 255), (byte)((Convert.ToInt16(input) >> 8) & 255), 0);
        }
        static HarpMessage ProcessOffsetPort1Channel0(int input) {            
            return new HarpMessage(true, 2, 6, 52, 255, (byte)PayloadType.S16, (byte)(Convert.ToInt16(input) & 255), (byte)((Convert.ToInt16(input) >> 8) & 255), 0);
        }
        static HarpMessage ProcessOffsetPort1Channel1(int input) {
            return new HarpMessage(true, 2, 6, 53, 255, (byte)PayloadType.S16, (byte)(Convert.ToInt16(input) & 255), (byte)((Convert.ToInt16(input) >> 8) & 255), 0);
        }
        static HarpMessage ProcessOffsetPort1Channel2(int input) {
            return new HarpMessage(true, 2, 6, 54, 255, (byte)PayloadType.S16, (byte)(Convert.ToInt16(input) & 255), (byte)((Convert.ToInt16(input) >> 8) & 255), 0);
        }
        static HarpMessage ProcessOffsetPort1Channel3(int input) {
            return new HarpMessage(true, 2, 6, 55, 255, (byte)PayloadType.S16, (byte)(Convert.ToInt16(input) & 255), (byte)((Convert.ToInt16(input) >> 8) & 255), 0);
        }
        

        static HarpMessage ProcessSetOutput0<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 42, 255, (byte)PayloadType.U16,   1, 0, 0); }
        static HarpMessage ProcessSetOutput1<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 42, 255, (byte)PayloadType.U16,   2, 0, 0); }
        static HarpMessage ProcessSetOutput2<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 42, 255, (byte)PayloadType.U16,   4, 0, 0); }
        static HarpMessage ProcessSetOutput3<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 42, 255, (byte)PayloadType.U16,   8, 0, 0); }
        static HarpMessage ProcessSetOutput4<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 42, 255, (byte)PayloadType.U16,  16, 0, 0); }
        static HarpMessage ProcessSetOutput5<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 42, 255, (byte)PayloadType.U16,  32, 0, 0); }
        static HarpMessage ProcessSetOutput6<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 42, 255, (byte)PayloadType.U16,  64, 0, 0); }
        static HarpMessage ProcessSetOutput7<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 42, 255, (byte)PayloadType.U16, 128, 0, 0); }
        static HarpMessage ProcessSetOutput8<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 42, 255, (byte)PayloadType.U16,   0, 1, 0); }
        
        static HarpMessage ProcessClearOutput0<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 43, 255, (byte)PayloadType.U16,   1, 0, 0); }
        static HarpMessage ProcessClearOutput1<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 43, 255, (byte)PayloadType.U16,   2, 0, 0); }
        static HarpMessage ProcessClearOutput2<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 43, 255, (byte)PayloadType.U16,   4, 0, 0); }
        static HarpMessage ProcessClearOutput3<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 43, 255, (byte)PayloadType.U16,   8, 0, 0); }
        static HarpMessage ProcessClearOutput4<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 43, 255, (byte)PayloadType.U16,  16, 0, 0); }
        static HarpMessage ProcessClearOutput5<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 43, 255, (byte)PayloadType.U16,  32, 0, 0); }
        static HarpMessage ProcessClearOutput6<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 43, 255, (byte)PayloadType.U16,  64, 0, 0); }
        static HarpMessage ProcessClearOutput7<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 43, 255, (byte)PayloadType.U16, 128, 0, 0); }
        static HarpMessage ProcessClearOutput8<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 43, 255, (byte)PayloadType.U16,   0, 1, 0); }
        
        static HarpMessage ProcessToggleOutput0<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 44, 255, (byte)PayloadType.U16,   1, 0, 0); }
        static HarpMessage ProcessToggleOutput1<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 44, 255, (byte)PayloadType.U16,   2, 0, 0); }
        static HarpMessage ProcessToggleOutput2<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 44, 255, (byte)PayloadType.U16,   4, 0, 0); }
        static HarpMessage ProcessToggleOutput3<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 44, 255, (byte)PayloadType.U16,   8, 0, 0); }
        static HarpMessage ProcessToggleOutput4<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 44, 255, (byte)PayloadType.U16,  16, 0, 0); }
        static HarpMessage ProcessToggleOutput5<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 44, 255, (byte)PayloadType.U16,  32, 0, 0); }
        static HarpMessage ProcessToggleOutput6<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 44, 255, (byte)PayloadType.U16,  64, 0, 0); }
        static HarpMessage ProcessToggleOutput7<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 44, 255, (byte)PayloadType.U16, 128, 0, 0); }
        static HarpMessage ProcessToggleOutput8<TSource>(TSource input) { return new HarpMessage(true, 2, 6, 44, 255, (byte)PayloadType.U16,   0, 1, 0); }

        static HarpMessage ProcessRegisterSetOutputs(UInt16 input) {
            return new HarpMessage(true, 2, 6, 42, 255, (byte)PayloadType.U16, (byte)(Convert.ToUInt16(input) & 255), (byte)((Convert.ToUInt16(input) >> 8) & 255), 0);
        }
        static HarpMessage ProcessClearDigitalOutputs(UInt16 input) {
            return new HarpMessage(true, 2, 6, 43, 255, (byte)PayloadType.U16, (byte)(Convert.ToUInt16(input) & 255), (byte)((Convert.ToUInt16(input) >> 8) & 255), 0);
        }
        static HarpMessage ProcessRegisterToggleOutputs(UInt16 input) {
            return new HarpMessage(true, 2, 6, 44, 255, (byte)PayloadType.U16, (byte)(Convert.ToUInt16(input) & 255), (byte)((Convert.ToUInt16(input) >> 8) & 255), 0);
        }
        static HarpMessage ProcessRegisterOutputs(UInt16 input) {
            return new HarpMessage(true, 2, 6, 45, 255, (byte)PayloadType.U16, (byte)(Convert.ToUInt16(input) & 255), (byte)((Convert.ToUInt16(input) >> 8) & 255), 0);
        }
    }
}
