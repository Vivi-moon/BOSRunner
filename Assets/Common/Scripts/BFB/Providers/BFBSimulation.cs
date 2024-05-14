using System;
using System.Text;
using UnityEngine;
using Biofeedback;

namespace Common.BFB
{
    public static class BFBSimulation
    {
        private const KeyCode MoveFrameUpKey = KeyCode.Keypad8;
        private const KeyCode MoveFrameDownKey = KeyCode.Keypad2;

        private const KeyCode ResizeFrameUpKey = KeyCode.KeypadMultiply;
        private const KeyCode ResizeFrameDownKey = KeyCode.KeypadDivide;

        private const KeyCode MoveValueUpKey = KeyCode.Keypad6;
        private const KeyCode MoveValueDownKey = KeyCode.Keypad4;

        private const KeyCode SwitchServerPauseKey = KeyCode.P;
        private const KeyCode SwitchServerStopKey = KeyCode.Q;


        public static IDataProviderBFB CreateSimulate()
        {
            int channelsCount = 4;
            bool needWaitPeriodFrame = true;

            Debug.LogFormat("Setup simulation with {0} channels (wait period frame {1})"
                , channelsCount, needWaitPeriodFrame);
            Debug.Log(GetSimulateControlInstruction());
            return GameFab.InstantiateBFBDesktopSimulation(channelsCount, needWaitPeriodFrame);
        }

        public static string GetSimulateControlInstruction()
        {
            StringBuilder str = new StringBuilder();

            string format = "  {0}: {1}";

            str.AppendLine("Simulate controls:");

            str.AppendLine(string.Format(format, nameof(MoveFrameUpKey), MoveFrameUpKey));
            str.AppendLine(string.Format(format, nameof(MoveFrameDownKey), MoveFrameDownKey));
            str.AppendLine(string.Format(format, nameof(ResizeFrameUpKey), ResizeFrameUpKey));
            str.AppendLine(string.Format(format, nameof(ResizeFrameDownKey), ResizeFrameDownKey));
            str.AppendLine(string.Format(format, nameof(MoveValueUpKey), MoveValueUpKey));
            str.AppendLine(string.Format(format, nameof(MoveValueDownKey), MoveValueDownKey));
            str.AppendLine(string.Format(format, nameof(SwitchServerPauseKey), SwitchServerPauseKey));
            str.AppendLine(string.Format(format, nameof(SwitchServerStopKey), SwitchServerStopKey));

            return str.ToString();
        }

        public static void UpdateControls(IDataProviderBFB bfb)
        {
            var simulate = bfb as IBFBSimulateControl;
            if (simulate == null) { return; }

            int frame = 10;
            int column = 5;

            if (Input.GetKeyDown(MoveFrameUpKey))
            {
                simulate.MoveFrame(frame);
            }
            else if (Input.GetKeyDown(MoveFrameDownKey))
            {
                simulate.MoveFrame(-frame);
            }

            if (Input.GetKeyDown(ResizeFrameUpKey))
            {
                simulate.ResizeFrame(frame);
            }
            else if (Input.GetKeyDown(ResizeFrameDownKey))
            {
                simulate.ResizeFrame(-frame);
            }

            if (Input.GetKeyDown(MoveValueUpKey))
            {
                simulate.MoveCurrentValue(column);
            }
            else if (Input.GetKeyDown(MoveValueDownKey))
            {
                simulate.MoveCurrentValue(-column);
            }

            if (Input.GetKeyDown(SwitchServerPauseKey))
            {
                simulate.SwitchServerPause();
            }
            if (Input.GetKeyDown(SwitchServerStopKey))
            {
                simulate.Stop();
            }
        }
    }
}