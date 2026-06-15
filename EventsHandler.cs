using System.Collections.Generic;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Utilities;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using PlayerRoles;
using Hint = HintServiceMeow.Core.Models.Hints.Hint;
using System.Linq;
using HintServiceMeow.Core.Extension;
using HintServiceMeow.UI.Utilities;
using MEC;
using PlayerRoles.PlayableScps.Scp079;

namespace SCPList;

public class EventsHandler : CustomEventsHandler
{
    private CoroutineHandle _scpListCoroutine;
    private readonly Dictionary<Player, Hint> _activeHints = new();
    private readonly Dictionary<Player, Hint> _activeWatermarks = new();
    private string jsToMakeSure = string.Empty;
    private readonly string _watermarkText = "<u><color=#90EE90>A</color><color=#95EFE5>l</color><color=#9AEFD9>e</color><color=#9FF0CE>x</color><color=#A4F0C3>'</color><color=#A9F1B8>s</color> <color=#AEF1AD>D</color><color=#B3F2A2>e</color><color=#B8F297>v</color> <color=#BDF38C>S</color><color=#C2F381>e</color><color=#C7F476>r</color><color=#CCF46B>v</color><color=#D1F560>e</color><color=#D6F555>r</color></u>";

    public override void OnPlayerJoined(PlayerJoinedEventArgs ev)
    {
        if (_activeWatermarks.ContainsKey(ev.Player)) return;

        Hint wm = new Hint { Text = _watermarkText, FontSize = 20, Alignment = HintAlignment.Center, YCoordinate = 1065f };
        _activeWatermarks[ev.Player] = wm;
        PlayerDisplay.Get(ev.Player).AddHint(wm);
    }

    public override void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        _activeHints.Remove(ev.Player);
        _activeWatermarks.Remove(ev.Player);
    }

    public override void OnServerRoundStarted()
    {
        _activeHints.Clear();
        jsToMakeSure = string.Empty;
        /*
        Timing.KillCoroutines(_scpListCoroutine);
        _scpListCoroutine = Timing.RunCoroutine(scplistMain());
        */
    }

    private IEnumerator<float> scplistMain()
    {
        while (Round.IsRoundStarted)
        {
            List<Player> scps = Player.List.Where(p => p.Role.GetTeam() == Team.SCPs).ToList();
            int zomboblies = scps.Count(x => x.Role == RoleTypeId.Scp0492);
            int generators = Generator.List.Count(x => x.Engaged);

            string currtexxt = "<color=red><u>SCP List:</u></color>\n";
            foreach (Player p in scps)
            {
                if (p.Role == RoleTypeId.Scp0492) continue;

                string stats = (p.Role == RoleTypeId.Scp079 && p.RoleBase is Scp079Role pc && pc.SubroutineModule.TryGetSubroutine(out Scp079TierManager tier))
                    ? $"{generators}/3 | T{tier.AccessTierLevel}"
                    : $"{(int)System.Math.Round(p.Health)} HP";

                string extraInfo = (p.Role == RoleTypeId.Scp049 && zomboblies > 0) ? $" | {zomboblies} Zombies" : "";
                currtexxt += $"<color=red>{p.Role.ToString().Replace("Scp", "SCP-")} ({stats}{extraInfo})</color>\n";
            }

            if (currtexxt != jsToMakeSure)
            {
                jsToMakeSure = currtexxt;
                foreach (Player p in scps)
                {
                    if (!_activeHints.TryGetValue(p, out Hint h))
                    {
                        h = new Hint { Text = currtexxt, FontSize = 20, Alignment = HintAlignment.Right, YCoordinate = 150f };
                        _activeHints[p] = h;
                        PlayerDisplay.Get(p).AddHint(h);
                    }
                    else h.Text = currtexxt;
                }
            }

            var toRemove = _activeHints.Keys.Where(p => !scps.Contains(p)).ToList();
            foreach (var p in toRemove)
            {
                if (p != null && _activeHints.TryGetValue(p, out Hint h))
                    PlayerDisplay.Get(p).RemoveHint(h);
                _activeHints.Remove(p);
            }

            yield return Timing.WaitForSeconds(1.0f);
        }
    }
}