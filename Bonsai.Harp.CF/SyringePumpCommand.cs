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
                    return Expression.Call(typeof(SyringePumpCommand), nameof(ProcessSetDigitalOutputs), null, expression);
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
    }
}
