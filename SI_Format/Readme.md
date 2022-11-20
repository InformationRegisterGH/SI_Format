# SI_Format

InfoReg.SI_Format Provides functions to parse strings like 23.56MHz as a float or single, double, or decimal value like
23,560,000.0. InfoReg.SI_Format provides functions to write 1,860 as 1.86km or 1.86 kilometres.

SI_Format also has a number of physical constants used by the engineering and scientific workers. An example is 
InfoReg.Physical_Constants.LightSpeed. These are described at the end of this file.

Version: 1.1.3 is built for .NET 6 and .NET 7 runtime environments. The Visual Studio 2022 project files,
C# source code, and unit tests are available on GitHub at:
https://github.com/InformationRegisterGH/SI_Format

The applicable license agreement is available at: 
https://github.com/InformationRegisterGH/SIFormat/blob/Main/SI_Format/license.txt

---
## InfoReg.SI_Format

SI_Format is a class that contains functions to adjust or parse numbers to and from
strings like 10pF or 123.46 kilo-metres or 65.123 ml.

### InfoReg.SI_Format.Padding

Padding is an enumberated type.
An enumerated value to indicate if a dash "-" is required between the SI prefix and 
the unit as in kilo-gram. 
Padding will also indcate if a trailing space should be appended as padding.

#### InfoReg.SI_Format.Padding.dashonly
    dashonly: Default behaviour for Format function.
#### InfoReg.SI_Format.Padding.dashWithPadding
    dashWithPadding: Use a dash but no trailing space
#### InfoReg.SI_Format.Padding.paddingOnly
    paddingOnly: Add a trailing space only
#### InfoReg.SI_Format.Padding.noPaddingOrDash
    noPaddingOrDash: Neither dash, nor trailing space required

---

### InfoReg.SI_Format.Format(System.Double, System.String, System.String, InfoReg.SI_Format.Padding)

Returns values in text in SI format. An example is 123.45km.
It takes a double value and looks at its decimal exponent. The exponent 
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
                  

