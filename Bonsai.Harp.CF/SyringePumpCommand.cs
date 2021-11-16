using Bonsai.Expressions;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Bonsai.Harp.CF
{
    public enum SyringePumpCommandType : byte
    {
        SetMotorDriver,

        SetDirection,

        EnableProtocol,

        SetDigitalOutputs,
        ClearDigitalOutputs,

        MotorMicrostep,

        ProtocolNumberOfSteps,
        ProtocolStepsPeriod,
        ProtocolFlowRate,
        ProtocolVolume,
        ProtocolType,
        ProtocolDirection,

        CalibrationValue1,
        CalibrationValue2
    }

    public enum MotorMicrostep : byte
    {
        Full = (0 << 0),
        Half = (1 << 0),
        Quarter = (2 << 0),
        Eighth = (3 << 0),
        Sixteenth = (4 << 0)
    }

    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<SyringePumpCommand>))]
    [Description("Creates standard command messages available to the Syringe Pump device")]
    public class SyringePumpCommand : SelectBuilder, INamedElement
    {
        [RefreshProperties(RefreshProperties.All)]
        [Description("Specifies which command to send to the Syringe Pump device.")]
        public SyringePumpCommandType Type { get; set; } = SyringePumpCommandType.EnableProtocol;

        string INamedElement.Name => $"SyringePump.{Type}";

        string Description
        {
            get
            {
                switch(Type)
                {
                    case SyringePumpCommandType.SetMotorDriver: return "Enables/disables the motor on the Syringe Pump.\n\n[Input]\nExpects a boolean.\n\n'true' enables the motor\n'false' disables the motor.";
                    case SyringePumpCommandType.SetDirection: return "Sets the direction.\n\n[Input]\nExpects a boolean.\n\n'true' will set the direction to FORWARD\n'false' will set the direction to REVERSE.";
                    case SyringePumpCommandType.EnableProtocol: return "Enables/disables the previously defined protocol.\n\n[Input]\nExpects a boolean.\n\n'true' will start the protocol\n'false' will stop the currently running protocol.";
                    case SyringePumpCommandType.SetDigitalOutputs: return "Set the state of digital outputs.\n\n[Input]\nExpects a byte\n\n0x01 will set DO0\n0x02 will set DO1.\n0x03 will set both DO0 and DO1.";
                    case SyringePumpCommandType.ClearDigitalOutputs: return "Clear the state of digital outputs.\n\n[Input]\nExpects a byte\n\n0x01 will clear DO0\n0x02 will clear DO1.\n0x03 will clear both DO0 and DO1.";
                    case SyringePumpCommandType.MotorMicrostep: return "Set the motor microstep value.\n\n[Input]\nExpects a byte with the following possible values:\nFull = 0\nHalf = 1\nQuarter = 2\nEighth = 3\nSixteenth = 4";
                    case SyringePumpCommandType.ProtocolNumberOfSteps: return "Set the number of steps to run in the protocol.\n\n[Input]\nExpects a UInt16 in the following range [1;65535]";
                    case SyringePumpCommandType.ProtocolStepsPeriod: return "Set the period in ms between each step on the protocol.\n\n[Input]\nExpects a UInt16 in the following range [1;65535]";
                    case SyringePumpCommandType.ProtocolFlowRate: return "Set the flow rate (in uL/s) of the protocol.\n\n[Input]\nExpects a float greater than 0.";
                    case SyringePumpCommandType.ProtocolVolume: return "Set the volume (in uL) of the protocol.\n\n[Input]\nExpects a float greater than 0.";
                    case SyringePumpCommandType.ProtocolType: return "Set the type of the protocol.\n\n[Input]\nExpects a boolean.\n\n'true' for volume-based protocol\n'false' for step-based protocol.";
                    case SyringePumpCommandType.ProtocolDirection: return "Sets the direction that the protocol will run.\n\n[Input]\nExpects a boolean.\n\n'true' will set the direction to FORWARD\n'false' will set the direction to REVERSE.";
                    case SyringePumpCommandType.CalibrationValue1: return "Set the calibration value 1 for protocol use.\n\n[Input]\nExpects a byte";
                    case SyringePumpCommandType.CalibrationValue2: return "Set the calibration value 2 for protocol use.\n\n[Input]\nExpects a byte";

                    default: return null;
                }
            }
        }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                case SyringePumpCommandType.SetMotorDriver:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessSetMotorDriver), null, expression);
                case SyringePumpCommandType.SetDirection:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessSetDirection), null, expression);
                case SyringePumpCommandType.EnableProtocol:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessEnableProtocol), null, expression);
                case SyringePumpCommandType.SetDigitalOutputs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessSetDigitalOutputs), null, expression);
                case SyringePumpCommandType.ClearDigitalOutputs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessClearDigitalOutputs), null, expression);
                case SyringePumpCommandType.MotorMicrostep:
                    if (expression.Type != typeof(MotorMicrostep)) { expression = Expression.Convert(expression, typeof(MotorMicrostep)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessMotorMicrostep), null, expression);
                case SyringePumpCommandType.ProtocolNumberOfSteps:
                    if (expression.Type != typeof(ushort)) { expression = Expression.Convert(expression, typeof(ushort)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessProtocolNumberOfSteps), null, expression);
                case SyringePumpCommandType.ProtocolStepsPeriod:
                    if (expression.Type != typeof(ushort)) { expression = Expression.Convert(expression, typeof(ushort)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessProtocolStepsPeriod), null, expression);
                case SyringePumpCommandType.ProtocolFlowRate:
                    if (expression.Type != typeof(float)) { expression = Expression.Convert(expression, typeof(float)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessProtocolFlowRate), null, expression);
                case SyringePumpCommandType.ProtocolVolume:
                    if (expression.Type != typeof(float)) { expression = Expression.Convert(expression, typeof(float)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessProtocolVolume), null, expression);
                case SyringePumpCommandType.ProtocolType:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessProtocolType), null, expression);
                case SyringePumpCommandType.ProtocolDirection:
                    if (expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessProtocolDirection), null, expression);
                case SyringePumpCommandType.CalibrationValue1:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessCalibrationValue1), null, expression);
                case SyringePumpCommandType.CalibrationValue2:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessCalibrationValue2), null, expression);
                default:
                    throw new InvalidOperationException("Invalid selection or not supported yet.");
            }
        }

        static HarpMessage ProcessSetMotorDriver(bool input) => HarpCommand.WriteByte(address: 32, (byte)(input ? 1 : 0));
        static HarpMessage ProcessSetDirection(bool input) => HarpCommand.WriteByte(address: 35, (byte)(input ? 1 : 0));
        static HarpMessage ProcessEnableProtocol(bool input) => HarpCommand.WriteByte(address: 33, (byte)(input ? 1 : 0));
        static HarpMessage ProcessSetDigitalOutputs(byte input) => HarpCommand.WriteByte(address: 39, input);
        static HarpMessage ProcessClearDigitalOutputs(byte input) => HarpCommand.WriteByte(address: 40, input);
        static HarpMessage ProcessMotorMicrostep(MotorMicrostep input)
        {
            if(!Enum.IsDefined(typeof(MotorMicrostep), input))
                throw new InvalidOperationException("Invalid MotorMicrostep value. Valid values are: \nFull = 0\nHalf = 1\nQuarter = 2\nEighth = 3\nSixteenth = 4.");

            return HarpCommand.WriteByte(address: 44, (byte) input);
        }

        static HarpMessage ProcessProtocolNumberOfSteps(ushort input)
        {
            if (input <= 0)
                throw new InvalidOperationException("Invalid number of steps. Must be greater than 0.");

            return HarpCommand.WriteUInt16(address: 45, input);
        }

        static HarpMessage ProcessProtocolStepsPeriod(ushort input)
        {
            if (input <= 0)
                throw new InvalidOperationException("Invalid steps period. Must be greater than 0.");

            return HarpCommand.WriteUInt16(address: 47, input);
        }

        static HarpMessage ProcessProtocolFlowRate(float input)
        {
            if (input <= 0)
                throw new InvalidOperationException("Invalid flow rate value. Must be greater than 0.");

            return HarpCommand.WriteSingle(address: 46, input);
        }

        static HarpMessage ProcessProtocolVolume(float input)
        {
            if (input <= 0)
                throw new InvalidOperationException("Invalid volume value. Must be greater than 0.");

            return HarpCommand.WriteSingle(address: 48, input);
        }

        static HarpMessage ProcessProtocolType(bool input) => HarpCommand.WriteByte(address: 49, (byte)(input? 1: 0));
        static HarpMessage ProcessProtocolDirection(bool input) => HarpCommand.WriteByte(address: 55, (byte)(input? 1: 0));
        static HarpMessage ProcessCalibrationValue1(byte input) => HarpCommand.WriteByte(address: 50, input);
        static HarpMessage ProcessCalibrationValue2(byte input) => HarpCommand.WriteByte(address: 51, input);
    }
}
