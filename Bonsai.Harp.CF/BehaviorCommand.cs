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
    //          0  1     2     3  4     5     6    7     8     9    10   11   12   13   14 15
    // [0-15]   P0 P0Led P0Val P1 P1Led P1Val P2   P2Led P2Val -    -    -    -    -    -  -
    // [16-31]  -  -     -     -  -     -     Led0 Led1  Rgb0  Rgb1 Dig0 Dig1 Dig2 Dig3 -  -
    [Flags]
    public enum BehaviorPorts : UInt32
    {
        Port0 =         (1 << 0),
        Poke0Led =      (1 << 1),
        Poke0Valve =    (1 << 2),
        Port1 =         (1 << 3),
        Poke1Led =      (1 << 4),
        Poke1Valve =    (1 << 5),
        Port2 =         (1 << 6),
        Poke2Led =      (1 << 7),
        Poke2Valve =    (1 << 8),
        Led0 =          (1 << 6)  << 16,
        Led1 =          (1 << 7)  << 16,
        Rgb0 =          (1 << 8)  << 16,
        Rgb1 =          (1 << 9)  << 16,
        Digital0 =      (1 << 10) << 16,
        Digital1 =      (1 << 11) << 16,
        Digital2 =      (1 << 12) << 16,
        Digital3 =      (1 << 13) << 16,
    }

    public enum BehaviorCommandType : byte
    {
        SetOutput = 0,
        ClearOutput,
        ToggleOutput,

        PulsePeriod,

        StartPwm,
        StopPwm,
        PwmFrequency,
        PwmDutyCycle,

        LedCurrent,

        ColorsRgb,
        ColorsRgbs,

        //SetInputOutput,
        //ClearInputOutput,
        //ToggleInputOutput,

        StartCamera,
        StopCamera,
        EnableServo,
        DisableServo,
        ServoPosition,
        ResetQuadratureCounter,
        UpdateQuadratureCounter,

        RegisterSetOutputs,
        RegisterClearOutputs,
        RegisterToggleOutputs,
        //RegisterSetInputsOutputs,
        //RegisterClearInputsOutputs,
        //RegisterToggleInputsOutputs,
        RegisterStartPwm,
        RegisterStopPwm
    }

    [Description(
        "\n" +
        "SetOutput: Any\n" +
        "ClearOutput: Any\n" +
        "ToggleOutput: Any\n" +
        "\n" +
        "PulsePeriod: Positive integer (ms)\n" +
        "\n" +
        "StartPwm: Any\n" +
        "StopPwm: Any\n" +
        "PwmFrequency: Positive integer (Hz)\n" +
        "PwmDutyCycle: Positive integer\n" +
        "\n" +
        "LedCurrent: Integer (mA)\n" +
        "\n" +
        "ColorsRgb: Positive integer array[3] (R,G,B)\n" +
        "ColorsRgbs: Positive integer array[6] (R,G,B,R,G,B)\n" +
        "\n" +
        "StartCamera: Any\n" +
        "StopCamera: Any\n" +
        "EnableServo: Any\n" +
        "DisableServo: Any\n" +
        "\n" +
        "ServoPosition: Positive integer\n" +
        "ResetQuadratureCounter: Any\n" +
        "UpdateQuadratureCounter: Integer\n" +
        "\n" +
        "RegisterSetOutputs: Bitmask\n" +
        "RegisterClearOutputs: Bitmask\n" +
        "RegisterToggleOutputs: Bitmask\n" +
        "RegisterStartPwm: Bitmask\n" +
        "RegisterStopPwm: Bitmask\n"
    )]

    public class BehaviorCommand : SelectBuilder, INamedElement
    {
        public BehaviorCommand()
        {
            Type = BehaviorCommandType.SetOutput;
            Mask = BehaviorPorts.Port0;
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

                case BehaviorCommandType.RegisterSetOutputs:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessRegisterSetOutputs", null, expression);
                case BehaviorCommandType.RegisterClearOutputs:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessRegisterClearOutputs", null, expression);
                case BehaviorCommandType.RegisterToggleOutputs:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessRegisterToggleOutputs", null, expression);

                /************************************************************************/
                /* Pulse Period                                                         */
                /************************************************************************/
                case BehaviorCommandType.PulsePeriod:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessPulsePeriod", null, expression, GetBitMask());

                /************************************************************************/
                /* Pwm                                                                  */
                /************************************************************************/
                case BehaviorCommandType.StartPwm:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessStartPwm", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.StopPwm:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessStopPwm", new[] { expression.Type }, expression, GetBitMask());

                case BehaviorCommandType.PwmFrequency:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessPwmFrequency", null, expression, GetBitMask());
                case BehaviorCommandType.PwmDutyCycle:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessPwmDutyCycle", null, expression, GetBitMask());

                case BehaviorCommandType.RegisterStartPwm:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessRegisterStartPwm", null, expression);
                case BehaviorCommandType.RegisterStopPwm:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessRegisterStopPwm", null, expression);

                /************************************************************************/
                /* Special device functions                                             */
                /************************************************************************/
                case BehaviorCommandType.StartCamera:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessStartCamera", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.StopCamera:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessStopCamera", new[] { expression.Type }, expression, GetBitMask());

                case BehaviorCommandType.EnableServo:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessEnableServo", new[] { expression.Type }, expression, GetBitMask());
                case BehaviorCommandType.DisableServo:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessDisableServo", new[] { expression.Type }, expression, GetBitMask());

                case BehaviorCommandType.ServoPosition:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessServoPosition", null, expression, GetBitMask());
                case BehaviorCommandType.UpdateQuadratureCounter:
                    if (expression.Type != typeof(UInt16)) { expression = Expression.Convert(expression, typeof(UInt16)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessUpdateQuadratureCounter", null, expression, GetBitMask());
                case BehaviorCommandType.ResetQuadratureCounter:
                    return Expression.Call(typeof(BehaviorCommand), "ProcessResetQuadratureCounter", new[] { expression.Type }, expression, GetBitMask());

                /************************************************************************/
                /* Led                                                                  */
                /************************************************************************/
                case BehaviorCommandType.LedCurrent:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessLedCurrent", null, expression, GetBitMask());

                /************************************************************************/
                /* RGBs                                                                 */
                /************************************************************************/
                case BehaviorCommandType.ColorsRgb:
                    if (expression.Type != typeof(byte[])) { expression = Expression.Convert(expression, typeof(byte[])); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessColorsRgb", null, expression, GetBitMask());

                case BehaviorCommandType.ColorsRgbs:
                    if (expression.Type != typeof(byte[])) { expression = Expression.Convert(expression, typeof(byte[])); }
                    return Expression.Call(typeof(BehaviorCommand), "ProcessColorsRgbs", null, expression);

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
            return Expression.Convert(Expression.Constant(Mask), typeof(UInt32));
        }

        static UInt16 GetRegOutputs(UInt32 BonsaiBitMaks)
        {
            UInt16 regOutputs = (UInt16)((BonsaiBitMaks & 0xFF00) >> 16);

            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Port0)) == (UInt32)(BehaviorPorts.Port0)) ? (UInt16)(1 << 0) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Port1)) == (UInt32)(BehaviorPorts.Port1)) ? (UInt16)(1 << 1) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Port2)) == (UInt32)(BehaviorPorts.Port2)) ? (UInt16)(1 << 2) : (UInt16)0;

            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Poke0Led)) == (UInt32)(BehaviorPorts.Poke0Led)) ? (UInt16)(1 << 0) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Poke1Led)) == (UInt32)(BehaviorPorts.Poke1Led)) ? (UInt16)(1 << 1) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Poke2Led)) == (UInt32)(BehaviorPorts.Poke2Led)) ? (UInt16)(1 << 2) : (UInt16)0;

            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Poke0Valve)) == (UInt32)(BehaviorPorts.Poke0Valve)) ? (UInt16)(1 << 3) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Poke1Valve)) == (UInt32)(BehaviorPorts.Poke1Valve)) ? (UInt16)(1 << 4) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Poke2Valve)) == (UInt32)(BehaviorPorts.Poke2Valve)) ? (UInt16)(1 << 5) : (UInt16)0;

            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Led0)) == (UInt32)(BehaviorPorts.Led0)) ? (UInt16)(1 << 6) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Led1)) == (UInt32)(BehaviorPorts.Led1)) ? (UInt16)(1 << 7) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Rgb0)) == (UInt32)(BehaviorPorts.Rgb0)) ? (UInt16)(1 << 8) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Rgb1)) == (UInt32)(BehaviorPorts.Rgb1)) ? (UInt16)(1 << 9) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Digital0)) == (UInt32)(BehaviorPorts.Digital0)) ? (UInt16)(1 << 10) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Digital1)) == (UInt32)(BehaviorPorts.Digital1)) ? (UInt16)(1 << 11) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Digital2)) == (UInt32)(BehaviorPorts.Digital2)) ? (UInt16)(1 << 12) : (UInt16)0;
            regOutputs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorPorts.Digital3)) == (UInt32)(BehaviorPorts.Digital3)) ? (UInt16)(1 << 13) : (UInt16)0;

            return regOutputs;
        }

        static byte GetRegDios(UInt32 BonsaiBitMaks)
        {
            byte regDios;

            regDios = ((UInt16)(BonsaiBitMaks & (UInt16)(BehaviorPorts.Port0)) == (UInt16)(BehaviorPorts.Port0)) ? (byte)(1 << 0) : (byte)0;
            regDios |= ((UInt16)(BonsaiBitMaks & (UInt16)(BehaviorPorts.Port1)) == (UInt16)(BehaviorPorts.Port1)) ? (byte)(1 << 1) : (byte)0;
            regDios |= ((UInt16)(BonsaiBitMaks & (UInt16)(BehaviorPorts.Port2)) == (UInt16)(BehaviorPorts.Port2)) ? (byte)(1 << 2) : (byte)0;

            return regDios;
        }

            static HarpMessage createFrameU8(byte registerAddress, int content)
        {
            return new HarpMessage(true, 2, 5, registerAddress, 255, (byte)PayloadType.U8, (byte)content, 0);
        }
        static HarpMessage createFrameU16(byte registerAddress, int content)
        {
            return new HarpMessage(true, 2, 6, registerAddress, 255, (byte)PayloadType.U16, (byte)(content & 255), (byte)((content >> 8) & 255), 0);
        }


        /************************************************************************/
        /* Outputs                                                              */
        /************************************************************************/
        static HarpMessage ProcessSetOutput<TSource>(TSource input, UInt32 bMask)       { return createFrameU16(34, GetRegOutputs(bMask)); }
        static HarpMessage ProcessClearOutput<TSource>(TSource input, UInt32 bMask)     { return createFrameU16(35, GetRegOutputs(bMask)); }
        static HarpMessage ProcessToggleOutput<TSource>(TSource input, UInt32 bMask)    { return createFrameU16(36, GetRegOutputs(bMask)); }

        static HarpMessage ProcessRegisterSetOutputs(UInt16 input)                      { return createFrameU16(34, input); }
        static HarpMessage ProcessRegisterClearOutputs(UInt16 input)                    { return createFrameU16(35, input); }
        static HarpMessage ProcessRegisterToggleOutputs(UInt16 input)                   { return createFrameU16(36, input); }

        /************************************************************************/
        /* Pulse Period                                                         */
        /************************************************************************/
        static HarpMessage ProcessPulsePeriod(UInt16 input, UInt32 bMask)
        {
            switch (bMask)
            {
                case (UInt32)BehaviorPorts.Port0: return createFrameU16(46, input);
                case (UInt32)BehaviorPorts.Port1: return createFrameU16(47, input);
                case (UInt32)BehaviorPorts.Port2: return createFrameU16(48, input);

                case (UInt32)BehaviorPorts.Poke0Led: return createFrameU16(46, input);
                case (UInt32)BehaviorPorts.Poke1Led: return createFrameU16(47, input);
                case (UInt32)BehaviorPorts.Poke2Led: return createFrameU16(48, input);

                case (UInt32)BehaviorPorts.Poke0Valve: return createFrameU16(49, input);
                case (UInt32)BehaviorPorts.Poke1Valve: return createFrameU16(50, input);
                case (UInt32)BehaviorPorts.Poke2Valve: return createFrameU16(51, input);

                case (UInt32)BehaviorPorts.Led0: return createFrameU16(52, input);
                case (UInt32)BehaviorPorts.Led1: return createFrameU16(53, input);
                case (UInt32)BehaviorPorts.Rgb0: return createFrameU16(54, input);
                case (UInt32)BehaviorPorts.Rgb1: return createFrameU16(55, input);
                case (UInt32)BehaviorPorts.Digital0: return createFrameU16(56, input);
                case (UInt32)BehaviorPorts.Digital1: return createFrameU16(57, input);
                case (UInt32)BehaviorPorts.Digital2: return createFrameU16(58, input);
                case (UInt32)BehaviorPorts.Digital3: return createFrameU16(59, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only one option can be selected.");
            }
        }

        /************************************************************************/
        /* Pwm                                                                  */
        /************************************************************************/
        static HarpMessage ProcessStartPwm<TSource>(TSource input, UInt32 bMask)
        {
            if (((GetRegOutputs(bMask) & ~((UInt16)((1 << 10) | (1 << 11) | (1 << 12) | (1 << 13))))) > 0)
                throw new InvalidOperationException("Invalid Mask selection. Any combination of Digital0, Digital1, Digital2 and/or Digital3 can be selected.");

            return createFrameU8(68, GetRegOutputs(bMask) >> 10);
        }
        static HarpMessage ProcessStopPwm<TSource>(TSource input, UInt32 bMask)
        {
            if (((GetRegOutputs(bMask) & ~((UInt16)((1 << 10) | (1 << 11) | (1 << 12) | (1 << 13))))) > 0)
                throw new InvalidOperationException("Invalid Mask selection. Any combination of Digital0, Digital1, Digital2 and/or Digital3 can be selected.");

            return createFrameU8(69, GetRegOutputs(bMask) >> 10);
        }

        static HarpMessage ProcessPwmFrequency(UInt16 input, UInt32 bMask)
        {
            switch (bMask)
            {
                case (UInt32)BehaviorPorts.Digital0: return createFrameU16(60, input);
                case (UInt32)BehaviorPorts.Digital1: return createFrameU16(61, input);
                case (UInt32)BehaviorPorts.Digital2: return createFrameU16(62, input);
                case (UInt32)BehaviorPorts.Digital3: return createFrameU16(63, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Digital0 or Digital1 or Digital2 or Digital3 can be individually selected.");
            }
        }

        static HarpMessage ProcessPwmDutyCycle(byte input, UInt32 bMask)
        {
            switch (bMask)
            {
                case (UInt32)BehaviorPorts.Digital0: return createFrameU8(64, input);
                case (UInt32)BehaviorPorts.Digital1: return createFrameU8(65, input);
                case (UInt32)BehaviorPorts.Digital2: return createFrameU8(66, input);
                case (UInt32)BehaviorPorts.Digital3: return createFrameU8(67, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Digital0 or Digital1 or Digital2 or Digital3 can be individually selected.");
            }
        }

        static HarpMessage ProcessRegisterStartPwm(byte input) { return createFrameU8(68, input); }
        static HarpMessage ProcessRegisterStopPwm(byte input) { return createFrameU8(69, input); }

        /************************************************************************/
        /* Led                                                                  */
        /************************************************************************/
        static HarpMessage ProcessLedCurrent(byte input, UInt32 bMask)
        {
            switch (bMask)
            {
                case (UInt32)BehaviorPorts.Led0: return createFrameU8(73, input);
                case (UInt32)BehaviorPorts.Led1: return createFrameU8(74, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Led0 or Led1 can be individually selected.");
            }
        }

        /************************************************************************/
        /* RGbs                                                                 */
        /************************************************************************/
        static HarpMessage ProcessColorsRgb(byte[] RGBs, UInt32 bMask)
        {
            switch (bMask)
            {
                case (UInt32)BehaviorPorts.Rgb0:
                    return new HarpMessage(true, 2, 7, 71, 255, (byte)PayloadType.U8, RGBs[0], RGBs[1], RGBs[2], 0);
                case (UInt32)BehaviorPorts.Rgb1:
                    return new HarpMessage(true, 2, 7, 72, 255, (byte)PayloadType.U8, RGBs[0], RGBs[1], RGBs[2], 0);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Rgb0 or Rgb1 can be individually selected.");
            }
        }

        static HarpMessage ProcessColorsRgbs(byte[] RGBs)
        {
            return new HarpMessage(true, 2, 10, 70, 255, (byte)PayloadType.U8, RGBs[0], RGBs[1], RGBs[2], RGBs[3], RGBs[4], RGBs[5], 0);
        }


        /************************************************************************/
        /* Special device functions                                             */
        /************************************************************************/
        static HarpMessage ProcessStartCamera<TSource>(TSource input, UInt32 bMask)
        {
            if (((GetRegOutputs(bMask) & ~((UInt16)((1 << 10) | (1 << 11))))) > 0)
                throw new InvalidOperationException("Invalid Mask selection. Any combination of Digital0 and/or Digital1 can be selected.");

            return createFrameU8(78, GetRegOutputs(bMask) >> 10);
        }

        static HarpMessage ProcessStopCamera<TSource>(TSource input, UInt32 bMask)
        {
            if (((GetRegOutputs(bMask) & ~((UInt16)((1 << 10) | (1 << 11))))) > 0)
                throw new InvalidOperationException("Invalid Mask selection. Any combination of Digital0 and/or Digital1 can be selected.");

            return createFrameU8(79, GetRegOutputs(bMask) >> 10);
        }

        static HarpMessage ProcessEnableServo<TSource>(TSource input, UInt32 bMask)
        {
            if (((GetRegOutputs(bMask) & ~((UInt16)((1 << 12) | (1 << 13))))) > 0)
                throw new InvalidOperationException("Invalid Mask selection. Any combination of Digital2 and/or Digital3 can be selected.");

            return createFrameU8(80, GetRegOutputs(bMask) >> 10);
        }

        static HarpMessage ProcessDisableServo<TSource>(TSource input, UInt32 bMask)
        {
            if (((GetRegOutputs(bMask) & ~((UInt16)((1 << 12) | (1 << 13))))) > 0)
                throw new InvalidOperationException("Invalid Mask selection. Any combination of Digital2 and/or Digital3 can be selected.");

            return createFrameU8(81, GetRegOutputs(bMask) >> 10);
        }

        static HarpMessage ProcessServoPosition(UInt16 input, UInt32 bMask)
        {
            switch (bMask)
            {
                case (UInt32)BehaviorPorts.Digital2: return createFrameU16(101, input);
                case (UInt32)BehaviorPorts.Digital3: return createFrameU16(103, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Digital2 or Digital3 can be individually selected.");
            }
        }

        static HarpMessage ProcessUpdateQuadratureCounter(UInt16 input, UInt32 bMask)
        {
            switch (bMask)
            {
                case (UInt32)BehaviorPorts.Port2:
                    return new HarpMessage(true, 2, 8, 44, 255, (byte)PayloadType.S16, 0, 0, (byte)(input & 255), (byte)((input >> 8) & 255), 0);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Port2 can be selected.");
            }
        }

        static HarpMessage ProcessResetQuadratureCounter<TSource>(TSource input, UInt32 bMask)
        {
            switch (bMask)
            {
                case (UInt32)BehaviorPorts.Port2:
                    return createFrameU8(108, (byte)(1 << 2));
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Port2 can be selected.");
            }
        }
    }
}