#### d_val
A double value to be SI normalized.
#### sformat
Is the format string usually based on G or N (see C# string.Format).
#### siunit
An SI unit like watt, metre or l
#### padding
    -Padding.dashOnly
    -Padding.dashWithPadding
    -Padding.paddingOnly
    -Padding.noPaddingOrDash
#### returns
Formatted string e.g. "9.46 peta-metres"

**example**

using InfoReg;

...

String ans;

double val = 123.456e17;

ans = InfoReg.SI_Format.Format(val, "G6", "metres");
    
    => ans contains: "12.3456 exa-metres"

ans = InfoReg.SI_Format.Format(val, "G6", "metres", noPaddingOrDash);
    
    => ans contains: "12.3456 exametres"

---

### InfoReg.SI_Format.Format(System.Single, System.String, System.String, InfoReg.SI_Format.Padding)

Returns values in a text SI format. An example is 123.45km.
It takes a float value and looks at its decimal exponent. The exponent 
is reduced to its residue three value. It then prefixes the unit
passed in with the appropriate SI prefix.

"yotta", "zetta", "exa", "peta", "tera", "giga", "mega", "kilo",
"", "milli", "micro", "nano", "pico", "femto", "atto", "zepto", "yocto"

If siunit is two or less characters the return will use short SI
prefixes like:

"Y", "Z", "E", "P", "T", "G", "M", "k",
"", "m", "μ", "n", "p", "f", "a", "z", "y"

No prefix is needed if the value of d_val lies in the range 0.0 to just under 1000.0.

**Note:** hecto, deca, deci, and centi are not supported.
SI does not support numbers above 10^27^ or below 10^-24^ 
and any such value will be returned unmodified without SI prefix units.

**Example:**

using InfoReg;

... 

String ans;
float fval = (float)123.789E-7;

ans = InfoReg.SI_Format.Format(fval, "G4", "F");

    => ans contains: "12.38 μF"

ans = InfoReg.SI_Format.Format(fval, "G4", "Farads", InfoReg.SI_Format.Padding.dashWithPadding);

    => ans contains: "12.38 micro-Farads " // Both a dash and trailing space are used

#### f_val
A float value to be SI normalized.
#### sformat
Is the format string usually based on G or N
#### siunit
An SI unit like watt, metre or l
#### padding
    -Padding.dashOnly
    -Padding.dashWithPadding
    -Padding.paddingOnly
    -Padding.noPaddingOrDash
#### returns
Formatted string e.g. "9.46 peta-metres"

---

### InfoReg.SI_Format.Format(System.Decimal, System.String, System.String, InfoReg.SI_Format.Padding)

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

**Note:** hecto, deca, deci, and centi are not supported.
SI does not support numbers above 10^27 or below 10^-24 
and any such value will be returned unmodified without SI prefix units.

**Example:**

using InfoReg;

...

String ans;

Decimal decimal_val = Decimal.Parse("1234.5678901234567890123");

ans = InfoReg.SI_Format.Format(decimal_val, "G21", "grams");

    => ans contains: "1.23456789012345678901 kilo-grams"

ans = InfoReg.SI_Format.Format(decimal_val, "G21", "grams", InfoReg.SI_Format.Padding.paddingOnly);

    => ans contains: "1.23456789012345678901 kilograms " // trailing space added

#### decimal_val
A decimal value to be SI normalized.
#### sformat
Is the format string usually based on G or N
#### siunit
An SI unit like watt, metre or l
#### padding
    -Padding.dashOnly
    -Padding.dashWithPadding
    -Padding.paddingOnly
    -Padding.noPaddingOrDash
#### returns
Formatted string e.g. "9.46 pm"

---

### InfoReg.SI_Format.Parse(System.String, System.Double@)

Takes an SI formatted value like "12.34 km" and returns a double with the
value 1.234e4. A String "10pF" would be returned as a double value 1e-11.

**Example:**

using InfoReg;

...

Double val;

InfoReg.SI_Format.Parse("1.23456 km", out val);
    => val has the value 1.23456e3

#### si_value
A string value like 12.345MHz
#### dnum
A double that will be assigned the parsed value from the SI formatted string

#### return
A double value adjusted for the SI prefix value.

---

### InfoReg.SI_Format.Parse(System.String, System.Decimal@)

Takes an SI formatted value like "12.34 km" and returns a decimal with the
value 1.234e4. A String "10pF" would be returned as a decimal value 1e-11.

**Example:**

using InfoReg;
...
Decimal val;
InfoReg.SI_Format.Parse("1.23456 km", out val);
    => val has the value 1.23456e3</p>
#### si_value
A string value like 12.345MHz
#### dnum
A decimal that will be assigned the parsed value from the SI formatted string
#### returns
A decimal value adjusted for the SI prefix value.

---

### InfoReg.SI_Format.Parse(System.String, System.Single@)

Takes an SI formatted value like "12.34 km" and returns a float with the
value 1.234e4. A String "10pF" would be returned as a float value 1e-11.</p>
**Example:**
using InfoReg;
...
float val;
InfoReg.SI_Format.Parse("1.23456 km", out val);
    => val has the value 1.23456e3          
#### si_value
A string value like 12.345MHz
#### f_num
A float that will be assigned the parsed value from the SI formatted string
#### returns
A float value adjusted for the SI prefix value.

---

## Physical Constants

Physical constants may be referenced unsing this class.
Please see https://physics.nist.gov/cuu/Constants/ 

### InfoReg.Physical_Constants.LightSpeed
LightSpeed is the speed of light in a vacuum.
### InfoReg.Physical_Constants.GravitationalConstant
GravitationalConstant is the gravitational constant
### InfoReg.Physical_Constants.PlanckConstant
PlanckConstant is Plank's constant
### InfoReg.Physical_Constants.ReducedPlankConstant
ReducedPlankConstant is the reduced Plank's constant
### InfoReg.Physical_Constants.MagneticConstant
MagneticConstant is the magnetic constant
### InfoReg.Physical_Constants.ElectricConstant
ElectricConstant is the electric constant
### InfoReg.Physical_Constants.MagneticFluxQuantum
MagneticFluxQuantum is the quantum magnetic flux constant
### InfoReg.Physical_Constants.ElementaryCharge
ElementaryCharge is the elementary charge constant
### InfoReg.Physical_Constants.ConductanceQuantum
ElementaryCharge is the quantum conductance constant
### InfoReg.Physical_Constants.ElectronMass
ElectronMass is the mass of an electron
### InfoReg.Physical_Constants.ProtonMass
ProtonMass is the mass of a proton
### InfoReg.Physical_Constants.FineStructureConstant
FineStructureConstant is the fine structure constant
### InfoReg.Physical_Constants.RydbergConstant
RydbergConstant is the Rydberg constant
### InfoReg.Physical_Constants.BohrRadius
BohrRadius is the Bohr radius constant
### InfoReg.Physical_Constants.ClassicalElectronRadius
ClassicalElectronRadius is the classical electron radius constant
### InfoReg.Physical_Constants.AtomicMassUnit
AtomicMassUnit is the atomic mass unit constant
### InfoReg.Physical_Constants.AvogadroConstant
AvogadroConstant is the Avogadro constant
### InfoReg.Physical_Constants.FaradayConstant
FaradayConstant is the Faraday constant
### InfoReg.Physical_Constants.MolarGasConstant
MolarGasConstant is the Molar Gas Constant
### InfoReg.Physical_Constants.BoltzmannConstant
BoltzmannConstant is the Boltzmann's constant
### InfoReg.Physical_Constants.Stefan_BoltzmannConstant
Stefan_BoltzmannConstant is the Stefan_Boltzmann's constant
### InfoReg.Physical_Constants.ElectronVolt
ElectronVolt is an electron volt
### InfoReg.Physical_Constants.StandardGravity
StandardGravity rate of accelleration due to gravity on Earth
### InfoReg.Physical_Constants.Pi
Pi is the value of the pi constant i.e. numer of radii to make a semi circle based on that radius
### InfoReg.Physical_Constants.NaturalLogBase
NaturalLogBase the base used for natural logs

---
