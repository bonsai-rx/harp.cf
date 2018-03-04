using Bonsai;
using Bonsai.Expressions;
using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Text;
using TResult = System.String;
using System.ComponentModel;

namespace Bonsai.Harp.CF
{   
    //          0  1    2 3  4    5 6  7    8   9   10   11   12   13   14  15
    // [0-15]   P0 P0IR - P1 P1IR - P2 P2IR -   -   -    -    -    -    -   -
    // [16-31]  -  -    - -  -    - -  -    -   -   Dig0 Dig1 Dig2 Dig3 -   -
    [Flags]
    public enum BehaviorInputPorts : UInt32
    {
        Port0 = (1 << 0),
        Poke0InfraRedBeam = (1 << 1),
        Port1 = (1 << 3),
        Poke1InfraRedBeam = (1 << 4),
        Port2 = (1 << 6),
        Poke2InfraRedBeam = (1 << 7),
        Digital0 = (1 << 10) << 16,
        Digital1 = (1 << 11) << 16,
        Digital2 = (1 << 12) << 16,
        Digital3 = (1 << 13) << 16,
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

    [Description(
        "\n" +
        "Input: Boolean (*)\n" +
        //"InputOutput: Boolean (*)\n" +
        "\n" +
        "AnalogInput: Decimal (V)\n" +
        "QuadratureCounter: Integer\n" +
        "\n" +
        "Camera: Boolean\n" +
        "\n" +
        "RegisterInputs: Bitmask U8\n" +
        //"RegisterInputsOutputs: Bitpmask U8\n" +
        "RegisterAnalogInput: U16\n" +
        "\n" +
        "(*) Only distinct contiguous elements are propagated."
    )]

    public class BehaviorEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        public BehaviorEvent()
        {
            Type = BehaviorEventType.Input;
            Mask = BehaviorInputPorts.Port0;
        }

        string INamedElement.Name
        {
            get { return typeof(BehaviorEvent).Name.Replace("Event", string.Empty) + "." + Type.ToString(); }
        }

        public BehaviorEventType Type { get; set; }
        public BehaviorInputPorts Mask { get; set; }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();
            switch (Type)
            {
                /************************************************************************/
                /* Register: INPUTS                                                     */
                /************************************************************************/
                case BehaviorEventType.Input:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessInput", null, expression, GetBitMask());

                /************************************************************************/
                /* Register: ADC_AND_ENCODER                                            */
                /************************************************************************/
                case BehaviorEventType.AnalogInput:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessAnalogInput", null, expression);
                case BehaviorEventType.QuadratureCounter:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessQuadratureCounter", null, expression, GetBitMask());

                /************************************************************************/
                /* Register: CAMERA                                                     */
                /************************************************************************/
                case BehaviorEventType.Camera:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessCamera", null, expression, GetBitMask());

                /************************************************************************/
                /* Raw Registers                                                        */
                /************************************************************************/
                case BehaviorEventType.RegisterInputs:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessRegisterInputs", null, expression);
                case BehaviorEventType.RegisterAnalogInput:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessRegisterAnalogInput", null, expression);
                case BehaviorEventType.RegisterCamera:
                    return Expression.Call(typeof(BehaviorEvent), "ProcessRegisterCamera", null, expression, GetBitMask());

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
            return Expression.Convert(Expression.Constant(Mask), typeof(UInt32));
        }

