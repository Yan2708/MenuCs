using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Menu
{
    internal class Menu
    {
        static MethodInfo[] Methods;
        static Menu()
        {
            Methods = typeof(Exo).GetMethods(BindingFlags.Static | BindingFlags.Public);
        }
        static void Launch()
        {
            while (true)
            {
                MenuPrint();
                string input = Console.ReadLine();
                if (input == "exit")
                    return;

                if (int.TryParse(input, out int index))
                {
                    InvokeMethod(index);
                }
                else
                {
                    Console.WriteLine("Function not found\n");
                }
                Console.ReadLine();
            }
        }

        static void MenuPrint()
        {
            Console.WriteLine("\nMenu");
            MethodInfo[] ok = typeof(Exo).GetMethods(BindingFlags.Static);
            for (int i = 0; i < Methods.Length; i++)
            {
                Console.WriteLine($"{i} {Methods[i].Name}");
            }
            Console.WriteLine("type exit to exit\n");
        }

        static void InvokeMethod(int index)
        {
            MethodInfo method = Methods[index];
            ParameterInfo[] parameters = method.GetParameters();
            object[] args = new object[parameters.Length];
            Console.WriteLine(method.Name);
            Console.WriteLine("Parameters : if array separate values with ','");
            for (int i = 0; i < parameters.Length; i++)
            {
                Console.WriteLine($"{parameters[i].Name} {parameters[i].ParameterType}");
                try
                {
                    args[i] = inputConverter(parameters[i]);
                    Console.WriteLine();
                }
                catch (Exception _)
                {
                    Console.WriteLine("the parameter is not valid");
                    return;
                }
            }
            dynamic result = method.Invoke(null, args);
            if (method.ReturnType.IsArray)
                PrintArray(result);
            else
                Console.WriteLine(result);
        }
        static dynamic inputConverter(ParameterInfo type)
        {
            string val = Console.ReadLine();

            if (type.ParameterType.IsArray)
            {
                string[] tab = val.Split(",");
                dynamic res = Activator.CreateInstance(type.ParameterType, args: tab.Length);
                for (int i = 0; i < tab.Length; i++)
                {
                    res[i] = (Converter(Type.GetTypeCode(type.ParameterType.GetElementType()), tab[i]));
                }
                return res;
            }
            else
            {
                return Converter(Type.GetTypeCode(type.ParameterType), val);
            }
        }

        static dynamic Converter(TypeCode code, dynamic val)
        {
            switch (code)
            {
                case TypeCode.Int32:
                    val = Convert.ToInt32(val);
                    break;
                case TypeCode.Boolean:
                    val = Convert.ToBoolean(val);
                    break;
                case TypeCode.Double:
                    val = Convert.ToDouble(val);
                    break;
                case TypeCode.Decimal:
                    val = Convert.ToDecimal(val);
                    break;

            }
            return val;
        }

        static void PrintArray(dynamic[] tab)
        {
            string res = "";
            foreach (dynamic val in tab)
            {
                res += $"{val.ToString()} ";
            }
            Console.WriteLine(res);
        }
    }
}
