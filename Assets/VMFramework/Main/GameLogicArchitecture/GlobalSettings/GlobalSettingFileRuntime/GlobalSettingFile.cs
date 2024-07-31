﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using VMFramework.Configuration;
using VMFramework.Core;
using VMFramework.Core.Linq;
using VMFramework.Procedure;

namespace VMFramework.GameLogicArchitecture
{
    public abstract partial class GlobalSettingFile : GameSettingBase, IGlobalSettingFile
    {
        public override void CheckSettings()
        {
            base.CheckSettings();

            GetAllGeneralSettings().CheckSettings();
            // foreach (var fieldInfo in GetType().GetFieldsByReturnType(typeof(IGeneralSetting),
            //              ReflectionUtility.ALL_INSTANCE_FIELDS_FLAGS))
            // {
            //     if (fieldInfo.GetValue(this) is not IGeneralSetting setting)
            //     {
            //         throw new ArgumentNullException($"{fieldInfo.Name}");
            //     }
            //     
            //     setting.CheckSettings();
            // }
        }

        IEnumerable<InitializationAction> IInitializer.GetInitializationActions()
        {
            return GetAllGeneralSettings().SelectMany(generalSetting => generalSetting.GetInitializationActions());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<IGeneralSetting> GetAllGeneralSettings()
        {
            return this.GetFieldsValueByReturnType<IGeneralSetting>(ReflectionUtility
                .ALL_INSTANCE_FIELDS_FLAGS).WhereNotNull();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<FieldInfo> GetAllGeneralSettingsFields()
        {
            return GetType()
                .GetFieldsByReturnType<IGeneralSetting>(ReflectionUtility.ALL_INSTANCE_FIELDS_FLAGS);
        }
    }
}