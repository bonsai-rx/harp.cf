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

        public MotorMicrostep Mask { get; set; } = MotorMicrostep.Full;

        string INamedElement.Name => $"SyringePump.{Type}";

        string Description
        {
            get
            {
                switch(Type)
                {
                    case SyringePumpCommandType.SetMotorDriver: return "Enables/disables the motor on the Syringe Pump. Expects a boolean. 'true' enables the motor, 'false' disables the motor.";
                    case SyringePumpCommandType.SetDirection: return "Sets the direction. Expects a boolean. 'true' will set the direction to FORWARD, 'false' will set the direction to REVERSE.";
                    case SyringePumpCommandType.EnableProtocol: return "Enables/disables the previously defined protocol. Expects a boolean. 'true' will start the protocol, 'false' will stop the currently running protocol.";
                    case SyringePumpCommandType.SetDigitalOutputs: return "Set the state of digital outputs. Expects a byte where 0 will set DO0 and 1 will set DO1.";
                    case SyringePumpCommandType.ClearDigitalOutputs: return "Clear the state of digital outputs. Expects a byte where 0 will clear DO0 and 1 will clear DO1.";
                    case SyringePumpCommandType.MotorMicrostep: return "Set the motor microstep value.";
                    case SyringePumpCommandType.ProtocolNumberOfSteps: return "Set the number of steps to run in the protocol [1;65535]";
                    case SyringePumpCommandType.ProtocolStepsPeriod: return "Set the period in ms between each step on the protocol [1;65535]";
                    case SyringePumpCommandType.ProtocolFlowRate: return "Set the flow rate of the protocol [0.5;2000.0]";
                    case SyringePumpCommandType.ProtocolVolume: return "Set the volume in uL of the protocol [0.5;2000.0]";
                    case SyringePumpCommandType.ProtocolType: return "Set the type of the protocol. False for step-based and True to volume-based protocol.";
                    case SyringePumpCommandType.CalibrationValue1: return "Set the calibration value 1 for protocol use.";
                    case SyringePumpCommandType.CalibrationValue2: return "Set the calibration value 2 for protocol use.";

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

        static HarpMessage ProcessSetMotorDriver(bool input) => HarpCommand.WriteByte(32, (byte) (input ? 1 : 0));
        static HarpMessage ProcessSetDirection(bool input) => HarpCommand.WriteByte(35, (byte) (input ? 1 : 0));
        static HarpMessage ProcessEnableProtocol(bool input) => HarpCommand.WriteByte(33, (byte) (input ? 1 : 0));
        static HarpMessage ProcessSetDigitalOutputs(byte input) => HarpCommand.WriteByte(address: 39, input);
        static HarpMessage ProcessClearDigitalOutputs(byte input) => HarpCommand.WriteByte(address: 40, input);
        static HarpMessage ProcessMotorMicrostep(MotorMicrostep input)
        {
            bool exists = Enum.IsDefined(typeof(MotorMicrostep), input);
            if (!exists)
                throw new InvalidOperationException("Invalid Mask selection. Please select an appropriate value.");

            return HarpCommand.WriteByte(address: 44, (byte)input);
        }

        static HarpMessage ProcessProtocolNumberOfSteps(ushort input)
        {
            if (input <= 0)
                throw new InvalidOperationException("Invalid number of steps. Must be above 0.");

            return HarpCommand.WriteUInt16(address: 45, input);
        }

        static HarpMessage ProcessProtocolStepsPeriod(ushort input)
        {
            if (input <= 0)
                throw new InvalidOperationException("Invalid steps period. Must be above 0.");

            return HarpCommand.WriteUInt16(address: 47, input);
        }

        static HarpMessage ProcessProtocolFlowRate(float input)
        {
            if (input < 0.5f || input > 2000.0f)
                throw new InvalidOperationException("Invalid flow rate value. Must be greater or equal to 0.5 and less or equal than 2000.");

            return HarpCommand.WriteSingle(address: 46, input);
        }

        static HarpMessage ProcessProtocolVolume(float input)
        {
            if (input < 0.5f || input > 2000.0f)
                throw new InvalidOperationException("Invalid volume value. Must be greater or equal to 0.5 and less or equal than 2000.");

            return HarpCommand.WriteSingle(address: 48, input);
        }

        static HarpMessage ProcessProtocolType(bool input) => HarpCommand.WriteByte(address: 49, (byte)(input? 1: 0));
        static HarpMessage ProcessCalibrationValue1(byte input) => HarpCommand.WriteByte(address: 50, input);
        static HarpMessage ProcessCalibrationValue2(byte input) => HarpCommand.WriteByte(address: 51, input);
    }
}
