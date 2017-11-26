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
    public enum BehaviorIndexMask : byte
    {
        _0 = 0,
        _0_1,
        _0_1_2,
        _0_1_2_3,
        _0_1_3,
        _0_2,
        _0_2_3,
        _0_3,

        _1,
        _1_2,
        _1_2_3,
        _1_3,

        _2,
        _2_3,

        _3,
    }
    public enum BehaviorCommandType : byte
    {
        SetPokeLed = 0,
        SetPokeValve,
        SetLed,
        SetRgb,
        SetDigitalOutput,

        ClearPokeLed,
        ClearPokeValve,
        ClearLed,
        ClearRgb,
        ClearDigitalOutput,

        OutputsSet,
        OuputsClear,
        OuputsToggle,
        Outputs,

        StartPwm,
        StopPwm,
        PwmFrequency,

        LedCurrent,

        PulsePokeLed,
        PulsePokeValve,
        PulseLed,
        PulseRgb,
        PulseDigitalOutput,

        ColorsRgb,
        ColorsRgbs
    }

    [Description(
        "\n" +
        "SetPokeLed: Any\n" +
        "SetPokeValve: Any\n" +
        "SetLed: Any\n" +
        "SetRgb: Any\n" +
        "SetDigitalOutput: Any\n" +
        "\n" +
        "ClearPokeLed: Any\n" +
        "ClearPokeValve: Any\n" +
        "ClearLed: Any\n" +
        "ClearRgb: Any\n" +
        "ClearDigitalOutput: Any\n" +
        "\n" +
        "OutputsSet: Bitmask\n" +
        "OuputsClear: Bitmask\n" +
        "OuputsToggle: Bitmask\n" +
        "Outputs: Bitmask\n" +
        "\n" +
        "StartPwm: Any\n" +
        "StopPwm: Any\n" +
        "PwmFrequency: Integer\n" +
        "\n" +
        "LedCurrent: Integer\n" +
        "\n" +
        "PulsePokeLed: Integer\n" +
        "PulsePokeValve: Integer\n" +
        "PulseLed: Integer\n" +
        "PulseRgb: Integer\n" +
        "PulseDigitalOutput: Integer\n" +
        "\n" +
        "ColorsRgb: Positive integer array[3] (G,R,B)\n" +
        "ColorsRgb: Positive integer array[6] (G,R,B,G,R,B)\n"
    )]

    public class BehaviorCommand : SelectBuilder, INamedElement
    {
        public BehaviorCommand()
        {
            Type = BehaviorCommandType.SetPokeLed;
            Mask = BehaviorIndexMask._0;
        }

        string INamedElement.Name
        {
            get { return typeof(BehaviorCommand).Name + "." + Type.ToString(); }
        }

        public BehaviorCommandType Type { get; set; }
        public BehaviorIndexMask Mask { get; set; }

    protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                /************************************************************************/
                /* Set                                                                  */
                /************************************************************************/
                case BehaviorCommandType.SetPokeLed:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessSetPokeLed", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.SetPokeValve:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessSetPokeValve", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.SetLed:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessSetLed", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.SetRgb:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessSetRgb", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.SetDigitalOutput:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessSetDigitalOutput", new[] { expression.Type }, expression, GetBitMask());

                /************************************************************************/
                /* Clear                                                                */
                /************************************************************************/
                case BehaviorCommandType.ClearPokeLed:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessClearPokeLed", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.ClearPokeValve:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessClearPokeValve", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.ClearLed:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessClearLed", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.ClearRgb:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessClearRgb", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.ClearDigitalOutput:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessClearDigitalOutput", new[] { expression.Type }, expression, GetBitMask());

                /************************************************************************/
                /* Outputs                                                              */
                /************************************************************************/
                case BehaviorCommandType.OutputsSet:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessOutputsSet", null, expression);
                case BehaviorCommandType.OuputsClear:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessOuputsClear", null, expression);
                case BehaviorCommandType.OuputsToggle:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessOuputsToggle", null, expression);
                case BehaviorCommandType.Outputs:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessOutputs", null, expression);                    

                /************************************************************************/
                /* PWM                                                                  */
                /************************************************************************/
                case BehaviorCommandType.StartPwm:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessStartPwm", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.StopPwm:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessStopPwm", new[] { expression.Type }, expression, GetBitMask());

                case BehaviorCommandType.PwmFrequency:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessPwmFrequency", null, expression, GetBitMask());
                
                /************************************************************************/
                /* LED Current                                                          */
                /************************************************************************/
                case BehaviorCommandType.LedCurrent:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessLedCurrent", null, expression, GetBitMask());

                /************************************************************************/
                /* Pulses                                                               */
                /************************************************************************/
                case BehaviorCommandType.PulsePokeLed:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessPulsePokeLed", null, expression, GetBitMask());
                case BehaviorCommandType.PulsePokeValve:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessPulsePokeValve", null, expression, GetBitMask());
                case BehaviorCommandType.PulseLed:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessPulseLed", null, expression, GetBitMask());
                case BehaviorCommandType.PulseRgb:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessPulseRgb", null, expression, GetBitMask());
                case BehaviorCommandType.PulseDigitalOutput:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessPulseDigitalOutput", null, expression, GetBitMask());
                    
                /************************************************************************/
                /* RGBs                                                                 */
                /************************************************************************/
                case BehaviorCommandType.ColorsRgb:
                    if (expression.Type != typeof(byte[])) { expression = Expression.Convert(expression, typeof(byte[])); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessColorsRgb", null, expression, GetBitMask());
                case BehaviorCommandType.ColorsRgbs:
                    if (expression.Type != typeof(byte[])) { expression = Expression.Convert(expression, typeof(byte[])); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessColorRgbs", null, expression);

                default:
                    break;
            }
            return expression;
        }


        Expression GetBitMask()
        {
            int bitMask = 0;
            switch (Mask)
            {
                case BehaviorIndexMask._0:          bitMask = 0x01; break;
                case BehaviorIndexMask._0_1:        bitMask = 0x03; break;
                case BehaviorIndexMask._0_1_2:      bitMask = 0x07; break;
                case BehaviorIndexMask._0_1_2_3:    bitMask = 0x0F; break;
                case BehaviorIndexMask._0_1_3:      bitMask = 0x0B; break;
                case BehaviorIndexMask._0_2_3:      bitMask = 0x0D; break;
                case BehaviorIndexMask._0_2:        bitMask = 0x05; break;
                case BehaviorIndexMask._0_3:        bitMask = 0x09; break;
                case BehaviorIndexMask._1:          bitMask = 0x02; break;
                case BehaviorIndexMask._1_2:        bitMask = 0x06; break;
                case BehaviorIndexMask._1_2_3:      bitMask = 0x0E; break;
                case BehaviorIndexMask._1_3:        bitMask = 0x0A; break;
                case BehaviorIndexMask._2:          bitMask = 0x04; break;
                case BehaviorIndexMask._2_3:        bitMask = 0x0C; break;
                case BehaviorIndexMask._3:          bitMask = 0x08; break;
            }
            return Expression.Constant(bitMask);
        }

        
        static void checkPokeLed(int bitMask)           { if (bitMask > 7)  throw new InvalidOperationException("Invalid Mask selection."); }
        static void checkPokeValve(int bitMask)         { if (bitMask > 7)  throw new InvalidOperationException("Invalid Mask selection."); }
        static void checkLed(int bitMask)               { if (bitMask > 3)  throw new InvalidOperationException("Invalid Mask selection."); }
        static void checkRgb(int bitMask)               { if (bitMask > 3)  throw new InvalidOperationException("Invalid Mask selection."); }
        static void checkDigitalOutput(int bitMask)     { if (bitMask > 15) throw new InvalidOperationException("Invalid Mask selection."); }
        static void checkPwm(int bitMask)               { if (bitMask > 15) throw new InvalidOperationException("Invalid Mask selection."); }

        static void checkPwmFrequency(int bitMask)      { if (bitMask != 1 && bitMask != 2 && bitMask != 4 && bitMask != 8) throw new InvalidOperationException("Invalid Mask selection. Available options are _0, _1, _2 and _3."); }
        static void checkLedCurrent(int bitMask)        { if (bitMask != 1 && bitMask != 2) throw new InvalidOperationException("Invalid Mask selection. Available options are _0 and _1."); }

        static void checkPulsePokeLed(int bitMask)      { if (bitMask != 1 && bitMask != 2 && bitMask != 4) throw new InvalidOperationException("Invalid Mask selection. Available options are _0, _1 and _2."); }
        static void checkPulsePokeValve(int bitMask)    { if (bitMask != 1 && bitMask != 2 && bitMask != 4) throw new InvalidOperationException("Invalid Mask selection. Available options are _0, _1 and _2."); }
        static void checkPulseLed(int bitMask)          { if (bitMask != 1 && bitMask != 2) throw new InvalidOperationException("Invalid Mask selection. Available options are _0 and _1."); }
        static void checkPulseRgb(int bitMask)          { if (bitMask != 1 && bitMask != 2) throw new InvalidOperationException("Invalid Mask selection. Available options are _0 and _1."); }
        static void checkPulseDigitalOutput(int bitMask) { if (bitMask != 1 && bitMask != 2 && bitMask != 4 && bitMask != 8) throw new InvalidOperationException("Invalid Mask selection. Available options are _0, _1, _2 and _3."); }

        static void checkColorRgb(int bitMask)          { if (bitMask != 1 && bitMask != 2) throw new InvalidOperationException("Invalid Mask selection. Available options are _0 and _1."); }


        static HarpDataFrame createFrameU8(byte registerAddress, int content)
        {
            return HarpDataFrame.UpdateChesksum(new HarpDataFrame(2, 5, registerAddress, 255, (byte)HarpType.U8, (byte)content, 0));
        }
        static HarpDataFrame createFrameU16(byte registerAddress, int content)
        {
            return HarpDataFrame.UpdateChesksum(new HarpDataFrame(2, 6, registerAddress, 255, (byte)HarpType.U16, (byte)(content & 255), (byte)((content >> 8) & 255), 0));
        }


        /************************************************************************/
        /* Set                                                                  */
        /************************************************************************/
        static HarpDataFrame ProcessSetPokeLed<TSource>(TSource input, int bMask)           { checkPokeLed(bMask); bMask = bMask << 0;          return createFrameU16(34, bMask); }
        static HarpDataFrame ProcessSetPokeValve<TSource>(TSource input, int bMask)         { checkPokeValve(bMask); bMask = bMask << 3;        return createFrameU16(34, bMask); }
        static HarpDataFrame ProcessSetLed<TSource>(TSource input, int bMask)               { checkLed(bMask); bMask = bMask << 6;              return createFrameU16(34, bMask); }
        static HarpDataFrame ProcessSetRgb<TSource>(TSource input, int bMask)               { checkRgb(bMask); bMask = bMask << 8;              return createFrameU16(34, bMask); }
        static HarpDataFrame ProcessSetDigitalOutput<TSource>(TSource input, int bMask)     { checkDigitalOutput(bMask); bMask = bMask << 10;   return createFrameU16(34, bMask); }

        /************************************************************************/
        /* Clear                                                                */
        /************************************************************************/
        static HarpDataFrame ProcessClearPokeLed<TSource>(TSource input, int bMask)         { checkPokeLed(bMask); bMask = bMask << 0;          return createFrameU16(35, bMask); }
        static HarpDataFrame ProcessClearPokeValve<TSource>(TSource input, int bMask)       { checkPokeValve(bMask); bMask = bMask << 3;        return createFrameU16(35, bMask); }
        static HarpDataFrame ProcessClearLed<TSource>(TSource input, int bMask)             { checkLed(bMask); bMask = bMask << 6;              return createFrameU16(35, bMask); }
        static HarpDataFrame ProcessClearRgb<TSource>(TSource input, int bMask)             { checkRgb(bMask); bMask = bMask << 8;              return createFrameU16(35, bMask); }
        static HarpDataFrame ProcessClearDigitalOutput<TSource>(TSource input, int bMask)   { checkDigitalOutput(bMask); bMask = bMask << 10;   return createFrameU16(35, bMask); }

        /************************************************************************/
        /* Outputs                                                              */
        /************************************************************************/
        static HarpDataFrame ProcessOutputsSet(UInt16 input)    { return createFrameU16(34, input); }
        static HarpDataFrame ProcessOuputsClear(UInt16 input)   { return createFrameU16(35, input); }
        static HarpDataFrame ProcessOuputsToggle(UInt16 input)  { return createFrameU16(36, input); }
        static HarpDataFrame ProcessOutputs(UInt16 input)       { return createFrameU16(37, input); }

        /************************************************************************/
        /* PWM                                                                  */
        /************************************************************************/
        static HarpDataFrame ProcessStartPwm<TSource>(TSource input, int bMask)             { checkPwm(bMask); bMask = bMask << 0;              return createFrameU8(81, bMask); }
        static HarpDataFrame ProcessStopPwm<TSource>(TSource input, int bMask)              { checkPwm(bMask); bMask = bMask << 0;              return createFrameU8(82, bMask); }

        static HarpDataFrame ProcessPwmFrequency(UInt16 input, int bMask)
        {
            checkPwmFrequency(bMask);
            switch (bMask)
            {
                default:
                case 1: return createFrameU16(73, input);
                case 2: return createFrameU16(74, input);
                case 4: return createFrameU16(75, input);
                case 8: return createFrameU16(76, input);
            }            
        }

        /************************************************************************/
        /* LED Current                                                          */
        /************************************************************************/
        static HarpDataFrame ProcessLedCurrent(byte input, int bMask)
        {
            checkLedCurrent(bMask);
            switch (bMask)
            {
                default:
                case 1: return createFrameU8(86, input);
                case 2: return createFrameU8(87, input);
            }
        }

        /************************************************************************/
        /* Pulses                                                               */
        /************************************************************************/
        static HarpDataFrame ProcessPulsePokeLed(UInt16 input, int bMask)
        {
            checkPulsePokeLed(bMask);
            switch (bMask)
            {
                default:
                case 1: return createFrameU16(59, input);
                case 2: return createFrameU16(60, input);
                case 4: return createFrameU16(61, input);
            }
        }
        static HarpDataFrame ProcessPulsePokeValve(UInt16 input, int bMask)
        {
            checkPulsePokeValve(bMask);
            switch (bMask)
            {
                default:
                case 1: return createFrameU16(62, input);
                case 2: return createFrameU16(63, input);
                case 4: return createFrameU16(64, input);
            }
        }
        static HarpDataFrame ProcessPulseLed(UInt16 input, int bMask)
        {
            checkPulseLed(bMask);
            switch (bMask)
            {
                default:
                case 1: return createFrameU16(65, input);
                case 2: return createFrameU16(66, input);
            }
        }
        static HarpDataFrame ProcessPulseRgb(UInt16 input, int bMask)
        {
            checkPulseRgb(bMask);
            switch (bMask)
            {
                default:
                case 1: return createFrameU16(67, input);
                case 2: return createFrameU16(68, input);
            }
        }
        static HarpDataFrame ProcessPulseDigitalOutput(UInt16 input, int bMask)
        {
            checkPulseDigitalOutput(bMask);
            switch (bMask)
            {
                default:
                case 1: return createFrameU16(69, input);
                case 2: return createFrameU16(70, input);
                case 4: return createFrameU16(71, input);
                case 8: return createFrameU16(72, input);
            }
        }

        /************************************************************************/
        /* Register: LEDS                                                       */
        /************************************************************************/
        static HarpDataFrame ProcessColorRgb(byte[] RGBs, int bMask)
        {
            checkColorRgb(bMask);
            switch (bMask)
            {
                default:
                case 1: return HarpDataFrame.UpdateChesksum(new HarpDataFrame(2, 7, 84, 255, (byte)HarpType.U8, RGBs[0], RGBs[1], RGBs[2], 0));
                case 2: return HarpDataFrame.UpdateChesksum(new HarpDataFrame(2, 7, 85, 255, (byte)HarpType.U8, RGBs[0], RGBs[1], RGBs[2], 0));
            }
        }

        static HarpDataFrame ProcessColorRgbs(byte [] RGBs)
        {
            return HarpDataFrame.UpdateChesksum(new HarpDataFrame(2, 83, 46, 255, (byte)HarpType.U8, RGBs[0], RGBs[1], RGBs[2], RGBs[3], RGBs[4], RGBs[5], 0));
        }
    }
}