using Bonsai.Expressions;
using System;
using System.Linq.Expressions;
using System.ComponentModel;

namespace Bonsai.Harp.CF
{
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<BehaviorCommand>))]
    [Description("Creates standard command messages available to the Behavior device.")]
    public class BehaviorCommand : SelectBuilder, INamedElement
    {
        [RefreshProperties(RefreshProperties.All)]
        [Description("Specifies which command to send to the Behavior device.")]
        public BehaviorCommandType Type { get; set; } = BehaviorCommandType.SetOutput;

        [Description("The set of flags used to control the Behavior command.")]
        public BehaviorPorts Mask { get; set; } = BehaviorPorts.Port0;

        string INamedElement.Name => $"Behavior.{Type}";

        string Description
        {
            get
            {
                switch (Type)
                {
                    case BehaviorCommandType.SetOutput: return "Sets the specified digital outputs to HIGH.";
                    case BehaviorCommandType.ClearOutput: return "Sets the specified digital outputs to LOW.";
                    case BehaviorCommandType.ToggleOutput: return "Toggles the state of the specified digital outputs.";
                    case BehaviorCommandType.PulsePeriod: return "Sets the duration of the fixed pulse on the selected output, in ms.";
                    case BehaviorCommandType.StartPwm: return "Start the configured PWM on the selected digital output.";
                    case BehaviorCommandType.StopPwm: return "Stop the configured PWM on the selected digital output.";
                    case BehaviorCommandType.PwmFrequency: return "Sets the frequency of the PWM signal on the selected output, in Hertz.";
                    case BehaviorCommandType.PwmDutyCycle: return "Sets the duty cycle of the PWM signal on the selected output, in percentage.";
                    case BehaviorCommandType.LedCurrent: return "Sets the current used to drive the LED on the selected digital output, in mA.";
                    case BehaviorCommandType.ColorsRgb: return "Sets the color of the LED on the selected digital output. The input is a positive integer array (R,G,B).";
                    case BehaviorCommandType.ColorsRgbs: return "Sets the color of all the LEDs simultaneously. The input is a positive integer array (R,G,B,R,G,B).";
                    case BehaviorCommandType.StartCamera: return "Start triggering frame acquisition on the selected digital output.";
                    case BehaviorCommandType.StopCamera: return "Stop triggering frame acquisition on the selected digital output.";
                    case BehaviorCommandType.EnableServo: return "Enable servo motor control on the selected output.";
                    case BehaviorCommandType.DisableServo: return "Disable servo motor control on the selected output.";
                    case BehaviorCommandType.ServoPosition: return "Sets the servo motor position on the selected output.";
                    case BehaviorCommandType.ResetQuadratureCounter: return "Resets the value of the quadrature counter at Port 2 to zero.";
                    case BehaviorCommandType.UpdateQuadratureCounter: return "Sets the current value of the quadrature counter at Port 2.";
                    case BehaviorCommandType.RegisterSetOutputs: return "Sets the state of the outputs specified in the input 16-bit mask to HIGH.";
                    case BehaviorCommandType.RegisterClearOutputs: return "Sets the state of the outputs specified in the input 16-bit mask to LOW.";
                    case BehaviorCommandType.RegisterToggleOutputs: return "Toggles the state of the outputs specified in the input 16-bit mask.";
                    case BehaviorCommandType.RegisterStartPwm: return "Starts the PWM at all outputs specified in the input 16-bit mask.";
                    case BehaviorCommandType.RegisterStopPwm: return "Stops the PWM at all outputs specified in the input 16-bit mask.";
                    default: return null;
                }
            }
        }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                /************************************************************************/
                /* Outputs                                                              */
                /************************************************************************/
                case BehaviorCommandType.SetOutput:
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessSetOutput), null, GetBitMask());
                case BehaviorCommandType.ClearOutput:
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessClearOutput), null, GetBitMask());
                case BehaviorCommandType.ToggleOutput:
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessToggleOutput), null, GetBitMask());

                case BehaviorCommandType.RegisterSetOutputs:
                    if (expression.Type != typeof(ushort)) { expression = Expression.Convert(expression, typeof(ushort)); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessRegisterSetOutputs), null, expression);
                case BehaviorCommandType.RegisterClearOutputs:
                    if (expression.Type != typeof(ushort)) { expression = Expression.Convert(expression, typeof(ushort)); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessRegisterClearOutputs), null, expression);
                case BehaviorCommandType.RegisterToggleOutputs:
                    if (expression.Type != typeof(ushort)) { expression = Expression.Convert(expression, typeof(ushort)); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessRegisterToggleOutputs), null, expression);

                /************************************************************************/
                /* Pulse Period                                                         */
                /************************************************************************/
                case BehaviorCommandType.PulsePeriod:
                    if (expression.Type != typeof(ushort)) { expression = Expression.Convert(expression, typeof(ushort)); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessPulsePeriod), null, expression, GetBitMask());

                /************************************************************************/
                /* Pwm                                                                  */
                /************************************************************************/
                case BehaviorCommandType.StartPwm:
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessStartPwm), null, GetBitMask());
                case BehaviorCommandType.StopPwm:
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessStopPwm), null, GetBitMask());

                case BehaviorCommandType.PwmFrequency:
                    if (expression.Type != typeof(ushort)) { expression = Expression.Convert(expression, typeof(ushort)); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessPwmFrequency), null, expression, GetBitMask());
                case BehaviorCommandType.PwmDutyCycle:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessPwmDutyCycle), null, expression, GetBitMask());

                case BehaviorCommandType.RegisterStartPwm:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessRegisterStartPwm), null, expression);
                case BehaviorCommandType.RegisterStopPwm:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessRegisterStopPwm), null, expression);

                /************************************************************************/
                /* Special device functions                                             */
                /************************************************************************/
                case BehaviorCommandType.StartCamera:
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessStartCamera), null, GetBitMask());
                case BehaviorCommandType.StopCamera:
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessStopCamera), null, GetBitMask());

                case BehaviorCommandType.EnableServo:
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessEnableServo), null, GetBitMask());
                case BehaviorCommandType.DisableServo:
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessDisableServo), null, GetBitMask());

                case BehaviorCommandType.ServoPosition:
                    if (expression.Type != typeof(ushort)) { expression = Expression.Convert(expression, typeof(ushort)); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessServoPosition), null, expression, GetBitMask());
                case BehaviorCommandType.UpdateQuadratureCounter:
                    if (expression.Type != typeof(ushort)) { expression = Expression.Convert(expression, typeof(ushort)); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessUpdateQuadratureCounter), null, expression, GetBitMask());
                case BehaviorCommandType.ResetQuadratureCounter:
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessResetQuadratureCounter), null, GetBitMask());

                /************************************************************************/
                /* Led                                                                  */
                /************************************************************************/
                case BehaviorCommandType.LedCurrent:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessLedCurrent), null, expression, GetBitMask());

                /************************************************************************/
                /* RGBs                                                                 */
                /************************************************************************/
                case BehaviorCommandType.ColorsRgb:
                    if (expression.Type != typeof(byte[])) { expression = Expression.Convert(expression, typeof(byte[])); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessColorsRgb), null, expression, GetBitMask());

                case BehaviorCommandType.ColorsRgbs:
                    if (expression.Type != typeof(byte[])) { expression = Expression.Convert(expression, typeof(byte[])); }
                    return Expression.Call(typeof(BehaviorCommand), nameof(ProcessColorsRgbs), null, expression);

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
            return Expression.Constant((uint)Mask);
        }

        static ushort GetRegOutputs(uint bitmask)
        {
            ushort regOutputs = (ushort)((bitmask & 0xFF00) >> 16);

            regOutputs |= ((bitmask & (uint)BehaviorPorts.Port0) == (uint)BehaviorPorts.Port0) ? (ushort)(1 << 0) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Port1) == (uint)BehaviorPorts.Port1) ? (ushort)(1 << 1) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Port2) == (uint)BehaviorPorts.Port2) ? (ushort)(1 << 2) : (ushort)0;

            regOutputs |= ((bitmask & (uint)BehaviorPorts.Poke0Led) == (uint)BehaviorPorts.Poke0Led) ? (ushort)(1 << 0) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Poke1Led) == (uint)BehaviorPorts.Poke1Led) ? (ushort)(1 << 1) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Poke2Led) == (uint)BehaviorPorts.Poke2Led) ? (ushort)(1 << 2) : (ushort)0;

            regOutputs |= ((bitmask & (uint)BehaviorPorts.Poke0Valve) == (uint)BehaviorPorts.Poke0Valve) ? (ushort)(1 << 3) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Poke1Valve) == (uint)BehaviorPorts.Poke1Valve) ? (ushort)(1 << 4) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Poke2Valve) == (uint)BehaviorPorts.Poke2Valve) ? (ushort)(1 << 5) : (ushort)0;

            regOutputs |= ((bitmask & (uint)BehaviorPorts.Led0) == (uint)BehaviorPorts.Led0) ? (ushort)(1 << 6) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Led1) == (uint)BehaviorPorts.Led1) ? (ushort)(1 << 7) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Rgb0) == (uint)BehaviorPorts.Rgb0) ? (ushort)(1 << 8) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Rgb1) == (uint)BehaviorPorts.Rgb1) ? (ushort)(1 << 9) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Digital0) == (uint)BehaviorPorts.Digital0) ? (ushort)(1 << 10) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Digital1) == (uint)BehaviorPorts.Digital1) ? (ushort)(1 << 11) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Digital2) == (uint)BehaviorPorts.Digital2) ? (ushort)(1 << 12) : (ushort)0;
            regOutputs |= ((bitmask & (uint)BehaviorPorts.Digital3) == (uint)BehaviorPorts.Digital3) ? (ushort)(1 << 13) : (ushort)0;

            return regOutputs;
        }

        static byte GetRegDios(uint bitmask)
        {
            byte regDios;

            regDios = ((ushort)(bitmask & (ushort)BehaviorPorts.Port0) == (ushort)BehaviorPorts.Port0) ? (byte)(1 << 0) : (byte)0;
            regDios |= ((ushort)(bitmask & (ushort)BehaviorPorts.Port1) == (ushort)BehaviorPorts.Port1) ? (byte)(1 << 1) : (byte)0;
            regDios |= ((ushort)(bitmask & (ushort)BehaviorPorts.Port2) == (ushort)BehaviorPorts.Port2) ? (byte)(1 << 2) : (byte)0;

            return regDios;
        }


        /************************************************************************/
        /* Outputs                                                              */
        /************************************************************************/
        static HarpMessage ProcessSetOutput(uint bMask) => HarpCommand.WriteUInt16(address: 34, GetRegOutputs(bMask));
        static HarpMessage ProcessClearOutput(uint bMask) => HarpCommand.WriteUInt16(address: 35, GetRegOutputs(bMask));
        static HarpMessage ProcessToggleOutput(uint bMask) => HarpCommand.WriteUInt16(address: 36, GetRegOutputs(bMask));

        static HarpMessage ProcessRegisterSetOutputs(ushort input) => HarpCommand.WriteUInt16(address: 34, input);
        static HarpMessage ProcessRegisterClearOutputs(ushort input) => HarpCommand.WriteUInt16(address: 35, input);
        static HarpMessage ProcessRegisterToggleOutputs(ushort input) => HarpCommand.WriteUInt16(address: 36, input);

        /************************************************************************/
        /* Pulse Period                                                         */
        /************************************************************************/
        static HarpMessage ProcessPulsePeriod(ushort input, uint bMask)
        {
            switch (bMask)
            {
                case (uint)BehaviorPorts.Port0: return HarpCommand.WriteUInt16(address: 46, input);
                case (uint)BehaviorPorts.Port1: return HarpCommand.WriteUInt16(address: 47, input);
                case (uint)BehaviorPorts.Port2: return HarpCommand.WriteUInt16(address: 48, input);

                case (uint)BehaviorPorts.Poke0Led: return HarpCommand.WriteUInt16(address: 46, input);
                case (uint)BehaviorPorts.Poke1Led: return HarpCommand.WriteUInt16(address: 47, input);
                case (uint)BehaviorPorts.Poke2Led: return HarpCommand.WriteUInt16(address: 48, input);

                case (uint)BehaviorPorts.Poke0Valve: return HarpCommand.WriteUInt16(address: 49, input);
                case (uint)BehaviorPorts.Poke1Valve: return HarpCommand.WriteUInt16(address: 50, input);
                case (uint)BehaviorPorts.Poke2Valve: return HarpCommand.WriteUInt16(address: 51, input);

                case (uint)BehaviorPorts.Led0: return HarpCommand.WriteUInt16(address: 52, input);
                case (uint)BehaviorPorts.Led1: return HarpCommand.WriteUInt16(address: 53, input);
                case (uint)BehaviorPorts.Rgb0: return HarpCommand.WriteUInt16(address: 54, input);
                case (uint)BehaviorPorts.Rgb1: return HarpCommand.WriteUInt16(address: 55, input);
                case (uint)BehaviorPorts.Digital0: return HarpCommand.WriteUInt16(address: 56, input);
                case (uint)BehaviorPorts.Digital1: return HarpCommand.WriteUInt16(address: 57, input);
                case (uint)BehaviorPorts.Digital2: return HarpCommand.WriteUInt16(address: 58, input);
                case (uint)BehaviorPorts.Digital3: return HarpCommand.WriteUInt16(address: 59, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only one option can be selected.");
            }
        }

        /************************************************************************/
        /* Pwm                                                                  */
        /************************************************************************/
        static HarpMessage ProcessStartPwm(uint bMask)
        {
            if ((GetRegOutputs(bMask) & ~((1 << 10) | (1 << 11) | (1 << 12) | (1 << 13))) > 0)
                throw new InvalidOperationException("Invalid Mask selection. Any combination of Digital0, Digital1, Digital2 and/or Digital3 can be selected.");

            return HarpCommand.WriteByte(address: 68, (byte)(GetRegOutputs(bMask) >> 10));
        }
        static HarpMessage ProcessStopPwm(uint bMask)
        {
            if ((GetRegOutputs(bMask) & ~((1 << 10) | (1 << 11) | (1 << 12) | (1 << 13))) > 0)
                throw new InvalidOperationException("Invalid Mask selection. Any combination of Digital0, Digital1, Digital2 and/or Digital3 can be selected.");

            return HarpCommand.WriteByte(address: 69, (byte)(GetRegOutputs(bMask) >> 10));
        }

        static HarpMessage ProcessPwmFrequency(ushort input, uint bMask)
        {
            switch (bMask)
            {
                case (uint)BehaviorPorts.Digital0: return HarpCommand.WriteUInt16(address: 60, input);
                case (uint)BehaviorPorts.Digital1: return HarpCommand.WriteUInt16(address: 61, input);
                case (uint)BehaviorPorts.Digital2: return HarpCommand.WriteUInt16(address: 62, input);
                case (uint)BehaviorPorts.Digital3: return HarpCommand.WriteUInt16(address: 63, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Digital0 or Digital1 or Digital2 or Digital3 can be individually selected.");
            }
        }

        static HarpMessage ProcessPwmDutyCycle(byte input, uint bMask)
        {
            switch (bMask)
            {
                case (uint)BehaviorPorts.Digital0: return HarpCommand.WriteByte(address: 64, input);
                case (uint)BehaviorPorts.Digital1: return HarpCommand.WriteByte(address: 65, input);
                case (uint)BehaviorPorts.Digital2: return HarpCommand.WriteByte(address: 66, input);
                case (uint)BehaviorPorts.Digital3: return HarpCommand.WriteByte(address: 67, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Digital0 or Digital1 or Digital2 or Digital3 can be individually selected.");
            }
        }

        static HarpMessage ProcessRegisterStartPwm(byte input) => HarpCommand.WriteByte(address: 68, input);
        static HarpMessage ProcessRegisterStopPwm(byte input) => HarpCommand.WriteByte(address: 69, input);

        /************************************************************************/
        /* Led                                                                  */
        /************************************************************************/
        static HarpMessage ProcessLedCurrent(byte input, uint bMask)
        {
            switch (bMask)
            {
                case (uint)BehaviorPorts.Led0: return HarpCommand.WriteByte(address: 73, input);
                case (uint)BehaviorPorts.Led1: return HarpCommand.WriteByte(address: 74, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Led0 or Led1 can be individually selected.");
            }
        }

        /************************************************************************/
        /* RGbs                                                                 */
        /************************************************************************/
        static HarpMessage ProcessColorsRgb(byte[] RGBs, uint bMask)
        {
            switch (bMask)
            {
                case (uint)BehaviorPorts.Rgb0:
                    return HarpCommand.WriteByte(address: 71, RGBs);
                case (uint)BehaviorPorts.Rgb1:
                    return HarpCommand.WriteByte(address: 72, RGBs);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Rgb0 or Rgb1 can be individually selected.");
            }
        }

        static HarpMessage ProcessColorsRgbs(byte[] RGBs)
        {
            return HarpCommand.WriteByte(address: 70, RGBs);
        }


        /************************************************************************/
        /* Special device functions                                             */
        /************************************************************************/
        static HarpMessage ProcessStartCamera(uint bMask)
        {
            if ((GetRegOutputs(bMask) & ~((1 << 10) | (1 << 11))) > 0)
                throw new InvalidOperationException("Invalid Mask selection. Any combination of Digital0 and/or Digital1 can be selected.");

            return HarpCommand.WriteByte(address: 78, (byte)(GetRegOutputs(bMask) >> 10));
        }

        static HarpMessage ProcessStopCamera(uint bMask)
        {
            if ((GetRegOutputs(bMask) & ~((1 << 10) | (1 << 11))) > 0)
                throw new InvalidOperationException("Invalid Mask selection. Any combination of Digital0 and/or Digital1 can be selected.");

            return HarpCommand.WriteByte(address: 79, (byte)(GetRegOutputs(bMask) >> 10));
        }

        static HarpMessage ProcessEnableServo(uint bMask)
        {
            if ((GetRegOutputs(bMask) & ~((1 << 12) | (1 << 13))) > 0)
                throw new InvalidOperationException("Invalid Mask selection. Any combination of Digital2 and/or Digital3 can be selected.");

            return HarpCommand.WriteByte(address: 80, (byte)(GetRegOutputs(bMask) >> 10));
        }

        static HarpMessage ProcessDisableServo(uint bMask)
        {
            if ((GetRegOutputs(bMask) & ~((1 << 12) | (1 << 13))) > 0)
                throw new InvalidOperationException("Invalid Mask selection. Any combination of Digital2 and/or Digital3 can be selected.");

            return HarpCommand.WriteByte(address: 81, (byte)(GetRegOutputs(bMask) >> 10));
        }

        static HarpMessage ProcessServoPosition(ushort input, uint bMask)
        {
            switch (bMask)
            {
                case (uint)BehaviorPorts.Digital2: return HarpCommand.WriteUInt16(address: 101, input);
                case (uint)BehaviorPorts.Digital3: return HarpCommand.WriteUInt16(address: 103, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Digital2 or Digital3 can be individually selected.");
            }
        }

        static HarpMessage ProcessUpdateQuadratureCounter(short input, uint bMask)
        {
            switch (bMask)
            {
                case (uint)BehaviorPorts.Port2:
                    return HarpCommand.WriteInt16(address: 44, input);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Port2 can be selected.");
            }
        }

        static HarpMessage ProcessResetQuadratureCounter(uint bMask)
        {
            switch (bMask)
            {
                case (uint)BehaviorPorts.Port2:
                    return HarpCommand.WriteByte(address: 108, 1 << 2);
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Port2 can be selected.");
            }
        }
    }

    //          0  1     2     3  4     5     6    7     8     9    10   11   12   13   14 15
    // [0-15]   P0 P0Led P0Val P1 P1Led P1Val P2   P2Led P2Val -    -    -    -    -    -  -
    // [16-31]  -  -     -     -  -     -     Led0 Led1  Rgb0  Rgb1 Dig0 Dig1 Dig2 Dig3 -  -
    [Flags]
    public enum BehaviorPorts : uint
    {
        Port0 = 1 << 0,
        Poke0Led = 1 << 1,
        Poke0Valve = 1 << 2,
        Port1 = 1 << 3,
        Poke1Led = 1 << 4,
        Poke1Valve = 1 << 5,
        Port2 = 1 << 6,
        Poke2Led = 1 << 7,
        Poke2Valve = 1 << 8,
        Led0 = 1 << 6 << 16,
        Led1 = 1 << 7 << 16,
        Rgb0 = 1 << 8 << 16,
        Rgb1 = 1 << 9 << 16,
        Digital0 = 1 << 10 << 16,
        Digital1 = 1 << 11 << 16,
        Digital2 = 1 << 12 << 16,
        Digital3 = 1 << 13 << 16,
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
        RegisterStartPwm,
        RegisterStopPwm
    }
}