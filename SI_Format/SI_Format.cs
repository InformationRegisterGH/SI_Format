// Maintain this file in UTF-8 coding
using System;
using System.Reflection;

namespace InfoReg
{

    /// <summary>
    /// SI_Format is a class that contains functions to adjust or parse numbers to and from
    /// strings like 10pF or 123.46 kilo-metres or 65.123 ml.
    /// </summary>
    public static class SI_Format
    {
        /// <summary>
        /// SI_Prefixes full name
        /// </summary>
        public static string[] SI_Prefixes = { "quetta", "ronna", "yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
                        "", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto", "ronto", "quecto" };
        /// <summary>
        /// SI Prefixes single character
        /// </summary>
        public static string[] SI_ShortPrefixes = { "Q", "R", "Y", "Z", "E", "P", "T", "G", "M", "k",
                        "", "m", "μ", "n", "p", "f", "a", "z", "y", "r", "q" };
        /// <summary>
        /// StringShort_Prefixes
        /// </summary>
        public static string StringShortPrefixes = "QRYZEPTGMk mμnpfazyrq";

        /// <summary>
        /// Padding is an enumerated type.
        /// An enumerated value to indicate if a dash "-" is required between the SI prefix and the unit as in kilo-gram. 
        /// Padding will also indicate if a trailing space should be appended as padding.
        /// </summary>
        public enum Padding
        {
            /// <summary>
            /// dashonly: Default behaviour for Format function.
            /// </summary>
            dashonly,
            /// <summary>
            /// dashWithPadding: Use a dash but no trailing space
            /// </summary>
            dashWithPadding,
            /// <summary>
            /// paddingOnly: Add a trailing space only
            /// </summary>
            paddingOnly,
            /// <summary>
            /// noPaddingOrDash: No dash and no trailing space required
            /// </summary>
            noPaddingOrDash
        };

        /// <summary>
        /// Format&lt;T&gt; 
        /// is a generic function that takes a numeric value like 123,450 and returns a string like "123.45 km".
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tval"></param>
        /// <param name="sformat"></param>
        /// <param name="siunit"></param>
        /// <param name="padding"></param>
        /// <returns>Formatted string e.g. "9.46 peta-metres"</returns>
        /// 
        /// Returns values in text in SI format. An example is 123.45km.
        /// It takes tval and looks at its decimal exponent. The exponent 
        /// is reduced to its residue three value. It then prefixes the unit
        /// passed in with the appropriate SI prefix.
        /// 
        /// "quetta", "ronna", "yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
        /// "", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto", "ronto", "quecto"
        /// 
        /// If siunit is two or less characters the return will use short SI
        /// prefixes like:
        /// "Q", "R", "Y", "Z", "E", "P", "T", "G", "M", "k",
        /// "", "m", "μ", "n", "p", "f", "a", "z", "y", "r", "q"
        /// 
        /// No prefix is needed if the value of d_val lies in the range 0.0 to just under 1000.0.
        /// 
        /// Note: hecto, deca, deci, and centi are not supported.
        ///       SI does not support numbers above 10^33 or below 10^-30 
        ///       and any such value will be returned unmodified without SI prefix units.
        ///       

        /// <example>
        ///          using InfoReg;
        ///          ...
        ///          String ans;
        ///          double val = 123.456e17;
        ///          ans = InfoReg.SI_Format.Format(val, "G6", "metres");
        ///          => ans contains: "12.3456 exa-metres"
        ///          ans = InfoReg.SI_Format.Format(val, "G6", "metres", Padding.noPaddingOrDash);
        ///          => ans contains: "12.3456 exametres"
        /// </example>
        public static String Format<T>(T tval, string sformat, string siunit, Padding padding = Padding.dashonly)
        {
            // Note: hecto, deca, deci, and centi are not supported
            // si does not support numbers above 10^27 or below 10^-24 
            // return unmodified without SI prefix units
            Double exp;
            T _val = tval;
            exp = Math.Log10((double)Convert.ChangeType(tval, typeof(Double)));
            if (exp >= 33.0 || exp <= -30.0)
            {
                return string.Format("{0:" + sformat + "} {1}", tval, siunit);
            }
            int exp1 = (int)(exp / 3) * 3;
            int adjust = 10; // Array element for no SI prefix
            if (exp < 0)
            {
                exp1 -= 3;
            }
            if (typeof(T) == typeof(Double))
            {
                _val = (T)Convert.ChangeType(((Double)Convert.ChangeType(_val, typeof(Double)) / Math.Pow(10.0, exp1)), typeof(T));
            }
            if (typeof(T) == typeof(Single))
            {
                _val = (T)Convert.ChangeType(((Single)Convert.ChangeType(_val, typeof(Single)) / MathF.Pow(10.0f, exp1)), typeof(T));
            }
            if (typeof(T) == typeof(Decimal))
            {
                _val = (T)Convert.ChangeType(((Decimal)Convert.ChangeType(_val, typeof(Decimal)) / (Decimal)Math.Pow(10.0, exp1)), typeof(T));
            }

            string si_prefixtouse = string.Empty;
            int prefix_choice = -(exp1 / 3) + adjust;
            if (siunit.Length >= 3)
            {
                si_prefixtouse = SI_Prefixes[-(exp1 / 3) + adjust];
            }
            else
            {
                si_prefixtouse = SI_ShortPrefixes[prefix_choice];
            }
            string si_paddingtouse = string.Empty;
            if (padding == Padding.dashWithPadding || padding == Padding.paddingOnly)
            {
                si_paddingtouse = " ";
            }
            string si_dashtouse = string.Empty;
            if (padding == Padding.dashWithPadding || padding == Padding.dashonly)
            {
                if (prefix_choice != 10) si_dashtouse = "-";  // No dash for no SI prefix (0 <= tval < 1,000)
            }
            return string.Format("{0:" + sformat + "} ", _val) + si_prefixtouse + si_dashtouse + siunit + si_paddingtouse;
        }

        /// <summary>
        /// *** Depreciated use Format<double> instead *** // </double>
        /// Returns values in text in SI format. An example is 123.45km.
        /// It takes a double value and looks at its decimal exponent. The exponent 
        /// is reduced to its residue three value. It then prefixes the unit
        /// passed in with the appropriate SI prefix.
        /// 
        /// "quetta", "ronna", "yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
        /// "", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto", "ronto", "quecto"
        /// 
        /// If siunit is two or less characters the return will use short SI
        /// prefixes like:
        /// "Q", "R", "Y", "Z", "E", "P", "T", "G", "M", "k",
        /// "", "m", "μ", "n", "p", "f", "a", "z", "y", "r", "q"
        /// 
        /// No prefix is needed if the value of d_val lies in the range 0.0 to just under 1000.0.
        /// 
        /// Note: hecto, deca, deci, and centi are not supported.
        ///       SI does not support numbers above 10^33 or below 10^-30 
        ///       and any such value will be returned unmodified without SI prefix units.
        ///       
        /// </summary>
        /// <param name="d_val">A double value to be SI normalized.</param>
        /// <param name="sformat">Is the format string usually based on G or N </param>
        /// <param name="siunit">An SI unit like watt, metre or l</param>
        /// <param name="padding">Padding.dashOnly | Padding.dashWithPadding | Padding.paddingOnly | Padding.noPaddingOrDash</param>
        /// <returns>Formatted string e.g. "9.46 peta-metres"</returns>
        /// <example>
        ///          using InfoReg;
        ///          ...
        ///          String ans;
        ///          double val = 123.456e17;
        ///          ans = InfoReg.SI_Format.Format(val, "G6", "metres");
        ///          => ans contains: "12.3456 exa-metres"
        ///          ans = InfoReg.SI_Format.Format(val, "G6", "metres", noPaddingOrDash);
        ///          => ans contains: "12.3456 exametres"
        /// </example>
        [Obsolete("Please use Format<double>(double d_val, string sformat, string siunit, Padding padding = Padding.dashonly)", true)]
        public static String Format(double d_val, string sformat, string siunit, Padding padding = Padding.dashonly)
        {
            return Format<double>(d_val, sformat, siunit, padding);
        }

        /// <summary>
        /// *** Depreciated use Format<float> instead *** // </float>
        /// Returns values in a text SI format. An example is 123.45km.
        /// It takes a float value and looks at its decimal exponent. The exponent 
        /// is reduced to its residue three value. It then prefixes the unit
        /// passed in with the appropriate SI prefix.
        /// 
        /// "quetta", "ronna", "yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
        /// "", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto", "ronto", "quecto"
        /// 
        /// If siunit is two or less characters the return will use short SI
        /// prefixes like:
        /// "Q", "R", "Y", "Z", "E", "P", "T", "G", "M", "k",
        /// "", "m", "μ", "n", "p", "f", "a", "z", "y", "r", "q"
        /// 
        /// No prefix is needed if the value of d_val lies in the range 0.0 to just under 1000.0.
        /// 
        /// Note: hecto, deca, deci, and centi are not supported.
        ///       SI does not support numbers above 10^33 or below 10^-30 
        ///       and any such value will be returned unmodified without SI prefix units.
        /// Example: ...
        ///          using InfoReg;
        ///          ...   
        ///          String ans;
        ///          float fval = (float)123.789E-7;
        ///          ans = InfoReg.SI_Format.Format(fval, "G4", "F");
        ///          => ans contains: "12.38 μF"
        ///          ans = InfoReg.SI_Format.Format(fval, "G4", "Farads", InfoReg.SI_Format.Padding.dashWithPadding);
        ///          => ans contains: "12.38 micro-Farads " // Both a dash and trailing space are used
        /// </summary>
        /// <param name="f_val">A float value to be SI normalized.</param>
        /// <param name="sformat">Is the format string usually based on G or N </param>
        /// <param name="siunit">An SI unit like watt, metre or l</param>
        /// <param name="padding">Padding.dashOnly | Padding.dashWithPadding | Padding.paddingOnly | Padding.noPaddingOrDash</param>
        /// <returns>Formatted string e.g. "9.46 peta-metres"</returns>

        [Obsolete("Please use Format<Single>(Single f_val, string sformat, string siunit, Padding padding = Padding.dashonly)", true)]
        public static string Format(Single f_val, string sformat, string siunit, Padding padding = Padding.dashonly)
        {
            return Format<Single>(f_val, sformat, siunit, padding);
        }

        /// <summary>
        /// *** Depreciated use Format<decimal> instead *** // </decimal>
        /// Returns values in text in SI format. An example is 123.45km.
        /// It takes a decimal value and looks at its decimal exponent. The exponent 
        /// is reduced to its residue three value. It then prefixes the unit
        /// passed in with the appropriate SI prefix.
        /// 
        /// "quetta", "ronna", "yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
        /// "", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto", "ronto", "quecto"
        /// 
        /// If siunit is two or less characters the return will use short SI
        /// prefixes like:
        /// "Q", "R", "Y", "Z", "E", "P", "T", "G", "M", "k",
        /// "", "m", "μ", "n", "p", "f", "a", "z", "y", "r", "q"
        /// 
        /// No prefix is needed if the value of d_val lies in the range 0.0 to just under 1000.0.
        /// 
        /// Note: hecto, deca, deci, and centi are not supported.
        ///       SI does not support numbers above 10^33 or below 10^-30 
        ///       and any such value will be returned unmodified without SI prefix units.
        /// Example: ...
        ///          using InfoReg;
        ///          ...
        ///          String ans;
        ///          Decimal decimal_val = Decimal.Parse("1234.5678901234567890123");
        ///          ans = InfoReg.SI_Format.Format(decimal_val, "G21", "grams");
        ///          => ans contains: "1.23456789012345678901 kilo-grams"
        ///          ans = InfoReg.SI_Format.Format(decimal_val, "G21", "grams", InfoReg.SI_Format.Padding.paddingOnly);
        ///          => ans contains: "1.23456789012345678901 kilograms " // trailing space added
        /// </summary>
        /// <param name="decimal_val">A decimal value to be SI normalized.</param>
        /// <param name="sformat">Is the format string usually based on G or N </param>
        /// <param name="siunit">An SI unit like watt, metre or l</param>
        /// <param name="padding">Padding.dashOnly | Padding.dashWithPadding | Padding.paddingOnly | Padding.noPaddingOrDash</param>
        /// <returns>Formatted string e.g. "9.46 pm"</returns>

        [Obsolete("Please use Format<decimal>(decimal decimal_val, string sformat, string siunit, Padding padding = Padding.dashonly)", true)]
        public static string Format(decimal decimal_val, string sformat, string siunit, Padding padding = Padding.dashonly)
        {
            return Format<decimal>(decimal_val, sformat, siunit, padding);
        }

        /// <summary>
        /// Parse&lt;T&gt; is a generic function that takes a string value like "12.34 km" and returns a numeric value
        /// Takes an SI formatted value like "12.34 km" and yields an out parameter of type double, float, or decimal with the
        /// value 1.234e4. A String "10pF" would be returned as a numeric value 1e-11.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="si_value"></param>
        /// <param name="tnum"></param>
        /// <exception cref="Exception"></exception>
        /// Example: ...
        ///          using InfoReg;
        ///          ...
        ///          Double val;
        ///          InfoReg.SI_Format.Parse("1.23456 km", out val);
        ///          => val has the value 1.23456e3
        ///
        public static void Parse<T>(string si_value, out T tnum)
        {
            // si_value is expected as 999.9999 km or 999.99999 kilo-metres
            // get numerical value
            Double? dnum = null;
            Single? fnum = null;
            Decimal? decnum = null;
            tnum = (T)Convert.ChangeType(0, typeof(T));
            string[] string_parts = si_value.Trim().Split(' ');
            try
            {
                if (typeof(T) == typeof(double))
                {
                    dnum = double.Parse(string_parts[0]);
                    tnum = (T)Convert.ChangeType(dnum, typeof(T));
                }
                if (typeof(T) == typeof(System.Single))
                {
                    fnum = Single.Parse(string_parts[0]);
                    tnum = (T)Convert.ChangeType(fnum, typeof(T));
                }
                if (typeof(T) == typeof(decimal))
                {
                    decnum = decimal.Parse(string_parts[0]);
                    tnum = (T)Convert.ChangeType(decnum, typeof(T));
                }
            }
            catch (Exception e1)
            {
                throw new Exception("Error: SI_Parse<" + typeof(T).Name + "> failed to parse number part from " + si_value, e1);
            }

            // Parse units to get the exponent multiplier
            string[] units = string_parts[1].Split('-'); // if units is null assume short types like kg
            double exp_adjust;
            int pos;
            if (units.Length == 1) // implies short notation
            {
                // m for metres on its own no need to adjust exponent
                if (units[0].Length == 1)
                {
                    return;
                }
                pos = StringShortPrefixes.IndexOf(string_parts[1][0]);
            }
            else
            {
                // A unit has been specfied
                // Space is used to avoid a false positive where no prefix was given.
                for (pos = 0; pos < SI_Prefixes.Length; pos++)
                {
                    if (SI_Prefixes[pos] == units[0])
                    {
                        break;
                    }
                }
                if (pos == SI_Prefixes.Length) pos = -1;
            }
            if (pos < 0 || pos == 10)
            {
                return;
            }
            if (pos < 10)
            {
                exp_adjust = (10.0 - pos) * 3.0;
            }
            else
            {
                exp_adjust = (pos - 10.0) * -3.0;
            }
            if (typeof(T) == typeof(double))
            {
                dnum *= Math.Pow(10.0, exp_adjust);
                tnum = (T)Convert.ChangeType(dnum, typeof(T));
            }
            if (typeof(T) == typeof(Single))
            {
                fnum *= (Single)MathF.Pow(10.0f, (Single)exp_adjust);
                tnum = (T)Convert.ChangeType(fnum, typeof(T));
            }
            if (typeof(T) == typeof(decimal))
            {
                decnum *= (decimal)Math.Pow(10.0, exp_adjust);
                tnum = (T)Convert.ChangeType(decnum, typeof(T));
            }
        }

        /// <summary>
        /// *** Depreciated use Parse<T> instead *** // </T>>
        /// Takes an SI formatted value like "12.34 km" and returns a double with the
        /// value 1.234e4. A String "10pF" would be returned as a double value 1e-11.
        /// Example: ...
        ///          using InfoReg;
        ///          ...
        ///          Double val;
        ///          InfoReg.SI_Format.Parse("1.23456 km", out val);
        ///          => val has the value 1.23456e3
        ///   
        /// </summary>
        /// <param name="si_value">A string value like 12.345MHz</param>
        /// <param name="num">A double that will be assigned the parsed value from the SI formatted string</param>
        /// <returns>A double value adjusted for the SI prefix value.</returns>

        [Obsolete("Please use Parse<double>(string si_value, out num)", true)]
        public static void Parse(string si_value, out double num)
        {
            Parse<double>(si_value, out num);
        }

        /// <summary>
        /// *** Depreciated use Parse<T> instead *** // </T>
        /// Takes an SI formatted value like "12.34 km" and returns a decimal with the
        /// value 1.234e4. A String "10pF" would be returned as a decimal value 1e-11.
        /// Example: ...
        ///          using InfoReg;
        ///          ...
        ///          Decimal val;
        ///          InfoReg.SI_Format.Parse("1.23456 km", out val);
        ///          => val has the value 1.23456e3
        /// </summary>
        /// <param name="si_value">A string value like 12.345MHz</param>
        /// <param name="num">A decimal that will be assigned the parsed value from the SI formatted string</param>
        /// <returns>A decimal value adjusted for the SI prefix value.</returns>

        [Obsolete("Please use Parse<decimal>(string si_value, out num)", true)]
        public static void Parse(string si_value, out decimal num)
        {
            Parse<decimal>(si_value, out num);
        }

        /// <summary>
        /// *** Depreciated use Parse<T> instead *** //</T>
        /// Takes an SI formatted value like "12.34 km" and returns a float with the
        /// value 1.234e4. A String "10pF" would be returned as a float value 1e-11.
        /// Example: ...
        ///          using InfoReg;
        ///          ...
        ///          float val;
        ///          InfoReg.SI_Format.Parse("1.23456 km", out val);
        ///          => val has the value 1.23456e3
        /// 
        /// </summary>
        /// <param name="si_value">A string value like 12.345MHz</param>
        /// <param name="num">A float that will be assigned the parsed value from the SI formatted string</param>
        /// <returns>A float value adjusted for the SI prefix value.</returns>

        [Obsolete("Please use Parse<float>(string si_value, out num)", true)]
        public static void Parse(string si_value, out float num)
        {
            Parse<float>(si_value, out num);
        }
    }
}
