﻿#pragma checksum "..\..\frmNewOrder.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "04E593FD404D4766F81FA5BF1C89AEBFB219C3384A0B3B5CE7BFC1E460826B1A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using SaindWhichPresentationLayer;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace SaindWhichPresentationLayer {
    
    
    /// <summary>
    /// frmNewOrder
    /// </summary>
    public partial class frmNewOrder : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 30 "..\..\frmNewOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbCrtOrder;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\frmNewOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lbCrtOrderAddons;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\frmNewOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lstAddOns;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\frmNewOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblOrderFirstName;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\frmNewOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtOrderFirstName;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\frmNewOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblOrderLastName;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\frmNewOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtOrderLastName;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\frmNewOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblOrderEmail;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\frmNewOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtOrderEmail;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\frmNewOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAdditionalItem;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\frmNewOrder.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnSubmitOrder;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/SaindWhichPresentationLayer;component/frmneworder.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\frmNewOrder.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\frmNewOrder.xaml"
            ((SaindWhichPresentationLayer.frmNewOrder)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.lbCrtOrder = ((System.Windows.Controls.ListBox)(target));
            
            #line 30 "..\..\frmNewOrder.xaml"
            this.lbCrtOrder.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.LbCrtOrder_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.lbCrtOrderAddons = ((System.Windows.Controls.ListBox)(target));
            return;
            case 4:
            this.lstAddOns = ((System.Windows.Controls.ListBox)(target));
            
            #line 45 "..\..\frmNewOrder.xaml"
            this.lstAddOns.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.LstAddOns_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.lblOrderFirstName = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.txtOrderFirstName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.lblOrderLastName = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.txtOrderLastName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.lblOrderEmail = ((System.Windows.Controls.Label)(target));
            return;
            case 10:
            this.txtOrderEmail = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.btnAdditionalItem = ((System.Windows.Controls.Button)(target));
            
            #line 59 "..\..\frmNewOrder.xaml"
            this.btnAdditionalItem.Click += new System.Windows.RoutedEventHandler(this.BtnAdditionalItem_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            this.btnSubmitOrder = ((System.Windows.Controls.Button)(target));
            
            #line 61 "..\..\frmNewOrder.xaml"
            this.btnSubmitOrder.Click += new System.Windows.RoutedEventHandler(this.BtnSubmitOrder_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

