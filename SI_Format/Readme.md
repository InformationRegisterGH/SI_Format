# SI_Format

InfoReg.SI_Format Provides functions to parse strings like 23.56MHz as a float or single, double, or decimal 
value like 23,560,000.0. InfoReg.SI_Format provides functions to write 1,860 as 1.86km or 1.86 kilometres.

SI_Format also has a number of physical constants used by the engineering and scientific workers. An example is 
InfoReg.Physical_Constants.LightSpeed. These are described at the end of this file.

Version: 1.1.4 is built for .net6.0, .net7.0, and .net8.0 runtime environments. The Visual Studio 2022 project files,
C# source code, and unit tests are available on GitHub at:
https://github.com/InformationRegisterGH/SI_Format

The applicable license agreement is available at: 
https://github.com/InformationRegisterGH/SI_Format/blob/Main/SI_Format/license.txt

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

### InfoReg.SI_Format.Format\<T\>(T tval, string sformat, string siunit, Padding padding = Padding.dashonly)

Returns values in text in SI format. An example is 123.45km.
It takes a double, float, or decimal value and looks at its decimal exponent. The exponent 
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
        SI does not support numbers above 10^33^ or below 10^-30^ 
        and any such value will be returned unmodified without SI prefix units.
                  
### T
One of [ double | single | decimal ] to be SI normalized.
#### tval
A double, single, or decimal value to be SI normalized.
#### sformat
Is the format string usually based on G or N (see C# string.Format).
#### siunit
An SI unit like watt, metre or l
#### padding
    -Padding.dashOnly // default value
    -Padding.dashWithPadding
    -Padding.paddingOnly
    -Padding.noPaddingOrDash
#### returns
Formatted string e.g. "9.46 peta-metres"

**example**

``` C#:
using InfoReg;
...
String ans;
double val = 123.456e17;
ans = InfoReg.SI_Format.Format<double>(val, "G6", "metres");
// => ans contains: "12.3456 exa-metres"
ans = InfoReg.SI_Format.Format<double>(val, "G6", "metres", noPaddingOrDash);
// => ans contains: "12.3456 exametres
```

---


### InfoReg.SI_Format.Parse\<T\>(System.String si_value, out num)

Takes an SI formatted value like "12.34 km" and returns a double, float, or decimal with the
value 1.234e4. A String "10pF" would be returned as a double value 1e-11.

#### T
One of [ double | single | decimal ] to be parsed from an SI formatted string.
#### si_value
A string value like 12.345MHz
#### num
A double, float, or decimal that will be assigned the parsed value from the SI formatted string.
#### return
void

**Example:**

``` C#:
using InfoReg;
...
Double val;
InfoReg.SI_Format.Parse<double>("1.23456 km", out val);
//    => val has the value 1.23456e3`
```

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
