﻿// ***********************************************************************
// Assembly         : DbHelper
// Author           : p.g.parpura
// Created          : 01-09-2019
//
// Last Modified By : p.g.parpura
// Last Modified On : 01-09-2019
// ***********************************************************************
// <copyright file="MessageBoxEx.cs" company="">
//     Copyright ©  2018
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace DbHelper
{
    /// <summary>
    /// Class MessageBoxEx.
    /// Taken from https://stackoverflow.com/a/3498791/660753
    /// </summary>
    /// <autogeneratedoc />
    public class MessageBoxEx
    {
        /// <summary>
        /// The owner
        /// </summary>
        /// <autogeneratedoc />
        private static IWin32Window _Owner;
        /// <summary>
        /// The hook proc
        /// </summary>
        /// <autogeneratedoc />
        private static readonly HookProc _HookProc;
        /// <summary>
        /// The h hook
        /// </summary>
        /// <autogeneratedoc />
        private static IntPtr _HHook;

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>DialogResult.</returns>
        /// <autogeneratedoc />
        public static DialogResult Show(string text)
        {
            Initialize();
            return MessageBox.Show(text);
        }

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <returns>DialogResult.</returns>
        /// <autogeneratedoc />
        public static DialogResult Show(string text, string caption)
        {
            Initialize();
            return MessageBox.Show(text, caption);
        }

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns>DialogResult.</returns>
        /// <autogeneratedoc />
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons);
        }

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <returns>DialogResult.</returns>
        /// <autogeneratedoc />
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon);
        }

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="defButton">The definition button.</param>
        /// <returns>DialogResult.</returns>
        /// <autogeneratedoc />
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton);
        }

        /// <summary>
        /// Shows the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="defButton">The definition button.</param>
        /// <param name="options">The options.</param>
        /// <returns>DialogResult.</returns>
        /// <autogeneratedoc />
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            Initialize();
            return MessageBox.Show(text, caption, buttons, icon, defButton, options);
        }

        /// <summary>
        /// Shows the specified owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="text">The text.</param>
        /// <returns>DialogResult.</returns>
        /// <autogeneratedoc />
        public static DialogResult Show(IWin32Window owner, string text)
        {
            _Owner = owner;
            Initialize();
            return MessageBox.Show(owner, text);
        }

        /// <summary>
        /// Shows the specified owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <returns>DialogResult.</returns>
        /// <autogeneratedoc />
        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            _Owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption);
        }

        /// <summary>
        /// Shows the specified owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="buttons">The buttons.</param>
        /// <returns>DialogResult.</returns>
        /// <autogeneratedoc />
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            _Owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons);
        }

        /// <summary>
        /// Shows the specified owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <returns>DialogResult.</returns>
        /// <autogeneratedoc />
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            _Owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon);
        }

        /// <summary>
        /// Shows the specified owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="defButton">The definition button.</param>
        /// <returns>DialogResult.</returns>
        /// <autogeneratedoc />
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            _Owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon, defButton);
        }

        /// <summary>
        /// Shows the specified owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="text">The text.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="buttons">The buttons.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="defButton">The definition button.</param>
        /// <param name="options">The options.</param>
        /// <returns>DialogResult.</returns>
        /// <autogeneratedoc />
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton, MessageBoxOptions options)
        {
            _Owner = owner;
            Initialize();
            return MessageBox.Show(owner, text, caption, buttons, icon,
                                   defButton, options);
        }

        /// <summary>
        /// Delegate HookProc
        /// </summary>
        /// <param name="nCode">The n code.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>IntPtr.</returns>
        /// <autogeneratedoc />
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Delegate TimerProc
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="uMsg">The u MSG.</param>
        /// <param name="nIDEvent">The n identifier event.</param>
        /// <param name="dwTime">The dw time.</param>
        /// <autogeneratedoc />
        public delegate void TimerProc(IntPtr hWnd, uint uMsg, UIntPtr nIdEvent, uint dwTime);

        /// <summary>
        /// The wh callwndprocret
        /// </summary>
        /// <autogeneratedoc />
        public const int WH_CALLWNDPROCRET = 12;

        /// <summary>
        /// Enum CbtHookAction
        /// </summary>
        /// <autogeneratedoc />
        public enum CbtHookAction : int
        {
            /// <summary>
            /// The HCBT movesize
            /// </summary>
            /// <autogeneratedoc />
            HCBT_MOVESIZE = 0,
            /// <summary>
            /// The HCBT minmax
            /// </summary>
            /// <autogeneratedoc />
            HCBT_MINMAX = 1,
            /// <summary>
            /// The HCBT qs
            /// </summary>
            /// <autogeneratedoc />
            HCBT_QS = 2,
            /// <summary>
            /// The HCBT createwnd
            /// </summary>
            /// <autogeneratedoc />
            HCBT_CREATEWND = 3,
            /// <summary>
            /// The HCBT destroywnd
            /// </summary>
            /// <autogeneratedoc />
            HCBT_DESTROYWND = 4,
            /// <summary>
            /// The HCBT activate
            /// </summary>
            /// <autogeneratedoc />
            HCBT_ACTIVATE = 5,
            /// <summary>
            /// The HCBT clickskipped
            /// </summary>
            /// <autogeneratedoc />
            HCBT_CLICKSKIPPED = 6,
            /// <summary>
            /// The HCBT keyskipped
            /// </summary>
            /// <autogeneratedoc />
            HCBT_KEYSKIPPED = 7,
            /// <summary>
            /// The HCBT syscommand
            /// </summary>
            /// <autogeneratedoc />
            HCBT_SYSCOMMAND = 8,
            /// <summary>
            /// The HCBT setfocus
            /// </summary>
            /// <autogeneratedoc />
            HCBT_SETFOCUS = 9
        }

        /// <summary>
        /// Gets the window rect.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="lpRect">The lp rect.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <autogeneratedoc />
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, ref Rectangle lpRect);

        /// <summary>
        /// Moves the window.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="X">The x.</param>
        /// <param name="Y">The y.</param>
        /// <param name="nWidth">Width of the n.</param>
        /// <param name="nHeight">Height of the n.</param>
        /// <param name="bRepaint">if set to <c>true</c> [b repaint].</param>
        /// <returns>System.Int32.</returns>
        /// <autogeneratedoc />
        [DllImport("user32.dll")]
        private static extern int MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        /// <summary>
        /// Sets the timer.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nIDEvent">The n identifier event.</param>
        /// <param name="uElapse">The u elapse.</param>
        /// <param name="lpTimerFunc">The lp timer function.</param>
        /// <returns>UIntPtr.</returns>
        /// <autogeneratedoc />
        [DllImport("User32.dll")]
        public static extern UIntPtr SetTimer(IntPtr hWnd, UIntPtr nIDEvent, uint uElapse, TimerProc lpTimerFunc);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="Msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>IntPtr.</returns>
        /// <autogeneratedoc />
        [DllImport("User32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Sets the windows hook ex.
        /// </summary>
        /// <param name="idHook">The identifier hook.</param>
        /// <param name="lpfn">The LPFN.</param>
        /// <param name="hInstance">The h instance.</param>
        /// <param name="threadId">The thread identifier.</param>
        /// <returns>IntPtr.</returns>
        /// <autogeneratedoc />
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        /// <summary>
        /// Unhooks the windows hook ex.
        /// </summary>
        /// <param name="idHook">The identifier hook.</param>
        /// <returns>System.Int32.</returns>
        /// <autogeneratedoc />
        [DllImport("user32.dll")]
        public static extern int UnhookWindowsHookEx(IntPtr idHook);

        /// <summary>
        /// Calls the next hook ex.
        /// </summary>
        /// <param name="idHook">The identifier hook.</param>
        /// <param name="nCode">The n code.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>IntPtr.</returns>
        /// <autogeneratedoc />
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Gets the length of the window text.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <returns>System.Int32.</returns>
        /// <autogeneratedoc />
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        /// <summary>
        /// Gets the window text.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="text">The text.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>System.Int32.</returns>
        /// <autogeneratedoc />
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int maxLength);

        /// <summary>
        /// Ends the dialog.
        /// </summary>
        /// <param name="hDlg">The h dialog.</param>
        /// <param name="nResult">The n result.</param>
        /// <returns>System.Int32.</returns>
        /// <autogeneratedoc />
        [DllImport("user32.dll")]
        public static extern int EndDialog(IntPtr hDlg, IntPtr nResult);

        /// <summary>
        /// Struct CWPRETSTRUCT
        /// </summary>
        /// <autogeneratedoc />
        [StructLayout(LayoutKind.Sequential)]
        public struct CWPRETSTRUCT
        {
            /// <summary>
            /// The l result
            /// </summary>
            /// <autogeneratedoc />
            public IntPtr lResult;
            /// <summary>
            /// The l parameter
            /// </summary>
            /// <autogeneratedoc />
            public IntPtr lParam;
            /// <summary>
            /// The w parameter
            /// </summary>
            /// <autogeneratedoc />
            public IntPtr wParam;
            /// <summary>
            /// The message
            /// </summary>
            /// <autogeneratedoc />
            public uint message;
            /// <summary>
            /// The HWND
            /// </summary>
            /// <autogeneratedoc />
            public IntPtr hwnd;
        };

        /// <summary>
        /// Initializes static members of the <see cref="MessageBoxEx"/> class.
        /// </summary>
        /// <autogeneratedoc />
        static MessageBoxEx()
        {
            _HookProc = new HookProc(MessageBoxHookProc);
            _HHook = IntPtr.Zero;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <exception cref="NotSupportedException">multiple calls are not supported</exception>
        /// <autogeneratedoc />
        private static void Initialize()
        {
            if (_HHook != IntPtr.Zero)
            {
                throw new NotSupportedException("multiple calls are not supported");
            }

            if (_Owner != null)
            {
                _HHook = SetWindowsHookEx(WH_CALLWNDPROCRET, _HookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
            }
        }

        /// <summary>
        /// Messages the box hook proc.
        /// </summary>
        /// <param name="nCode">The n code.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>IntPtr.</returns>
        /// <autogeneratedoc />
        private static IntPtr MessageBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(_HHook, nCode, wParam, lParam);
            }

            CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));
            IntPtr hook = _HHook;

            if (msg.message == (int)CbtHookAction.HCBT_ACTIVATE)
            {
                try
                {
                    CenterWindow(msg.hwnd);
                }
                finally
                {
                    UnhookWindowsHookEx(_HHook);
                    _HHook = IntPtr.Zero;
                }
            }

            return CallNextHookEx(hook, nCode, wParam, lParam);
        }

        /// <summary>
        /// Centers the window.
        /// </summary>
        /// <param name="hChildWnd">The h child WND.</param>
        /// <autogeneratedoc />
        private static void CenterWindow(IntPtr hChildWnd)
        {
            Rectangle recChild = new Rectangle(0, 0, 0, 0);
            bool success = GetWindowRect(hChildWnd, ref recChild);

            int width = recChild.Width - recChild.X;
            int height = recChild.Height - recChild.Y;

            Rectangle recParent = new Rectangle(0, 0, 0, 0);
            success = GetWindowRect(_Owner.Handle, ref recParent);

            Point ptCenter = new Point(0, 0);
            ptCenter.X = recParent.X + ((recParent.Width - recParent.X) / 2);
            ptCenter.Y = recParent.Y + ((recParent.Height - recParent.Y) / 2);


            Point ptStart = new Point(0, 0);
            ptStart.X = (ptCenter.X - (width / 2));
            ptStart.Y = (ptCenter.Y - (height / 2));

            ptStart.X = (ptStart.X < 0) ? 0 : ptStart.X;
            ptStart.Y = (ptStart.Y < 0) ? 0 : ptStart.Y;

            int result = MoveWindow(hChildWnd, ptStart.X, ptStart.Y, width,
                                    height, false);
        }
    }
}
