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
    [Flags]
    public enum BehaviorPorts : UInt16
    {
        PokeLed0 = 1,
        PokeLed1 = 2,
        PokeLed2 = 4,
        PokeValve0 = 8,
        PokeValve1 = 16,
        PokeValve2 = 32,
        Led0 = 64,
        Led1 = 128,
        Rgb0 = 256,
        Rgb1 = 512,
        Digital0 = 1024,
        Digital1 = 2048,
        Digital2 = 4096,
        Digital3 = 8192
    }

    public enum BehaviorCommandType : byte
    {
        SetOutput,
        ClearOutput,
        ToggleOutput,

        StartPwm,
        StopPwm,
        WritePwmFrequency,

        WriteLedCurrent,

        WritePulsePeriod,

        WriteColorsRgb,
        WriteColorsRgbs,

        RegisterSetOutput,
        RegisterClearOutput,
        RegisterToggleOutput,
        RegisterStartPwm,
        RegisterStopPwm
    }

    [Description(
        "\n" +
        "SetOutput: Any\n" +
        "ClearOutput: Any\n" +
        "ToggleOutput: Any\n" +
        "\n" +
        "StartPwm: Any\n" +
        "StopPwm: Any\n" +
        "WritePwmFrequency: Integer\n" +
        "\n" +
        "WriteLedCurrent: Integer\n" +
        "\n" +
        "WritePulsePeriod: Integer\n" +
        "\n" +
        "WriteColorsRgb: Positive integer array[3] (G,R,B)\n" +
        "WriteColorsRgbs: Positive integer array[6] (G,R,B,G,R,B)\n" +
        "\n" +
        "RegisterSetOutput: Bitmask U16\n" +
        "RegisterClearOutput: Bitmask U16\n" +
        "RegisterToggleOutput: Bitmask U16\n" +
        "RegisterStartPwm: Bitmask U8\n" +
        "RegisterStopPwm: Bitmask U8\n"
    )]

    public class BehaviorCommand : SelectBuilder, INamedElement
    {
        public BehaviorCommand()
        {
            Type = BehaviorCommandType.SetOutput;
            Mask = BehaviorPorts.PokeLed0;
        }

        string INamedElement.Name
        {
            get { return typeof(BehaviorCommand).Name.Replace("Command", string.Empty) + "." + Type.ToString(); }
        }

        public BehaviorCommandType Type { get; set; }
        public BehaviorPorts Mask { get; set; }

    protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                /************************************************************************/
                /* Outputs                                                              */
                /************************************************************************/
                case BehaviorCommandType.SetOutput:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessSetOutput", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.ClearOutput:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessClearOutput", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.ToggleOutput:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessToggleOutput", new[] { expression.Type }, expression, GetBitMask());

                case BehaviorCommandType.RegisterSetOutput:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessRegisterSetOutput", null, expression);
                case BehaviorCommandType.RegisterClearOutput:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessRegisterClearOutput", null, expression);
                case BehaviorCommandType.RegisterToggleOutput:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessRegisterToggleOutput", null, expression);

                /************************************************************************/
                /* Pwm                                                                  */
                /************************************************************************/
                case BehaviorCommandType.StartPwm:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessStartPwm", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.StopPwm:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessStopPwm", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.WritePwmFrequency:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessWritePwmFrequency", null, expression, GetBitMask());

                case BehaviorCommandType.RegisterStartPwm:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessRegisterStartPwm", null, expression);
                case BehaviorCommandType.RegisterStopPwm:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessRegisterStopPwm", null, expression);

                /************************************************************************/
                /* Led                                                                  */
                /************************************************************************/
                case BehaviorCommandType.WriteLedCurrent:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessWriteLedCurrent", null, expression, GetBitMask());

               /************************************************************************/
                /* Pulse Period                                                         */
                /************************************************************************/
                case BehaviorCommandType.WritePulsePeriod:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessWritePulsePeriod", null, expression, GetBitMask());

                /************************************************************************/
                /* RGBs                                                                 */
                /************************************************************************/
                case BehaviorCommandType.WriteColorsRgb:
                    if (expression.Type != typeof(byte[])) { expression = Expression.Convert(expression, typeof(byte[])); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessWriteColorsRgb", null, expression, GetBitMask());

                case BehaviorCommandType.WriteColorsRgbs:
                    if (expression.Type != typeof(byte[])) { expression = Expression.Convert(expression, typeof(byte[])); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessWriteColorsRgbs", null, expression);

                default:
                    break;
            }
            return expression;
        }
        
        /************************************************************************/
        /* Local functions                                                      */
        /************************************************************************/
        Expression GetBitMask()
        {
            return Expression.Convert(Expression.Constant(Mask), typeof(int));
        }
        
        static HarpDataFrame createFrameU8(byte registerAddress, int content)
        {
            return HarpDataFrame.UpdateChesksum(new HarpDataFrame(2, 5, registerAddress, 255, (byte)HarpType.U8, (byte)content, 0));
        }
        static HarpDataFrame createFrameU16(byte registerAddress, int content)
        {
            return HarpDataFrame.UpdateChesksum(new HarpDataFrame(2, 6, registerAddress, 255, (byte)HarpType.U16, (byte)(content & 255), (byte)((content >> 8) & 255), 0));
        }


        /************************************************************************/
        /* Outputs                                                              */
        /************************************************************************/
        static HarpDataFrame ProcessSetOutput<TSource>(TSource input, int bMask)            { return createFrameU16(34, bMask); }
        static HarpDataFrame ProcessClearOutput<TSource>(TSource input, int bMask)          { return createFrameU16(35, bMask); }
        static HarpDataFrame ProcessToggleOutput<TSource>(TSource input, int bMask)         { return createFrameU16(36, bMask); }

        static HarpDataFrame ProcessRegisterSetOutput(UInt16 input)                         { return createFrameU16(34, input); }
        static HarpDataFrame ProcessRegisterClearOutput(UInt16 input)                       { return createFrameU16(35, input); }
        static HarpDataFrame ProcessRegisterToggleOutput(UInt16 input)                      { return createFrameU16(36, input); }

        /************************************************************************/
        /* Pwm                                                                  */
        /************************************************************************/
        static HarpDataFrame ProcessStartPwm<TSource>(TSource input, int bMask)
        {
            if (bMask < 1024 || bMask > 15360)
                throw new InvalidOperationException("Invalid Mask selection. Only Digital0, Digital1, Digital2 and/or Digital3 can be selected.");

            return createFrameU8(81, bMask);
        }
        static HarpDataFrame ProcessStopPwm<TSource>(TSource input, int bMask)
        {
            if (bMask < 1024 || bMask > 15360)
                throw new InvalidOperationException("Invalid Mask selection. Only Digital0, Digital1, Digital2 and/or Digital3 can be selected.");

            return createFrameU8(82, bMask);
        }

        static HarpDataFrame ProcessWritePwmFrequency(UInt16 input, int bMask)
        {
            switch (bMask)
            {
                case (UInt16)BehaviorPorts.Digital0: return createFrameU16(73, input);
                case (UInt16)BehaviorPorts.Digital1: return createFrameU16(74, input);
                case (UInt16)BehaviorPorts.Digital2: return createFrameU16(75, input);
                case (UInt16)BehaviorPorts.Digital3: return createFrameU16(76, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Digital0, Digital1, Digital2 or Digital3 can be selected.");
            }
        }

        static HarpDataFrame ProcessRegisterStartPwm(byte input)                          { return createFrameU16(81, input); }
        static HarpDataFrame ProcessRegisterStopPwm(byte input)                           { return createFrameU16(82, input); }

        /************************************************************************/
        /* Led                                                                  */
        /************************************************************************/
        static HarpDataFrame ProcessWriteLedCurrent(byte input, int bMask)
        {
            switch (bMask)
            {
                case (UInt16)BehaviorPorts.Led0: return createFrameU8(86, input);
                case (UInt16)BehaviorPorts.Led1: return createFrameU8(86, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Led0 or Led1 can be selected.");
            }
        }

        /************************************************************************/
        /* Pulse Period                                                         */
        /************************************************************************/
        static HarpDataFrame ProcessWritePulsePeriod(UInt16 input, int bMask)
        {
            switch (bMask)
            {
                case (UInt16)BehaviorPorts.PokeLed0: return createFrameU16(59, input);
                case (UInt16)BehaviorPorts.PokeLed1: return createFrameU16(60, input);
                case (UInt16)BehaviorPorts.PokeLed2: return createFrameU16(61, input);
                case (UInt16)BehaviorPorts.PokeValve0: return createFrameU16(62, input);
                case (UInt16)BehaviorPorts.PokeValve1: return createFrameU16(63, input);
                case (UInt16)BehaviorPorts.PokeValve2: return createFrameU16(64, input);
                case (UInt16)BehaviorPorts.Led0: return createFrameU16(65, input);
                case (UInt16)BehaviorPorts.Led1: return createFrameU16(66, input);
                case (UInt16)BehaviorPorts.Rgb0: return createFrameU16(67, input);
                case (UInt16)BehaviorPorts.Rgb1: return createFrameU16(68, input);
                case (UInt16)BehaviorPorts.Digital0: return createFrameU16(69, input);
                case (UInt16)BehaviorPorts.Digital1: return createFrameU16(70, input);
                case (UInt16)BehaviorPorts.Digital2: return createFrameU16(71, input);
                case (UInt16)BehaviorPorts.Digital3: return createFrameU16(71, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only one can be selected.");
            }
        }

        /************************************************************************/
        /* RGbs                                                                 */
        /************************************************************************/
        static HarpDataFrame ProcessWriteColorsRgb(byte[] RGBs, int bMask)
        {
            switch (bMask)
            {
                case (UInt16)BehaviorPorts.Rgb0:
                    return HarpDataFrame.UpdateChesksum(new HarpDataFrame(2, 7, 84, 255, (byte)HarpType.U8, RGBs[0], RGBs[1], RGBs[2], 0));
                case (UInt16)BehaviorPorts.Rgb1:
                    return HarpDataFrame.UpdateChesksum(new HarpDataFrame(2, 7, 85, 255, (byte)HarpType.U8, RGBs[0], RGBs[1], RGBs[2], 0));
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Rgb0 or Rgb1 can be selected.");
            }
        }

        static HarpDataFrame ProcessWriteColorsRgbs(byte [] RGBs)
        {
            return HarpDataFrame.UpdateChesksum(new HarpDataFrame(2, 83, 46, 255, (byte)HarpType.U8, RGBs[0], RGBs[1], RGBs[2], RGBs[3], RGBs[4], RGBs[5], 0));
        }
    }
}