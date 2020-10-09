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
                default:
                    throw new InvalidOperationException("Invalid selection or not supported yet.");
            }
        }

        static HarpMessage ProcessEnableMotorDriver() => HarpCommand.WriteByte(address: 32, 1);
        static HarpMessage ProcessDisableMotorDriver() => HarpCommand.WriteByte(address: 32, 0);
        static HarpMessage ProcessStartProtocol() => HarpCommand.WriteByte(address: 33, 1);
        static HarpMessage ProcessStopProtocol() => HarpCommand.WriteByte(address: 33, 0);
    }
}
