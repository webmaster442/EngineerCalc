using System.ComponentModel;

using DynamicEvaluator;
using DynamicEvaluator.TypeSystem;

using EngineerCalc.Domain;

using Spectre.Console;
using Spectre.Console.Cli;

namespace EngineerCalc.Commands;

internal sealed class CurrencyCommand : AsyncCommand<CurrencyCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("The unit to convert from.")]
        [CommandOption("-f|--from <UNIT>")]
        public string FromUnit { get; init; } = string.Empty;

        [Description("The unit to convert to.")]
        [CommandOption("-t|--to <UNIT>")]
        public string ToUnit { get; init; } = string.Empty;

        [CommandArgument(0, "[expression]")]
        [Description("An Expression to operate on")]
        public string Expression { get; set; } = string.Empty;
        
        [Description("List all available currencies in a table.")]
        [CommandOption("-l|--list")]
        public bool ListTable { get; set; }

        public override ValidationResult Validate()
        {
            return string.IsNullOrWhiteSpace(Expression) && !ListTable
                ? ValidationResult.Error("Expression cannot be empty.")
                : ValidationResult.Success();
        }
    }

    private readonly IRemoteApiClient _remoteApiClient;
    private readonly IEvaluatorApi _api;
    private readonly State _state;

    public CurrencyCommand(IEvaluatorApi api,
                           State state,
                           IRemoteApiClient remoteApiClient)
    {
        _api = api;
        _state = state;
        _remoteApiClient = remoteApiClient;
    }

    public static void PrintTable()
    {
        var table = new Table();
        table.AddColumns("Currency Code", "Currency Name", "Country");
        table.AddRow("AED", "UAE Dirham", "United Arab Emirates");
        table.AddRow("AFN", "Afghan Afghani", "Afghanistan");
        table.AddRow("ALL", "Albanian Lek", "Albania");
        table.AddRow("AMD", "Armenian Dram", "Armenia");
        table.AddRow("ANG", "Netherlands Antillian Guilder", "Netherlands Antilles");
        table.AddRow("AOA", "Angolan Kwanza", "Angola");
        table.AddRow("ARS", "Argentine Peso", "Argentina");
        table.AddRow("AUD", "Australian Dollar", "Australia");
        table.AddRow("AWG", "Aruban Florin", "Aruba");
        table.AddRow("AZN", "Azerbaijani Manat", "Azerbaijan");
        table.AddRow("BAM", "Bosnia and Herzegovina Mark", "Bosnia and Herzegovina");
        table.AddRow("BBD", "Barbados Dollar", "Barbados");
        table.AddRow("BDT", "Bangladeshi Taka", "Bangladesh");
        table.AddRow("BGN", "Bulgarian Lev", "Bulgaria");
        table.AddRow("BHD", "Bahraini Dinar", "Bahrain");
        table.AddRow("BIF", "Burundian Franc", "Burundi");
        table.AddRow("BMD", "Bermudian Dollar", "Bermuda");
        table.AddRow("BND", "Brunei Dollar", "Brunei");
        table.AddRow("BOB", "Bolivian Boliviano", "Bolivia");
        table.AddRow("BRL", "Brazilian Real", "Brazil");
        table.AddRow("BSD", "Bahamian Dollar", "Bahamas");
        table.AddRow("BTN", "Bhutanese Ngultrum", "Bhutan");
        table.AddRow("BWP", "Botswana Pula", "Botswana");
        table.AddRow("BYN", "Belarusian Ruble", "Belarus");
        table.AddRow("BZD", "Belize Dollar", "Belize");
        table.AddRow("CAD", "Canadian Dollar", "Canada");
        table.AddRow("CDF", "Congolese Franc", "Democratic Republic of the Congo");
        table.AddRow("CHF", "Swiss Franc", "Switzerland");
        table.AddRow("CLF", "Chilean Unidad de Fomento", "Chile");
        table.AddRow("CLP", "Chilean Peso", "Chile");
        table.AddRow("CNH", "Offshore Chinese Renminbi", "China");
        table.AddRow("CNY", "Chinese Renminbi", "China");
        table.AddRow("COP", "Colombian Peso", "Colombia");
        table.AddRow("CRC", "Costa Rican Colon", "Costa Rica");
        table.AddRow("CUP", "Cuban Peso", "Cuba");
        table.AddRow("CVE", "Cape Verdean Escudo", "Cape Verde");
        table.AddRow("CZK", "Czech Koruna", "Czech Republic");
        table.AddRow("DJF", "Djiboutian Franc", "Djibouti");
        table.AddRow("DKK", "Danish Krone", "Denmark");
        table.AddRow("DOP", "Dominican Peso", "Dominican Republic");
        table.AddRow("DZD", "Algerian Dinar", "Algeria");
        table.AddRow("EGP", "Egyptian Pound", "Egypt");
        table.AddRow("ERN", "Eritrean Nakfa", "Eritrea");
        table.AddRow("ETB", "Ethiopian Birr", "Ethiopia");
        table.AddRow("EUR", "Euro", "European Union");
        table.AddRow("FJD", "Fiji Dollar", "Fiji");
        table.AddRow("FKP", "Falkland Islands Pound", "Falkland Islands");
        table.AddRow("FOK", "Faroese Króna", "Faroe Islands");
        table.AddRow("GBP", "Pound Sterling", "United Kingdom");
        table.AddRow("GEL", "Georgian Lari", "Georgia");
        table.AddRow("GGP", "Guernsey Pound", "Guernsey");
        table.AddRow("GHS", "Ghanaian Cedi", "Ghana");
        table.AddRow("GIP", "Gibraltar Pound", "Gibraltar");
        table.AddRow("GMD", "Gambian Dalasi", "The Gambia");
        table.AddRow("GNF", "Guinean Franc", "Guinea");
        table.AddRow("GTQ", "Guatemalan Quetzal", "Guatemala");
        table.AddRow("GYD", "Guyanese Dollar", "Guyana");
        table.AddRow("HKD", "Hong Kong Dollar", "Hong Kong");
        table.AddRow("HNL", "Honduran Lempira", "Honduras");
        table.AddRow("HRK", "Croatian Kuna", "Croatia");
        table.AddRow("HTG", "Haitian Gourde", "Haiti");
        table.AddRow("HUF", "Hungarian Forint", "Hungary");
        table.AddRow("IDR", "Indonesian Rupiah", "Indonesia");
        table.AddRow("ILS", "Israeli New Shekel", "Israel");
        table.AddRow("IMP", "Manx Pound", "Isle of Man");
        table.AddRow("INR", "Indian Rupee", "India");
        table.AddRow("IQD", "Iraqi Dinar", "Iraq");
        table.AddRow("ISK", "Icelandic Króna", "Iceland");
        table.AddRow("JEP", "Jersey Pound", "Jersey");
        table.AddRow("JMD", "Jamaican Dollar", "Jamaica");
        table.AddRow("JOD", "Jordanian Dinar", "Jordan");
        table.AddRow("JPY", "Japanese Yen", "Japan");
        table.AddRow("KES", "Kenyan Shilling", "Kenya");
        table.AddRow("KGS", "Kyrgyzstani Som", "Kyrgyzstan");
        table.AddRow("KHR", "Cambodian Riel", "Cambodia");
        table.AddRow("KID", "Kiribati Dollar", "Kiribati");
        table.AddRow("KMF", "Comorian Franc", "Comoros");
        table.AddRow("KRW", "South Korean Won", "South Korea");
        table.AddRow("KWD", "Kuwaiti Dinar", "Kuwait");
        table.AddRow("KYD", "Cayman Islands Dollar", "Cayman Islands");
        table.AddRow("KZT", "Kazakhstani Tenge", "Kazakhstan");
        table.AddRow("LAK", "Lao Kip", "Laos");
        table.AddRow("LBP", "Lebanese Pound", "Lebanon");
        table.AddRow("LKR", "Sri Lanka Rupee", "Sri Lanka");
        table.AddRow("LRD", "Liberian Dollar", "Liberia");
        table.AddRow("LSL", "Lesotho Loti", "Lesotho");
        table.AddRow("LYD", "Libyan Dinar", "Libya");
        table.AddRow("MAD", "Moroccan Dirham", "Morocco");
        table.AddRow("MDL", "Moldovan Leu", "Moldova");
        table.AddRow("MGA", "Malagasy Ariary", "Madagascar");
        table.AddRow("MKD", "Macedonian Denar", "North Macedonia");
        table.AddRow("MMK", "Burmese Kyat", "Myanmar");
        table.AddRow("MNT", "Mongolian Tögrög", "Mongolia");
        table.AddRow("MOP", "Macanese Pataca", "Macau");
        table.AddRow("MRU", "Mauritanian Ouguiya", "Mauritania");
        table.AddRow("MUR", "Mauritian Rupee", "Mauritius");
        table.AddRow("MVR", "Maldivian Rufiyaa", "Maldives");
        table.AddRow("MWK", "Malawian Kwacha", "Malawi");
        table.AddRow("MXN", "Mexican Peso", "Mexico");
        table.AddRow("MYR", "Malaysian Ringgit", "Malaysia");
        table.AddRow("MZN", "Mozambican Metical", "Mozambique");
        table.AddRow("NAD", "Namibian Dollar", "Namibia");
        table.AddRow("NGN", "Nigerian Naira", "Nigeria");
        table.AddRow("NIO", "Nicaraguan Córdoba", "Nicaragua");
        table.AddRow("NOK", "Norwegian Krone", "Norway");
        table.AddRow("NPR", "Nepalese Rupee", "Nepal");
        table.AddRow("NZD", "New Zealand Dollar", "New Zealand");
        table.AddRow("OMR", "Omani Rial", "Oman");
        table.AddRow("PAB", "Panamanian Balboa", "Panama");
        table.AddRow("PEN", "Peruvian Sol", "Peru");
        table.AddRow("PGK", "Papua New Guinean Kina", "Papua New Guinea");
        table.AddRow("PHP", "Philippine Peso", "Philippines");
        table.AddRow("PKR", "Pakistani Rupee", "Pakistan");
        table.AddRow("PLN", "Polish Złoty", "Poland");
        table.AddRow("PYG", "Paraguayan Guaraní", "Paraguay");
        table.AddRow("QAR", "Qatari Riyal", "Qatar");
        table.AddRow("RON", "Romanian Leu", "Romania");
        table.AddRow("RSD", "Serbian Dinar", "Serbia");
        table.AddRow("RUB", "Russian Ruble", "Russia");
        table.AddRow("RWF", "Rwandan Franc", "Rwanda");
        table.AddRow("SAR", "Saudi Riyal", "Saudi Arabia");
        table.AddRow("SBD", "Solomon Islands Dollar", "Solomon Islands");
        table.AddRow("SCR", "Seychellois Rupee", "Seychelles");
        table.AddRow("SDG", "Sudanese Pound", "Sudan");
        table.AddRow("SEK", "Swedish Krona", "Sweden");
        table.AddRow("SGD", "Singapore Dollar", "Singapore");
        table.AddRow("SHP", "Saint Helena Pound", "Saint Helena");
        table.AddRow("SLE", "Sierra Leonean Leone", "Sierra Leone");
        table.AddRow("SOS", "Somali Shilling", "Somalia");
        table.AddRow("SRD", "Surinamese Dollar", "Suriname");
        table.AddRow("SSP", "South Sudanese Pound", "South Sudan");
        table.AddRow("STN", "São Tomé and Príncipe Dobra", "São Tomé and Príncipe");
        table.AddRow("SYP", "Syrian Pound", "Syria");
        table.AddRow("SZL", "Eswatini Lilangeni", "Eswatini");
        table.AddRow("THB", "Thai Baht", "Thailand");
        table.AddRow("TJS", "Tajikistani Somoni", "Tajikistan");
        table.AddRow("TMT", "Turkmenistan Manat", "Turkmenistan");
        table.AddRow("TND", "Tunisian Dinar", "Tunisia");
        table.AddRow("TOP", "Tongan Paʻanga", "Tonga");
        table.AddRow("TRY", "Turkish Lira", "Turkey");
        table.AddRow("TTD", "Trinidad and Tobago Dollar", "Trinidad and Tobago");
        table.AddRow("TVD", "Tuvaluan Dollar", "Tuvalu");
        table.AddRow("TWD", "New Taiwan Dollar", "Taiwan");
        table.AddRow("TZS", "Tanzanian Shilling", "Tanzania");
        table.AddRow("UAH", "Ukrainian Hryvnia", "Ukraine");
        table.AddRow("UGX", "Ugandan Shilling", "Uganda");
        table.AddRow("USD", "United States Dollar", "United States");
        table.AddRow("UYU", "Uruguayan Peso", "Uruguay");
        table.AddRow("UZS", "Uzbekistani So'm", "Uzbekistan");
        table.AddRow("VES", "Venezuelan Bolívar Soberano", "Venezuela");
        table.AddRow("VND", "Vietnamese Đồng", "Vietnam");
        table.AddRow("VUV", "Vanuatu Vatu", "Vanuatu");
        table.AddRow("WST", "Samoan Tālā", "Samoa");
        table.AddRow("XAF", "Central African CFA Franc", "CEMAC");
        table.AddRow("XCD", "East Caribbean Dollar", "Organisation of Eastern Caribbean States");
        table.AddRow("XDR", "Special Drawing Rights", "International Monetary Fund");
        table.AddRow("XOF", "West African CFA franc", "CFA");
        table.AddRow("XPF", "CFP Franc", "Collectivités d'Outre-Mer");
        table.AddRow("YER", "Yemeni Rial", "Yemen");
        table.AddRow("ZAR", "South African Rand", "South Africa");
        table.AddRow("ZMW", "Zambian Kwacha", "Zambia");
        table.AddRow("ZWL", "Zimbabwean Dollar", "Zimbabwe");

        AnsiConsole.Write(table);
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        if (settings.ListTable)
        {
            PrintTable();
            return ExitCodes.Success;
        }    

        try
        {
            IExpression expression = _state.ParseMode == ParseMode.Infix
                ? _api.Parse(settings.Expression)
                : _api.ParseRpn(settings.Expression);

            Result value = expression.Simplify().Evaluate(_api.VariablesAndConstants);

            var rates = await _remoteApiClient.GetOpenExchangeRatesAsync(cancellationToken);

            double baseNumber = rates.Rates[rates.BaseCode];

            var lookupTable = new Dictionary<string, double>(rates.Rates, StringComparer.OrdinalIgnoreCase);
  
            if (!lookupTable.TryGetValue(settings.FromUnit, out double fromRate))
            {
                AnsiConsole.MarkupLineInterpolated($"[red]Error: Unknown currency unit '{settings.FromUnit}'[/]");
                return ExitCodes.GeneralError;
            }

            if (!lookupTable.TryGetValue(settings.ToUnit, out double toRate))
            {
                AnsiConsole.MarkupLineInterpolated($"[red]Error: Unknown currency unit '{settings.ToUnit}'[/]");
                return ExitCodes.GeneralError;
            }

            double inBaseRate = (value.CastToDouble() / fromRate) * baseNumber;
            double resultRate = inBaseRate * toRate;

            AnsiConsole.MarkupLineInterpolated($"[green]{resultRate}[/] [blue link=https://www.exchangerate-api.com]// Rates By Exchange Rate API[/]");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error: {ex.Message}[/]");
            return ExitCodes.GeneralError;
        }

        return ExitCodes.Success;
    }
}
