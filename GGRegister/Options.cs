using CommandLine;

namespace GGRegister
{
    class Options
    {
        [Value(0)]
        public string PersonalDataPath { get; set; }

        [Value(1)]
        public string PhoneNumbersPath { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('u', "uncheched-path", Required =false, HelpText = "If set, program will save unchecked numbers to that path and additionali will save checked numbers to original number path.")]
        public string UncheckedNumbersPath { get; set; }
    }
}
