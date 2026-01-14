// Maintain this file in UTF-8
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using InfoReg;
using System.Security;

namespace InfoRegSI
{
    [TestClass]
    public class SI_Format_Test
    {
        [TestMethod]
        public void SI_Double()
        {
            string AssertErrorMsg = "InfoReg.SI_Format.Format(Double, String) failed.";
            string ans;
            double val = 123.456e17;
            ans = InfoReg.SI_Format.Format<double>(val, "G6", "metres");
            Assert.AreEqual("12.3456 exa-metres", ans, false, AssertErrorMsg);
            ans = InfoReg.SI_Format.Format<double>(val, "G6", "m", SI_Format.Padding.noPaddingOrDash);
            Assert.AreEqual("12.3456 Em", ans, false, AssertErrorMsg);
            val = 98.7654E-21;
            ans = InfoReg.SI_Format.Format<double>(val, "G6", "g");
            Assert.AreEqual("98.7654 z-g", ans, false, AssertErrorMsg);
            val = 99.9995E32;
            ans = InfoReg.SI_Format.Format<double>(val, "G7", "litres");
            Assert.AreEqual("9.99995E+33 litres", ans, false, AssertErrorMsg);
            val = 99.9994E-29;
            ans = InfoReg.SI_Format.Format<double>(val, "G7", "litres");
            Assert.AreEqual("999.994 quecto-litres", ans, false, AssertErrorMsg);
            val = 99.9994E-32;
            ans = InfoReg.SI_Format.Format<double>(val, "G7", "litres");
            Assert.AreEqual("9.99994E-31 litres", ans, false, AssertErrorMsg);
            val = 45.7;
            ans = InfoReg.SI_Format.Format<double>(val, "G7", "m");
            Assert.AreEqual("45.7 m", ans, false, AssertErrorMsg);
            // "G4" will only print 3 characters in next test
            val = 0.5;
            ans = InfoReg.SI_Format.Format<double>(val, "G4", "l");
            Assert.AreEqual("500 m-l", ans, false, AssertErrorMsg);
            val = 1500;
            ans = InfoReg.SI_Format.Format<double>(val, "N2", "m", SI_Format.Padding.noPaddingOrDash);
            Assert.AreEqual("1.50 km", ans, false, AssertErrorMsg);
            // A light year (based on an average 365.25 Earth days) in a vacuum
            val = 365.25 * 24.0 * 60.0 * 60.0 * InfoReg.Physical_Constants.LightSpeed;
            ans = InfoReg.SI_Format.Format<double>(val, "G3", "metres");
            Assert.AreEqual("9.46 peta-metres", ans, false, AssertErrorMsg);
            ans = InfoReg.SI_Format.Format<double>(val, "G3", "metres", SI_Format.Padding.paddingOnly);
            Assert.AreEqual("9.46 petametres ", ans, false, AssertErrorMsg);
            // IEC Prefixes test
            val = 2048.0;
            ans = InfoReg.SI_Format.Format<double>(val, "G4", "bytes", SI_Format.Padding.noPaddingOrDash, SI_Format.FormatType.IEC);
            Assert.AreEqual("2 Kibibytes", ans, false, AssertErrorMsg);
            val = 2048 * 2048 + 512 * 1024;
            ans = InfoReg.SI_Format.Format<double>(val, "G4", "B", SI_Format.Padding.noPaddingOrDash, SI_Format.FormatType.IEC);
            Assert.AreEqual("4.5 MiB", ans, false, AssertErrorMsg);
            val = ((2.0 * Math.Pow(2, 40)) + 768 * Math.Pow(2, 30));
            ans = InfoReg.SI_Format.Format<double>(val, "G4", "B", SI_Format.Padding.noPaddingOrDash, SI_Format.FormatType.IEC);
            Assert.AreEqual("2.75 TiB", ans, false, AssertErrorMsg);
        }

