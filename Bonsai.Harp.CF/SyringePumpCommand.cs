using Bonsai.Expressions;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Bonsai.Harp.CF
{
    public enum SyringePumpCommandType : byte
    {
        EnableMotorDriver,
        DisableMotorDriver,

        StartProtocol,
        StopProtocol,

        SetDigitalOutputs,
        ClearDigitalOutputs,

        ProtocolNumberOfSteps,
        ProtocolStepsPeriod,
        ProtocolFlowRate,
        ProtocolVolume,
        ProtocolType,
    }

    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<SyringePumpCommand>))]
    [Description("Creates standard command messages available to the Syringe Pump device")]
    public class SyringePumpCommand : SelectBuilder, INamedElement
    {
        [RefreshProperties(RefreshProperties.All)]
        [Description("Specifies which command to send to the Syringe Pump device.")]
        public SyringePumpCommandType Type { get; set; } = SyringePumpCommandType.StartProtocol;

        string INamedElement.Name => $"SyringePump.{Type}";

        string Description
        {
            get
            {
                switch(Type)
                {
                    case SyringePumpCommandType.EnableMotorDriver: return "Enables the motor on the Syringe Pump";
                    case SyringePumpCommandType.DisableMotorDriver: return "Disables the motor on the Syringe Pump";
                    case SyringePumpCommandType.StartProtocol: return "Start the configured protocol on the Syringe Pump";
                    case SyringePumpCommandType.StopProtocol: return "Stop the running protocol on the Syringe Pump";
                    case SyringePumpCommandType.SetDigitalOutputs: return "Set the state of digital output 0 using the input boolean value.";
                    case SyringePumpCommandType.ClearDigitalOutputs: return "Set the state of digital output 1 using the input boolean value.";
                    case SyringePumpCommandType.ProtocolNumberOfSteps: return "Set the number of steps to run in the protocol [1;65535]";
                    case SyringePumpCommandType.ProtocolStepsPeriod: return "Set the period in ms between each step on the protocol [1;65535]";
                    case SyringePumpCommandType.ProtocolFlowRate: return "Set the flow rate of the protocol [0.5;2000.0]";
                    case SyringePumpCommandType.ProtocolVolume: return "Set the volume in uL of the protocol [0.5;2000.0]";
                    case SyringePumpCommandType.ProtocolType: return "Set the type of the protocol. False for step-based and True to volume-based protocol.";
                    default: return null;
                }
            }
        }

        protected override Expression BuildSelector(Expression expression)
        {
            switch (Type)
            {
                case SyringePumpCommandType.EnableMotorDriver:
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessEnableMotorDriver), null);
                case SyringePumpCommandType.DisableMotorDriver:
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessDisableMotorDriver), null);
                case SyringePumpCommandType.StartProtocol:
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessStartProtocol), null);
                case SyringePumpCommandType.StopProtocol:
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessStopProtocol), null);
                case SyringePumpCommandType.SetDigitalOutputs:
                    if(expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessSetDigitalOutputs), null, expression);
                case SyringePumpCommandType.ClearDigitalOutputs:
                    if (expression.Type != typeof(byte)) { expression = Expression.Convert(expression, typeof(byte)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessClearDigitalOutputs), null, expression);
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
                    if(expression.Type != typeof(bool)) { expression = Expression.Convert(expression, typeof(bool)); }
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessProtocolType), null, expression);
                default:
                    throw new InvalidOperationException("Invalid selection or not supported yet.");
            }
        }

        static HarpMessage ProcessEnableMotorDriver() => HarpCommand.WriteByte(address: 32, 1);
        static HarpMessage ProcessDisableMotorDriver() => HarpCommand.WriteByte(address: 32, 0);
        static HarpMessage ProcessStartProtocol() => HarpCommand.WriteByte(address: 33, 1);
        static HarpMessage ProcessStopProtocol() => HarpCommand.WriteByte(address: 33, 0);
        static HarpMessage ProcessSetDigitalOutputs(byte input) => HarpCommand.WriteByte(address: 39, input);
        static HarpMessage ProcessClearDigitalOutputs(byte input) => HarpCommand.WriteByte(address: 40, input);

        static HarpMessage ProcessProtocolNumberOfSteps(ushort input)
        {
            if(input <= 0)
                throw new InvalidOperationException("Invalid number of steps. Must be above 0.");

            return HarpCommand.WriteUInt16(address: 45, input);
        }

        static HarpMessage ProcessProtocolStepsPeriod(ushort input)
        {
            if(input <= 0)
                throw new InvalidOperationException("Invalid steps period. Must be above 0.");

            return HarpCommand.WriteUInt16(address: 47, input);
        }

        static HarpMessage ProcessProtocolFlowRate(float input)
        {
            if(input < 0.5f || input > 2000.0f)
                throw new InvalidOperationException("Invalid flow rate value. Must be greater or equal to 0.5 and less or equal than 2000.");

            return HarpCommand.WriteSingle(address: 46, input);
        }

        static HarpMessage ProcessProtocolVolume(float input)
        {
            if(input < 0.5f || input > 2000.0f)
                throw new InvalidOperationException("Invalid volume value. Must be greater or equal to 0.5 and less or equal than 2000.");

            return HarpCommand.WriteSingle(address: 48, input);
        }

        static HarpMessage ProcessProtocolType(bool input) => HarpCommand.WriteByte(address: 49, (byte)(input? 1: 0));
    }
}
