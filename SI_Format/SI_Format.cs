// Maintain this file in UTF-8 coding
using System;
using System.Net.Http.Headers;
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
        /// IEC_Prefixes full name
        /// </summary>
        public static string[] IEC_Prefixes = { "", "Kibi", "Mebi", "Gibi", "Tebi", "Pebi", "Exbi", "Zebi", "Yobi" };

        /// <summary>
        /// SI Prefixes single character
        /// </summary>
        public static string[] SI_ShortPrefixes = { "Q", "R", "Y", "Z", "E", "P", "T", "G", "M", "k",
                        "", "m", "μ", "n", "p", "f", "a", "z", "y", "r", "q" };

        /// <summary>
        /// Provides the set of standard IEC binary unit prefixes, ordered by increasing magnitude.
        /// </summary>
        /// <remarks>The array includes prefixes such as "Ki" for kibibyte, "Mi" for mebibyte, and so on,
        /// following the IEC 60027-2 standard. The first element is an empty string, representing the base unit with no
        /// prefix.</remarks>
        public static string[] IEC_ShortPrefixes = { "", "Ki", "Mi", "Gi", "Ti", "Pi", "Ei", "Zi", "Yi" };

        /// <summary>
        /// SIStringShort_Prefixes
        /// </summary>
        public static string SIStringShortPrefixes = "QRYZEPTGMk mμnpfazyrq";

        /// <summary>
        /// IECStringShort_Prefixes
        /// </summary>
        public static string IECStringShortPrefixes = " KMGTPEZY";

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
        /// Specifies the unit format type to use when representing data sizes.
        /// </summary>
        /// <remarks>Use the SI format for decimal-based units (e.g., kilobyte = 1,000 bytes) and the IEC
        /// format for binary-based units (e.g., kibibyte = 1,024 bytes). Choose the appropriate format based on the
        /// standard required for display or calculation.</remarks>
        public enum FormatType
        {
            /// <summary>
            /// SI format type
            /// </summary>
            SI,
            /// <summary>
            /// IEC format type
            /// </summary>
            IEC
        };

        /// <summary>
        /// Format&lt;T&gt; 
        /// is a generic function that takes a numeric value like 123,450 and returns a string like "123.45 km".
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tval">
        ///     tval is a numeric value of type T (double, float, decimal, int, long, short, byte)
        /// </param>
        /// <param name="sformat"></param>
        /// <param name="siunit"></param>
        /// <param name="padding"></param>
        /// <param name="formatType"></param>
        /// <returns>Format returns a string like "9.46 peta-metres" or "8 Gibibytes"</returns>
        /// 
        /// Returns values in text using SI format or IEC format. An example is 123.45km.
        /// Note: hecto, deca, deci, and centi are not supported
        /// si does not support numbers above 10^27 or below 10^-24 
        /// return unmodified without SI prefix units. IEC format is valid for positive
        /// values in the range 2^10 to 2^80.
        /// It takes tval and looks at its decimal exponent. The exponent 
        /// is reduced to its residue three value. It then prefixes the unit
        /// passed in with the appropriate SI or IEC prefix...
        /// 
        /// "quetta", "ronna", "yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
        /// "", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto", "ronto", "quecto"
        /// 
        /// and IEC prefixes like:
        ///
        /// "", "Kibi", "Mebi", "Gibi", "Tebi", "Pebi", "Exbi", "Zebi", "Yobi"
        ///
        /// or short prefixes like:
        /// 
        /// "Q", "R", "Y", "Z", "E", "P", "T", "G", "M", "k",
        /// "", "m", "μ", "n", "p", "f", "a", "z", "y", "r", "q"
        /// 
        /// and short IEC prefixes like:
        /// 
        /// "Ki", "Mi", "Gi", "Ti", "Pi", "Ei", "Zi", "Yi"
        /// 
        /// No prefix is needed if the value of d_val lies in the range 0.0 to just under 1000.0
        /// for SI units or 0 to just under 2^10 for IEC units.
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
        public static String Format<T>(T tval, string sformat, string siunit, Padding padding = Padding.dashonly, FormatType formatType = FormatType.SI)
        {
            if (formatType == FormatType.SI)
            {
                object _val;
                if (typeof(T) == typeof(long) || typeof(T) == typeof(int) || typeof(T) == typeof(short) || typeof(T) == typeof(byte))
                {
                    _val = (Double)Convert.ChangeType(tval, typeof(Double));
                }
                else
                {
                    _val = tval;
                }
                Double exp;
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
                if (_val.GetType() == typeof(Double))
                {
                    _val = (double)_val / Math.Pow(10.0, exp1);
                }
                if (_val.GetType() == typeof(Single))
                {
                    _val = (T)Convert.ChangeType(((Single)Convert.ChangeType(_val, typeof(Single)) / MathF.Pow(10.0f, exp1)), typeof(T));
                }
                if (_val.GetType() == typeof(Decimal))
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
            else
            {
                // IEC Format
                object _val;
                if (typeof(T) == typeof(long) || typeof(T) == typeof(int) || typeof(T) == typeof(short) || typeof(T) == typeof(byte))
                {
                    _val = (Double)Convert.ChangeType(tval, typeof(Double));
                }
                else
                {
                    _val = tval;
                }
                Double exp;
                exp = Math.Log((double)Convert.ChangeType(tval, typeof(Double)), 2.0);
                if (exp < 10.0 || exp >= 80.0)
                {
                    return string.Format("{0:" + sformat + "} {1}", tval, siunit);
                }
                int exp1 = (int)(exp / 10) * 10;
                if (exp < 0)
                {
                    exp1 -= 10;
                }
                if (_val.GetType() == typeof(Double))
                {
                    _val = (double)_val / Math.Pow(2.0, exp1);
                }
                if (_val.GetType() == typeof(Single))
                {
                    _val = (T)Convert.ChangeType(((Single)Convert.ChangeType(_val, typeof(Single)) / MathF.Pow(2.0f, exp1)), typeof(T));
                }
                if (_val.GetType() == typeof(Decimal))
                {
                    _val = (T)Convert.ChangeType(((Decimal)Convert.ChangeType(_val, typeof(Decimal)) / (Decimal)Math.Pow(2.0, exp1)), typeof(T));
                }
                string iec_prefixtouse = string.Empty;
                int prefix_choice = exp1 / 10;
                if (siunit.Length >= 3)
                {
                    iec_prefixtouse = IEC_Prefixes[exp1 / 10];
                }
                else
                {
                    iec_prefixtouse = IEC_ShortPrefixes[prefix_choice];
                }
                string iec_paddingtouse = string.Empty;
                if (padding == Padding.dashWithPadding || padding == Padding.paddingOnly)
                {
                    iec_paddingtouse = " ";
                }
                string iec_dashtouse = string.Empty;
                if (padding == Padding.dashWithPadding || padding == Padding.dashonly)
                {
                    if (prefix_choice != 0) iec_dashtouse = "-";  // No dash
                }
                return string.Format("{0:" + sformat + "} ", _val) + iec_prefixtouse + iec_dashtouse + siunit + iec_paddingtouse;
            }
        }

        /// <summary>
        /// Parse&lt;T&gt; is a generic function that takes a string value like "12.34 km" and returns a numeric value
        /// Takes an SI formatted value like "12.34 km" and yields an out parameter of type double, float, or decimal with the
        /// value 1.234e4. A String "10pF" would be returned as a numeric value 1e-11.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="si_value"></param>
        /// <param name="tnum"></param>
        /// <param name="formatType"></param>
        /// <exception cref="Exception"></exception>
        /// Example: ...
        ///          using InfoReg;
        ///          ...
        ///          Double val;
        ///          InfoReg.SI_Format.Parse("1.23456 km", out val);
        ///          => val has the value 1.23456e3
        ///
        public static void Parse<T>(string si_value, out T tnum, FormatType formatType = FormatType.SI)
        {
            // si_value is expected as 999.9999 km or 999.9999 kilo-metres
            // get numerical value
            int position = 0;
            double exp_adjust = 0.0;
            Double? dnum = null;
            Single? fnum = null;
            Decimal? decnum = null;
            tnum = (T)Convert.ChangeType(0, typeof(T));
            string[] units;
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
            switch (formatType)
            {
                case FormatType.SI:
                    if(string_parts.Length < 2) // No IEC prefix.
                    {
                        return;
                    }
                    // Parse units to get the exponent multiplier
                    if (string_parts[1].Contains("-") == true)
                    {
                        // Dash found, split on dash
                        units = string_parts[1].Split('-');
                    }
                    else
                    {
                        // No dash found, treat entire string as unit
                        units = new string[] { string_parts[1] };
                    }
                    if (units[0].Length == 1) // implies short notation
                    {
                        return; // No SI prefix present
                    }
                    if (units[0].Length <= 3) // km or MHz etc.
                    {
                        position = SIStringShortPrefixes.IndexOf(units[0][0]);
                    }
                    else
                    {
                        for(position = 0; position < SI_Prefixes.Length; position++)
                        {
                            if (position == 10) continue; // Skip no prefix entry
                            if (units[0].StartsWith(SI_Prefixes[position], StringComparison.OrdinalIgnoreCase))
                            {
                                break;
                            }
                        }
                    }

                    // If position < 0 i.e. not found or position == 10 (no prefix) return
                    if (position < 0 || position == 10)
                    {
                        return;
                    }
                    if (position < 10)
                    {
                        exp_adjust = (10.0 - position) * 3.0;
                    }
                    else
                    {
                        exp_adjust = (position - 10.0) * -3.0;
                    }
                    if (typeof(T) == typeof(double))
                    {
                        dnum *= Math.Pow(10.0, exp_adjust);
                        tnum = (T)Convert.ChangeType(dnum, typeof(T));
                        return;
                    }
                    if (typeof(T) == typeof(Single))
                    {
                        fnum *= (Single)MathF.Pow(10.0f, (Single)exp_adjust);
                        tnum = (T)Convert.ChangeType(fnum, typeof(T));
                        return;
                    }
                    if (typeof(T) == typeof(decimal))
                    {
                        decnum *= (decimal)Math.Pow(10.0, exp_adjust);
                        tnum = (T)Convert.ChangeType(decnum, typeof(T));
                        return;
                    }
                    break;
                case FormatType.IEC:
                    // Parse units to get the exponent multiplier
                    units = string_parts[1].Split('-'); // if units is null assume short types like kg
                    if (units.Length == 1) // implies short notation
                    {
                        // B for bytes on its own no need to adjust exponent
                        if (units[0].Length == 1)
                        {
                            return;
                        }
                        position = IECStringShortPrefixes.IndexOf(string_parts[1][0]);
                    }
                    else
                    {
                        // A unit has been specfied
                        // Space is used to avoid a false positive where no prefix was given.
                        for (position = 0; position < IEC_Prefixes.Length; position++)
                        {
                            if (IEC_Prefixes[position] == units[0])
                            {
                                break;
                            }
                        }
                        if (position == IEC_Prefixes.Length) position = -1;
                    }
                    if (position < 0 || position > 9)
                    {
                        return;
                    }
                    //if (position < 10)
                    //{
                    //    exp_adjust = (10.0 - position) * 3.0;
                    //}
                    //else
                    //{
                    //    exp_adjust = (position - 10.0) * -3.0;
                    //}
                    if (typeof(T) == typeof(double))
                    {
                        dnum = Math.Pow(2, position * 10.0) * dnum;
                        tnum = (T)Convert.ChangeType(dnum, typeof(T));
                        return;
                    }
                    if (typeof(T) == typeof(Single))
                    {
                        fnum = MathF.Pow(2, position * 10.0f) * fnum;
                        tnum = (T)Convert.ChangeType(fnum, typeof(T));
                        return;
                    }
                    if (typeof(T) == typeof(decimal))
                    {
                        exp_adjust = position * 10.0;
                        decnum = (decimal)Math.Pow(2, exp_adjust) * decnum;
                        tnum = (T)Convert.ChangeType(decnum, typeof(T));
                        return;
                    }
                    break;
                default:
                    throw new Exception("Error: SI_Parse<" + typeof(T).Name + "> unknown FormatType");
            }
        }
    }
}