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
    public enum CameraEventType : byte
    {
        /* Event: INPUT0 */
        Input0,

        /* Events: CAM0, CAM1, SYNC0 and SYNC1 */
        Camera0Trig,
        Camera0Sync,
        Camera1Trig,
        Camera1Sync
    }

    [Description(
        "\n" +
        "Input0: Boolean\n" +
        "\n" +
        "Camera0Trig: Boolean\n" +
        "Camera0Sync: Boolean\n" +
        "\n" +
        "Camera1Trig: Boolean\n" +
        "Camera1Sync: Boolean\n"
    )]

    public class CameraEvent : SingleArgumentExpressionBuilder, INamedElement
    {
        public CameraEvent()
        {
            Type = CameraEventType.Input0;
        }

        string INamedElement.Name
        {
            get { return typeof(CameraEvent).Name.Replace("Event", string.Empty) + "." + Type.ToString(); }
        }

        public CameraEventType Type { get; set; }

        public override Expression Build(IEnumerable<Expression> expressions)
        {
            var expression = expressions.First();
            switch (Type)
            {
                /************************************************************************/
                /* Register: INPUT0                                                     */
                /************************************************************************/
                case CameraEventType.Input0:
                    return Expression.Call(typeof(CameraEvent), "ProcessInputs0", null, expression);

                /************************************************************************/
                /* Register:  CAM0, CAM1, SYNC0 and SYNC1                               */
                /************************************************************************/
                case CameraEventType.Camera0Trig:
                    return Expression.Call(typeof(CameraEvent), "ProcessCamera0Trig", null, expression);
                case CameraEventType.Camera0Sync:
                    return Expression.Call(typeof(CameraEvent), "ProcessCamera0Sync", null, expression);
                case CameraEventType.Camera1Trig:
                    return Expression.Call(typeof(CameraEvent), "ProcessCamera1Trig", null, expression);
                case CameraEventType.Camera1Sync:
                    return Expression.Call(typeof(CameraEvent), "ProcessCamera1Sync", null, expression);

                /************************************************************************/
                /* Default                                                              */
                /************************************************************************/
                default:
                    throw new InvalidOperationException("Invalid selection or not supported yet.");
            }
        }

        static bool is_evt39(HarpMessage input) { return ((input.Address == 39) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt40(HarpMessage input) { return ((input.Address == 40) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt41(HarpMessage input) { return ((input.Address == 41) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt42(HarpMessage input) { return ((input.Address == 42) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
        static bool is_evt43(HarpMessage input) { return ((input.Address == 43) && (input.Error == false) && (input.MessageType == MessageType.Event)); }
                
        /************************************************************************/
        /* Register: INPUT0                                                     */
        /************************************************************************/
        static IObservable<bool> ProcessInputs0(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt39).Select(input => { return ((input.MessageBytes[11] & (1 << 0)) == (1 << 0)) ? true : false; });
        }

        /************************************************************************/
        /* Register: CAM0, CAM1, SYNC0 and SYNC1                                */
        /************************************************************************/
        static IObservable<bool> ProcessCamera0Trig(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt40).Select(input => { return ((input.MessageBytes[11] & (1 << 0)) == (1 << 0)) ? true : false; });
        }
        static IObservable<bool> ProcessCamera0Sync(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt42).Select(input => { return ((input.MessageBytes[11] & (1 << 0)) == (1 << 0)) ? true : false; });
        }
        static IObservable<bool> ProcessCamera1Trig(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt41).Select(input => { return ((input.MessageBytes[11] & (1 << 0)) == (1 << 0)) ? true : false; });
        }
        static IObservable<bool> ProcessSync1(IObservable<HarpMessage> source)
        {
            return source.Where(is_evt43).Select(input => { return ((input.MessageBytes[11] & (1 << 0)) == (1 << 0)) ? true : false; });
        }
    }
}
