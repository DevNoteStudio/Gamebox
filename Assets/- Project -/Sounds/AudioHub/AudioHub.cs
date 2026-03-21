using System.Collections.Generic;
using DevNote;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Audio Hub", fileName = "AudioHub")]
public class AudioHub : ScriptableObject
{

    [field: SerializeField, Expandable] public SoundUnit Music { get; private set; }

    [field: Foldout("▶ COMMON"), SerializeField, Expandable] public SoundUnit ShowElement { get; private set; }
    [field: Foldout("▶ COMMON"), SerializeField, Expandable] public SoundUnit ShowWindow { get; private set; }
    [field: Foldout("▶ COMMON"), SerializeField, Expandable] public SoundUnit HideWindow { get; private set; }
    [field: Foldout("▶ COMMON"), SerializeField, Expandable] public SoundUnit Click { get; private set; }
    [field: Foldout("▶ COMMON"), SerializeField, Expandable] public SoundUnit OpenClick { get; private set; }
    [field: Foldout("▶ COMMON"), SerializeField, Expandable] public SoundUnit PointerEnter { get; private set; }


    [field: Foldout("▶ CURRENCY WIDGET"), SerializeField, Expandable] public SoundUnit CoinsRollupStart { get; private set; }
    [field: Foldout("▶ CURRENCY WIDGET"), SerializeField, Expandable] public SoundUnit CoinsRollupFinish { get; private set; }


    [field: Foldout("▶ WIN WINDOW"), SerializeField, Expandable] public SoundUnit WinShow { get; private set; }
    [field: Foldout("▶ WIN WINDOW"), SerializeField, Expandable] public SoundUnit WinConfetti { get; private set; }
    [field: Foldout("▶ WIN WINDOW"), SerializeField, Expandable] public SoundUnit RouletteTick { get; private set; }
    [field: Foldout("▶ WIN WINDOW"), SerializeField, Expandable] public SoundUnit RouletteStop { get; private set; }
    [field: Foldout("▶ WIN WINDOW"), SerializeField, Expandable] public List<SoundUnit> WinStars { get; private set; }


    [field: Foldout("▶ LOSE WINDOW"), SerializeField, Expandable] public SoundUnit LoseShow { get; private set; }
    [field: Foldout("▶ LOSE WINDOW"), SerializeField, Expandable] public SoundUnit Revive { get; private set; }


    [field: Foldout("▶ UNLOCK WINDOW"), SerializeField, Expandable] public SoundUnit UnlockWindowProgressSliderFilling { get; private set; }
    [field: Foldout("▶ UNLOCK WINDOW"), SerializeField, Expandable] public SoundUnit UnlockWindowProgressIconFilling { get; private set; }
    [field: Foldout("▶ UNLOCK WINDOW"), SerializeField, Expandable] public SoundUnit UnlockWindowShowProgressIcon { get; private set; }
    [field: Foldout("▶ UNLOCK WINDOW"), SerializeField, Expandable] public SoundUnit UnlockWindowShowCenterIcon { get; private set; }

    
    [field: Foldout("▶ SCORE WIDGET"), SerializeField, Expandable] public SoundUnit AddScore { get; private set; }
    [field: Foldout("▶ SCORE WIDGET"), SerializeField, Expandable] public SoundUnit ScoreParticleStart { get; private set; }
    [field: Foldout("▶ SCORE WIDGET"), SerializeField, Expandable] public SoundUnit ScoreCompleted{ get; private set; }


    [field: Foldout("▶ BOOSTER BUTTONS"), SerializeField, Expandable] public SoundUnit BuyBooster { get; private set; }
    [field: Foldout("▶ BOOSTER BUTTONS"), SerializeField, Expandable] public SoundUnit BoosterUsingStart { get; private set; }
    [field: Foldout("▶ BOOSTER BUTTONS"), SerializeField, Expandable] public SoundUnit BoosterUsingFinish { get; private set; }


    [field: Foldout("▶ LEADER BUTTON"), SerializeField, Expandable] public SoundUnit LeaderParticleStart { get; private set; }
    [field: Foldout("▶ LEADER BUTTON"), SerializeField, Expandable] public SoundUnit LeaderParticleFinish { get; private set; }





}
