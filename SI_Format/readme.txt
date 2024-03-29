<?xml version="1.0"?>
<doc>
    <assembly>
        <name>SI_Format</name>
    </assembly>
    <members>
        <member name="T:InfoReg.Physical_Constants">
            <summary>
                Physical constants may be referenced unsing this class.
                Please see https://physics.nist.gov/cuu/Constants/
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.LightSpeed">
            <summary>
            LightSpeed is the spped of light in a vacuum.
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.GravitationalConstant">
            <summary>
            GravitationalConstant is the gravitational constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.PlanckConstant">
            <summary>
            PlanckConstant is Plank's constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.ReducedPlankConstant">
            <summary>
            ReducedPlankConstant is the reduced Plank's constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.MagneticConstant">
            <summary>
            MagneticConstant is the magnetic constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.ElectricConstant">
            <summary>
            ElectricConstant is the electric constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.MagneticFluxQuantum">
            <summary>
            MagneticFluxQuantum is the quantum magnetic flux constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.ElementaryCharge">
            <summary>
            ElementaryCharge is the elementary charge constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.ConductanceQuantum">
            <summary>
            ElementaryCharge is the quantum conductance constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.ElectronMass">
            <summary>
            ElectronMass is the mass of an electron
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.ProtonMass">
            <summary>
            ProtonMass is the mass of a proton
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.FineStructureConstant">
            <summary>
            FineStructureConstant is the fine structure constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.RydbergConstant">
            <summary>
            RydbergConstant is the Rydberg constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.BohrRadius">
            <summary>
            BohrRadius is the Bohr radius constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.ClassicalElectronRadius">
            <summary>
            ClassicalElectronRadius is the classical electron radius constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.AtomicMassUnit">
            <summary>
            AtomicMassUnit is the atomic mass unit constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.AvogadroConstant">
            <summary>
            AvogadroConstant is the Avogadro constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.FaradayConstant">
            <summary>
            FaradayConstant is the Faraday constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.MolarGasConstant">
            <summary>
            MolarGasConstant is the Molar Gas Constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.BoltzmannConstant">
            <summary>
            BoltzmannConstant is the Boltzmann's constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.Stefan_BoltzmannConstant">
            <summary>
            Stefan_BoltzmannConstant is the Stefan_Boltzmann's constant
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.ElectronVolt">
            <summary>
            ElectronVolt is an electron volt
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.StandardGravity">
            <summary>
            StandardGravity rate of accelleration due to gravity on Earth
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.Pi">
            <summary>
            Pi is the value of the pi constant i.e. numer of radii to make a semi circle based on that radius
            </summary>
        </member>
        <member name="F:InfoReg.Physical_Constants.NaturalLogBase">
            <summary>
            NaturalLogBase the base used for natural logs
            </summary>
        </member>
        <member name="T:InfoReg.SI_Format">
            <summary>
            SI_Format is a class that contains functions to adjust or parse numbers to and from
            strings like 10pF or 123.46 kilo-metres or 65.123 ml.
            </summary>
        </member>
        <member name="T:InfoReg.SI_Format.Padding">
            <summary>
            Padding is an enumberated type.
            An enumerated value to indicate if a dash "-" is required between the SI prefix and the unit as in kilo-gram. 
            Padding will also indcate if a traling space should be appended as padding.
            </summary>
        </member>
        <member name="F:InfoReg.SI_Format.Padding.dashonly">
            <summary>
            dashonly: Default behaviour for Format function.
            </summary>
        </member>
        <member name="F:InfoReg.SI_Format.Padding.dashWithPadding">
            <summary>
            dashWithPadding: Use a dash but no trailing space
            </summary>
        </member>
        <member name="F:InfoReg.SI_Format.Padding.paddingOnly">
            <summary>
            paddingOnly: Add a trailing space only
            </summary>
        </member>
        <member name="F:InfoReg.SI_Format.Padding.noPaddingOrDash">
            <summary>
            noPaddingOrDash: No dash and no trailing space required
            </summary>
        </member>
        <member name="M:InfoReg.SI_Format.Format(System.Double,System.String,System.String,InfoReg.SI_Format.Padding)">
            <summary>
            Returns values in text in SI format. An example is 123.45km.
            It takes a double value and looks at its decimal exponent. The exponent 
            is reduced to its residue three value. It then prefixes the unit
            passed in with the appropriate SI prefix.
            
            "quetta", "ronna", "yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
            "", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto", "ronto", "quecto"

            If siunit is two or less characters the return will use short SI
            prefixes like:

            "Q", "R", "Y", "Z", "E", "P", "T", "G", "M", "k",
            "", "m", "μ", "n", "p", "f", "a", "z", "y", "r", "q"

            No prefix is needed if the value of d_val lies in the range 0.0 to just under 1000.0.

            Note: hecto, deca, deci, and centi are not supported.
                    SI does not support numbers above 10^33 or below 10^-30 
                    and any such value will be returned unmodified without SI prefix units.
                  
            </summary>
            <param name="d_val">A double value to be SI normalized.</param>
            <param name="sformat">Is the format string usually based on G or N </param>
            <param name="siunit">An SI unit like watt, metre or l</param>
            <param name="padding">Padding.dashOnly | Padding.dashWithPadding | Padding.paddingOnly | Padding.noPaddingOrDash</param>
            <returns>Formatted string e.g. "9.46 peta-metres"</returns>
            <example>
                     using InfoReg;
                     ...
                     String ans;
                     double val = 123.456e17;
                     ans = InfoReg.SI_Format.Format(val, "G6", "metres");
                     => ans contains: "12.3456 exa-metres"
                     ans = InfoReg.SI_Format.Format(val, "G6", "metres", noPaddingOrDash);
                     => ans contains: "12.3456 exametres"
            </example>
        </member>
        <member name="M:InfoReg.SI_Format.Format(System.Single,System.String,System.String,InfoReg.SI_Format.Padding)">
            <summary>
            Returns values in a text SI format. An example is 123.45km.
            It takes a float value and looks at its decimal exponent. The exponent 
            is reduced to its residue three value. It then prefixes the unit
            passed in with the appropriate SI prefix.
            
            "quetta", "ronna", "yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
            "", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto", "ronto", "quecto"

            If siunit is two or less characters the return will use short SI
            prefixes like:

            "Q", "R", "Y", "Z", "E", "P", "T", "G", "M", "k",
            "", "m", "μ", "n", "p", "f", "a", "z", "y", "r", "q"

            No prefix is needed if the value of d_val lies in the range 0.0 to just under 1000.0.

            Note: hecto, deca, deci, and centi are not supported.
                    SI does not support numbers above 10^33 or below 10^-30 
                    and any such value will be returned unmodified without SI prefix units.
            Example: ...
                     using InfoReg;
                     ...   
                     String ans;
                     float fval = (float)123.789E-7;
                     ans = InfoReg.SI_Format.Format(fval, "G4", "F");
                     => ans contains: "12.38 μF"
                     ans = InfoReg.SI_Format.Format(fval, "G4", "Farads", InfoReg.SI_Format.Padding.dashWithPadding);
                     => ans contains: "12.38 micro-Farads " // Both a dash and trailing space are used
            </summary>
            <param name="f_val">A float value to be SI normalized.</param>
            <param name="sformat">Is the format string usually based on G or N </param>
            <param name="siunit">An SI unit like watt, metre or l</param>
            <param name="padding">Padding.dashOnly | Padding.dashWithPadding | Padding.paddingOnly | Padding.noPaddingOrDash</param>
            <returns>Formatted string e.g. "9.46 peta-metres"</returns>
        </member>
        <member name="M:InfoReg.SI_Format.Format(System.Decimal,System.String,System.String,InfoReg.SI_Format.Padding)">
            <summary>
            Returns values in text in SI format. An example is 123.45km.
            It takes a decimal value and looks at its decimal exponent. The exponent 
            is reduced to its residue three value. It then prefixes the unit
            passed in with the appropriate SI prefix.
            
            "yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
            "", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto"
            
            If siunit is two or less characters the return will use short SI
            prefixes like:
            "Y", "Z", "E", "P", "T", "G", "M", "k",
            "", "m", "μ", "n", "p", "f", "a", "z", "y"
            
            No prefix is needed if the value of d_val lies in the range 0.0 to just under 1000.0.
            
            Note: hecto, deca, deci, and centi are not supported.
                  SI does not support numbers above 10^27 or below 10^-24 
                  and any such value will be returned unmodified without SI prefix units.
            Example: ...
                     using InfoReg;
                     ...
                     String ans;
                     Decimal decimal_val = Decimal.Parse("1234.5678901234567890123");
                     ans = InfoReg.SI_Format.Format(decimal_val, "G21", "grams");
                     => ans contains: "1.23456789012345678901 kilo-grams"
                     ans = InfoReg.SI_Format.Format(decimal_val, "G21", "grams", InfoReg.SI_Format.Padding.paddingOnly);
                     => ans contains: "1.23456789012345678901 kilograms " // trailing space added
            </summary>
            <param name="decimal_val">A decimal value to be SI normalized.</param>
            <param name="sformat">Is the format string usually based on G or N </param>
            <param name="siunit">An SI unit like watt, metre or l</param>
            <param name="padding">Padding.dashOnly | Padding.dashWithPadding | Padding.paddingOnly | Padding.noPaddingOrDash</param>
            <returns>Formatted string e.g. "9.46 pm"</returns>
        </member>
        <member name="M:InfoReg.SI_Format.Parse(System.String,System.Double@)">
            <summary>
            Takes an SI formatted value like "12.34 km" and returns a double with the
            value 1.234e4. A String "10pF" would be returned as a double value 1e-11.
            Example: ...
                     using InfoReg;
                     ...
                     Double val;
                     InfoReg.SI_Format.Parse("1.23456 km", out val);
                     => val has the value 1.23456e3
              
            </summary>
            <param name="si_value">A string value like 12.345MHz</param>
            <param name="dnum">A double that will be assigned the parsed value from the SI formatted string</param>
            <returns>A double value adjusted for the SI prefix value.</returns>
        </member>
        <member name="M:InfoReg.SI_Format.Parse(System.String,System.Decimal@)">
            <summary>
            Takes an SI formatted value like "12.34 km" and returns a decimal with the
            value 1.234e4. A String "10pF" would be returned as a decimal value 1e-11.
            Example: ...
                     using InfoReg;
                     ...
                     Decimal val;
                     InfoReg.SI_Format.Parse("1.23456 km", out val);
                     => val has the value 1.23456e3
            </summary>
            <param name="si_value">A string value like 12.345MHz</param>
            <param name="dnum">A decimal that will be assigned the parsed value from the SI formatted string</param>
            <returns>A decimal value adjusted for the SI prefix value.</returns>
        </member>
        <member name="M:InfoReg.SI_Format.Parse(System.String,System.Single@)">
            <summary>
            Takes an SI formatted value like "12.34 km" and returns a float with the
            value 1.234e4. A String "10pF" would be returned as a float value 1e-11.
            Example: ...
                     using InfoReg;
                     ...
                     float val;
                     InfoReg.SI_Format.Parse("1.23456 km", out val);
                     => val has the value 1.23456e3
            
            This function calls Parse(String, out Double) and converts the double to a float.
            </summary>
            <param name="si_value">A string value like 12.345MHz</param>
            <param name="f_num">A float that will be assigned the parsed value from the SI formatted string</param>
            <returns>A float value adjusted for the SI prefix value.</returns>
        </member>
    </members>
</doc>


