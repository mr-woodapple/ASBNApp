namespace ASBNApp.Enums
{
    /// <summary>
    /// Mapping for the fields in the template pdf
    /// </summary>
    public enum ASBNPdfFields
    {
        /// <summary>
        /// Date column, specifies the date of the entry
        /// </summary>
        Date1 = 27,
        Date2 = 28,
        Date3 = 29,
        Date4 = 14,
        Date5 = 15,

        /// <summary>
        /// Note column, specifies what has been done that day
        /// </summary>
        Note1 = 1,
        Note2 = 2,
        Note3 = 3,
        Note4 = 4,
        Note5 = 5,

        /// <summary>
        /// SuggestedHours column, specifies the amount of hours worked that day
        /// </summary>
        Hours1 = 6,
        Hours2 = 33,
        Hours3 = 7,
        Hours4 = 8,
        Hours5 = 9,

        /// <summary>
        /// LocationName column, specifies the working location
        /// </summary>
        Location1 = 32,
        Location2 = 10,
        Location3 = 11,
        Location4 = 12,
        Location5 = 13,

        /// <summary>
        /// Header fields for user information and time period of the report
        /// </summary>
        Username = 16,
        HeaderProfession = 37,
        HeaderApprenticeYear = 0,
        HeaderTimeperiod = 17,
        HeaderCalendarWeek = 18,

        /// <summary>
        /// Footer fields for signature, date, etc.
        /// </summary>
        FooterUserSignatureDate = 19,
        FooterRepresentativeName = 21,
        FooterRepresentativeSignatureDate = 22,
        FooterCompanyName = 24,
        FooterSchoolName = 35,
    }
}
