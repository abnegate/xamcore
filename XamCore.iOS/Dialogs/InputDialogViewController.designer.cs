﻿// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace XamCore.iOS.Dialogs
{
    [Register ("InputDialogViewController")]
    partial class InputDialogViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel MessageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView InputText { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton OkButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CancelButton is not null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (InputText is not null) {
                InputText.Dispose ();
                InputText = null;
            }

            if (OkButton is not null) {
                OkButton.Dispose ();
                OkButton = null;
            }

            if (TitleLabel is not null) {
                TitleLabel.Dispose();
                TitleLabel = null;
            }
        }
    }
}