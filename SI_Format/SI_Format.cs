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
        /// Format<typeparamref name="T"/> // T may be float, double, or decimal
        /// </summary>
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
        ///          ans = InfoReg.SI_Format.Format(val, "G6", "metres", noPaddingOrDash);
        ///          => ans contains: "12.3456 exametres"
        /// </example>
        public static String Format<T>(T tval, string sformat, string siunit, Padding padding = Padding.dashonly)
        {
            // Note: hecto, deca, deci, and centi are not supported
            // si does not support numbers above 10^27 or below 10^-24 
            // return unmodified without SI prefix units
            double exp;
            double dval = 0.0;
            float fval = (float)0.0;
            decimal decval = 0.0m;
            decimal decval1 = 0.0m;
            if (typeof(T) == typeof(double))
            {
                dval = (double)Convert.ChangeType(tval, typeof(double));
            }
            if(typeof(T) == typeof(float))
            {
                fval = (float)Convert.ChangeType(tval, typeof(float));
                dval = (double)fval;
            }
            if(typeof(T) == typeof(decimal))
            {
                decval = (decimal)Convert.ChangeType(tval, typeof(decimal));
                dval = (double)decval;
            }
            exp = Math.Log10(dval);
            if (exp >= 33.0 || exp <= -30.0)
            {
                return string.Format("{0:" + sformat + "} {1}", tval, siunit);
            }
            else
            {
                int exp1 = (int)(exp / 3) * 3;
                int adjust = 10; // Array element for no SI prefix
                if (exp < 0)
                {
                    exp1 -= 3;
                }
                if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
                {
                    dval = dval / Math.Pow(10.0, exp1);
                }
                else
                {
                    decval1 = decval / (decimal)Math.Pow(10.0, exp1);
                }
                int prefix_choice = -(exp1 / 3) + adjust;
                if (siunit.Length >= 3)
                {
                    if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
                    {
                        switch (padding)
                        {
                            case Padding.dashonly:
                                if (prefix_choice != 10)
                                {
                                    return string.Format("{0:" + sformat + "} ", dval) + SI_Prefixes[prefix_choice] + "-" + siunit;
                                }
                                else
                                {
                                    return string.Format("{0:" + sformat + "} ", dval) + SI_Prefixes[prefix_choice] + siunit;
                                }
                            case Padding.dashWithPadding:
                                if (prefix_choice != 10)
                                {
                                    return string.Format("{0:" + sformat + "} ", dval) + SI_Prefixes[prefix_choice] + "-" + siunit + " ";
                                }
                                else
                                {
                                    return string.Format("{0:" + sformat + "} ", dval) + SI_Prefixes[prefix_choice] + siunit + " ";
                                }
                            case Padding.paddingOnly:
                                return string.Format("{0:" + sformat + "} ", dval) + SI_Prefixes[prefix_choice] + siunit + " ";
                        }
                        return string.Format("{0:" + sformat + "} ", dval) + SI_Prefixes[prefix_choice] + siunit; // Padding.noPaddingOrDash
                    }
                    else
                    {
                        switch (padding)
                        {
                            case Padding.dashonly:
                                if (prefix_choice != 10)
                                {
                                    return string.Format("{0:" + sformat + "} ", decval1) + SI_Prefixes[-(exp1 / 3) + adjust] + "-" + siunit;
                                }
                                else
                                {
                                    return string.Format("{0:" + sformat + "} ", decval1) + SI_Prefixes[-(exp1 / 3) + adjust] + siunit;
                                }
                            case Padding.dashWithPadding:
                                if (prefix_choice != 10)
                                {
                                    return string.Format("{0:" + sformat + "} ", decval1) + SI_Prefixes[-(exp1 / 3) + adjust] + "-" + siunit + " ";
                                }
                                else
                                {
                                    return string.Format("{0:" + sformat + "} ", decval1) + SI_Prefixes[-(exp1 / 3) + adjust] + siunit + " ";
                                }
                            case Padding.paddingOnly:
                                return string.Format("{0:" + sformat + "} ", decval1) + SI_Prefixes[-(exp1 / 3) + adjust] + siunit + " ";
                        }
                        return string.Format("{0:" + sformat + "} ", decval1) + SI_Prefixes[-(exp1 / 3) + adjust] + siunit; // Padding.noPaddingOrDash
                    }
                }
                else
                {
                    if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
                    {
                        // A dash is not supported for short SI unit prefixes. Thus, supply with padding (a trailing space) or without
                        if (padding == Padding.dashWithPadding || padding == Padding.paddingOnly)
                        {
                            return string.Format("{0:" + sformat + "} ", dval) + SI_ShortPrefixes[-(exp1 / 3) + adjust] + siunit + " ";
                        }
                        else
                        {
                            return string.Format("{0:" + sformat + "} ", dval) + SI_ShortPrefixes[-(exp1 / 3) + adjust] + siunit;
                        }
                    }
                    else
                    {
                        if (padding == Padding.dashWithPadding || padding == Padding.paddingOnly)
                        {
                            return string.Format("{0:" + sformat + "} ", decval1) + SI_ShortPrefixes[-(exp1 / 3) + adjust] + siunit + " ";
                        }
                        else
                        {
                            return string.Format("{0:" + sformat + "} ", decval1) + SI_ShortPrefixes[-(exp1 / 3) + adjust] + siunit;
                        }
                    }
                }
            }
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
        [Obsolete("Please use Format<double>(double d_val, string sformat, string siunit, Padding padding = Padding.dashonly)")]
        public static String Format(double d_val, string sformat, string siunit, Padding padding = Padding.dashonly)
        {
            return Format<double>(d_val, sformat, siunit, padding);
            //// Note: hecto, deca, deci, and centi are not supported
            //// si does not support numbers above 10^27 or below 10^-24 
            //// return unmodified without SI prefix units
            //double exp;
            //exp = Math.Log10(d_val);
            //if (exp >= 33.0 || exp <= -30.0)
            //{
            //    return string.Format("{0:" + sformat + "} {1}", d_val, siunit);
            //}
            //else
            //{
            //    int exp1 = (int)exp / 3 * 3;
            //    int adjust = 10; // Array element for no SI prefix
            //    if (exp < 0)
            //    {
            //        exp1 -= 3;
            //    }
            //    double dval = d_val / Math.Pow(10.0, exp1);
            //    int prefix_choice = -(exp1 / 3) + adjust;
            //    if (siunit.Length >= 3)
            //    {
            //        switch (padding)
            //        {
            //            case Padding.dashonly:
            //                if (prefix_choice != 10)
            //                {
            //                    return string.Format("{0:" + sformat + "} ", dval) + SI_Prefixes[prefix_choice] + "-" + siunit;
            //                }
            //                else
            //                {
            //                    return string.Format("{0:" + sformat + "} ", dval) + SI_Prefixes[prefix_choice] + siunit;
            //                }
            //            case Padding.dashWithPadding:
            //                if (prefix_choice != 10)
            //                {
            //                    return string.Format("{0:" + sformat + "} ", dval) + SI_Prefixes[prefix_choice] + "-" + siunit + " ";
            //                }
            //                else
            //                {
            //                    return string.Format("{0:" + sformat + "} ", dval) + SI_Prefixes[prefix_choice] + siunit + " ";
            //                }
            //            case Padding.paddingOnly:
            //                return string.Format("{0:" + sformat + "} ", dval) + SI_Prefixes[prefix_choice] + siunit + " ";
            //        }
            //        return string.Format("{0:" + sformat + "} ", dval) + SI_Prefixes[prefix_choice] + siunit; // Padding.noPaddingOrDash
            //    }
            //    else
            //    {
            //        // A dash is not supported for short SI unit prefixes. Thus, supply with padding (a trailing space) or without
            //        if (padding == Padding.dashWithPadding || padding == Padding.paddingOnly)
            //        {
            //            return string.Format("{0:" + sformat + "} ", dval) + SI_ShortPrefixes[-(exp1 / 3) + adjust] + siunit + " ";
            //        }
            //        else
            //        {
            //            return string.Format("{0:" + sformat + "} ", dval) + SI_ShortPrefixes[-(exp1 / 3) + adjust] + siunit;
            //        }
            //    }
            //}
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

        [Obsolete("Please use Format<float>(float f_val, string sformat, string siunit, Padding padding = Padding.dashonly)")]
        public static string Format(float f_val, string sformat, string siunit, Padding padding = Padding.dashonly)
        {
            return Format<float>(f_val, sformat, siunit, padding);
            //// Note: hecto, deca, deci, and centi are not supported
            //// si does not support numbers above 10^27 or below 10^-24 
            //// return unmodified without SI prefix units
            //float exp;
            //exp = System.MathF.Log10(f_val);
            //if (exp >= 33.0 || exp <= -30.0)
            //{
            //    return string.Format("{0:" + sformat + "} {1}", f_val, siunit);
            //}
            //else
            //{
            //    int exp1 = (int)exp / 3 * 3;
            //    int adjust = 10; // Array element for no SI prefix
            //    if (exp < 0)
            //    {
            //        exp1 -= 3;
            //    }
            //    float ffval = f_val / MathF.Pow(10.0f, exp1);
            //    int prefix_choice = -(exp1 / 3) + adjust;
            //    if (siunit.Length >= 3)
            //    {
            //        switch (padding)
            //        {
            //            case Padding.dashonly:
            //                if (prefix_choice != 10)
            //                {
            //                    return string.Format("{0:" + sformat + "} ", ffval) + SI_Prefixes[prefix_choice] + "-" + siunit;
            //                }
            //                else
            //                {
            //                    return string.Format("{0:" + sformat + "} ", ffval) + SI_Prefixes[prefix_choice] + siunit;
            //                }
            //            case Padding.dashWithPadding:
            //                if (prefix_choice != 10)
            //                {
            //                    return string.Format("{0:" + sformat + "} ", ffval) + SI_Prefixes[prefix_choice] + "-" + siunit + " ";
            //                }
            //                else
            //                {
            //                    return string.Format("{0:" + sformat + "} ", ffval) + SI_Prefixes[prefix_choice] + siunit + " ";
            //                }
            //            case Padding.paddingOnly:
            //                return string.Format("{0:" + sformat + "} ", ffval) + SI_Prefixes[prefix_choice] + siunit + " ";
            //        }
            //        return string.Format("{0:" + sformat + "} ", ffval) + SI_Prefixes[prefix_choice] + siunit; // Padding.noPaddingOrDash
            //    }
            //    else
            //    {
            //        // A dash is not supported for short SI unit prefixes. Thus, supply with padding (a trailing space) or without
            //        if (padding == Padding.dashWithPadding || padding == Padding.paddingOnly)
            //        {
            //            return string.Format("{0:" + sformat + "} ", ffval) + SI_ShortPrefixes[-(exp1 / 3) + adjust] + siunit + " ";
            //        }
            //        else
            //        {
            //            return string.Format("{0:" + sformat + "} ", ffval) + SI_ShortPrefixes[-(exp1 / 3) + adjust] + siunit;
            //        }
            //    }
            //}
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

        [Obsolete("Please use Format<decimal>(decimal decimal_val, string sformat, string siunit, Padding padding = Padding.dashonly)")]
        public static string Format(decimal decimal_val, string sformat, string siunit, Padding padding = Padding.dashonly)
        {
            return Format<decimal>(decimal_val, sformat, siunit, padding);
            //double exp = Math.Log10((double)decimal_val);
            //if (exp >= 33.0 || exp <= -30.0)
            //{
            //    return string.Format("{0:" + sformat + "} {1}", decimal_val, siunit);
            //}
            //else
            //{
            //    int exp1 = (int)exp / 3 * 3;
            //    int adjust = 10;  // Array element for no SI prefix
            //    if (exp < 0)
            //    {
            //        exp1 -= 3;
            //    }
            //    decimal decimal_val1 = decimal_val / (decimal)Math.Pow(10.0, exp1);
            //    if (siunit.Length >= 3)
            //    {
            //        int prefix_choice = -(exp1 / 3) + adjust;
            //        switch (padding)
            //        {
            //            case Padding.dashonly:
            //                if (prefix_choice != 10)
            //                {
            //                    return string.Format("{0:" + sformat + "} ", decimal_val1) + SI_Prefixes[prefix_choice] + "-" + siunit;
            //                }
            //                else
            //                {
            //                    return string.Format("{0:" + sformat + "} ", decimal_val1) + SI_Prefixes[prefix_choice] + siunit;
            //                }
            //            case Padding.dashWithPadding:
            //                if (prefix_choice != 10)
            //                {
            //                    return string.Format("{0:" + sformat + "} ", decimal_val1) + SI_Prefixes[prefix_choice] + "-" + siunit + " ";
            //                }
            //                else
            //                {
            //                    return string.Format("{0:" + sformat + "} ", decimal_val1) + SI_Prefixes[prefix_choice] + siunit + " ";

            //                }
            //            case Padding.paddingOnly:
            //                return string.Format("{0:" + sformat + "} ", decimal_val1) + SI_Prefixes[prefix_choice] + siunit + " ";
            //        }
            //        return string.Format("{0:" + sformat + "} ", decimal_val1) + SI_Prefixes[-(exp1 / 3) + adjust] + siunit; // Padding.noPaddingOrDash
            //    }
            //    else
            //    {
            //        return string.Format("{0:" + sformat + "} ", decimal_val1) + SI_ShortPrefixes[-(exp1 / 3) + adjust] + siunit;
            //    }
            //}
        }

        /// <summary>
        /// Parse<typeparamref name="T"/> is a generic function that takes a string value like "12.34 km" and returns a numeric value
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
        
        public static void Parse<T>(string si_value, out T tnum)
        {
            // si_value is expected as 999.9999 km or 999.99999 kilo-metres
            // get numerical value
            double? dnum = null;
            float? fnum = null;
            decimal? decnum = null;
            tnum = (T)Convert.ChangeType(0, typeof(T));
            string[] string_parts = si_value.Trim().Split(' ');
            try
            {
                if(typeof(T) == typeof(double))
                {
                    dnum = double.Parse(string_parts[0]);
                    tnum = (T)Convert.ChangeType(dnum, typeof(T));
                }
                if (typeof(T) == typeof(float))
                {
                    fnum = float.Parse(string_parts[0]);
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
                    if (SI_Prefixes[pos] == units[0]) { break; }
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
            if (typeof(T) == typeof(float))
            {
                fnum *= (float)MathF.Pow(10.0f, (float)exp_adjust);
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

        [Obsolete("Please use Parse<double>(string si_value, out num)")]
        public static void Parse(string si_value, out double num)
        {
            Parse<double>(si_value, out num);
            //// si_value is expected as 999.9999 km or 999.99999 kilo-metres
            //// get numerical value
            //string[] string_parts = si_value.Trim().Split(' ');
            //try
            //{
            //    dnum = double.Parse(string_parts[0]);
            //}
            //catch (Exception e1)
            //{
            //    throw new Exception("Error: SI_ParseDouble failed to parse number part from " + si_value, e1);
            //}

            //// Parse units to get the exponent multiplier
            //string[] units = string_parts[1].Split('-'); // if units is null assume short types like kg
            //double exp_adjust;
            //int pos;
            //if (units.Length == 1) // implies short notation
            //{
            //    // m for metres on its own no need to adjust exponent
            //    if (units[0].Length == 1)
            //    {
            //        return;
            //    }
            //    pos = StringShortPrefixes.IndexOf(string_parts[1][0]);
            //}
            //else
            //{
            //    // A unit has been specfied
            //    // Space is used to avoid a false positive where no prefix was given.
            //    for (pos = 0; pos < SI_Prefixes.Length; pos++)
            //    {
            //        if (SI_Prefixes[pos] == units[0]) { break; }
            //    }
            //    if (pos == SI_Prefixes.Length) pos = -1;
            //}
            //if (pos < 0 || pos == 10)
            //{
            //    return;
            //}
            //if (pos < 10)
            //{
            //    exp_adjust = (10.0 - pos) * 3.0;
            //}
            //else
            //{
            //    exp_adjust = (pos - 10.0) * -3.0;
            //}
            //double ten = 10.0;
            //dnum *= Math.Pow(ten, exp_adjust);
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

        [Obsolete("Please use Parse<decimal>(string si_value, out num)")]
        public static void Parse(string si_value, out decimal num)
        {
            Parse<decimal>(si_value, out num);
            //// si_vale is expected as 999.9999 km or 999.99999 kilo-metres
            //// get numerical value
            //string[] string_parts = si_value.Trim().Split(' ');
            //try
            //{
            //    dnum = decimal.Parse(string_parts[0]);
            //}
            //catch (Exception e1)
            //{
            //    throw new Exception("Error: SI_ParseDecimal failed to parse number part from " + si_value, e1);
            //}

            //// Parse units to get the exponent multiplier
            //string[] units = string_parts[1].Split('-'); // if units is null assume short types like kg
            //double exp_adjust;
            //int pos;
            //if (units.Length == 1) // implies short notation
            //{
            //    // m for metres on its own no need to adjust exponent
            //    if (units[0].Length == 1)
            //    {
            //        return;
            //    }

            //    pos = StringShortPrefixes.IndexOf(string_parts[1][0]);
            //}
            //else
            //{
            //    // A unit has been specfied
            //    // Space is used to avoid a false positive where no prefix was given.
            //    for (pos = 0; pos < SI_Prefixes.Length; pos++)
            //    {
            //        if (SI_Prefixes[pos] == units[0]) { break; }
            //    }
            //    if (pos == SI_Prefixes.Length) pos = -1;
            //}
            //if (pos < 0 || pos == 10)
            //{
            //    return;
            //}
            //if (pos < 10)
            //{
            //    exp_adjust = (double)((10 - pos) * 3);
            //}
            //else
            //{
            //    exp_adjust = (double)((pos - 10) * -3);
            //}
            //double ten = 10.0;
            //exp_adjust = Math.Pow(ten, exp_adjust);
            //dnum *= (decimal)exp_adjust;
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

        [Obsolete("Please use Parse<float>(string si_value, out num)")]
        public static void Parse(string si_value, out float num)
        {
            Parse<float>(si_value, out num);
            //// si_value is expected as 999.9999 km or 999.99999 kilo-metres
            //// get numerical value
            //string[] string_parts = si_value.Trim().Split(' ');
            //try
            //{
            //    f_num = float.Parse(string_parts[0]);
            //}
            //catch (Exception e1)
            //{
            //    throw new Exception("Error: SI_ParseDouble failed to parse number part from " + si_value, e1);
            //}

            //// Parse units to get the exponent multiplier
            //string[] units = string_parts[1].Split('-'); // if units is null assume short types like kg
            //float exp_adjust;
            //int pos;
            //if (units.Length == 1) // implies short notation
            //{
            //    // m for metres on its own no need to adjust exponent
            //    if (units[0].Length == 1)
            //    {
            //        return;
            //    }
            //    pos = StringShortPrefixes.IndexOf(string_parts[1][0]);
            //}
            //else
            //{
            //    // A unit has been specfied
            //    // Space is used to avoid a false positive where no prefix was given.
            //    // si_prefixes array is order dependent
            //    for (pos = 0; pos < SI_Prefixes.Length; pos++)
            //    {
            //        if (SI_Prefixes[pos] == units[0]) { break; }
            //    }
            //    if (pos == SI_Prefixes.Length) pos = -1;
            //}
            //if (pos < 0 || pos == 10)
            //{
            //    return;
            //}
            //if (pos < 10)
            //{
            //    exp_adjust = (float)((10 - pos) * 3);
            //}
            //else
            //{
            //    exp_adjust = (float)((pos - 10) * -3);
            //}
            //float ten = (float)10.0;
            //f_num *= MathF.Pow(ten, exp_adjust);
        }
    }
}
