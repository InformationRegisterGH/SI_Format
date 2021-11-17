<h1>SI_Format</h1>
<p>
InfoReg.SI_Format Provides functions to parse strings like 23.56MHz as a float or single, double, or decimal value like
23,560,000.0. InfoReg.SI_Format provides functions to write 1,860 as 1.86km or 1.86 kilometres.
</p>
<p>
SI_Format also has a number of physical constants used by the engineering and scientific workers. An example is 
InfoReg.Physical_Constants.LightSpeed. These are described at the end of this file.
</p>
<p>
Version: 1.1.2 is built for .NET 5 and .NET 6 runtime environments. The Visual Studio 2022 project files,
C# source code, and unit tests are available on GitHub at:
https://github.com/InformationRegisterGH/SI_Format
</p>
<p>
The applicable license agreement is available at:
https://github.com/InformationRegisterGH/SI_Format/blob/Main/SI_Format/license.txt
</p>
<hr />
<h2>InfoReg.SI_Format</h2>
<p>
SI_Format is a class that contains functions to adjust or parse numbers to and from
strings like 10pF or 123.46 kilo-metres or 65.123 ml.
</p>
<h3>InfoReg.SI_Format.Padding</h3>
<p>
Padding is an enumberated type.
An enumerated value to indicate if a dash "-" is required between the SI prefix and 
the unit as in kilo-gram. 
Padding will also indcate if a trailing space should be appended as padding.
</p>
<h4>InfoReg.SI_Format.Padding.dashonly</h4>
<p>
dashonly: Default behaviour for Format function.
</p>
<h4>InfoReg.SI_Format.Padding.dashWithPadding</h4>
<p>
dashWithPadding: Use a dash but no trailing space
</p>
<h4>InfoReg.SI_Format.Padding.paddingOnly</h4>
<p>
paddingOnly: Add a trailing space only
</p>
<h4>InfoReg.SI_Format.Padding.noPaddingOrDash</h4>
<p>
noPaddingOrDash: No dash and no trailing space required
</p>
<hr />
<h3>InfoReg.SI_Format.Format(System.Double, System.String, System.String, InfoReg.SI_Format.Padding)</h3>
<p>
Returns values in text in SI format. An example is 123.45km.
It takes a double value and looks at its decimal exponent. The exponent 
is reduced to its residue three value. It then prefixes the unit
passed in with the appropriate SI prefix.
</p>
<p>
"yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
"", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto"
</p>
<p>
If siunit is two or less characters the return will use short SI
prefixes like:
</p>
<p>
"Y", "Z", "E", "P", "T", "G", "M", "k",
"", "m", "μ", "n", "p", "f", "a", "z", "y"
</p>
<p>
No prefix is needed if the value of d_val lies in the range 0.0 to just under 1000.0.
</p>
<p>
Note: hecto, deca, deci, and centi are not supported.
        SI does not support numbers above 10^27 or below 10^-24 
        and any such value will be returned unmodified without SI prefix units.
                  
