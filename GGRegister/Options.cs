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
    }
}
