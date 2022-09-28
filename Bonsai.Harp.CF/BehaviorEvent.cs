using Bonsai.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.ComponentModel;

namespace Bonsai.Harp.CF
{
    [TypeDescriptionProvider(typeof(DeviceTypeDescriptionProvider<BehaviorEvent>))]
    [Description("Filters and selects event messages reported by the Behavior device.")]
    public class BehaviorEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        [RefreshProperties(RefreshProperties.All)]
        [Description("Specifies which event to select from the Behavior device.")]
        public BehaviorEventType Type { get; set; } = BehaviorEventType.Input;

        [Description("The bitmask specifying which digital ports to report with the Behavior device event.")]
        public BehaviorInputPorts Mask { get; set; } = BehaviorInputPorts.Port0;

        string INamedElement.Name => $"Behavior.{Type}";

        string Description
        {
            get
            {
                switch (Type)
                {
                    case BehaviorEventType.Input: return "The state of the digital input line at the specified port.";
                    case BehaviorEventType.AnalogInput: return "The value of the auxiliary analog input, in volts.";
                    case BehaviorEventType.QuadratureCounter: return "The value of the quadrature counter at Port 2, in ticks.";
                    case BehaviorEventType.Camera: return "Emits a value whenever a camera frame has been triggered at the specified digital line.";
                    case BehaviorEventType.RegisterInputs: return "The timestamped raw value of the digital input lines at each port.";
                    case BehaviorEventType.RegisterAnalogInput: return "The timestamped raw value of the auxiliary analog input.";
                    case BehaviorEventType.RegisterCamera: return "Emits a timestamped raw value whenever a camera frame has been triggered at the specified digital line.";
                    default: return null;
                }
            }
        }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();
            switch (Type)
            {
                /************************************************************************/
                /* Register: INPUTS                                                     */
                /************************************************************************/
                case BehaviorEventType.Input:
                    return Expression.Call(typeof(BehaviorEvent), nameof(ProcessInput), null, expression, GetBitMask());

                /************************************************************************/
                /* Register: ADC_AND_ENCODER                                            */
                /************************************************************************/
                case BehaviorEventType.AnalogInput:
                    return Expression.Call(typeof(BehaviorEvent), nameof(ProcessAnalogInput), null, expression);
                case BehaviorEventType.QuadratureCounter:
                    return Expression.Call(typeof(BehaviorEvent), nameof(ProcessQuadratureCounter), null, expression, GetBitMask());

                /************************************************************************/
                /* Register: CAMERA                                                     */
                /************************************************************************/
                case BehaviorEventType.Camera:
                    return Expression.Call(typeof(BehaviorEvent), nameof(ProcessCamera), null, expression, GetBitMask());

                /************************************************************************/
                /* Raw Registers                                                        */
                /************************************************************************/
                case BehaviorEventType.RegisterInputs:
                    return Expression.Call(typeof(BehaviorEvent), nameof(ProcessRegisterInputs), null, expression);
                case BehaviorEventType.RegisterAnalogInput:
                    return Expression.Call(typeof(BehaviorEvent), nameof(ProcessRegisterAnalogInput), null, expression);
                case BehaviorEventType.RegisterCamera:
                    return Expression.Call(typeof(BehaviorEvent), nameof(ProcessRegisterCamera), null, expression, GetBitMask());

                /************************************************************************/
                /* Default                                                              */
                /************************************************************************/
                default:
                    throw new InvalidOperationException("Invalid selection or not supported yet.");
            }
        }

        /************************************************************************/
        /* Local functions                                                      */
        /************************************************************************/
        Expression GetBitMask()
        {
            return Expression.Constant((uint)Mask);
        }

        static byte GetRegDIs(uint bitmask)
        {
            byte regPortDIs;

            regPortDIs = ((bitmask & (uint)BehaviorInputPorts.Port0) == (uint)BehaviorInputPorts.Port0) ? (byte)(1 << 0) : (byte)0;
            regPortDIs |= ((bitmask & (uint)BehaviorInputPorts.Port1) == (uint)BehaviorInputPorts.Port1) ? (byte)(1 << 1) : (byte)0;
            regPortDIs |= ((bitmask & (uint)BehaviorInputPorts.Port2) == (uint)BehaviorInputPorts.Port2) ? (byte)(1 << 2) : (byte)0;

            regPortDIs |= ((bitmask & (uint)BehaviorInputPorts.Poke0InfraRedBeam) == (uint)BehaviorInputPorts.Poke0InfraRedBeam) ? (byte)(1 << 0) : (byte)0;
            regPortDIs |= ((bitmask & (uint)BehaviorInputPorts.Poke1InfraRedBeam) == (uint)BehaviorInputPorts.Poke1InfraRedBeam) ? (byte)(1 << 1) : (byte)0;
            regPortDIs |= ((bitmask & (uint)BehaviorInputPorts.Poke2InfraRedBeam) == (uint)BehaviorInputPorts.Poke2InfraRedBeam) ? (byte)(1 << 2) : (byte)0;

            return regPortDIs;
        }

        static byte GetOutputsBitMask(uint bitmask)
        {
            byte regOutputsBitMask;

            regOutputsBitMask = ((bitmask & (uint)BehaviorInputPorts.Digital0) == (uint)BehaviorInputPorts.Digital0) ? (byte)(1 << 0) : (byte)0;
            regOutputsBitMask |= ((bitmask & (uint)BehaviorInputPorts.Digital1) == (uint)BehaviorInputPorts.Digital1) ? (byte)(1 << 1) : (byte)0;
            regOutputsBitMask |= ((bitmask & (uint)BehaviorInputPorts.Digital2) == (uint)BehaviorInputPorts.Digital2) ? (byte)(1 << 2) : (byte)0;
            regOutputsBitMask |= ((bitmask & (uint)BehaviorInputPorts.Digital3) == (uint)BehaviorInputPorts.Digital3) ? (byte)(1 << 3) : (byte)0;

            return regOutputsBitMask;
        }

        /************************************************************************/
        /* Register: INPUTS                                                     */
        /************************************************************************/
        static IObservable<bool> ProcessInput(IObservable<HarpMessage> source, uint bMask)
        {
            switch (bMask)
            {
                case (uint)BehaviorInputPorts.Port0:
                case (uint)BehaviorInputPorts.Poke0InfraRedBeam:
                    return source.Event(address: 32).Select(input => (input.GetPayloadByte() & (1 << 0)) == (1 << 0)).DistinctUntilChanged();
                
                case (uint)BehaviorInputPorts.Port1:
                case (uint)BehaviorInputPorts.Poke1InfraRedBeam:
                    return source.Event(address: 32).Select(input => (input.GetPayloadByte() & (1 << 1)) == (1 << 1)).DistinctUntilChanged();
                
                case (uint)BehaviorInputPorts.Port2:
                case (uint)BehaviorInputPorts.Poke2InfraRedBeam:
                    return source.Event(address: 32).Select(input => (input.GetPayloadByte() & (1 << 2)) == (1 << 2)).DistinctUntilChanged();
                
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Port0 or Port1 or Port2 or Poke0InfraRedBeam or Poke1InfraRedBeam or Poke2InfraRedBeam can be individually selected.");
            }
        }

        /************************************************************************/
        /* Register: DATA                                                       */
        /************************************************************************/
        static IObservable<float> ProcessAnalogInput(IObservable<HarpMessage> source)
        {
            // ADC input = 2.0 V means 5.0 V on boards input
            // 4096 -> 3.3/1.6 = 2.0625 V
            // ~3972 -> 2.0 V @ ADC -> 5.0 V @ Analog input
            return source.Event(address: 44).Select(input => (float)(5.0 / 3972.0) * input.GetPayloadUInt16());
        }

        static IObservable<int> ProcessQuadratureCounter(IObservable<HarpMessage> source, uint bMask)
        {
            switch (bMask)
            {
                case (uint)BehaviorInputPorts.Port2:
                    return source.Event(address: 44).Select(input => (int)input.GetPayloadInt16(1));
                
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Port2 can be selected.");
            }
        }

        /************************************************************************/
        /* Register: CAMERA                                                     */
        /************************************************************************/
        static IObservable<bool> ProcessCamera(IObservable<HarpMessage> source, uint bMask)
        {
            switch (bMask)
            {
                case (uint)BehaviorInputPorts.Digital0:
                    return source.Event(address: 92).Select(input => true);

                case (uint)BehaviorInputPorts.Digital1:
                    return source.Event(address: 94).Select(input => true);

                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Digital0 or Digital1 can be individually selected.");
            }
        }

        /************************************************************************/
        /* Raw Registers                                                        */
        /************************************************************************/
        static IObservable<Timestamped<byte>> ProcessRegisterInputs(IObservable<HarpMessage> source)
        {
            return source.Event(address: 32).Select(input => input.GetTimestampedPayloadByte());
        }

        static IObservable<Timestamped<ushort>> ProcessRegisterAnalogInput(IObservable<HarpMessage> source)
        {
            return source.Event(address: 44).Select(input => input.GetTimestampedPayloadUInt16());
        }

        static IObservable<Timestamped<byte>> ProcessRegisterCamera(IObservable<HarpMessage> source, uint bMask)
        {
            switch (bMask)
            {
                case (uint)BehaviorInputPorts.Digital0:
                    return source.Event(address: 92).Select(input => input.GetTimestampedPayloadByte());

                case (uint)BehaviorInputPorts.Digital1:
                    return source.Event(address: 94).Select(input => input.GetTimestampedPayloadByte());

                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Digital0 or Digital1 can be individually selected.");
            }
        }
    }

    //          0  1    2 3  4    5 6  7    8   9   10   11   12   13   14  15
    // [0-15]   P0 P0IR - P1 P1IR - P2 P2IR -   -   -    -    -    -    -   -
    // [16-31]  -  -    - -  -    - -  -    -   -   Dig0 Dig1 Dig2 Dig3 -   -
    [Flags]
    public enum BehaviorInputPorts : uint
    {
        Port0 = 1 << 0,
        Poke0InfraRedBeam = 1 << 1,
        Port1 = 1 << 3,
        Poke1InfraRedBeam = 1 << 4,
        Port2 = 1 << 6,
        Poke2InfraRedBeam = 1 << 7,
        Digital0 = 1 << 10 << 16,
        Digital1 = 1 << 11 << 16,
        Digital2 = 1 << 12 << 16,
        Digital3 = 1 << 13 << 16,
    }

    public enum BehaviorEventType : byte
    {
        /* Event: INPUTS */
        Input = 0,
        //InputOutput,

        /* Event: ADC_AND_ENCODER */
        AnalogInput,
        QuadratureCounter,

        /* Event: CAM_OUTx_FRAME_ACQUIRED */
        Camera,

        /* Raw Registers */
        RegisterInputs,
        //RegisterInputsOutputs,
        RegisterAnalogInput,
        RegisterCamera,
    }
}