        [TestMethod]
        public void SI_Float()
        {
            string AssertErrorMsg = "InfoReg.SI_Format.Format(float, String) failed.";
            string ans;
            float fval = (float)123.789E-7;
            ans = InfoReg.SI_Format.Format<Single>(fval, "G4", "F");
            Assert.AreEqual("12.38 μ-F", ans, false, AssertErrorMsg);
            fval = (float)24.765e6;
            ans = InfoReg.SI_Format.Format<Single>(fval, "G6", "meters");
            Assert.AreEqual("24.765 mega-meters", ans, false, AssertErrorMsg);
            ans = InfoReg.SI_Format.Format<Single>(fval, "G5", "meters", SI_Format.Padding.dashWithPadding);
            Assert.AreEqual("24.765 mega-meters ", ans, false, AssertErrorMsg);
            fval = (float)99.634e-5;
            ans = InfoReg.SI_Format.Format<Single>(fval, "G5", "Farads", SI_Format.Padding.noPaddingOrDash);
            Assert.AreEqual("996.34 microFarads", ans, false, AssertErrorMsg);
            fval = (float)19.65e-29;
            ans = InfoReg.SI_Format.Format<Single>(fval, "G5", "Farads", SI_Format.Padding.noPaddingOrDash);
            Assert.AreEqual("196.5 quectoFarads", ans, false, AssertErrorMsg);
            // IEC Prefixes test
            fval = 2048;
            ans = InfoReg.SI_Format.Format<Single>(fval, "G4", "bytes", SI_Format.Padding.noPaddingOrDash, SI_Format.FormatType.IEC);
            Assert.AreEqual("2 Kibibytes", ans, false, AssertErrorMsg);
            fval = 2048 * 2048 + 512 * 1024;
            ans = InfoReg.SI_Format.Format<Single>(fval, "G4", "B", SI_Format.Padding.noPaddingOrDash, SI_Format.FormatType.IEC);
            Assert.AreEqual("4.5 MiB", ans, false, AssertErrorMsg);
            fval = ((2f * MathF.Pow(2, 30)) + 512f * MathF.Pow(2, 20));
            ans = InfoReg.SI_Format.Format<float>(fval, "G4", "B", SI_Format.Padding.noPaddingOrDash, SI_Format.FormatType.IEC);
            Assert.AreEqual("2.5 GiB", ans, false, AssertErrorMsg);
        }

        [TestMethod]
        public void SI_Decimal()
        {
            string ans;
            string AssertErrorMsg = "InfoReg.SI_Format.Format(Decimal, String) failed.";
            decimal decimal_val = decimal.Parse("1234.5678901234567890123");
            ans = InfoReg.SI_Format.Format<decimal>(decimal_val, "G21", "grams");
            Assert.AreEqual("1.23456789012345678901 kilo-grams", ans, false, AssertErrorMsg);
            ans = InfoReg.SI_Format.Format<decimal>(decimal_val, "G21", "grams", SI_Format.Padding.dashWithPadding);
            Assert.AreEqual("1.23456789012345678901 kilo-grams ", ans, false, AssertErrorMsg);
            ans = InfoReg.SI_Format.Format<decimal>(decimal_val, "G21", "grams", SI_Format.Padding.noPaddingOrDash);
            Assert.AreEqual("1.23456789012345678901 kilograms", ans, false, AssertErrorMsg);
            // IEC Prefixes test
            decimal_val = 2048;
            ans = InfoReg.SI_Format.Format<decimal>(decimal_val, "G4", "bytes", SI_Format.Padding.noPaddingOrDash, SI_Format.FormatType.IEC);
            Assert.AreEqual("2 Kibibytes", ans, false, AssertErrorMsg);
            decimal_val = 2048 * 2048 + 512 * 1024;
            ans = InfoReg.SI_Format.Format<decimal>(decimal_val, "G4", "B", SI_Format.Padding.noPaddingOrDash, SI_Format.FormatType.IEC);
            Assert.AreEqual("4.5 MiB", ans, false, AssertErrorMsg);
            decimal_val = (Decimal)((2 * Math.Pow(2, 30)) + 512 * Math.Pow(2, 20));
            ans = InfoReg.SI_Format.Format<decimal>(decimal_val, "G4", "B", SI_Format.Padding.noPaddingOrDash, SI_Format.FormatType.IEC);
            Assert.AreEqual("2.5 GiB", ans, false, AssertErrorMsg);
        }

        [TestMethod]
        public void SI_ParseDouble()
        {
            string AssertErrorMsg = "Double InfoReg.SI_Format.Parse failed.";
            InfoReg.SI_Format.Parse<double>("1.23456 km", out double val);
            double dans = 1.23456e3;
            Assert.AreEqual(dans, val, 1.0e-6, AssertErrorMsg);
            InfoReg.SI_Format.Parse<double>("1.23456 mega-litres", out val);
            dans = 1.23456e6;
            Assert.AreEqual(dans, val, 1.0e-66, AssertErrorMsg);
            InfoReg.SI_Format.Parse<double>("1.23456 pico-farad", out val);
            dans = 1.23456e-12;
            Assert.AreEqual(dans, val, 1.0e-18, AssertErrorMsg);
            InfoReg.SI_Format.Parse<double>("1.23456 ns", out val);
            dans = 1.23456e-9;
            Assert.AreEqual(dans, val, 1.0e-15, AssertErrorMsg);
            InfoReg.SI_Format.Parse<double>("1.23456 m", out val);
            dans = 1.23456;
            Assert.AreEqual(dans, val, 1.0e-6, AssertErrorMsg);
            InfoReg.SI_Format.Parse<double>("1.23456 kilometres", out val);
            dans = 1.23456e3;
            Assert.AreEqual(dans, val, 1.0e-6, AssertErrorMsg);
        }

