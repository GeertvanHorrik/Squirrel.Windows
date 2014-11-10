namespace Squirrel.Update
{
    public static class ExitCodes
    {
        // Possible exit codes:
        // -1 => An error occurred. Check the log file for more information about this error
        //  0 => No errors, no additional information available
        //  1 => New version available or new version is installed successfully (depending on switch /checkonly)
        //  2 => New version which is mandatory (forced) is available (for the future?)
        //  3 => No new version available

        public const int ErrorOccurred = -1;
        public const int NothingToReport = 0;
        public const int NewVersionInstalled = 1;
        public const int NewMandatoryVersionInstalled = 2;
        public const int NoNewVersionAvailable = 3;
        public const int NewVersionAvailableButNewerThanMaximumDate = 4;
    }
}
