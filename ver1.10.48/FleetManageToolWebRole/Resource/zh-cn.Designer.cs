﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18051
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FleetManageToolWebRole.Resource {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class zh_cn {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal zh_cn() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FleetManageToolWebRole.Resource.zh-cn", typeof(zh_cn).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
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
        ///   查找类似 添加电子围栏 的本地化字符串。
        /// </summary>
        internal static string AddGeofence {
            get {
                return ResourceManager.GetString("AddGeofence", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 报警车辆 的本地化字符串。
        /// </summary>
        internal static string AlertVehicle {
            get {
                return ResourceManager.GetString("AlertVehicle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 故障车辆 的本地化字符串。
        /// </summary>
        internal static string BreakVehicle {
            get {
                return ResourceManager.GetString("BreakVehicle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 行驶车辆 的本地化字符串。
        /// </summary>
        internal static string DriverVehicle {
            get {
                return ResourceManager.GetString("DriverVehicle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 整个车队 的本地化字符串。
        /// </summary>
        internal static string Entire {
            get {
                return ResourceManager.GetString("Entire", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无联系车辆 的本地化字符串。
        /// </summary>
        internal static string MissTargetVehicle {
            get {
                return ResourceManager.GetString("MissTargetVehicle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 我的主界面 的本地化字符串。
        /// </summary>
        internal static string MyDashBoard {
            get {
                return ResourceManager.GetString("MyDashBoard", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 当前位置 的本地化字符串。
        /// </summary>
        internal static string NowLocation {
            get {
                return ResourceManager.GetString("NowLocation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 停驶车辆 的本地化字符串。
        /// </summary>
        internal static string StopVehicle {
            get {
                return ResourceManager.GetString("StopVehicle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 查看全部记录 的本地化字符串。
        /// </summary>
        internal static string ViewAllTripLog {
            get {
                return ResourceManager.GetString("ViewAllTripLog", resourceCulture);
            }
        }
    }
}