        [TestMethod]
        public void IEC_ParseDouble()
        {
            string AssertErrorMsg = "Double InfoReg.IEC_Format.Parse failed.";
            InfoReg.SI_Format.Parse<double>("10 Ki", out double val, SI_Format.FormatType.IEC);
            double dans = 1024.0 * 10.0;
            Assert.AreEqual(dans, val, 1.0e-6, AssertErrorMsg);
            InfoReg.SI_Format.Parse<double>("10 Ki", out double val1, SI_Format.FormatType.IEC);
            dans = 1024.0 * 10.0;
            Assert.AreEqual(dans, val1, 1.0e-6, AssertErrorMsg);
            InfoReg.SI_Format.Parse<double>("25 Ti", out double val2, SI_Format.FormatType.IEC);
            dans = Math.Pow(2, 10 * 4) * 25.0;
            Assert.AreEqual(dans, val2, 1.0e-6, AssertErrorMsg);
            InfoReg.SI_Format.Parse<double>("10.5 Yi", out double val3, SI_Format.FormatType.IEC);
            dans = Math.Pow(2, 10 * 8) * 10.5;
            Assert.AreEqual(dans, val3, 1.0e-6, AssertErrorMsg);
        }

        [TestMethod]
        public void SI_ParseFloat()
        {
            string AssertErrorMsg = "InfoReg.SI_Format.ParseFloat failed.";
            InfoReg.SI_Format.Parse<Single>("1.23456 km", out float val);
            float dans = (float)1.23456e3;
            Assert.AreEqual(dans, val, 1.0e-4, AssertErrorMsg);
            InfoReg.SI_Format.Parse<Single>("1.23456 mega-litres", out val);
            dans = (float)1.23456e6;
            Assert.AreEqual(dans, val, 1.0e-3, AssertErrorMsg);
            InfoReg.SI_Format.Parse<Single>("1.23456 pico-farad", out val);
            dans = (float)1.23456e-12;
            Assert.AreEqual(dans, val, 1.0e-18, AssertErrorMsg);
            InfoReg.SI_Format.Parse<Single>("1.23456 ns", out val);
            dans = (float)1.23456e-9;
            Assert.AreEqual(dans, val, 1.0e-15, AssertErrorMsg);
            InfoReg.SI_Format.Parse<Single>("1.23456 m", out val);
            dans = (float)1.23456;
            Assert.AreEqual(dans, val, 1.0e-6, AssertErrorMsg);
            InfoReg.SI_Format.Parse<Single>("1.23456 kilometres", out val);
            dans = (float)1.23456e3;
            Assert.AreEqual(dans, val, 1.0e-4, AssertErrorMsg);
        }

        [TestMethod]
        public void IEC_ParseFloat()
        {
            string AssertErrorMsg = "IECParseFloat InfoReg.IEC_Format.ParseFloat failed.";
            InfoReg.SI_Format.Parse<float>("10 Ki", out float fval, SI_Format.FormatType.IEC);
            float fans = 1024.0f * 10.0f;
            Assert.AreEqual(fans, fval, 1.0e-6, AssertErrorMsg);
            InfoReg.SI_Format.Parse<float>("10 Ki", out float val1, SI_Format.FormatType.IEC);
            fans = 1024.0f * 10.0f;
            Assert.AreEqual(fans, val1, 1.0e-6, AssertErrorMsg);
            InfoReg.SI_Format.Parse<float>("25 Ti", out float val2, SI_Format.FormatType.IEC);
            fans = MathF.Pow(2, 10 * 4) * 25.0f;
            Assert.AreEqual(fans, val2, 1.0e-6, AssertErrorMsg);
            InfoReg.SI_Format.Parse<float>("10.5 Yi", out float val3, SI_Format.FormatType.IEC);
            fans = MathF.Pow(2, 10 * 8) * 10.5f;
            Assert.AreEqual(fans, val3, 1.0e-6, AssertErrorMsg);
        }


        [TestMethod]
        public void SI_ParseDecimal()
        {
            string AssertErrorMsg = "InfoReg.SI_Format.ParseDecimal failed.";
            InfoReg.SI_Format.Parse<decimal>("1.234567890123456789012 km", out decimal val);
            decimal dans = (decimal)1234.567890123456789012;
            Assert.AreEqual(dans.ToString("N"), val.ToString("N"), true, AssertErrorMsg);
            InfoReg.SI_Format.Parse<decimal>("1.234567890123456789012 mega-litres", out val);
            dans = (decimal)1234567.890123456789012;
            Assert.AreEqual(dans.ToString("N8"), val.ToString("N8"), true, AssertErrorMsg);
            InfoReg.SI_Format.Parse<decimal>("1.234567890123456789012 pico-farad", out val);
            dans = (decimal)0.00000000000123456789012;
            Assert.AreEqual(dans.ToString("N1.16"), val.ToString("N1.16"), true, AssertErrorMsg);
        }

