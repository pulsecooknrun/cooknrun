using System.Collections.Generic;

namespace AmoClient
{
    public static class AmoFilterItems
    {
        public static List<AmoFilterItem> Items { get; set; } = new List<AmoFilterItem>
        {
            new AmoFilterItem { FilterItemType =  AmoFilterItemType.Sell_PrepaymentRecieved, No = "142", Name = "Предоплата получена" },
            new AmoFilterItem { FilterItemType =  AmoFilterItemType.Sell_ClosedPotention, No = "143", Name = "Закрыто/потенциал" },
            new AmoFilterItem { FilterItemType =  AmoFilterItemType.Sell_Disassembled, No = "40928542", Name = "Неразобранное" },
            new AmoFilterItem { FilterItemType =  AmoFilterItemType.Sell_New, No = "40928545", Name = "Новая заявка" },
            new AmoFilterItem { FilterItemType =  AmoFilterItemType.Sell_PreReservation, No = "40928548", Name = "Предбронь" },
            new AmoFilterItem { FilterItemType =  AmoFilterItemType.Sell_Unqualified, No = "40928551", Name = "Не квалифицирован" },
            new AmoFilterItem { FilterItemType =  AmoFilterItemType.Sell_Qualified, No = "40928584", Name = "Квалифицирован" },
            new AmoFilterItem { FilterItemType =  AmoFilterItemType.Sell_OfferSended, No = "40928587", Name = "Предложение отправлено" },

            new AmoFilterItem { FilterItemType =  AmoFilterItemType.Game_Disassembled, No = "40928554", Name = "Неразобранное" },
            new AmoFilterItem { FilterItemType =  AmoFilterItemType.Game_Prepayment, No = "40928557", Name = "Предоплата" },
            new AmoFilterItem { FilterItemType =  AmoFilterItemType.Game_GameEnded, No = "142", Name = "Игра прошла" },
            new AmoFilterItem { FilterItemType =  AmoFilterItemType.Game_ClosedAndNotRealized, No = "143", Name = "Закрыто и нереализовано" }
        };
    }

    public class AmoFilterItem
    {
        public AmoFilterItemType FilterItemType { get; set; }
        public string Name { get; set; }
        public string No { get; set; }
    }

    public enum AmoFilterItemType
    {
        Sell_PrepaymentRecieved,
        Sell_ClosedPotention,
        Sell_Disassembled,
        Sell_New,
        Sell_PreReservation,
        Sell_Unqualified,
        Sell_Qualified,
        Sell_OfferSended,

        Game_Disassembled,
        Game_Prepayment,
        Game_GameEnded,
        Game_ClosedAndNotRealized
    }
}