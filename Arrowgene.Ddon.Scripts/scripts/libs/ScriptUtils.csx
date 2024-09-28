public class ScriptUtils
{
    /**
     * @brief Parses a StageLayoutId value in the form of
     * StageId.LayerNo.GroupId.
     *
     * @param[in] layoutString String in the format of 'StageId.LayerNo.GroupId'.
     *
     * @return Returns the CDataStageLayoutId parsed from the layout string.
     */
    public static CDataStageLayoutId ParseLayoutId(string layoutString)
    {
        layoutString = layoutString.Trim();
        List<string> parts = layoutString.Split(".").ToList();

        if (parts.Count != 3)
        {
            throw new Exception($"Malformed layout string '{layoutString}'. Expects format to be 'StageId.LayerNo.GroupId'.");
        }

        return new CDataStageLayoutId()
        {
            StageId = uint.Parse(parts[0]),
            LayerNo = byte.Parse(parts[1]),
            GroupId = uint.Parse(parts[2]),
        };
    }
}