        [TestMethod]
        public void IEC_ParseDecimal()
        {
            string AssertErrorMsg = "Double InfoReg.IEC_Format.Parse failed.";
            InfoReg.SI_Format.Parse<decimal>("10 Ki", out decimal val, SI_Format.FormatType.IEC);
            decimal dans = 1024 * 10;
            Assert.AreEqual(dans, val, AssertErrorMsg);
            InfoReg.SI_Format.Parse<decimal>("10 Ki", out decimal val1, SI_Format.FormatType.IEC);
            dans = (decimal)1024.0 * 10;
            Assert.AreEqual(dans, val1, AssertErrorMsg);
            InfoReg.SI_Format.Parse<decimal>("25 Ti", out decimal val2, SI_Format.FormatType.IEC);
            dans = (decimal)Math.Pow(2, 10 * 4) * 25;
            Assert.AreEqual(dans, val2, AssertErrorMsg);
            InfoReg.SI_Format.Parse<decimal>("15 Yi", out decimal val3, SI_Format.FormatType.IEC);
            dans = (decimal)Math.Pow(2, 10 * 8) * 15;
            Assert.AreEqual(dans, val3, AssertErrorMsg);
        }

        [TestMethod]
        public void SI_GenericFormat()
        {
            string AssertErrorMsg = "InfoReg.SI_Format.GenericFormat failed.";
            string ans;
            double val = 123.456e17;
            ans = InfoReg.SI_Format.Format<double>(val, "G6", "metres");
            Assert.AreEqual("12.3456 exa-metres", ans, false, AssertErrorMsg);
            float fval = (float)123.789E-7;
            ans = InfoReg.SI_Format.Format<float>(fval, "G4", "F", SI_Format.Padding.noPaddingOrDash);
            Assert.AreEqual("12.38 μF", ans, false, AssertErrorMsg);
            decimal decimal_val = decimal.Parse("1234.5678901234567890123");
            ans = InfoReg.SI_Format.Format<decimal>(decimal_val, "G21", "grams");
            Assert.AreEqual("1.23456789012345678901 kilo-grams", ans, false, AssertErrorMsg);
            val = 768 * Math.Pow(2, 40);
            ans = InfoReg.SI_Format.Format<double>(val, "G4", "B", SI_Format.Padding.noPaddingOrDash, SI_Format.FormatType.IEC);
            Assert.AreEqual("768 TiB", ans, false, AssertErrorMsg);
        }

        [TestMethod]
        public void SI_GenericParse()
        {
            string AssertErrorMsg = "InfoReg.SI_Format.Parse<T> failed.";
            double val;
            InfoReg.SI_Format.Parse<double>("1.23456 km", out val);
            double dans = 1.23456e3;
            Assert.AreEqual(dans, val, 1.0e-6, AssertErrorMsg);
            float fval;
            InfoReg.SI_Format.Parse<float>("1.23456 km", out fval);
            float fdans = (float)1.23456e3;
            Assert.AreEqual(fdans, fval, 1.0e-4, AssertErrorMsg);
            decimal decimal_val;
            InfoReg.SI_Format.Parse<decimal>("1.234567890123456789012 km", out decimal_val);
            decimal ddans = (decimal)1234.567890123456789012;
            Assert.AreEqual(ddans.ToString("N"), decimal_val.ToString("N"), true, AssertErrorMsg);
        }

        [TestMethod]
        public void IEC_GenericParse()
        {
            string AssertErrorMsg = "InfoReg.SI_Format.Parse<T> failed.";
            InfoReg.SI_Format.Parse<double>("1.5 Yi", out double val, SI_Format.FormatType.IEC);
            double dans = 1.5 * Math.Pow(2, 10 * 8);
            Assert.AreEqual(dans, val, 1.0e-6, AssertErrorMsg);
            float fval;
            InfoReg.SI_Format.Parse<float>("32 Gi", out fval, SI_Format.FormatType.IEC);
            float fdans = 32f * MathF.Pow(2, 10 * 3);
            Assert.AreEqual(fdans, fval, 1.0e-4, AssertErrorMsg);
            InfoReg.SI_Format.Parse<decimal>("64 Ki", out decimal decimal_val, SI_Format.FormatType.IEC);
            decimal ddans = (decimal)(64.0 * Math.Pow(2.0, 10.0 * 1.0));
            Assert.AreEqual(ddans, decimal_val, 1000000000000, AssertErrorMsg);
        }
    }
}