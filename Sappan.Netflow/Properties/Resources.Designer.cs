﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sappan.Netflow.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sappan.Netflow.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified alignment is a multiple of a byte..
        /// </summary>
        internal static string ErrorAlignmentNotByteMultiple {
            get {
                return ResourceManager.GetString("ErrorAlignmentNotByteMultiple", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The size of the copy buffer is too small..
        /// </summary>
        internal static string ErrorCopyBufferSize {
            get {
                return ResourceManager.GetString("ErrorCopyBufferSize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The enumerator is out of range..
        /// </summary>
        internal static string ErrorEnumeratorRange {
            get {
                return ResourceManager.GetString("ErrorEnumeratorRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The length of the data structure cannot be expressed with 16 bits. Remove elements from the structure or template to decrease its size..
        /// </summary>
        internal static string ErrorLengthTooLarge {
            get {
                return ResourceManager.GetString("ErrorLengthTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The option template is malformed as the total size {0} is not divisble by the size {1} of a field..
        /// </summary>
        internal static string ErrorMalformedOptions {
            get {
                return ResourceManager.GetString("ErrorMalformedOptions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The scope template is malformed as the total size {0} is not divisble by the size {1} of a single scope..
        /// </summary>
        internal static string ErrorMalformedScopes {
            get {
                return ResourceManager.GetString("ErrorMalformedScopes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The size of a NetFlow field must be positive or 0 to indicate a variable length field..
        /// </summary>
        internal static string ErrorNegativeNetFlowFieldLength {
            get {
                return ResourceManager.GetString("ErrorNegativeNetFlowFieldLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The offset cannot be negative..
        /// </summary>
        internal static string ErrorNegativeOffset {
            get {
                return ResourceManager.GetString("ErrorNegativeOffset", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NetFlow version 5 only supports IPv4 addresses..
        /// </summary>
        internal static string ErrorNetflow5OnlyIPv4 {
            get {
                return ResourceManager.GetString("ErrorNetflow5OnlyIPv4", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field {0} is not annotated with a fixed size..
        /// </summary>
        internal static string ErrorNonFixedFieldType {
            get {
                return ResourceManager.GetString("ErrorNonFixedFieldType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The list of data records contains at least one non-trivial type. Please make sure to use only primitive types or structures with a sequential or explicit layout as data record..
        /// </summary>
        internal static string ErrorNonTrivialRecord {
            get {
                return ResourceManager.GetString("ErrorNonTrivialRecord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified type has no fields or properties that are marked as part of the on-wire representation..
        /// </summary>
        internal static string ErrorNoOnWireMembers {
            get {
                return ResourceManager.GetString("ErrorNoOnWireMembers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Members to be considered for the on-wire representation must either be fields or properties..
        /// </summary>
        internal static string ErrorOnWireMemberType {
            get {
                return ResourceManager.GetString("ErrorOnWireMemberType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The flow reader is not expecting the requested kind of data..
        /// </summary>
        internal static string ErrorReaderState {
            get {
                return ResourceManager.GetString("ErrorReaderState", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are insufficient data provided to convert to the selected target type..
        /// </summary>
        internal static string ErrorTooFewDataToConvert {
            get {
                return ResourceManager.GetString("ErrorTooFewDataToConvert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The current number of fields cannot be expressed with 16 bits. Remove elements from the structure or template..
        /// </summary>
        internal static string ErrorTooManyFields {
            get {
                return ResourceManager.GetString("ErrorTooManyFields", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In order to be decoded automatically, the trivial members of a NetFlow data class must have contigous OnWireOrder attributes. Variable-length fields must be located at the end of the structure..
        /// </summary>
        internal static string ErrorTrivialMembersNotContiguous {
            get {
                return ResourceManager.GetString("ErrorTrivialMembersNotContiguous", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The flow writer is not in a state that would allow writing the specified data..
        /// </summary>
        internal static string ErrorWriterState {
            get {
                return ResourceManager.GetString("ErrorWriterState", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to By convention, flow set IDs of data flow sets must must be 256 or larger..
        /// </summary>
        internal static string ErrorWrongDataFlowSetID {
            get {
                return ResourceManager.GetString("ErrorWrongDataFlowSetID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to By convention, set IDs of IPFIX data flow sets must must be 256 or larger..
        /// </summary>
        internal static string ErrorWrongDataSetID {
            get {
                return ResourceManager.GetString("ErrorWrongDataSetID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The on-wire representation of the specified value does not match the declared length of the field..
        /// </summary>
        internal static string ErrorWrongOnWireLength {
            get {
                return ResourceManager.GetString("ErrorWrongOnWireLength", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to By convention, the flow set ID of an option template must be smaller than 256..
        /// </summary>
        internal static string ErrorWrongOptionsTemplateID {
            get {
                return ResourceManager.GetString("ErrorWrongOptionsTemplateID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified range cannot be extracted from the given array..
        /// </summary>
        internal static string ErrorWrongSubarray {
            get {
                return ResourceManager.GetString("ErrorWrongSubarray", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to By convention, flow set IDs of templates must be smaller than 256..
        /// </summary>
        internal static string ErrorWrongTemplateFlowSetID {
            get {
                return ResourceManager.GetString("ErrorWrongTemplateFlowSetID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to By convention, the IDs of templates that define data records must be 256 or larger..
        /// </summary>
        internal static string ErrorWrongTemplateID {
            get {
                return ResourceManager.GetString("ErrorWrongTemplateID", resourceCulture);
            }
        }
    }
}
