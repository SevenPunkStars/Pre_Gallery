using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class ExperimentDBModel
{
    [RealtimeProperty(1, true, true)]
    private string _avatarDB = "";

    [RealtimeProperty(2, true, true)]
    private string _environment = "Ga";
}