</p>
<h4>d_val</h4>
<p>A double value to be SI normalized.</p>
<h4>sformat</h4>
<p>Is the format string usually based on G or N </p>
<h4>siunit</h4><p>An SI unit like watt, metre or l</p>
<h4>padding</h4><p>Padding.dashOnly | Padding.dashWithPadding | Padding.paddingOnly | Padding.noPaddingOrDash</>
<h4>returns</h4><p>Formatted string e.g. "9.46 peta-metres"</p>
<p><em><b>example</b></em></p>
<p>using InfoReg;</P>
<p>...</p>
<p>String ans;</p>
<p>double val = 123.456e17;</p>
<p>ans = InfoReg.SI_Format.Format(val, "G6", "metres");</p>
<p>=> ans contains: "12.3456 exa-metres"</p>
<p>ans = InfoReg.SI_Format.Format(val, "G6", "metres", noPaddingOrDash);</p>
<p>=> ans contains: "12.3456 exametres"</p>
<hr />
<h3>InfoReg.SI_Format.Format(System.Single, System.String, System.String, InfoReg.SI_Format.Padding)</h3>
<p>
Returns values in a text SI format. An example is 123.45km.
It takes a float value and looks at its decimal exponent. The exponent 
is reduced to its residue three value. It then prefixes the unit
passed in with the appropriate SI prefix.
</p><p>
"yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
"", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto"
</p><p>
If siunit is two or less characters the return will use short SI
prefixes like:
</p><p>
"Y", "Z", "E", "P", "T", "G", "M", "k",
"", "m", "μ", "n", "p", "f", "a", "z", "y"
</p><p>
No prefix is needed if the value of d_val lies in the range 0.0 to just under 1000.0.
</p>
<p>
Note: hecto, deca, deci, and centi are not supported.
SI does not support numbers above 10^27 or below 10^-24 
and any such value will be returned unmodified without SI prefix units.
</p>
<p><b><em>Example: ...</em></b></p>
<p>
using InfoReg;
</p>
<p> ... </p>
<p>
String ans;
</p><p>
float fval = (float)123.789E-7;
</p><p>
ans = InfoReg.SI_Format.Format(fval, "G4", "F");
</p><p>
=> ans contains: "12.38 μF"
</p>
<p>
ans = InfoReg.SI_Format.Format(fval, "G4", "Farads", InfoReg.SI_Format.Padding.dashWithPadding);
</p>
<p>
=> ans contains: "12.38 micro-Farads " // Both a dash and trailing space are used
</p>
<h4>f_val</h4><p>A float value to be SI normalized.</p>
<h4>sformat</h4><p>Is the format string usually based on G or N </p>
<h4>siunit</h4><p>An SI unit like watt, metre or l</p>
<h4>padding</h4>
<p>
Padding.dashOnly | Padding.dashWithPadding | Padding.paddingOnly | Padding.noPaddingOrDash
</p>
<h4>returns</h4>
<p>Formatted string e.g. "9.46 peta-metres"</p>
<hr />
<h3>InfoReg.SI_Format.Format(System.Decimal, System.String, System.String, InfoReg.SI_Format.Padding)</h3>
<p>
Returns values in text in SI format. An example is 123.45km.
It takes a decimal value and looks at its decimal exponent. The exponent 
is reduced to its residue three value. It then prefixes the unit
passed in with the appropriate SI prefix.
</p><p>
"yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
"", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto"
</p>
<p>
If siunit is two or less characters the return will use short SI
prefixes like:
</p>
<p>
"Y", "Z", "E", "P", "T", "G", "M", "k",
"", "m", "μ", "n", "p", "f", "a", "z", "y"
</p><p>
No prefix is needed if the value of d_val lies in the range 0.0 to just under 1000.0.
</p><p>
Note: hecto, deca, deci, and centi are not supported.
SI does not support numbers above 10^27 or below 10^-24 
and any such value will be returned unmodified without SI prefix units.
</p>
<p><b><em>
Example: ...
</em></b></p>
<p>
using InfoReg;
</p><p>
...
</p><p>
String ans;
</p><p>
Decimal decimal_val = Decimal.Parse("1234.5678901234567890123");
</p><p>
ans = InfoReg.SI_Format.Format(decimal_val, "G21", "grams");
</p><p>
=> ans contains: "1.23456789012345678901 kilo-grams"
</p><p>
ans = InfoReg.SI_Format.Format(decimal_val, "G21", "grams", InfoReg.SI_Format.Padding.paddingOnly);
</p><p>
=> ans contains: "1.23456789012345678901 kilograms " // trailing space added
</p>
<h4>decimal_val</h4><p>A decimal value to be SI normalized.</p>
<h4>sformat</h4><p>Is the format string usually based on G or N </p>
<h4>siunit</h4><p>An SI unit like watt, metre or l</p>
<h4>padding</h4>
<p>
Padding.dashOnly | Padding.dashWithPadding | Padding.paddingOnly | Padding.noPaddingOrDash
</p>
<h4>returns</h4><p>Formatted string e.g. "9.46 pm"</p>
<hr />
<h3>InfoReg.SI_Format.Parse(System.String, System.Double@)</h3>
<p>
Takes an SI formatted value like "12.34 km" and returns a double with the
value 1.234e4. A String "10pF" would be returned as a double value 1e-11.
</p>
<p><b><em>Example: ...</em></b></p>
<p>using InfoReg;</p>
<p>...</p>
<p>Double val;</p>
<p>InfoReg.SI_Format.Parse("1.23456 km", out val);</p>
<p>=> val has the value 1.23456e3</p>
<h4>si_value</h4><p>A string value like 12.345MHz</p>
<h4>dnum</h4><p>A double that will be assigned the parsed value from the SI formatted string</p>
<h4>return</h4><p>A double value adjusted for the SI prefix value.</p>
<hr />
<h3>InfoReg.SI_Format.Parse(System.String, System.Decimal@)</h3>
<p>
Takes an SI formatted value like "12.34 km" and returns a decimal with the
value 1.234e4. A String "10pF" would be returned as a decimal value 1e-11.
</p><p>
Example: ...</p><p>
using InfoReg;</p><p>
...</p><p>
Decimal val;</p><p>
InfoReg.SI_Format.Parse("1.23456 km", out val);</p><p>
=> val has the value 1.23456e3</p>
<h4>si_value</h4><p>A string value like 12.345MHz</p>
<h4>dnum</h4><p>A decimal that will be assigned the parsed value from the SI formatted string</p>
<h4>returns</h4><p>A decimal value adjusted for the SI prefix value.</p>
<hr />
<h3>InfoReg.SI_Format.Parse(System.String, System.Single@)</h3>
<p>
Takes an SI formatted value like "12.34 km" and returns a float with the
value 1.234e4. A String "10pF" would be returned as a float value 1e-11.</p>
<h4>Example: ...</h4>
<p>
using InfoReg;</p><p>
...</p><p>
float val;</p><p>
InfoReg.SI_Format.Parse("1.23456 km", out val);</p><p>
=> val has the value 1.23456e3          
</p>
<h4>si_value</h4><p>A string value like 12.345MHz</p>
<h4>f_num</h4><p>A float that will be assigned the parsed value from the SI formatted string</p>
<h4>returns</h4><p>A float value adjusted for the SI prefix value.</p>
<hr />
<h2>Physical Constants</h2>
<p>
Physical constants may be referenced unsing this class.
Please see https://physics.nist.gov/cuu/Constants/
</p>
<h3>InfoReg.Physical_Constants.LightSpeed"</h3>
<p>
LightSpeed is the speed of light in a vacuum.
</p>
<h3>InfoReg.Physical_Constants.GravitationalConstant</h3>
<p>
GravitationalConstant is the gravitational constant
</p>
<h3>InfoReg.Physical_Constants.PlanckConstant</h3>
<p>
PlanckConstant is Plank's constant
</p>
<h3>InfoReg.Physical_Constants.ReducedPlankConstant</h3>
<p>
ReducedPlankConstant is the reduced Plank's constant
</p>
<h3>InfoReg.Physical_Constants.MagneticConstant</h3>
<p>
MagneticConstant is the magnetic constant
</p>
<h3>InfoReg.Physical_Constants.ElectricConstant</h3>
<p>
ElectricConstant is the electric constant
</p>
<h3>InfoReg.Physical_Constants.MagneticFluxQuantum</h3>
<p>
MagneticFluxQuantum is the quantum magnetic flux constant
</p>
<h3>InfoReg.Physical_Constants.ElementaryCharge</h3>
<p>
ElementaryCharge is the elementary charge constant
</p>
<h3>InfoReg.Physical_Constants.ConductanceQuantum</h3>
<p>
ElementaryCharge is the quantum conductance constant
</p>
<h3>InfoReg.Physical_Constants.ElectronMass</h3>
<p>
ElectronMass is the mass of an electron
</p>
<h3>InfoReg.Physical_Constants.ProtonMass</h3>
<p>
ProtonMass is the mass of a proton
</p>
<h3>InfoReg.Physical_Constants.FineStructureConstant</h3>
<p>
FineStructureConstant is the fine structure constant
</p>
<h3>InfoReg.Physical_Constants.RydbergConstant</h3>
<p>
RydbergConstant is the Rydberg constant
</p>
<h3>InfoReg.Physical_Constants.BohrRadius</h3>
<p>
BohrRadius is the Bohr radius constant
</p>
<h3>InfoReg.Physical_Constants.ClassicalElectronRadius</h3>
<p>
ClassicalElectronRadius is the classical electron radius constant
</p>
<h3>InfoReg.Physical_Constants.AtomicMassUnit</h3>
<p>
AtomicMassUnit is the atomic mass unit constant
</p>
<h3>InfoReg.Physical_Constants.AvogadroConstant</h3>
<p>
AvogadroConstant is the Avogadro constant
</p>
<h3>InfoReg.Physical_Constants.FaradayConstant</h3>
<p>
FaradayConstant is the Faraday constant
</p>
<h3>InfoReg.Physical_Constants.MolarGasConstant</h3>
<p>
MolarGasConstant is the Molar Gas Constant
</p>
<h3>InfoReg.Physical_Constants.BoltzmannConstant</h3>
<p>
BoltzmannConstant is the Boltzmann's constant
</p>
<h3>InfoReg.Physical_Constants.Stefan_BoltzmannConstant</h3>
<p>
Stefan_BoltzmannConstant is the Stefan_Boltzmann's constant
</p>
</member>
<h3>InfoReg.Physical_Constants.ElectronVolt</h3>
<p>
ElectronVolt is an electron volt
</p>
<h3>InfoReg.Physical_Constants.StandardGravity</h3>
<p>
StandardGravity rate of accelleration due to gravity on Earth
</p>
<h3>InfoReg.Physical_Constants.Pi</h3>
<p>
Pi is the value of the pi constant i.e. numer of radii to make a semi circle based on that radius
</p>
<h3>InfoReg.Physical_Constants.NaturalLogBase</h3>
<p>
NaturalLogBase the base used for natural logs
</p>

<hr />