        static byte GetRegDIs(UInt32 BonsaiBitMaks)
        {
            byte regPortDIs;

            regPortDIs = ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorInputPorts.Port0)) == (UInt32)(BehaviorInputPorts.Port0)) ? (byte)(1 << 0) : (byte)0;
            regPortDIs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorInputPorts.Port1)) == (UInt32)(BehaviorInputPorts.Port1)) ? (byte)(1 << 1) : (byte)0;
            regPortDIs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorInputPorts.Port2)) == (UInt32)(BehaviorInputPorts.Port2)) ? (byte)(1 << 2) : (byte)0;

            regPortDIs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorInputPorts.Poke0InfraRedBeam)) == (UInt32)(BehaviorInputPorts.Poke0InfraRedBeam)) ? (byte)(1 << 0) : (byte)0;
            regPortDIs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorInputPorts.Poke1InfraRedBeam)) == (UInt32)(BehaviorInputPorts.Poke1InfraRedBeam)) ? (byte)(1 << 1) : (byte)0;
            regPortDIs |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorInputPorts.Poke2InfraRedBeam)) == (UInt32)(BehaviorInputPorts.Poke2InfraRedBeam)) ? (byte)(1 << 2) : (byte)0;

            return regPortDIs;
        }

        static byte GetOutputsBitMask(UInt32 BonsaiBitMaks)
        {
            byte regOutputsBitMask;

            regOutputsBitMask = ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorInputPorts.Digital0)) == (UInt32)(BehaviorInputPorts.Digital0)) ? (byte)(1 << 0) : (byte)0;
            regOutputsBitMask |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorInputPorts.Digital1)) == (UInt32)(BehaviorInputPorts.Digital1)) ? (byte)(1 << 1) : (byte)0;
            regOutputsBitMask |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorInputPorts.Digital2)) == (UInt32)(BehaviorInputPorts.Digital2)) ? (byte)(1 << 2) : (byte)0;
            regOutputsBitMask |= ((UInt32)(BonsaiBitMaks & (UInt32)(BehaviorInputPorts.Digital3)) == (UInt32)(BehaviorInputPorts.Digital3)) ? (byte)(1 << 3) : (byte)0;

            return regOutputsBitMask;
        }

        static double ParseTimestamp(byte[] message, int index)
        {
            var seconds = BitConverter.ToUInt32(message, index);
            var microseconds = BitConverter.ToUInt16(message, index + 4);
            return seconds + microseconds * 32e-6;
        }

        static bool is_evt32(HarpMessage input) { return ((input.Address == 32) && (input.Error == false) && (input.MessageType == MessageType.Event)); }        
        static bool is_evt44(HarpMessage input) { return ((input.Address == 44) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt92(HarpMessage input) { return ((input.Address == 92) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt94(HarpMessage input) { return ((input.Address == 94) && (input.Error == false) && (input.MessageType == MessageType.Event)); }

        /************************************************************************/
        /* Register: INPUTS                                                     */
        /************************************************************************/
        static IObservable<bool> ProcessInput(IObservable<HarpMessage> source, UInt32 bMask)
        {
            switch (bMask)
            {
                case (UInt32)BehaviorInputPorts.Port0:
                case (UInt32)BehaviorInputPorts.Poke0InfraRedBeam:
                    return source.Where(is_evt32).Select(input => { return ((input.MessageBytes[11] & (1 << 0)) == (1 << 0)); }).DistinctUntilChanged();
                
                case (UInt32)BehaviorInputPorts.Port1:
                case (UInt32)BehaviorInputPorts.Poke1InfraRedBeam:
                    return source.Where(is_evt32).Select(input => { return ((input.MessageBytes[11] & (1 << 1)) == (1 << 1)); }).DistinctUntilChanged();
                
                case (UInt32)BehaviorInputPorts.Port2:
                case (UInt32)BehaviorInputPorts.Poke2InfraRedBeam:
                    return source.Where(is_evt32).Select(input => { return ((input.MessageBytes[11] & (1 << 2)) == (1 << 2)); }).DistinctUntilChanged();
                
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
            return source.Where(is_evt44).Select(input => { return (float)(5.0 / 3972.0) * ((int)((UInt16)(BitConverter.ToUInt16(input.MessageBytes, 11) & (UInt16)(0x0FFF)))); });
        }

        static IObservable<int> ProcessQuadratureCounter(IObservable<HarpMessage> source, UInt32 bMask)
        {
            switch (bMask)
            {
                case (UInt32)BehaviorInputPorts.Port2:
                    return source.Where(is_evt44).Select(input => { return (int)(BitConverter.ToInt16(input.MessageBytes, 13)); });
                
                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Port2 can be selected.");
            }
        }

        /************************************************************************/
        /* Register: CAMERA                                                     */
        /************************************************************************/
        static IObservable<bool> ProcessCamera(IObservable<HarpMessage> source, UInt32 bMask)
        {
            switch (bMask)
            {
                case (UInt32)BehaviorInputPorts.Digital0:
                    return source.Where(is_evt92).Select(input => { return true; });

                case (UInt32)BehaviorInputPorts.Digital1:
                    return source.Where(is_evt94).Select(input => { return true; });

                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Digital0 or Digital1 can be individually selected.");
            }
        }

        /************************************************************************/
        /* Raw Registers                                                        */
        /************************************************************************/
        static IObservable<Timestamped<byte>> ProcessRegisterInputs(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt32).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });
        }

        static IObservable<Timestamped<UInt16>> ProcessRegisterAnalogInput(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt44).Select(input => { return new Timestamped<UInt16>(BitConverter.ToUInt16(input.MessageBytes, 11), ParseTimestamp(input.MessageBytes, 5)); });
        }

        static IObservable<Timestamped<byte>> ProcessRegisterCamera(IObservable<HarpMessage> source, UInt32 bMask)
        {
            switch (bMask)
            {
                case (UInt32)BehaviorInputPorts.Digital0:
                    return source.Where(is_evt92).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });

                case (UInt32)BehaviorInputPorts.Digital1:
                    return source.Where(is_evt94).Select(input => { return new Timestamped<byte>(input.MessageBytes[11], ParseTimestamp(input.MessageBytes, 5)); });

                default:
                    throw new InvalidOperationException("Invalid Mask selection. Only Digital0 or Digital1 can be individually selected.");
            }
        }
    }
}
