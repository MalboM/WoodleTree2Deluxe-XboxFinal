using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace ICode
{
	public static class ReflectionUtility
	{
		private static readonly Dictionary<Type,FieldInfo[]> fieldInfoLookup;
		private static readonly Dictionary<Type,PropertyInfo[]> propertyInfoLookup;

		static ReflectionUtility ()
		{
			ReflectionUtility.fieldInfoLookup = new Dictionary<Type, FieldInfo[]> ();
			ReflectionUtility.propertyInfoLookup = new Dictionary<Type, PropertyInfo[]> ();
		}

		public static string[] GetAllComponentNames ()
		{
			IEnumerable<Type> types = typeof(Component).GetAssembly ().GetTypes ().Where (type => type.IsSubclassOf (typeof(Component)));//AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()) .Where(type => type.IsSubclassOf(typeof(Component)));
			return types.Select (x => x.FullName).ToArray ();
		}

		public static string[] GetFieldNames (this Type type)
		{
			FieldInfo[] fields = type.GetAllFields (BindingFlags.Public | BindingFlags.Instance).ToArray ();
			fields = fields.Where (x => FsmUtility.GetVariableType (x.FieldType) != null).ToArray ();
			return fields.Select (x => x.Name).ToArray ();
		}

		public static string[] GetPropertyNames (this Type type, bool requiresWrite)
		{
			PropertyInfo[] properties = type.GetProperties (BindingFlags.Public | BindingFlags.Instance).ToArray ();
			if (requiresWrite) {
				properties = properties.Where (x => x.CanWrite && FsmUtility.GetVariableType (x.PropertyType) != null).ToArray ();			
			}
			return properties.Select (x => x.Name).ToArray ();
		}

		public static string[] GetPropertyAndFieldNames (this Type type, bool requiresWrite)
		{
			List<string> names = new List<string> (type.GetPropertyNames (requiresWrite));
			names.AddRange (type.GetFieldNames ());
			return names.ToArray ();
		}

		public static string[] GetMethodNames (this Type type)
		{
			MethodInfo[] methods = type
				.GetMethods (BindingFlags.Public | BindingFlags.Instance)
					.ToArray ();
			return methods.Where (y => y.GetParameters ().Length == 0 && y.ReturnType == typeof(void)).Select (x => x.Name).ToArray ();
		}

		public static FieldInfo[] GetAllFields (this Type type, BindingFlags flags)
		{
			if (type == null) {
				return new FieldInfo[0];
			}
			return type.GetFields (flags).Concat (GetAllFields (type.GetBaseType (), flags)).ToArray ();
		}

		public static FieldInfo[] GetPublicFields (this object obj)
		{
			return GetPublicFields (obj.GetType ());
		}

		public static FieldInfo[] GetPublicFields (this Type type)
		{
			if (!ReflectionUtility.fieldInfoLookup.ContainsKey (type)) {
				fieldInfoLookup [type] = type.GetFields (BindingFlags.Instance | BindingFlags.Public);
			}
			
			return fieldInfoLookup [type];
		}

		public static PropertyInfo[] GetPublicProperties (this object obj)
		{
			return GetPublicProperties (obj.GetType ());
		}

		public static PropertyInfo[] GetPublicProperties (this Type type)
		{
			if (!ReflectionUtility.propertyInfoLookup.ContainsKey (type)) {
				propertyInfoLookup [type] = type.GetProperties (BindingFlags.Instance | BindingFlags.Public);
			}
			
			return propertyInfoLookup [type];
		}
		
		/*Moved to WindowsRuntimeExtension.cs
		 * 
		 * public static FieldInfo[] GetFields(this Type type){
			#if NETFX_CORE
			return type.GetRuntimeFields ().ToArray();
			#else
			return type.GetFields ();
			#endif
		}

		public static FieldInfo[] GetFields(this Type type, BindingFlags bindingFlags)
		{
			#if NETFX_CORE
			Type baseType;
			List<FieldInfo> fieldInfos = new List<FieldInfo>();
			Boolean flag = false;
			while (type != null)
			{
				foreach (FieldInfo runtimeField in type.GetRuntimeFields())
				{
					if (!ReflectionUtility.MatchBindingFlags(runtimeField.IsPublic, runtimeField.IsPrivate, runtimeField.IsStatic, bindingFlags, flag))
					{
						continue;
					}
					fieldInfos.Add(runtimeField);
				}
				if ((bindingFlags & BindingFlags.DeclaredOnly) == BindingFlags.Default)
				{
					baseType = type.GetTypeInfo().BaseType;
				}
				else
				{
					baseType = null;
				}
				type = baseType;
				flag = true;
			}
			return fieldInfos.ToArray();
			#else
			return type.GetFields(bindingFlags);
			#endif
		}


		
		public static FieldInfo GetField(this Type type,string name){
			
			#if NETFX_CORE
			return type.GetRuntimeField (name);
			#else
			return type.GetField (name);
			#endif
		}
		
		public static PropertyInfo GetProperty(this Type type,string name){
			#if NETFX_CORE
			return type.GetRuntimeProperty (name);
			#else
			return type.GetProperty (name);
			#endif
		}

		public static PropertyInfo[] GetProperties(this Type type, BindingFlags bindingFlags)
		{
			#if NETFX_CORE
			Type baseType;
			List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
			Boolean flag = false;
			while (type != null)
			{
				foreach (PropertyInfo runtimeProperty in type.GetRuntimeProperties())
				{
					MethodInfo getMethod = runtimeProperty.GetMethod ?? runtimeProperty.SetMethod;
					if (!ReflectionUtility.MatchBindingFlags(getMethod.IsPublic, getMethod.IsPrivate, getMethod.IsStatic, bindingFlags, flag))
					{
						continue;
					}
					propertyInfos.Add(runtimeProperty);
				}
				if ((bindingFlags & BindingFlags.DeclaredOnly) == BindingFlags.Default)
				{
					baseType = type.GetTypeInfo().BaseType;
				}
				else
				{
					baseType = null;
				}
				type = baseType;
				flag = true;
			}
			return propertyInfos.ToArray();
			#else
			return type.GetProperties(bindingFlags);
			#endif
		}

		public static Type[] GetGenericArguments(this Type type){
			#if NETFX_CORE
			return type.GetTypeInfo().GetGenericArguments();
			#else
			return type.GetGenericArguments();
			#endif
		}
		
		public static bool IsSubclassOf(this Type type,Type c){
			#if NETFX_CORE
			return type.GetTypeInfo().IsSubclassOf(c);
			#else
			return type.IsSubclassOf(c);
			#endif
		}

		public static bool IsValueType(this Type type)
		{
			#if NETFX_CORE
			return type.GetTypeInfo().IsValueType;
			#else
			return type.IsValueType;
			#endif
		}


		public static bool IsGenericType(this Type type)
		{
			#if NETFX_CORE
			return type.GetTypeInfo().IsGenericType;
			#else
			return type.IsGenericType;
			#endif
		}

		public static bool IsAssignableFrom(this Type type,Type c){
			#if NETFX_CORE
			return type.GetTypeInfo().IsAssignableFrom(c);
			#else
			return type.IsAssignableFrom(c);
			#endif
		}

		public static object[] GetCustomAttributes(this Type type,bool inherit){
			#if NETFX_CORE
			return type.GetTypeInfo().GetCustomAttributes(inherit);
			#else
			return type.GetCustomAttributes(inherit);
			#endif
		}

		public static MethodInfo GetMethod(this Type type,string methodName){
			#if NETFX_CORE
			return type.GetTypeInfo().GetMethod(methodName);
			#else
			return type.GetMethod(methodName);
			#endif
		}

		public static Boolean MatchBindingFlags(Boolean isPublic, Boolean isPrivate, Boolean isStatic, BindingFlags bindingAttr, Boolean baseClass)
		{
			if (isPublic)
			{
				if ((bindingAttr & BindingFlags.Public) == BindingFlags.Default)
				{
					return false;
				}
			}
			else if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
			{
				return false;
			}
			if (isStatic)
			{
				if ((bindingAttr & BindingFlags.Static) == BindingFlags.Default)
				{
					return false;
				}
				if (baseClass)
				{
					if ((bindingAttr & BindingFlags.FlattenHierarchy) == BindingFlags.Default)
					{
						return false;
					}
					if (isPrivate)
					{
						return false;
					}
				}
			}
			else if ((bindingAttr & BindingFlags.Instance) == BindingFlags.Default)
			{
				return false;
			}
			return true;
		}*/
	}
}